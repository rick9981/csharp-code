using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using ConfigurationLib.SourceGenerators.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ConfigurationLib.SourceGenerators
{
    [Generator]
    public class ConfigurationGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // 查找标记了ConfigurationAttribute的类
            var configClasses = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null);

            // 合并所有配置类信息
            var compilation = context.CompilationProvider.Combine(configClasses.Collect());

            // 生成代码
            context.RegisterSourceOutput(compilation, static (spc, source) => Execute(source.Left, source.Right!, spc));
        }

        private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        {
            return node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };
        }

        private static ConfigurationClassInfo? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

            if (classSymbol == null)
                return null;

            var configAttribute = classSymbol.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.Name == "ConfigurationAttribute");

            if (configAttribute == null)
                return null;

            var fileName = configAttribute.NamedArguments
                .FirstOrDefault(arg => arg.Key == "FileName").Value.Value?.ToString();
            var section = configAttribute.NamedArguments
                .FirstOrDefault(arg => arg.Key == "Section").Value.Value?.ToString();
            var required = configAttribute.NamedArguments
                .FirstOrDefault(arg => arg.Key == "Required").Value.Value as bool? ?? true;

            var properties = classSymbol.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.CanBeReferencedByName && p.SetMethod != null)
                .Select(p => new ConfigurationPropertyInfo
                {
                    Name = p.Name,
                    Type = p.Type.ToDisplayString(),
                    ConfigKey = GetConfigKey(p),
                    DefaultValue = GetDefaultValue(p),
                    IsRequired = GetIsRequired(p)
                })
                .ToList();

            return new ConfigurationClassInfo
            {
                ClassName = classSymbol.Name,
                Namespace = classSymbol.ContainingNamespace.ToDisplayString(),
                FileName = fileName,
                Section = section,
                Required = required,
                Properties = properties
            };
        }

        private static string GetConfigKey(IPropertySymbol property)
        {
            var configPropAttribute = property.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.Name == "ConfigPropertyAttribute");

            var key = configPropAttribute?.NamedArguments
                .FirstOrDefault(arg => arg.Key == "Key").Value.Value?.ToString();

            return key ?? property.Name;
        }

        private static object? GetDefaultValue(IPropertySymbol property)
        {
            var configPropAttribute = property.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.Name == "ConfigPropertyAttribute");

            return configPropAttribute?.NamedArguments
                .FirstOrDefault(arg => arg.Key == "DefaultValue").Value.Value;
        }

        private static bool GetIsRequired(IPropertySymbol property)
        {
            var configPropAttribute = property.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.Name == "ConfigPropertyAttribute");

            return configPropAttribute?.NamedArguments
                .FirstOrDefault(arg => arg.Key == "Required").Value.Value as bool? ?? false;
        }

        private static void Execute(Compilation compilation, ImmutableArray<ConfigurationClassInfo> configClasses, SourceProductionContext context)
        {
            if (configClasses.IsDefaultOrEmpty)
                return;

            var sourceBuilder = new StringBuilder();

            // 生成头部
            sourceBuilder.AppendLine("using System;");
            sourceBuilder.AppendLine("using System.Collections.Generic;");
            sourceBuilder.AppendLine("using System.IO;");
            sourceBuilder.AppendLine("using System.Text.Json;");
            sourceBuilder.AppendLine("using ConfigurationLib;");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("namespace ConfigurationLib.Generated");
            sourceBuilder.AppendLine("{");

            // 生成ConfigurationManager类
            sourceBuilder.Append(GenerateConfigurationManager(configClasses));

            // 生成每个配置类的加载器
            foreach (var configClass in configClasses)
            {
                sourceBuilder.Append(GenerateConfigurationLoader(configClass));
            }

            // 结束类和命名空间
            sourceBuilder.AppendLine("    }"); // 结束 GeneratedConfigurationManager 类
            sourceBuilder.AppendLine("}"); // 结束 namespace

            context.AddSource("GeneratedConfigurationManager.g.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        private static string GenerateConfigurationManager(ImmutableArray<ConfigurationClassInfo> configClasses)
        {
            var sb = new StringBuilder();

            sb.AppendLine("    public class GeneratedConfigurationManager : IConfigurationManager");
            sb.AppendLine("    {");
            sb.AppendLine("        private readonly Dictionary<Type, object> _configInstances = new();");
            sb.AppendLine("        private readonly Dictionary<string, JsonDocument> _configFiles = new();");
            sb.AppendLine();
            sb.AppendLine("        public void LoadConfigurations(string configDirectory = \"config\")");
            sb.AppendLine("        {");
            sb.AppendLine("            LoadConfigFiles(configDirectory);");
            sb.AppendLine();

            // 为每个配置类生成加载代码
            foreach (var configClass in configClasses)
            {
                sb.AppendLine($"            Load{configClass.ClassName}Configuration();");
            }

            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        private void LoadConfigFiles(string configDirectory)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (!Directory.Exists(configDirectory))");
            sb.AppendLine("                return;");
            sb.AppendLine();
            sb.AppendLine("            var jsonFiles = Directory.GetFiles(configDirectory, \"*.json\", SearchOption.AllDirectories);");
            sb.AppendLine("            foreach (var filePath in jsonFiles)");
            sb.AppendLine("            {");
            sb.AppendLine("                try");
            sb.AppendLine("                {");
            sb.AppendLine("                    var fileName = Path.GetFileNameWithoutExtension(filePath);");
            sb.AppendLine("                    var jsonContent = File.ReadAllText(filePath);");
            sb.AppendLine("                    var jsonDocument = JsonDocument.Parse(jsonContent);");
            sb.AppendLine("                    _configFiles[fileName.ToLower()] = jsonDocument;");
            sb.AppendLine("                }");
            sb.AppendLine("                catch (Exception ex)");
            sb.AppendLine("                {");
            sb.AppendLine("                    Console.WriteLine($\"Failed to load config file {filePath}: {ex.Message}\");");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public T GetConfiguration<T>() where T : class, new()");
            sb.AppendLine("        {");
            sb.AppendLine("            return _configInstances.TryGetValue(typeof(T), out var instance) ? (T)instance : null;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public IEnumerable<object> GetAllConfigurations()");
            sb.AppendLine("        {");
            sb.AppendLine("            return _configInstances.Values;");
            sb.AppendLine("        }");
            sb.AppendLine();

            return sb.ToString();
        }

        private static string GenerateConfigurationLoader(ConfigurationClassInfo configClass)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"        private void Load{configClass.ClassName}Configuration()");
            sb.AppendLine("        {");
            sb.AppendLine($"            var config = new {configClass.Namespace}.{configClass.ClassName}();");
            sb.AppendLine();

            // 确定配置文件名
            var fileNameExpression = configClass.FileName != null
                ? $"\"{configClass.FileName.ToLower()}\""
                : $"\"{configClass.ClassName.ToLower()}\"";

            if (configClass.FileName == null)
            {
                sb.AppendLine($"            var fileName = {fileNameExpression};");
                sb.AppendLine("            if (fileName.EndsWith(\"config\"))");
                sb.AppendLine("                fileName = fileName.Replace(\"config\", \"\");");
            }
            else
            {
                sb.AppendLine($"            var fileName = {fileNameExpression};");
            }

            sb.AppendLine();
            sb.AppendLine("            if (!_configFiles.TryGetValue(fileName, out var jsonDocument))");
            sb.AppendLine("            {");
            if (configClass.Required)
            {
                sb.AppendLine("                throw new FileNotFoundException($\"Required configuration file not found: {fileName}.json\");");
            }
            else
            {
                sb.AppendLine("                _configInstances[typeof(" + configClass.Namespace + "." + configClass.ClassName + ")] = config;");
                sb.AppendLine("                return;");
            }
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            var rootElement = jsonDocument.RootElement;");

            // 处理Section
            if (!string.IsNullOrEmpty(configClass.Section))
            {
                sb.AppendLine($"            if (rootElement.TryGetProperty(\"{configClass.Section}\", out var sectionElement))");
                sb.AppendLine("                rootElement = sectionElement;");
                if (configClass.Required)
                {
                    sb.AppendLine("            else");
                    sb.AppendLine($"                throw new InvalidOperationException($\"Required section '{configClass.Section}' not found in {{fileName}}.json\");");
                }
            }

            sb.AppendLine();

            // 为每个属性生成赋值代码
            foreach (var property in configClass.Properties)
            {
                GeneratePropertyAssignment(sb, property);
            }

            sb.AppendLine();
            sb.AppendLine($"            _configInstances[typeof({configClass.Namespace}.{configClass.ClassName})] = config;");
            sb.AppendLine("        }");
            sb.AppendLine();

            return sb.ToString();
        }

        private static void GeneratePropertyAssignment(StringBuilder sb, ConfigurationPropertyInfo property)
        {
            sb.AppendLine($"            // Setting property: {property.Name}");
            sb.AppendLine($"            if (rootElement.TryGetProperty(\"{property.ConfigKey}\", out var {property.Name.ToLower()}Element))");
            sb.AppendLine("            {");
            sb.AppendLine("                try");
            sb.AppendLine("                {");

            // 根据属性类型生成不同的赋值代码
            GenerateTypeSpecificAssignment(sb, property);

            sb.AppendLine("                }");
            sb.AppendLine("                catch (Exception ex)");
            sb.AppendLine("                {");
            sb.AppendLine($"                    Console.WriteLine($\"Error setting property {property.Name}: {{ex.Message}}\");");
            sb.AppendLine("                }");
            sb.AppendLine("            }");

            // 处理默认值
            if (property.DefaultValue != null)
            {
                sb.AppendLine("            else");
                sb.AppendLine("            {");
                sb.AppendLine($"                config.{property.Name} = {FormatDefaultValue(property.DefaultValue, property.Type)};");
                sb.AppendLine("            }");
            }
            else if (property.IsRequired)
            {
                sb.AppendLine("            else");
                sb.AppendLine("            {");
                sb.AppendLine($"                throw new InvalidOperationException($\"Required property '{property.ConfigKey}' not found in configuration\");");
                sb.AppendLine("            }");
            }

            sb.AppendLine();
        }

        private static void GenerateTypeSpecificAssignment(StringBuilder sb, ConfigurationPropertyInfo property)
        {
            var elementVar = $"{property.Name.ToLower()}Element";

            // 改进类型判断逻辑
            var type = property.Type;
            var isNullable = type.EndsWith("?");
            var baseType = isNullable ? type.Substring(0, type.Length - 1) : type;

            switch (baseType)
            {
                case "string":
                    sb.AppendLine($"                    config.{property.Name} = {elementVar}.GetString();");
                    break;
                case "int":
                    sb.AppendLine($"                    config.{property.Name} = {elementVar}.GetInt32();");
                    break;
                case "long":
                    sb.AppendLine($"                    config.{property.Name} = {elementVar}.GetInt64();");
                    break;
                case "bool":
                    sb.AppendLine($"                    config.{property.Name} = {elementVar}.GetBoolean();");
                    break;
                case "double":
                    sb.AppendLine($"                    config.{property.Name} = {elementVar}.GetDouble();");
                    break;
                case "float":
                    sb.AppendLine($"                    config.{property.Name} = {elementVar}.GetSingle();");
                    break;
                case "decimal":
                    sb.AppendLine($"                    config.{property.Name} = {elementVar}.GetDecimal();");
                    break;
                case "System.DateTime":
                case "DateTime":
                    sb.AppendLine($"                    config.{property.Name} = {elementVar}.GetDateTime();");
                    break;
                case "System.Guid":
                case "Guid":
                    sb.AppendLine($"                    config.{property.Name} = {elementVar}.GetGuid();");
                    break;
                default:
                    // 对于复杂类型和数组，使用JsonSerializer
                    sb.AppendLine($"                    config.{property.Name} = JsonSerializer.Deserialize<{property.Type}>({elementVar}.GetRawText());");
                    break;
            }
        }

        private static string FormatDefaultValue(object defaultValue, string type)
        {
            if (defaultValue == null)
                return "null";

            if (type == "string")
                return $"\"{defaultValue}\"";

            if (type == "bool" || type == "bool?")
                return defaultValue.ToString()!.ToLower();

            if (type == "char")
                return $"'{defaultValue}'";

            return defaultValue.ToString()!;
        }
    }
}