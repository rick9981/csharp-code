using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppPortScan;

namespace AppPortScanner
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource cancellationTokenSource;
        private readonly object lockObject = new object();
        private int totalPorts;
        private int scannedPorts;
        private int openPorts;
        private readonly List<ScanResult> scanResults = new List<ScanResult>();

        public Form1()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // 设置窗体图标和标题
            this.Text = "端口扫描工具 v1.0";

            // 初始化状态
            UpdateUI(false);
            lblStatus.Text = "就绪";
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            StartScan();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopScan();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lstResults.Items.Clear();
            scanResults.Clear();
            progressBar.Value = 0;
            lblStatus.Text = "就绪";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportResults();
        }

        private bool ValidateInput()
        {
            // 验证主机地址
            string target = txtTarget.Text.Trim();
            if (string.IsNullOrEmpty(target))
            {
                MessageBox.Show("请输入主机地址!", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTarget.Focus();
                return false;
            }

            // 验证端口范围
            if (numStartPort.Value > numEndPort.Value)
            {
                MessageBox.Show("起始端口不能大于结束端口!", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numStartPort.Focus();
                return false;
            }

            return true;
        }

        private async void StartScan()
        {
            try
            {
                cancellationTokenSource = new CancellationTokenSource();
                UpdateUI(true);

                string target = txtTarget.Text.Trim();
                int startPort = (int)numStartPort.Value;
                int endPort = (int)numEndPort.Value;
                int threadCount = (int)numThreads.Value;
                int timeout = (int)numTimeout.Value;

                totalPorts = endPort - startPort + 1;
                scannedPorts = 0;
                openPorts = 0;

                progressBar.Maximum = totalPorts;
                progressBar.Value = 0;

                lblStatus.Text = $"正在扫描 {target}:{startPort}-{endPort}...";

                // 首先进行ping测试
                bool isHostAlive = await PingHost(target);
                if (!isHostAlive)
                {
                    AddResult($"警告: 主机 {target} 可能无法访问或不响应ping请求", Color.Orange);
                }

                // 开始端口扫描
                await ScanPortsAsync(target, startPort, endPort, threadCount, timeout, cancellationTokenSource.Token);

                if (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    lblStatus.Text = $"扫描完成! 共扫描 {totalPorts} 个端口，发现 {openPorts} 个开放端口";
                    AddResult("=== 扫描完成 ===", Color.Green);
                }
            }
            catch (OperationCanceledException)
            {
                lblStatus.Text = "扫描已取消";
                AddResult("=== 扫描已取消 ===", Color.Red);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "扫描出错";
                AddResult($"错误: {ex.Message}", Color.Red);
            }
            finally
            {
                UpdateUI(false);
            }
        }

        private void StopScan()
        {
            cancellationTokenSource?.Cancel();
        }

        private async Task<bool> PingHost(string target)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(target, 3000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        private async Task ScanPortsAsync(string target, int startPort, int endPort, int threadCount, int timeout, CancellationToken cancellationToken)
        {
            var semaphore = new SemaphoreSlim(threadCount, threadCount);
            var tasks = new List<Task>();

            for (int port = startPort; port <= endPort; port++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                int currentPort = port;
                var task = Task.Run(async () =>
                {
                    await semaphore.WaitAsync(cancellationToken);
                    try
                    {
                        await ScanPortAsync(target, currentPort, timeout, cancellationToken);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }, cancellationToken);

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }

        private async Task ScanPortAsync(string target, int port, int timeout, CancellationToken cancellationToken)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var connectTask = client.ConnectAsync(target, port);
                    var timeoutTask = Task.Delay(timeout, cancellationToken);

                    var completedTask = await Task.WhenAny(connectTask, timeoutTask);

                    if (completedTask == connectTask && client.Connected)
                    {
                        string service = GetServiceName(port);
                        var result = new ScanResult
                        {
                            Host = target,
                            Port = port,
                            IsOpen = true,
                            Service = service,
                            ScanTime = DateTime.Now
                        };

                        lock (lockObject)
                        {
                            scanResults.Add(result);
                            openPorts++;
                        }

                        Invoke(new Action(() =>
                        {
                            AddResult($"端口 {port} 开放 - {service}", Color.Green);
                        }));
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
                // 端口关闭或连接失败
            }
            finally
            {
                lock (lockObject)
                {
                    scannedPorts++;
                }

                Invoke(new Action(() =>
                {
                    progressBar.Value = scannedPorts;
                    lblStatus.Text = $"正在扫描... ({scannedPorts}/{totalPorts}) 已发现 {openPorts} 个开放端口";
                }));
            }
        }

        private string GetServiceName(int port)
        {
            var commonPorts = new Dictionary<int, string>
            {
                {21, "FTP"},
                {22, "SSH"},
                {23, "Telnet"},
                {25, "SMTP"},
                {53, "DNS"},
                {80, "HTTP"},
                {110, "POP3"},
                {135, "RPC"},
                {139, "NetBIOS"},
                {143, "IMAP"},
                {443, "HTTPS"},
                {445, "SMB"},
                {993, "IMAPS"},
                {995, "POP3S"},
                {1433, "MSSQL"},
                {1521, "Oracle"},
                {3306, "MySQL"},
                {3389, "RDP"},
                {5432, "PostgreSQL"},
                {5900, "VNC"},
                {6379, "Redis"},
                {8080, "HTTP-Alt"},
                {8443, "HTTPS-Alt"}
            };

            return commonPorts.ContainsKey(port) ? commonPorts[port] : "Unknown";
        }

        private void AddResult(string message, Color color)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddResult(message, color)));
                return;
            }

            lstResults.Items.Add($"[{DateTime.Now:HH:mm:ss}] {message}");

            // 滚动到最后一项
            if (lstResults.Items.Count > 0)
            {
                lstResults.TopIndex = lstResults.Items.Count - 1;
            }
        }

        private void UpdateUI(bool isScanning)
        {
            btnScan.Enabled = !isScanning;
            btnStop.Enabled = isScanning;
            txtTarget.Enabled = !isScanning;
            numStartPort.Enabled = !isScanning;
            numEndPort.Enabled = !isScanning;
            numThreads.Enabled = !isScanning;
            numTimeout.Enabled = !isScanning;
        }

        private void ExportResults()
        {
            if (scanResults.Count == 0)
            {
                MessageBox.Show("没有扫描结果可导出!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "文本文件 (*.txt)|*.txt|CSV文件 (*.csv)|*.csv";
                saveDialog.FileName = $"PortScan_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var content = new StringBuilder();

                        if (saveDialog.FilterIndex == 1) // TXT格式
                        {
                            content.AppendLine("端口扫描结果");
                            content.AppendLine("================");
                            content.AppendLine($"扫描时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                            content.AppendLine($"目标主机: {txtTarget.Text}");
                            content.AppendLine($"端口范围: {numStartPort.Value}-{numEndPort.Value}");
                            content.AppendLine($"开放端口数: {openPorts}");
                            content.AppendLine();

                            foreach (var result in scanResults)
                            {
                                if (result.IsOpen)
                                {
                                    content.AppendLine($"{result.Host}:{result.Port} - {result.Service}");
                                }
                            }
                        }
                        else // CSV格式
                        {
                            content.AppendLine("主机,端口,状态,服务,扫描时间");
                            foreach (var result in scanResults)
                            {
                                if (result.IsOpen)
                                {
                                    content.AppendLine($"{result.Host},{result.Port},开放,{result.Service},{result.ScanTime:yyyy-MM-dd HH:mm:ss}");
                                }
                            }
                        }

                        File.WriteAllText(saveDialog.FileName, content.ToString(), Encoding.UTF8);
                        MessageBox.Show($"结果已导出到: {saveDialog.FileName}", "导出成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"导出失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }
    }

    
}