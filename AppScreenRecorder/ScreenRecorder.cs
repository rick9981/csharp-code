using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FFMpegCore;
using FFMpegCore.Enums;

namespace AppScreenRecorder
{
    public class ScreenRecorder : IDisposable
    {
        private bool _isRecording = false;
        private bool _isPaused = false;
        private string _outputPath;
        private Rectangle _captureArea;
        private string _tempDirectory;
        private int _frameRate = 30;
        private int _quality = 720;
        private string _tempVideoPath;

        // 使用并发队列来缓存帧
        private readonly ConcurrentQueue<FrameData> _frameQueue = new ConcurrentQueue<FrameData>();
        private Task _captureTask;
        private Task _saveTask;
        private CancellationTokenSource _cancellationTokenSource;

        // 高精度计时器
        private System.Diagnostics.Stopwatch _stopwatch;
        private long _nextFrameTime;
        private long _frameInterval;
        private int _frameIndex = 0;

        public string TempVideoPath => _tempVideoPath;

        private struct FrameData
        {
            public Bitmap Bitmap;
            public int Index;
            public DateTime Timestamp;
        }

        public ScreenRecorder()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(),
                "ScreenRecorder_" + Guid.NewGuid().ToString("N")[..8]);
            Directory.CreateDirectory(_tempDirectory);
        }

        public async Task StartRecording(string outputPath, Rectangle? captureArea = null,
            int frameRate = 30, int quality = 720)
        {
            _outputPath = outputPath;
            _captureArea = captureArea ?? Screen.PrimaryScreen.Bounds;
            _frameRate = frameRate;
            _quality = quality;
            _isRecording = true;
            _isPaused = false;
            _frameIndex = 0;

            // 初始化高精度计时器
            _stopwatch = System.Diagnostics.Stopwatch.StartNew();
            _frameInterval = TimeSpan.TicksPerSecond / _frameRate;
            _nextFrameTime = _frameInterval;

            _cancellationTokenSource = new CancellationTokenSource();

            // 启动捕获和保存任务
            _captureTask = Task.Run(() => CaptureScreenFrames(_cancellationTokenSource.Token));
            _saveTask = Task.Run(() => SaveFramesToDisk(_cancellationTokenSource.Token));

            await Task.CompletedTask;
        }

        public async Task StopRecording()
        {
            // 立即停止录制标志
            _isRecording = false;
            _isPaused = false;

            Console.WriteLine("开始停止录制...");

            if (_cancellationTokenSource != null)
            {
                // 立即取消捕获任务
                _cancellationTokenSource.Cancel();

                try
                {
                    // 等待捕获任务完成
                    if (_captureTask != null)
                    {
                        var timeoutTask = Task.Delay(3000); // 3秒超时
                        var completedTask = await Task.WhenAny(_captureTask, timeoutTask);
                        if (completedTask == timeoutTask)
                        {
                            Console.WriteLine("警告: 捕获任务超时");
                        }
                        else
                        {
                            Console.WriteLine("捕获任务已完成");
                        }
                    }

                    // 等待保存任务处理完所有帧
                    if (_saveTask != null)
                    {
                        Console.WriteLine($"等待保存任务完成，队列剩余: {_frameQueue.Count} 帧");

                        var timeoutTask = Task.Delay(10000); // 10秒超时，给保存任务更多时间
                        var completedTask = await Task.WhenAny(_saveTask, timeoutTask);
                        if (completedTask == timeoutTask)
                        {
                            Console.WriteLine("警告: 保存任务超时");
                            // 即使超时也继续，清理剩余帧
                            while (_frameQueue.TryDequeue(out var frame))
                            {
                                frame.Bitmap?.Dispose();
                            }
                        }
                        else
                        {
                            Console.WriteLine("保存任务已完成");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"停止录制任务时出错: {ex.Message}");
                }
            }

            // 停止计时器
            _stopwatch?.Stop();

            // 此时所有帧应该都已保存完毕，生成视频
            if (_frameIndex > 0)
            {
                Console.WriteLine($"开始生成视频，总帧数: {_frameIndex}");
                var tempVideoPath = Path.Combine(_tempDirectory, "temp_video.mp4");
                await GenerateVideoOnly(tempVideoPath);
                _tempVideoPath = tempVideoPath;
                Console.WriteLine("视频生成完成");
            }
        }

        public Task PauseRecording()
        {
            _isPaused = true;
            return Task.CompletedTask;
        }

        public Task ResumeRecording()
        {
            _isPaused = false;
            // 重置计时器以避免时间跳跃
            if (_stopwatch != null)
            {
                _stopwatch.Restart();
                _nextFrameTime = _frameInterval;
            }
            return Task.CompletedTask;
        }

        private void CaptureScreenFrames(CancellationToken cancellationToken)
        {
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
            Console.WriteLine("捕获任务开始");

            while (_isRecording && !cancellationToken.IsCancellationRequested)
            {
                if (_isPaused)
                {
                    // 暂停时重置计时器
                    _stopwatch.Restart();
                    _nextFrameTime = _frameInterval;
                    Thread.Sleep(50);
                    continue;
                }

                var currentTime = _stopwatch.ElapsedTicks;

                if (currentTime >= _nextFrameTime)
                {
                    // 最后一次检查录制状态
                    if (!_isRecording)
                    {
                        break;
                    }

                    try
                    {
                        var bitmap = CaptureScreenOptimized(_captureArea);

                        // 检查队列大小，避免内存溢出
                        if (_frameQueue.Count < 500) 
                        {
                            _frameQueue.Enqueue(new FrameData
                            {
                                Bitmap = bitmap,
                                Index = _frameIndex++,
                                Timestamp = DateTime.Now
                            });
                        }
                        else
                        {
                            bitmap?.Dispose();
                            Console.WriteLine("队列满，丢弃一帧");
                        }

                        _nextFrameTime += _frameInterval;

                        if (currentTime > _nextFrameTime + _frameInterval * 2)
                        {
                            _nextFrameTime = currentTime + _frameInterval;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"捕获帧时出错: {ex.Message}");
                        _nextFrameTime += _frameInterval;
                    }
                }
                else
                {
                    // 精确等待
                    var waitTicks = _nextFrameTime - currentTime;
                    var waitMs = Math.Max(1, Math.Min(waitTicks / TimeSpan.TicksPerMillisecond, 15));
                    Thread.Sleep((int)waitMs);
                }
            }

            Console.WriteLine($"捕获任务结束，总共捕获 {_frameIndex} 帧，队列剩余: {_frameQueue.Count}");
        }

        private void SaveFramesToDisk(CancellationToken cancellationToken)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Normal;
            var processedFrames = 0;

            Console.WriteLine("保存任务开始");

            while (true)
            {
                bool hasMoreFrames = _frameQueue.TryDequeue(out var frameData);

                if (hasMoreFrames)
                {
                    try
                    {
                        var frameFile = Path.Combine(_tempDirectory, $"frame_{frameData.Index:D6}.png");

                        using (frameData.Bitmap)
                        {
                            frameData.Bitmap.Save(frameFile, ImageFormat.Png);
                        }

                        processedFrames++;

                        if (processedFrames % 100 == 0)
                        {
                            Console.WriteLine($"已保存 {processedFrames} 帧，队列剩余: {_frameQueue.Count}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"保存帧时出错: {ex.Message}");
                        frameData.Bitmap?.Dispose();
                    }
                }
                else
                {
                    if (cancellationToken.IsCancellationRequested && !_isRecording)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }
            }

            Console.WriteLine($"保存任务结束，总共保存 {processedFrames} 帧");
        }

        private Bitmap CaptureScreenOptimized(Rectangle area)
        {
            var bitmap = new Bitmap(area.Width, area.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

                graphics.CopyFromScreen(area.X, area.Y, 0, 0, area.Size, CopyPixelOperation.SourceCopy);
            }

            return bitmap;
        }

        private async Task GenerateVideoOnly(string outputPath)
        {
            try
            {
                var inputPattern = Path.Combine(_tempDirectory, "frame_%06d.png");

                await FFMpegArguments
                    .FromFileInput(inputPattern, false, options => options
                        .WithFramerate(_frameRate))
                    .OutputToFile(outputPath, true, options => options
                        .WithVideoCodec(VideoCodec.LibX264)
                        .WithCustomArgument("-preset ultrafast") 
                        .WithCustomArgument("-tune zerolatency") 
                        .WithVideoBitrate(GetBitrateForQuality(_quality))
                        .WithFramerate(_frameRate)
                        .WithVideoFilters(filterOptions => filterOptions
                            .Scale(GetVideoSize(_quality))))
                    .ProcessAsynchronously();
            }
            catch (Exception ex)
            {
                throw new Exception($"视频生成失败: {ex.Message}", ex);
            }
        }

        public static async Task MergeVideoAndAudio(string videoPath, string audioPath, string outputPath)
        {
            try
            {
                await FFMpegArguments
                    .FromFileInput(videoPath)
                    .AddFileInput(audioPath)
                    .OutputToFile(outputPath, true, options => options
                        .WithCustomArgument("-c:v copy")
                        .WithAudioCodec(AudioCodec.Aac)
                        .WithAudioBitrate(128)
                        .WithCustomArgument("-shortest"))
                    .ProcessAsynchronously();
            }
            catch (Exception ex)
            {
                throw new Exception($"视频音频合并失败: {ex.Message}", ex);
            }
        }

        public static async Task MergeVideoWithMultipleAudio(string videoPath, string systemAudioPath,
            string micAudioPath, string outputPath)
        {
            try
            {
                bool hasMicAudio = !string.IsNullOrEmpty(micAudioPath) && File.Exists(micAudioPath);

                if (hasMicAudio)
                {
                    await FFMpegArguments
                        .FromFileInput(videoPath)
                        .AddFileInput(systemAudioPath)
                        .AddFileInput(micAudioPath)
                        .OutputToFile(outputPath, true, options => options
                            .WithCustomArgument("-c:v copy")
                            .WithAudioCodec(AudioCodec.Aac)
                            .WithAudioBitrate(128)
                            .WithCustomArgument("-filter_complex [1:a][2:a]amix=inputs=2:duration=shortest[a]")
                            .WithCustomArgument("-map 0:v")
                            .WithCustomArgument("-map [a]"))
                        .ProcessAsynchronously();
                }
                else
                {
                    await MergeVideoAndAudio(videoPath, systemAudioPath, outputPath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"视频多音频合并失败: {ex.Message}", ex);
            }
        }

        private int GetBitrateForQuality(int quality)
        {
            return quality switch
            {
                1080 => 4000,
                720 => 2000,
                480 => 1000,
                _ => 2000
            };
        }

        private VideoSize GetVideoSize(int quality)
        {
            return quality switch
            {
                1080 => VideoSize.FullHd,
                720 => VideoSize.Hd,
                480 => VideoSize.Ed,
                _ => VideoSize.Hd
            };
        }

        private void CleanupTempFiles()
        {
            try
            {
                if (Directory.Exists(_tempDirectory))
                {
                    Directory.Delete(_tempDirectory, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"清理临时文件时出错: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (_isRecording)
            {
                _isRecording = false;
            }

            _cancellationTokenSource?.Cancel();

            while (_frameQueue.TryDequeue(out var frame))
            {
                frame.Bitmap?.Dispose();
            }

            _stopwatch?.Stop();
            _cancellationTokenSource?.Dispose();
            CleanupTempFiles();
        }
    }
}