using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using NAudio.Wave;

namespace AppScreenRecorder
{
    public class AudioRecorder : IDisposable
    {
        private WasapiLoopbackCapture _loopbackCapture;
        private WaveInEvent _micCapture;
        private string _systemAudioPath;
        private string _micAudioPath;
        private WaveFileWriter _audioWriter;
        private WaveFileWriter _micWriter;
        private bool _isRecording = false;
        private bool _isPaused = false;

        public string SystemAudioPath => _systemAudioPath;
        public string MicAudioPath => _micAudioPath;

        public void StartRecording(string basePath, bool recordMic = true, bool recordSystem = true)
        {
            _isRecording = true;
            _isPaused = false;

            if (recordSystem)
            {
                _systemAudioPath = Path.ChangeExtension(basePath, "_system.wav");

                _loopbackCapture = new WasapiLoopbackCapture();
                _loopbackCapture.DataAvailable += OnSystemAudioDataAvailable;
                _loopbackCapture.RecordingStopped += OnRecordingStopped;

                _audioWriter = new WaveFileWriter(_systemAudioPath, _loopbackCapture.WaveFormat);
                _loopbackCapture.StartRecording();
            }

            if (recordMic)
            {
                _micAudioPath = Path.ChangeExtension(basePath, "_mic.wav");

                _micCapture = new WaveInEvent();
                _micCapture.WaveFormat = new WaveFormat(44100, 16, 2);
                _micCapture.DataAvailable += OnMicDataAvailable;

                _micWriter = new WaveFileWriter(_micAudioPath, _micCapture.WaveFormat);
                _micCapture.StartRecording();
            }
        }

        public void StopRecording()
        {
            // 立即设置停止标志
            _isRecording = false;
            _isPaused = false;

            try
            {
                // 立即停止录制设备
                _loopbackCapture?.StopRecording();
                _micCapture?.StopRecording();

                // 立即刷新并关闭写入器
                _audioWriter?.Flush();
                _audioWriter?.Dispose();
                _micWriter?.Flush();
                _micWriter?.Dispose();

                // 释放捕获设备
                _loopbackCapture?.Dispose();
                _micCapture?.Dispose();

                _audioWriter = null;
                _micWriter = null;
                _loopbackCapture = null;
                _micCapture = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"停止音频录制时出错: {ex.Message}");
            }
        }

        public void PauseRecording()
        {
            _isPaused = true;
        }

        public void ResumeRecording()
        {
            _isPaused = false;
        }

        private void OnSystemAudioDataAvailable(object sender, WaveInEventArgs e)
        {
            if (_isRecording && !_isPaused && _audioWriter != null)
            {
                try
                {
                    _audioWriter.Write(e.Buffer, 0, e.BytesRecorded);
                }
                catch (ObjectDisposedException)
                {

                }
            }
        }

        private void OnMicDataAvailable(object sender, WaveInEventArgs e)
        {
            if (_isRecording && !_isPaused && _micWriter != null)
            {
                try
                {
                    _micWriter.Write(e.Buffer, 0, e.BytesRecorded);
                }
                catch (ObjectDisposedException)
                {

                }
            }
        }
        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            if (e.Exception != null)
            {
                Console.WriteLine($"音频录制出错: {e.Exception.Message}");
            }
        }

        public void Dispose()
        {
            StopRecording();
        }
    }
}