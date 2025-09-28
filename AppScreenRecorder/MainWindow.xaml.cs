using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System.Windows.Input;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace AppScreenRecorder
{
    public partial class MainWindow : Window
    {
        private ScreenRecorder _screenRecorder;
        private AudioRecorder _audioRecorder;
        private DispatcherTimer _recordingTimer;
        private DateTime _startTime;
        private bool _isRecording = false;
        private bool _isPaused = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            SetInitialState();
            this.Focusable = true;
            this.Focus();
        }

        private void InitializeTimer()
        {
            _recordingTimer = new DispatcherTimer();
            _recordingTimer.Interval = TimeSpan.FromSeconds(1);
            _recordingTimer.Tick += UpdateRecordingTime;
        }

        private void SetInitialState()
        {
            RecordButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            PauseButton.IsEnabled = false;
            StatusLabel.Content = "就绪";
            StatusLabel.Foreground = System.Windows.Media.Brushes.Green;
            RecordTimeLabel.Content = "00:00:00";
            StatusIndicator.Fill = System.Windows.Media.Brushes.Green;
            DetailStatusLabel.Text = "系统就绪";

            // 设置默认输出路径
            string defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "录屏文件");
            if (!Directory.Exists(defaultPath))
            {
                Directory.CreateDirectory(defaultPath);
            }
            OutputPathTextBox.Text = Path.Combine(defaultPath, "录屏_");
        }

        private async void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_isRecording)
                {
                    await StartRecording();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"开始录制时出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                SetInitialState();
            }
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await StopRecording();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止录制时出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_isPaused)
                {
                    ResumeRecording();
                }
                else
                {
                    PauseRecording();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"暂停/恢复录制时出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task StartRecording()
        {
            // 生成输出文件路径
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string finalOutputPath = OutputPathTextBox.Text + timestamp + ".mp4";
            string audioBasePath = OutputPathTextBox.Text + timestamp;

            // 获取录制设置
            var quality = GetSelectedQuality();
            var frameRate = GetSelectedFrameRate();
            var recordArea = GetRecordArea();

            // 更新UI状态
            _isRecording = true;
            _isPaused = false;
            _startTime = DateTime.Now;

            RecordButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            PauseButton.IsEnabled = true;
            PauseButton.Content = "暂停录制";

            StatusLabel.Content = "录制中...";
            StatusLabel.Foreground = System.Windows.Media.Brushes.Red;
            StatusIndicator.Fill = System.Windows.Media.Brushes.Red;
            DetailStatusLabel.Text = "正在录制屏幕...";

            // 启动计时器
            _recordingTimer.Start();

            try
            {
                // 初始化录制器
                _screenRecorder = new ScreenRecorder();
                _audioRecorder = new AudioRecorder();

                // 启动音频录制（如果需要）
                bool recordMic = RecordMicAudioCheckBox.IsChecked ?? false;
                bool recordSystem = RecordSystemAudioCheckBox.IsChecked ?? false;

                if (recordMic || recordSystem)
                {
                    _audioRecorder.StartRecording(audioBasePath, recordMic, recordSystem);
                }

                // 启动屏幕录制
                await _screenRecorder.StartRecording(finalOutputPath, recordArea, frameRate, quality);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"录制失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                await StopRecording();
                throw;
            }
        }

        private async Task StopRecording()
        {
            if (!_isRecording) return;

            _isRecording = false;
            _recordingTimer.Stop();

            // 更新UI状态
            StatusLabel.Content = "正在停止录制...";
            StatusLabel.Foreground = System.Windows.Media.Brushes.Orange;
            StatusIndicator.Fill = System.Windows.Media.Brushes.Orange;
            DetailStatusLabel.Text = "正在停止录制，请稍候...";

            try
            {
                string tempVideoPath = null;
                string systemAudioPath = null;
                string micAudioPath = null;

                // 1. 立即停止屏幕录制
                if (_screenRecorder != null)
                {
                    await _screenRecorder.StopRecording();
                    tempVideoPath = _screenRecorder.TempVideoPath;
                }

                // 2. 立即停止音频录制
                if (_audioRecorder != null)
                {
                    _audioRecorder.StopRecording();
                    systemAudioPath = _audioRecorder.SystemAudioPath;
                    micAudioPath = _audioRecorder.MicAudioPath;
                }

                // 3. 减少等待时间，只等待文件系统同步
                await Task.Delay(200);

                // 更新UI状态
                StatusLabel.Content = "处理中...";
                DetailStatusLabel.Text = "正在合并视频和音频...";

                // 4. 生成最终输出路径
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string finalOutputPath = OutputPathTextBox.Text + timestamp + ".mp4";

                // 5. 合并视频和音频
                await MergeVideoAndAudio(tempVideoPath, systemAudioPath, micAudioPath, finalOutputPath);

                // 6. 清理资源
                _screenRecorder?.Dispose();
                _audioRecorder?.Dispose();
                _screenRecorder = null;
                _audioRecorder = null;

                // 恢复UI状态
                SetInitialState();

                MessageBox.Show($"录制完成！\n文件保存至：{finalOutputPath}", "成功",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _screenRecorder?.Dispose();
                _audioRecorder?.Dispose();
                _screenRecorder = null;
                _audioRecorder = null;

                MessageBox.Show($"停止录制时出错：{ex.Message}", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SetInitialState();
            }
        }

        private async Task MergeVideoAndAudio(string videoPath, string systemAudioPath,
    string micAudioPath, string outputPath)
        {
            try
            {
                if (string.IsNullOrEmpty(videoPath) || !File.Exists(videoPath))
                {
                    throw new Exception("视频文件不存在");
                }

                if (string.IsNullOrEmpty(systemAudioPath) && string.IsNullOrEmpty(micAudioPath))
                {
                    File.Copy(videoPath, outputPath, true);
                    return;
                }

                if (!string.IsNullOrEmpty(systemAudioPath) && File.Exists(systemAudioPath) &&
                    !string.IsNullOrEmpty(micAudioPath) && File.Exists(micAudioPath))
                {
                    await ScreenRecorder.MergeVideoWithMultipleAudio(videoPath, systemAudioPath,
                        micAudioPath, outputPath);
                }

                else if (!string.IsNullOrEmpty(systemAudioPath) && File.Exists(systemAudioPath))
                {
                    await ScreenRecorder.MergeVideoAndAudio(videoPath, systemAudioPath, outputPath);
                }

                else if (!string.IsNullOrEmpty(micAudioPath) && File.Exists(micAudioPath))
                {
                    await ScreenRecorder.MergeVideoAndAudio(videoPath, micAudioPath, outputPath);
                }
                else
                {
                    File.Copy(videoPath, outputPath, true);
                }

                CleanupTempFiles(videoPath, systemAudioPath, micAudioPath);
            }
            catch (Exception ex)
            {
                throw new Exception($"合并视频音频失败: {ex.Message}", ex);
            }
        }

        // 清理临时文件的方法
        private void CleanupTempFiles(params string[] filePaths)
        {
            foreach (var filePath in filePaths)
            {
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"删除临时文件失败: {ex.Message}");
                    }
                }
            }
        }


        private void PauseRecording()
        {
            if (!_isRecording) return;

            _isPaused = true;
            _recordingTimer.Stop();

            _screenRecorder?.PauseRecording();
            _audioRecorder?.PauseRecording();

            PauseButton.Content = "恢复录制";
            StatusLabel.Content = "已暂停";
            StatusLabel.Foreground = System.Windows.Media.Brushes.Orange;
            StatusIndicator.Fill = System.Windows.Media.Brushes.Orange;
            DetailStatusLabel.Text = "录制已暂停";
        }

        private void ResumeRecording()
        {
            if (!_isRecording) return;

            _isPaused = false;
            _recordingTimer.Start();

            _screenRecorder?.ResumeRecording();
            _audioRecorder?.ResumeRecording();

            PauseButton.Content = "暂停录制";
            StatusLabel.Content = "录制中...";
            StatusLabel.Foreground = System.Windows.Media.Brushes.Red;
            StatusIndicator.Fill = System.Windows.Media.Brushes.Red;
            DetailStatusLabel.Text = "正在录制屏幕...";
        }

        private void UpdateRecordingTime(object sender, EventArgs e)
        {
            if (_isRecording && !_isPaused)
            {
                TimeSpan elapsed = DateTime.Now - _startTime;
                RecordTimeLabel.Content = elapsed.ToString(@"hh\:mm\:ss");
            }
        }

        private int GetSelectedQuality()
        {
            var selectedItem = VideoQualityComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem;
            return selectedItem?.Content.ToString() switch
            {
                "高质量 (1080p)" => 1080,
                "标准质量 (720p)" => 720,
                "低质量 (480p)" => 480,
                _ => 720
            };
        }

        private int GetSelectedFrameRate()
        {
            var selectedItem = FrameRateComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem;
            return selectedItem?.Content.ToString() switch
            {
                "60 FPS" => 60,
                "30 FPS" => 30,
                "24 FPS" => 24,
                "15 FPS" => 15,
                _ => 30
            };
        }

        private Rectangle? GetRecordArea()
        {
            var selectedItem = RecordAreaComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem;
            return selectedItem?.Content.ToString() switch
            {
                "全屏录制" => Screen.PrimaryScreen.Bounds,
                "选择窗口" => null, // 这里可以添加窗口选择逻辑
                "自定义区域" => null, // 这里可以添加区域选择逻辑
                _ => Screen.PrimaryScreen.Bounds
            };
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "选择录屏文件保存路径";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OutputPathTextBox.Text = Path.Combine(dialog.SelectedPath, "录屏_");
            }
        }

        private void OpenOutputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string folderPath = Path.GetDirectoryName(OutputPathTextBox.Text);
                if (Directory.Exists(folderPath))
                {
                    Process.Start("explorer.exe", folderPath);
                }
                else
                {
                    MessageBox.Show("输出文件夹不存在！", "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开文件夹失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.Key)
                {
                    case Key.F9:
                        await HandleF9KeyPress();
                        e.Handled = true;
                        break;
                    case Key.F10:
                        HandleF10KeyPress();
                        e.Handled = true;
                        break;
                    case Key.F11:
                        HandleF11KeyPress();
                        e.Handled = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"快捷键操作出错：{ex.Message}", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // F9 - 开始/停止录制
        private async Task HandleF9KeyPress()
        {
            if (_isRecording)
            {
                await StopRecording();
            }
            else
            {
                await StartRecording();
            }
        }

        // F10 - 暂停/恢复录制
        private void HandleF10KeyPress()
        {
            if (!_isRecording)
            {
                MessageBox.Show("请先开始录制后再使用暂停功能", "提示",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (_isPaused)
            {
                ResumeRecording();
            }
            else
            {
                PauseRecording();
            }
        }

        // F11 - 截图功能
        private void HandleF11KeyPress()
        {
            try
            {
                TakeScreenshot();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"截图失败：{ex.Message}", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 截图功能实现
        private void TakeScreenshot()
        {
            try
            {
                var bounds = Screen.PrimaryScreen.Bounds;
                using (var bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (var graphics = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        graphics.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size);
                    }

                    // 生成截图文件名
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string screenshotPath = Path.Combine(
                        Path.GetDirectoryName(OutputPathTextBox.Text),
                        $"截图_{timestamp}.png");

                    // 确保目录存在
                    Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath));

                    // 保存截图
                    bitmap.Save(screenshotPath, System.Drawing.Imaging.ImageFormat.Png);

                    // 显示成功消息
                    MessageBox.Show($"截图已保存：\n{screenshotPath}", "截图成功",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"截图操作失败：{ex.Message}");
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            this.Focus();
        }


        protected override void OnClosed(EventArgs e)
        {
            // 确保在关闭窗口时停止录制
            if (_isRecording)
            {
                _screenRecorder?.Dispose();
                _audioRecorder?.Dispose();
            }
            _recordingTimer?.Stop();
            base.OnClosed(e);
        }
    }
}