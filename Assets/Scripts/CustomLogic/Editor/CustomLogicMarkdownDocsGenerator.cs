using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomLogic.Editor.Models;

namespace CustomLogic.Editor
{
    class CustomLogicMarkdownDocsGenerator : BaseCustomLogicDocsGenerator
    {
        private readonly StringBuilder _sb = new();
        private readonly List<string> _propertiesHeaders = new() { "Name", "Type", "Readonly", "Description" };

        private readonly Dictionary<string, CLType> _typeNameMap;
        private readonly Dictionary<CLType, int> _typeIndexMap;

        public CustomLogicMarkdownDocsGenerator(CLType[] allTypes) : base(allTypes)
        {
            var i = 0;
            _typeNameMap = new Dictionary<string, CLType>();
            _typeIndexMap = new Dictionary<CLType, int>();
            foreach (var type in allTypes)
            {
                _typeNameMap[type.Name] = type;
                _typeIndexMap[type] = i++;
            }
        }

        public override string GetRelativeFilePath(CLType type)
        {
            if (type.IsStatic && type.IsAbstract)
                return $"md/static/{type.Name}.md";

            return $"md/objects/{type.Name}.md";
        }

        public string GetRelativeRefPath(CLType type)
        {
            return GetRelativeFilePath(type)[3..];
        }

        public override string Generate(CLType type)
        {
            var hasInstanceMethods = type.InstanceMethods != null && type.InstanceMethods.Length > 0;

            _sb.Clear();

            _sb.AppendLine($"# {type.Name}");
            if (type.InheritBaseMembers && type.BaseType != null)
            {
                _sb.AppendLine($"Inherits from [{type.BaseTypeName}](../{GetRelativeRefPath(type.BaseType)})");
            }

            if (string.IsNullOrEmpty(type.Info.Summary) == false)
            {
                _sb.AppendLine();
                _sb.AppendLine(TrimAndCleanLines(type.Info.Summary));
                _sb.AppendLine();
            }

            var hasRemarks = string.IsNullOrEmpty(type.Info.Remarks) == false;
            var overloadsOperators = hasInstanceMethods && type.InstanceMethods.Any(x => x.Name.StartsWith("__"));

            if (hasRemarks || overloadsOperators)
            {
                _sb.AppendLine("### Remarks");

                if (hasRemarks)
                {
                    _sb.AppendLine(TrimAndCleanLines(type.Info.Remarks));
                    _sb.AppendLine();
                }

                if (overloadsOperators)
                {
                    _sb.AppendLine("Overloads operators: ");
                    if (type.InstanceMethods.Any(x => x.Name.StartsWith("__")))
                    {
                        string str = "";
                        foreach (var method in type.InstanceMethods.Where(x => x.Name.StartsWith("__")))
                        {
                            if (method.Name == nameof(ICustomLogicMathOperators.__Add__)) str += $"`+`, ";
                            if (method.Name == nameof(ICustomLogicMathOperators.__Sub__)) str += $"`-`, ";
                            if (method.Name == nameof(ICustomLogicMathOperators.__Mul__)) str += $"`*`, ";
                            if (method.Name == nameof(ICustomLogicMathOperators.__Div__)) str += $"`/`, ";
                            if (method.Name == nameof(ICustomLogicEquals.__Eq__)) str += $"`==`, ";

                            if (method.Name == nameof(ICustomLogicCopyable.__Copy__)) str += $"`{method.Name}`, ";

                            if (method.Name == nameof(ICustomLogicEquals.__Hash__)) str += $"`{method.Name}`, ";
                            if (method.Name == nameof(ICustomLogicToString.__Str__)) str += $"`{method.Name}`, ";
                        }
                        str = str[..^2]; // trim ending ', '
                        _sb.AppendLine(str);
                    }
                }
            }

            if (string.IsNullOrEmpty(type.Info.Code) == false)
            {
                _sb.AppendLine("### Example");
                _sb.AppendLine("```csharp");
                _sb.AppendLine(TrimAndCleanLines(type.Info.Code, true));
                _sb.AppendLine("```");
            }

            if (type.IsAbstract == false && (type.IsStatic == false || type.Constructors != null && type.Constructors.Length > 0))
            {
                _sb.AppendLine("### Initialization");
                _sb.AppendLine("```csharp");
                foreach (var ctor in type.Constructors)
                {
                    var parameters = GetParametersStr(ctor.Parameters, TypeLinkKind.None);
                    var signature = $"{type.Name}({parameters})";
                    var comment = string.Empty;
                    if (string.IsNullOrEmpty(ctor.Info.Summary) == false)
                    {
                        // should be # instead of // but that will break highlighting
                        comment = $" // {TrimAndCleanLines(ctor.Info.Summary)}";
                    }
                    _sb.AppendLine($"{signature}{comment}");
                }
                _sb.AppendLine("```");
                _sb.AppendLine();
            }

            if (type.InstanceProperties != null && type.InstanceProperties.Length > 0)
            {
                _sb.AppendLine("### Properties");
                AppendProperties(type.InstanceProperties);
                _sb.AppendLine();
            }

            if (type.StaticProperties != null && type.StaticProperties.Length > 0)
            {
                _sb.AppendLine("### Static Properties");
                AppendProperties(type.StaticProperties);
                _sb.AppendLine();
            }

            if (type.InstanceMethods != null && type.InstanceMethods.Length > 0)
            {
                _sb.AppendLine("### Methods");
                AppendMethods(type.InstanceMethods);
                _sb.AppendLine();
            }

            if (type.StaticMethods != null && type.StaticMethods.Length > 0)
            {
                _sb.AppendLine("### Static Methods");
                AppendMethods(type.StaticMethods);
                _sb.AppendLine();
            }

            foreach (var (t, i) in _typeIndexMap)
            {
                var n = t.Name;
                var path = GetRelativeRefPath(t);
                _sb.AppendLine($"[^{i}]: [{n}](../{path})");
            }

            return _sb.ToString();
        }

        private void AppendProperties(CLProperty[] properties)
        {
            var rows = new List<List<string>>();
            foreach (var property in properties)
            {
                var row = new List<string>
                    {
                        property.Name,
                        GetTypeReferenceStr(property.Type, TypeLinkKind.Absolute),
                        property.IsReadonly.ToString(),
                        TrimAndCleanLines(property.Info.Summary).Replace("\r\n", " ").Replace('\n', ' ').Replace('\t', ' ')
                    };

                rows.Add(row);
            }
            _sb.AppendLine(CreateTable(_propertiesHeaders, rows));
        }

        private void AppendMethods(CLMethod[] methods)
        {
            foreach (var method in methods)
            {
                // ignore operators
                if (method.Name.StartsWith("__")) continue;

                string parameters = GetParametersStr(method.Parameters, TypeLinkKind.Footnote);
                string returnStr = GetTypeReferenceStr(method.ReturnType, TypeLinkKind.Footnote);

                if (string.IsNullOrEmpty(returnStr) || returnStr == "null")
                    returnStr = string.Empty;
                else
                    returnStr = $" -> {returnStr}";

                _sb.Append("<pre class=\"language-typescript\"><code class=\"lang-typescript\">function ");
                _sb.Append($"{method.Name}({parameters}){returnStr}");
                _sb.AppendLine("</code></pre>");

                if (method.IsObsolete)
                {
                    _sb.AppendLine();
                    _sb.AppendLine("{% hint style=\"warning\" %}");
                    _sb.AppendLine($"**Obsolete**: {method.ObsoleteMessage}");
                    _sb.AppendLine("{% endhint %}");
                    _sb.AppendLine();
                }

                if (string.IsNullOrEmpty(method.Info.Summary) == false)
                {
                    _sb.AppendLine($"> {TrimAndCleanLines(method.Info.Summary)}");
                    _sb.AppendLine("> ");

                    if (string.IsNullOrEmpty(method.Info.Remarks) == false)
                    {
                        _sb.AppendLine($"> **Remarks**: {TrimAndCleanLines(method.Info.Remarks)}");
                        _sb.AppendLine("> ");
                    }

                    var hasParameterDocs = method.Parameters != null && method.Parameters.Any(p => p.Info != null && !string.IsNullOrEmpty(p.Info.Summary));
                    if (hasParameterDocs)
                    {
                        _sb.AppendLine("> **Parameters**:");
                        foreach (var parameter in method.Parameters)
                        {
                            if (parameter.Info == null || string.IsNullOrEmpty(parameter.Info.Summary))
                                continue;
                            _sb.AppendLine($"> - `{parameter.Name}`: {TrimAndCleanLines(parameter.Info.Summary)}");
                        }
                        _sb.AppendLine("> ");
                    }

                    if (string.IsNullOrEmpty(method.Info.Returns) == false)
                    {
                        _sb.AppendLine($"> **Returns**: {TrimAndCleanLines(method.Info.Returns)}");
                    }
                }
            }
        }

        private enum TypeLinkKind
        {
            None,
            Absolute,
            Footnote,
        }

        private string GetParametersStr(CLParameter[] parameters, TypeLinkKind linkKind)
        {
            if (parameters == null || parameters.Length == 0)
                return string.Empty;

            var parametersStr = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var p = parameters[i];
                var defaultValue = p.IsOptional ? $" = {p.DefaultValue}" : string.Empty;
                parametersStr[i] = $"{p.Name}: {GetTypeReferenceStr(p.Type, linkKind)}{defaultValue}";
                // if (p.IsOptional)
                // {
                //     parametersStr[i] = $"[{parametersStr[i]}]";
                // }
            }

            return string.Join(", ", parametersStr);
        }

        private string GetTypeReferenceStr(TypeReference typeReference, TypeLinkKind linkKind)
        {
            var name = typeReference.Name;

            if (_typeNameMap.ContainsKey(name))
            {
                if (linkKind == TypeLinkKind.Absolute)
                    name = $"[{name}](../{GetRelativeRefPath(_typeNameMap[name])})";
                else if (linkKind == TypeLinkKind.Footnote)
                {
                    var type = _typeNameMap[name];
                    var i = _typeIndexMap[type];
                    name = $"<a data-footnote-ref href=\"#user-content-fn-{i}\">{name}</a>";
                }
            }

            if (typeReference.Arguments != null && typeReference.Arguments.Length > 0)
                return $"{name}<{string.Join(",", typeReference.Arguments.Select(x => GetTypeReferenceStr(x, linkKind)))}>";

            return name;
        }

        private static string CreateTable(List<string> headers, List<List<string>> rows)
        {
            var sb = new StringBuilder();

            sb.Append('|');
            foreach (var header in headers)
            {
                sb.Append($"{header}|");
            }
            sb.AppendLine();
            sb.Append('|');
            for (int i = 0; i < headers.Count; i++)
            {
                sb.Append("---|");
            }
            sb.AppendLine();
            foreach (var row in rows)
            {
                sb.Append("|");
                foreach (var cell in row)
                {
                    sb.Append($"{cell}|");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private static string TrimAndCleanLines(string val, bool isCodeBlock = false)
        {
            var lines = val.Split('\n');
            int start = 0;
            int end = lines.Length - 1;

            // Remove empty lines from the beginning
            while (start <= end && string.IsNullOrWhiteSpace(lines[start]))
                start++;

            // Remove empty lines from the end
            while (end >= start && string.IsNullOrWhiteSpace(lines[end]))
                end--;

            if (isCodeBlock)
            {
                // Find minimum indent (ignoring empty lines)
                int minIndent = int.MaxValue;
                for (int i = start; i <= end; i++)
                {
                    var line = lines[i];
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    int indent = line.TakeWhile(char.IsWhiteSpace).Count();
                    if (indent < minIndent) minIndent = indent;
                }
                if (minIndent == int.MaxValue) minIndent = 0;

                var sb = new StringBuilder();
                for (int i = start; i <= end; i++)
                {
                    var line = lines[i];
                    if (line.Length >= minIndent)
                        sb.AppendLine(line[minIndent..].TrimEnd('\r', '\n'));
                    else
                        sb.AppendLine(line.TrimEnd('\r', '\n'));
                }
                return sb.ToString().TrimEnd('\n', '\r');
            }
            else
            {
                var sb = new StringBuilder();
                for (int i = start; i <= end; i++)
                {
                    sb.AppendLine(lines[i].Trim());
                }
                return sb.ToString().TrimEnd('\n', '\r');
            }
        }
    }
}
