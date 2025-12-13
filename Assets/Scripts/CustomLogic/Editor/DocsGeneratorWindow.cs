using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomLogic.Editor
{
#if UNITY_EDITOR
    public class DocsGeneratorWindow : EditorWindow
    {
        private bool _runBuildCommand = true;
        private bool _generateJson;
        private bool _generateMarkdown;
        private bool _generateVSCExtensionJson;
        private bool _resolveInheritDoc = true;

        private Button _generateButton;

        private ListView _logsView;
        private readonly List<string> _logs = new();
        private readonly ConcurrentQueue<string> _logsConcurrent = new();

        [MenuItem("AoTTG2/CustomLogic Docs Generator")]
        public static void Open()
        {
            var window = GetWindow(typeof(DocsGeneratorWindow));
            window.titleContent = new GUIContent("CustomLogic Docs Generator");
        }

        private void CreateGUI()
        {
            var root = rootVisualElement;

            var optionsContainer = new VisualElement();

            var runBuildCommandToggle = CreateOption("Run build command", "Run `dotnet build` before generating the docs which will generate Scripts.xml. Only enable this if this is the first time running docs generator (since unity startup) or the docs comments (xml) have changed.");
            var generateJsonToggle = CreateOption("Generate JSON", "Generate docs in JSON format");
            var generateMarkdownToggle = CreateOption("Generate Markdown", "Generate docs in Markdown format");
            var generateVSCExtensionJsonToggle = CreateOption("Generate VSCExtension JSON", "Generate docs in JSON format for VSCode Extension");
            var resolveInheritDocToggle = CreateOption("Resolve <inheritdoc />", "Slow process. Can be disabled when debugging.");

            runBuildCommandToggle.SetValueWithoutNotify(_runBuildCommand);
            generateJsonToggle.SetValueWithoutNotify(_generateJson);
            generateMarkdownToggle.SetValueWithoutNotify(_generateMarkdown);
            generateVSCExtensionJsonToggle.SetValueWithoutNotify(_generateVSCExtensionJson);
            resolveInheritDocToggle.SetValueWithoutNotify(_resolveInheritDoc);

            runBuildCommandToggle.RegisterValueChangedCallback(e => _runBuildCommand = e.newValue);
            generateJsonToggle.RegisterValueChangedCallback(e => _generateJson = e.newValue);
            generateMarkdownToggle.RegisterValueChangedCallback(e => _generateMarkdown = e.newValue);
            generateVSCExtensionJsonToggle.RegisterValueChangedCallback(e => _generateVSCExtensionJson = e.newValue);
            resolveInheritDocToggle.RegisterValueChangedCallback(e => _resolveInheritDoc = e.newValue);

            _generateButton = new Button(() => _ = Generate())
            {
                text = "Generate"
            };

            _logsView = new ListView
            {
                itemsSource = _logs,
                makeItem = () => new Label { enableRichText = true },
                bindItem = (element, index) => ((Label)element).text = _logs[index],
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                selectionType = SelectionType.None,
                style = { flexGrow = 1 }
            };

            root.Add(optionsContainer);
            root.Add(_generateButton);
            root.Add(_logsView);

            return;

            Toggle CreateOption(string label, string help)
            {
                var container = new VisualElement();

                var toggleHelp = new Label(help);
                var toggle = new Toggle(label);

                optionsContainer.style.paddingBottom = 10f;
                optionsContainer.style.paddingLeft = 10f;
                optionsContainer.style.paddingRight = 10f;
                optionsContainer.style.paddingTop = 10f;

                toggleHelp.style.width = new Length(65, LengthUnit.Percent);
                toggleHelp.style.color = ColorFromHex("#a6a6a6");
                toggleHelp.style.whiteSpace = WhiteSpace.Normal;

                container.style.marginTop = 5f;
                container.style.marginBottom = 5f;
                container.style.paddingLeft = 5f;
                container.style.borderLeftWidth = 2f;
                container.style.borderLeftColor = ColorFromHex("#b273ff6f");

                container.Add(toggleHelp);
                container.Add(toggle);

                optionsContainer.Add(container);

                return toggle;
            }

            static Color ColorFromHex(string hexCode)
            {
                if (ColorUtility.TryParseHtmlString(hexCode, out var color))
                    return color;

                return Color.white;
            }
        }

        private void OnEnable() => EditorApplication.update += UpdateLogs;
        private void OnDisable() => EditorApplication.update -= UpdateLogs;

        private void UpdateLogs()
        {
            while (_logsConcurrent.TryDequeue(out var message))
                Log(message);
        }

        private async Task Generate()
        {
            _logs.Clear();
            _logsView.Clear();

            if (!_generateJson && !_generateMarkdown && !_generateVSCExtensionJson)
            {
                LogError("At least one format must be selected.");
                return;
            }

            _generateButton.SetEnabled(false);

            LogInfo("Generating docs...");
            if (_runBuildCommand)
            {
                LogInfo("Running `dotnet build ./Aottg2-Unity.sln`...");
                _logsConcurrent.Clear();
                var process = new Process()
                {
                    StartInfo = {
                            FileName = "dotnet",
                            Arguments = "build ./Aottg2-Unity.sln",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                        },
                    EnableRaisingEvents = true
                };

                process.OutputDataReceived += (sender, e) =>
                {
                    if (string.IsNullOrEmpty(e.Data) == false)
                        _logsConcurrent.Enqueue(FormatSuccess(e.Data));
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (string.IsNullOrEmpty(e.Data) == false)
                        _logsConcurrent.Enqueue(FormatError(e.Data));
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                while (process.HasExited == false)
                    await Task.Delay(100);
            }

            var path = "Docs/";

            LogInfo("Clearing existing docs...");

            if (Directory.Exists(path))
                Directory.Delete(path, true);

            try
            {
                const string xmlPath = "Temp/Bin/Debug/Scripts.xml";
                var xmlDocument = XmlDocumentUtils.LoadXml(xmlPath);

                if (_resolveInheritDoc)
                {
                    LogInfo("Resolving inheritdocs...");
                    await XmlDocumentUtils.ResolveAndReplaceInheritDocNodeAsync(xmlDocument, LogError);
                }

                var clTypeProvider = new CLTypeProvider();
                var clTypes = clTypeProvider.GetCLTypes(xmlDocument);

                var generators = new List<BaseCustomLogicDocsGenerator>(3);
                if (_generateJson) generators.Add(new CustomLogicJsonDocsGenerator(clTypes));
                if (_generateMarkdown) generators.Add(new CustomLogicMarkdownDocsGenerator(clTypes));
                if (_generateVSCExtensionJson) generators.Add(new CustomLogicVSCExtensionJsonDocsGenerator(clTypes));

                LogInfo("Running generators...");

                foreach (var clType in clTypes)
                {
                    foreach (var generator in generators)
                    {
                        var rPath = generator.GetRelativeFilePath(clType);
                        var json = generator.Generate(clType);
                        var fullPath = Path.Join(path, rPath);
                        var dir = Path.GetDirectoryName(fullPath);
                        Directory.CreateDirectory(dir);
                        await File.WriteAllTextAsync(fullPath, json);
                        LogSuccess($"Generated {fullPath}");
                    }
                }
            }
            catch (Exception e)
            {
                LogError($"An error occured while generating docs: {e.Message}");

                foreach (var line in e.StackTrace.Split('\n'))
                    LogError(line);

                return;
            }
            finally
            {
                _generateButton.SetEnabled(true);
            }

            LogSuccess("Done.");
        }

        private void LogInfo(string message) => Log(FormatInfo(message));
        private void LogSuccess(string message) => Log(FormatSuccess(message));
        private void LogError(string message) => Log(FormatError(message));

        private void Log(string message)
        {
            _logs.Add(message);
            _logsView.RefreshItems();
            _logsView.ScrollToItem(-1);
        }

        private static string FormatError(string msg) => $"<color=#ff4824><b>ERR</b></color>: {msg}";
        private static string FormatSuccess(string msg) => $"<color=#b5ff2b><b>OK</b></color>: {msg}";
        private static string FormatInfo(string msg) => $"<color=#f5f5f5><b>INFO</b></color>: {msg}";
    }
#endif
}
