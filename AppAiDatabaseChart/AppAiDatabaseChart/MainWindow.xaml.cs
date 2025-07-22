using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts.Wpf;
using LiveCharts;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Data;
using System.Data.SQLite;
using System.IO;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf.Charts.Base;

namespace AppAiDatabaseChart
{
    public partial class MainWindow : Window
    {
        private Kernel _kernel;
        private ChatHistory _chatHistory;
        private IChatCompletionService _chatCompletionService;

        public MainWindow()
        {
            InitializeComponent();
            InitializeAI();
            AddSystemMessage("🤖 SCADA智能AI数据助手已启动！我可以帮您查询和分析数据库信息，并生成可视化图表。");
        }

        /// <summary>
        /// 初始化AI服务
        /// </summary>
        private void InitializeAI()
        {
            try
            {
                // 初始化Kernel
                var kernelBuilder = Kernel.CreateBuilder();
                kernelBuilder.AddOpenAIChatCompletion(
                    modelId: "deepseek-chat",
                    apiKey: Environment.GetEnvironmentVariable("DEEPSEEK_API_KEY") ?? "sk-xxxx",
                    endpoint: new Uri("https://api.deepseek.com/v1")
                );
                _kernel = kernelBuilder.Build();

                // 注册数据库查询插件
                RegisterDatabaseQueryPlugin(_kernel);

                // 注册图表生成插件
                RegisterChartPlugin(_kernel);

                // 初始化聊天历史
                _chatHistory = new ChatHistory();
                _chatHistory.AddSystemMessage(@"你是一个智能SCADA数据助手，具备数据查询、分析和可视化能力。
你可以帮助用户查询设备信息、测点数据、历史数据、报警信息等，并生成相应的图表。
当用户的查询涉及数值数据、统计分析、趋势分析时，主动建议或生成图表可视化。
请根据用户需求选择合适的工具来协助用户，并提供友好、专业的回复。");

                // 获取聊天完成服务
                _chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

                StatusText.Text = "AI助手已准备就绪";
            }
            catch (Exception ex)
            {
                AddSystemMessage($"❌ AI初始化失败: {ex.Message}");
                StatusText.Text = "AI初始化失败";
            }
        }

        /// <summary>
        /// 注册图表生成插件
        /// </summary>
        private void RegisterChartPlugin(Kernel kernel)
        {
            // 1. 生成柱状图
            var createBarChartFunction = kernel.CreateFunctionFromMethod(
                method: (string title, string data, string xLabel, string yLabel) =>
                {
                    try
                    {
                        CreateBarChart(title, data, xLabel, yLabel);
                        return $"✅ 已生成柱状图: {title}";
                    }
                    catch (Exception ex)
                    {
                        return $"❌ 生成柱状图失败: {ex.Message}";
                    }
                },
                functionName: "CreateBarChart",
                description: "创建柱状图。参数：title(图表标题), data(数据，格式：标签1:值1,标签2:值2), xLabel(X轴标签), yLabel(Y轴标签)"
            );

            // 2. 生成折线图
            var createLineChartFunction = kernel.CreateFunctionFromMethod(
                method: (string title, string data, string xLabel, string yLabel) =>
                {
                    try
                    {
                        CreateLineChart(title, data, xLabel, yLabel);
                        return $"✅ 已生成折线图: {title}";
                    }
                    catch (Exception ex)
                    {
                        return $"❌ 生成折线图失败: {ex.Message}";
                    }
                },
                functionName: "CreateLineChart",
                description: "创建折线图。参数：title(图表标题), data(数据，格式：标签1:值1,标签2:值2), xLabel(X轴标签), yLabel(Y轴标签)"
            );

            // 3. 生成饼图
            var createPieChartFunction = kernel.CreateFunctionFromMethod(
                method: (string title, string data) =>
                {
                    try
                    {
                        CreatePieChart(title, data);
                        return $"✅ 已生成饼图: {title}";
                    }
                    catch (Exception ex)
                    {
                        return $"❌ 生成饼图失败: {ex.Message}";
                    }
                },
                functionName: "CreatePieChart",
                description: "创建饼图。参数：title(图表标题), data(数据，格式：标签1:值1,标签2:值2)"
            );

            // 4. 智能图表建议
            var suggestChartFunction = kernel.CreateFunctionFromMethod(
                method: (string queryResult, string queryType) =>
                {
                    return SuggestChartType(queryResult, queryType);
                },
                functionName: "SuggestChart",
                description: "根据查询结果建议合适的图表类型和数据格式"
            );

            kernel.ImportPluginFromFunctions("ChartPlugin", [createBarChartFunction, createLineChartFunction, createPieChartFunction, suggestChartFunction]);
        }

        /// <summary>
        /// 建议图表类型
        /// </summary>
        private string SuggestChartType(string queryResult, string queryType)
        {
            var suggestion = new StringBuilder();
            suggestion.AppendLine("📊 图表建议：");

            if (queryResult.Contains("COUNT") || queryResult.Contains("统计") || queryResult.Contains("数量"))
            {
                suggestion.AppendLine("- 推荐使用柱状图显示统计数据");
                suggestion.AppendLine("- 可以使用饼图显示比例关系");
            }

            if (queryResult.Contains("时间") || queryResult.Contains("Timestamp") || queryResult.Contains("趋势"))
            {
                suggestion.AppendLine("- 推荐使用折线图显示时间趋势");
            }

            if (queryResult.Contains("类型") || queryResult.Contains("分组") || queryResult.Contains("GROUP BY"))
            {
                suggestion.AppendLine("- 推荐使用饼图显示分类分布");
            }

            suggestion.AppendLine("\n如需生成图表，请告诉我您希望显示哪些数据！");
            return suggestion.ToString();
        }

        /// <summary>
        /// 创建柱状图
        /// </summary>
        private void CreateBarChart(string title, string data, string xLabel, string yLabel)
        {
            Dispatcher.Invoke(() =>
            {
                var chart = new CartesianChart
                {
                    Height = 300,
                    Margin = new Thickness(0, 10, 0, 10)
                };

                var series = new ColumnSeries
                {
                    Title = title,
                    Values = new ChartValues<double>()
                };

                var labels = new List<string>();

                // 解析数据 (格式: 标签1:值1,标签2:值2)
                var dataItems = data.Split(',');
                foreach (var item in dataItems)
                {
                    var parts = item.Split(':');
                    if (parts.Length == 2)
                    {
                        labels.Add(parts[0].Trim());
                        if (double.TryParse(parts[1].Trim(), out double value))
                        {
                            series.Values.Add(value);
                        }
                    }
                }

                chart.Series = new SeriesCollection { series };
                chart.AxisX.Add(new Axis
                {
                    Title = xLabel,
                    Labels = labels
                });
                chart.AxisY.Add(new Axis
                {
                    Title = yLabel
                });

                AddChartToPanel(title, chart);
            });
        }

        /// <summary>
        /// 创建折线图
        /// </summary>
        private void CreateLineChart(string title, string data, string xLabel, string yLabel)
        {
            Dispatcher.Invoke(() =>
            {
                var chart = new CartesianChart
                {
                    Height = 300,
                    Margin = new Thickness(0, 10, 0, 10)
                };

                var series = new LineSeries
                {
                    Title = title,
                    Values = new ChartValues<double>(),
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 8
                };

                var labels = new List<string>();

                // 解析数据
                var dataItems = data.Split(',');
                foreach (var item in dataItems)
                {
                    var parts = item.Split(':');
                    if (parts.Length == 2)
                    {
                        labels.Add(parts[0].Trim());
                        if (double.TryParse(parts[1].Trim(), out double value))
                        {
                            series.Values.Add(value);
                        }
                    }
                }

                chart.Series = new SeriesCollection { series };
                chart.AxisX.Add(new Axis
                {
                    Title = xLabel,
                    Labels = labels
                });
                chart.AxisY.Add(new Axis
                {
                    Title = yLabel
                });

                AddChartToPanel(title, chart);
            });
        }

        /// <summary>
        /// 创建饼图
        /// </summary>
        private void CreatePieChart(string title, string data)
        {
            Dispatcher.Invoke(() =>
            {
                var chart = new PieChart
                {
                    Height = 300,
                    Margin = new Thickness(0, 10, 0, 10),
                    LegendLocation = LegendLocation.Right
                };

                var series = new SeriesCollection();

                // 解析数据
                var dataItems = data.Split(',');
                foreach (var item in dataItems)
                {
                    var parts = item.Split(':');
                    if (parts.Length == 2)
                    {
                        var label = parts[0].Trim();
                        if (double.TryParse(parts[1].Trim(), out double value))
                        {
                            series.Add(new PieSeries
                            {
                                Title = label,
                                Values = new ChartValues<double> { value },
                                DataLabels = true
                            });
                        }
                    }
                }

                chart.Series = series;
                AddChartToPanel(title, chart);
            });
        }

        /// <summary>
        /// 将图表添加到面板
        /// </summary>
        private void AddChartToPanel(string title, FrameworkElement chart)
        {
            var container = new StackPanel();

            // 图表标题
            var titleBlock = new TextBlock
            {
                Text = $"📊 {title}",
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            container.Children.Add(titleBlock);
            container.Children.Add(chart);

            // 添加关闭按钮
            var closeButton = new Button
            {
                Content = "❌",
                Width = 30,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, -10, 0, 0),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand
            };

            var border = new Border
            {
                Style = (Style)FindResource("ChartContainer"),
                Child = container
            };

            closeButton.Click += (s, e) => ChartPanel.Children.Remove(border);
            container.Children.Add(closeButton);

            ChartPanel.Children.Add(border);

            // 更新图表标题
            ChartTitleText.Text = $"📈 数据可视化 ({ChartPanel.Children.Count})";
        }

        /// <summary>
        /// 注册数据库查询插件
        /// </summary>
        private void RegisterDatabaseQueryPlugin(Kernel kernel)
        {
            var getSchemaFunction = kernel.CreateFunctionFromMethod(
                method: () =>
                {
                    try
                    {
                        var dbFilePath = "test.db";
                        if (!File.Exists(dbFilePath))
                            return "数据库文件不存在，请先初始化数据库。";

                        using var connection = new SQLiteConnection($"Data Source={dbFilePath};Version=3;");
                        connection.Open();

                        var schema = GetDatabaseSchema(connection);
                        return schema;
                    }
                    catch (Exception ex)
                    {
                        return $"获取数据库结构失败: {ex.Message}";
                    }
                },
                functionName: "GetDatabaseSchema",
                description: "获取SCADA数据库的完整结构信息，包括所有表、字段、数据类型等"
            );

            var executeQueryFunction = kernel.CreateFunctionFromMethod(
                method: (string sql) =>
                {
                    try
                    {
                        var dbFilePath = "test.db";
                        if (!File.Exists(dbFilePath))
                            return "数据库文件不存在，请先初始化数据库。";

                        using var connection = new SQLiteConnection($"Data Source={dbFilePath};Version=3;");
                        connection.Open();

                        if (!sql.Trim().ToUpper().StartsWith("SELECT"))
                        {
                            return "⚠️ 出于安全考虑，只允许执行SELECT查询语句。";
                        }

                        using var command = new SQLiteCommand(sql, connection);
                        using var reader = command.ExecuteReader();

                        var dt = new DataTable();
                        dt.Load(reader);

                        if (dt.Rows.Count == 0)
                            return "查询结果为空。";

                        var result = FormatQueryResult(dt);

                        // 检查是否适合生成图表
                        if (ShouldSuggestChart(dt, sql))
                        {
                            result += "\n\n💡 这些数据很适合用图表展示！您可以说'生成图表'来可视化这些数据。";
                        }

                        return result;
                    }
                    catch (Exception ex)
                    {
                        return $"SQL执行出错: {ex.Message}";
                    }
                },
                functionName: "ExecuteSQL",
                description: "执行SQL查询语句并返回格式化的SCADA数据结果，如果适合会建议生成图表"
            );

            kernel.ImportPluginFromFunctions("DatabasePlugin", [getSchemaFunction, executeQueryFunction]);
        }

        /// <summary>
        /// 判断是否应该建议生成图表
        /// </summary>
        private bool ShouldSuggestChart(DataTable dt, string sql)
        {
            // 如果结果有数值列且行数适中，建议生成图表
            if (dt.Rows.Count > 1 && dt.Rows.Count <= 20)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.DataType == typeof(int) || col.DataType == typeof(double) || col.DataType == typeof(decimal))
                    {
                        return true;
                    }
                }
            }

            // 如果SQL包含聚合函数，也建议生成图表
            var upperSql = sql.ToUpper();
            if (upperSql.Contains("COUNT") || upperSql.Contains("SUM") || upperSql.Contains("AVG") || upperSql.Contains("GROUP BY"))
            {
                return true;
            }

            return false;
        }

        // 输入框键盘事件
        /// <summary>
        /// 输入框键盘事件
        /// </summary>
        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                SendMessage();
                e.Handled = true;
            }
        }

        private void InitDbButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InitDbButton.IsEnabled = false;
                StatusText.Text = "正在初始化数据库...";

                AddSystemMessage("🔧 开始初始化SCADA数据库...");

                Task.Run(() =>
                {
                    DatabaseInitializer.InitializeScadaDatabase();

                    Dispatcher.Invoke(() =>
                    {
                        AddSystemMessage("✅ SCADA数据库初始化完成！已创建1000条测试记录。\n💡 您现在可以询问：\n- 显示所有设备类型的统计\n- 查询最近的报警趋势\n- 生成设备状态分布图");
                        StatusText.Text = "数据库初始化完成";
                        InitDbButton.IsEnabled = true;
                    });
                });
            }
            catch (Exception ex)
            {
                AddSystemMessage($"❌ 数据库初始化失败: {ex.Message}");
                StatusText.Text = "数据库初始化失败";
                InitDbButton.IsEnabled = true;
            }
        }

        private void ShowStatsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!File.Exists("test.db"))
                {
                    AddSystemMessage("❌ 数据库文件不存在，请先初始化数据库。");
                    return;
                }

                var stats = GetDatabaseStatistics();
                AddSystemMessage(stats);
            }
            catch (Exception ex)
            {
                AddSystemMessage($"❌ 获取统计信息失败: {ex.Message}");
            }
        }

        private void ClearChatButton_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Children.Clear();
            ChartPanel.Children.Clear();
            _chatHistory?.Clear();
            _chatHistory?.AddSystemMessage(@"你是一个智能SCADA数据助手，具备数据查询、分析和可视化能力。
你可以帮助用户查询设备信息、测点数据、历史数据、报警信息等，并生成相应的图表。
当用户的查询涉及数值数据、统计分析、趋势分析时，主动建议或生成图表可视化。
请根据用户需求选择合适的工具来协助用户，并提供友好、专业的回复。");

            AddSystemMessage("🗑️ 对话和图表已清空，您可以开始新的对话。");
            ChartTitleText.Text = "📈 数据可视化";
        }

        private async void SendMessage()
        {
            var userInput = InputTextBox.Text.Trim();
            if (string.IsNullOrEmpty(userInput)) return;

            InputTextBox.Text = string.Empty;
            AddUserMessage(userInput);

            SendButton.IsEnabled = false;
            StatusText.Text = "AI正在思考...";

            try
            {
                _chatHistory.AddUserMessage(userInput);
                await ProcessAIResponse();
            }
            catch (Exception ex)
            {
                AddSystemMessage($"❌ 发生错误: {ex.Message}");
            }
            finally
            {
                SendButton.IsEnabled = true;
                StatusText.Text = "准备就绪";
            }
        }

        private async Task ProcessAIResponse()
        {
            var executionSettings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                MaxTokens = 2000,
                Temperature = 0.7
            };

            var responseTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                FontSize = 12,
                Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51))
            };

            var responseBorder = new Border
            {
                Style = (Style)FindResource("AssistantMessage"),
                Child = responseTextBlock
            };

            ChatPanel.Children.Add(responseBorder);
            ScrollToBottom();

            try
            {
                var streamResponse = _chatCompletionService.GetStreamingChatMessageContentsAsync(
                    chatHistory: _chatHistory,
                    executionSettings: executionSettings,
                    kernel: _kernel
                );

                string fullResponse = "";

                await foreach (var content in streamResponse)
                {
                    if (!string.IsNullOrEmpty(content.Content))
                    {
                        fullResponse += content.Content;

                        await Dispatcher.InvokeAsync(() =>
                        {
                            responseTextBlock.Text = fullResponse;
                            ScrollToBottom();
                        });

                        await Task.Delay(30);
                    }
                }

                if (!string.IsNullOrWhiteSpace(fullResponse))
                {
                    _chatHistory.AddAssistantMessage(fullResponse);
                }
            }
            catch (Exception ex)
            {
                responseTextBlock.Text = $"❌ AI响应失败: {ex.Message}";
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void AddUserMessage(string message)
        {
            var textBlock = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 12,
                Foreground = Brushes.White
            };

            var border = new Border
            {
                Style = (Style)FindResource("UserMessage"),
                Child = textBlock
            };

            ChatPanel.Children.Add(border);
            ScrollToBottom();
        }

        private void AddSystemMessage(string message)
        {
            var textBlock = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromRgb(133, 77, 14))
            };

            var border = new Border
            {
                Style = (Style)FindResource("SystemMessage"),
                Child = textBlock
            };

            ChatPanel.Children.Add(border);
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            ChatScrollViewer.ScrollToEnd();
        }

        private string GetDatabaseStatistics()
        {
            var stats = new StringBuilder();
            stats.AppendLine("📊 SCADA数据库统计信息");
            stats.AppendLine("========================");

            using var connection = new SQLiteConnection($"Data Source=test.db;Version=3;");
            connection.Open();

            var tables = new[] { "Devices", "Tags", "RealTimeData", "HistoricalData", "Alarms" };
            var displayNames = new[] { "设备", "测点", "实时数据", "历史数据", "报警" };

            for (int i = 0; i < tables.Length; i++)
            {
                var count = GetTableRecordCount(connection, tables[i]);
                stats.AppendLine($"📋 {displayNames[i]}: {count} 条记录");
            }

            return stats.ToString();
        }

        private static string GetDatabaseSchema(SQLiteConnection connection)
        {
            var schema = new StringBuilder();
            schema.AppendLine("📊 SCADA数据库结构信息");
            schema.AppendLine("=" + new string('=', 50));

            var tables = GetTableNames(connection);

            foreach (var tableName in tables)
            {
                schema.AppendLine($"\n🔹 表名: {tableName}");
                schema.AppendLine($"   字段信息:");

                var tableInfo = GetTableStructure(connection, tableName);
                foreach (var column in tableInfo)
                {
                    schema.AppendLine($"   - {column.Name} ({column.Type}) {(column.IsPrimaryKey ? "[主键]" : "")} {(column.IsNotNull ? "[非空]" : "")}");
                }

                var count = GetTableRecordCount(connection, tableName);
                schema.AppendLine($"   📋 记录数: {count}");
            }

            schema.AppendLine($"\n💡 图表生成示例:");
            schema.AppendLine($"   - 生成设备类型统计图");
            schema.AppendLine($"   - 显示报警级别分布饼图");
            schema.AppendLine($"   - 创建设备状态趋势图");

            return schema.ToString();
        }

        private static List<string> GetTableNames(SQLiteConnection connection)
        {
            var tables = new List<string>();
            string sql = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%'";

            using var command = new SQLiteCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tables.Add(reader.GetString(0));
            }

            return tables;
        }

        private static List<ColumnInfo> GetTableStructure(SQLiteConnection connection, string tableName)
        {
            var columns = new List<ColumnInfo>();
            string sql = $"PRAGMA table_info({tableName})";

            using var command = new SQLiteCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                columns.Add(new ColumnInfo
                {
                    Name = reader.GetString("name"),
                    Type = reader.GetString("type"),
                    IsNotNull = reader.GetBoolean("notnull"),
                    IsPrimaryKey = reader.GetBoolean("pk")
                });
            }

            return columns;
        }

        private static int GetTableRecordCount(SQLiteConnection connection, string tableName)
        {
            string sql = $"SELECT COUNT(*) FROM {tableName}";
            using var command = new SQLiteCommand(sql, connection);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        private static string FormatQueryResult(DataTable dt)
        {
            var result = new StringBuilder();
            result.AppendLine($"📋 查询结果 (共 {dt.Rows.Count} 条记录):");
            result.AppendLine(new string('=', 60));

            foreach (DataColumn col in dt.Columns)
            {
                result.Append($"{col.ColumnName,-15} | ");
            }
            result.AppendLine();
            result.AppendLine(new string('-', 60));

            int maxRows = Math.Min(20, dt.Rows.Count);
            for (int i = 0; i < maxRows; i++)
            {
                foreach (var item in dt.Rows[i].ItemArray)
                {
                    string value = item?.ToString() ?? "NULL";
                    if (value.Length > 12) value = value.Substring(0, 12) + "...";
                    result.Append($"{value,-15} | ");
                }
                result.AppendLine();
            }

            if (dt.Rows.Count > 20)
            {
                result.AppendLine($"... 还有 {dt.Rows.Count - 20} 条记录未显示");
            }

            return result.ToString();
        }
    }
}