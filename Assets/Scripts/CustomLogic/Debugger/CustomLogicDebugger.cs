using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace CustomLogic.Debugger
{
    /// <summary>
    /// Manages debug sessions for Custom Logic scripts.
    /// Communicates with VSCode via Debug Adapter Protocol over TCP.
    /// </summary>
    public class CustomLogicDebugger
    {
        private static CustomLogicDebugger _instance;
        public static CustomLogicDebugger Instance => _instance ??= new CustomLogicDebugger();

        private TcpListener _tcpListener;
        private TcpClient _client;
        private NetworkStream _stream;
        private Thread _listenerThread;
        private Thread _messageThread;
        private bool _isRunning;
        private int _sequenceNumber = 1;

        // Breakpoint management
        private Dictionary<string, HashSet<int>> _breakpoints = new Dictionary<string, HashSet<int>>();

        // Execution state
        private bool _isPaused;
        private ManualResetEvent _pauseEvent = new ManualResetEvent(true);
        private CustomLogicClassInstance _currentInstance;
        private Dictionary<string, object> _currentLocalVariables;
        private CustomLogicBaseAst _currentStatement;
        private string _currentFileName = "main.cl";
        private int _currentLineNumber = 0; // Actual file line number (not AST line)

        // Stack trace
        private Stack<DebugStackFrame> _callStack = new Stack<DebugStackFrame>();

        // Step control
        private StepMode _stepMode = StepMode.None;
        private int _stepDepth = 0;

        // Variable handle registry for expandable objects
        private readonly Dictionary<int, object> _variableHandles = new Dictionary<int, object>();
        private int _nextVariableHandle = 100; // start > 0, 0 means no children

        // Global static classes (Main, extensions, etc.)
        private Dictionary<string, CustomLogicClassInstance> _globalStaticClasses;

        // Connection wait configuration
        private bool _waitingForConnection;
        private float _connectionWaitStartTime;
        private const float CONNECTION_WAIT_TIMEOUT = 30f; // 30 seconds timeout

        public bool IsEnabled { get; set; }
        public bool IsPaused => _isPaused;
        public string CurrentFileName { get => _currentFileName; set => _currentFileName = value; }
        public bool IsConnected => _client != null && _client.Connected;

        public void StartDebugServer(int port = 4711)
        {
            if (_isRunning) return;

            _isRunning = true;
            _listenerThread = new Thread(() => ListenForConnections(port));
            _listenerThread.IsBackground = true;
            _listenerThread.Start();

            Debug.Log($"[CL Debugger] Started debug server on port {port}");
        }

        /// <summary>
        /// Waits for a debugger to connect before continuing execution.
        /// Call this before executing Custom Logic that you want to debug.
        /// </summary>
        /// <param name="timeout">Maximum seconds to wait. 0 = wait forever.</param>
        /// <returns>True if debugger connected, false if timed out</returns>
        public bool WaitForConnection(float timeout = CONNECTION_WAIT_TIMEOUT)
        {
            if (IsConnected)
            {
                Debug.Log("[CL Debugger] Already connected");
                return true;
            }

            _waitingForConnection = true;
            _connectionWaitStartTime = Time.realtimeSinceStartup;

            Debug.Log($"[CL Debugger] Waiting for debugger connection (timeout: {(timeout > 0 ? timeout.ToString() + "s" : "none")})...");

            while (!IsConnected)
            {
                if (timeout > 0 && (Time.realtimeSinceStartup - _connectionWaitStartTime) > timeout)
                {
                    Debug.LogWarning("[CL Debugger] Connection wait timed out");
                    _waitingForConnection = false;
                    return false;
                }

                Thread.Sleep(100); // Check every 100ms
            }

            Debug.Log("[CL Debugger] Debugger connected!");
            _waitingForConnection = false;
            
            // Give the debugger a moment to finish initialization
            Thread.Sleep(500);
            
            return true;
        }

        public void StopDebugServer()
        {
            _isRunning = false;
            _client?.Close();
            _tcpListener?.Stop();
            _pauseEvent.Set(); // Unblock any paused execution
            
            // Clear state and disable debugger
            ClearState();
            IsEnabled = false;
        }

        /// <summary>
        /// Clears all debugger state. Called when debugger is stopped or CL is reloaded.
        /// Preserves IsEnabled if debugger is still connected.
        /// </summary>
        public void ClearState()
        {
            // Clear execution state
            _currentInstance = null;
            _currentLocalVariables = null;
            _currentStatement = null;
            _currentFileName = "main.cl";
            _currentLineNumber = 0; // Reset current line number
            
            // Clear call stack
            _callStack.Clear();
            
            // Clear step control
            _stepMode = StepMode.None;
            _stepDepth = 0;
            
            // Clear variable handles
            _variableHandles.Clear();
            _nextVariableHandle = 100;
            
            // Clear globals reference
            _globalStaticClasses = null;
            
            // Reset pause state
            _isPaused = false;
            _pauseEvent.Set();
            
            // Note: We don't clear breakpoints as they should persist across CL reloads
            // Note: We don't disable IsEnabled - it stays active if debugger is connected
            
            Debug.Log("[CL Debugger] State cleared");
        }

        public void RestartDebugServer(int port = 4711)
        {
            StopDebugServer();
            
            // Give a moment for cleanup
            System.Threading.Thread.Sleep(100);
            
            StartDebugServer(port);
        }

        private void ListenForConnections(int port)
        {
            try
            {
                _tcpListener = new TcpListener(IPAddress.Loopback, port);
                _tcpListener.Start();

                while (_isRunning)
                {
                    try
                    {
                        _client = _tcpListener.AcceptTcpClient();
                        _stream = _client.GetStream();
                        Debug.Log("[CL Debugger] Client connected");
                        IsEnabled = true;

                        _messageThread = new Thread(HandleClient);
                        _messageThread.IsBackground = true;
                        _messageThread.Start();
                    }
                    catch (Exception e)
                    {
                        if (_isRunning)
                            Debug.LogError($"[CL Debugger] Connection error: {e.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[CL Debugger] Server error: {e.Message}");
            }
        }

        private void HandleClient()
        {
            byte[] buffer = new byte[4096];
            StringBuilder messageBuffer = new StringBuilder();

            try
            {
                while (_isRunning && _client.Connected)
                {
                    int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    messageBuffer.Append(data);

                    // Process complete messages
                    string bufferStr = messageBuffer.ToString();
                    int contentStart = 0;

                    while (true)
                    {
                        int headerEnd = bufferStr.IndexOf("\r\n\r\n", contentStart);
                        if (headerEnd == -1) break;

                        string header = bufferStr.Substring(contentStart, headerEnd - contentStart);
                        int contentLength = ParseContentLength(header);

                        if (contentLength == -1) break;

                        int messageStart = headerEnd + 4;
                        if (bufferStr.Length < messageStart + contentLength) break;

                        string message = bufferStr.Substring(messageStart, contentLength);
                        ProcessMessage(message);

                        contentStart = messageStart + contentLength;
                    }

                    if (contentStart > 0)
                        messageBuffer.Remove(0, contentStart);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[CL Debugger] Message handling error: {e.Message}");
            }
            finally
            {
                _client?.Close();
                IsEnabled = false;
                _isPaused = false;
                _pauseEvent.Set();
                Debug.Log("[CL Debugger] Client disconnected");
            }
        }

        private int ParseContentLength(string header)
        {
            string[] lines = header.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                if (line.StartsWith("Content-Length: "))
                {
                    if (int.TryParse(line.Substring(16), out int length))
                        return length;
                }
            }
            return -1;
        }

        private void ProcessMessage(string message)
        {
            try
            {
                var msg = JObject.Parse(message);
                string type = msg["type"]?.ToString();

                if (type == "request")
                {
                    string command = msg["command"]?.ToString();
                    int seq = msg["seq"]?.ToObject<int>() ?? 0;

                    switch (command)
                    {
                        case "initialize":
                            HandleInitialize(seq);
                            break;
                        case "setBreakpoints":
                            HandleSetBreakpoints(seq, msg);
                            break;
                        case "configurationDone":
                            HandleConfigurationDone(seq);
                            break;
                        case "launch":
                        case "attach":
                            HandleLaunch(seq);
                            break;
                        case "threads":
                            HandleThreads(seq);
                            break;
                        case "stackTrace":
                            HandleStackTrace(seq);
                            break;
                        case "scopes":
                            HandleScopes(seq, msg);
                            break;
                        case "variables":
                            HandleVariables(seq, msg);
                            break;
                        case "evaluate":
                            HandleEvaluate(seq, msg);
                            break;
                        case "continue":
                            HandleContinue(seq);
                            break;
                        case "next":
                            HandleNext(seq);
                            break;
                        case "stepIn":
                            HandleStepIn(seq);
                            break;
                        case "stepOut":
                            HandleStepOut(seq);
                            break;
                        case "pause":
                            HandlePause(seq);
                            break;
                        case "disconnect":
                            HandleDisconnect(seq);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[CL Debugger] Error processing message: {e.Message}\n{message}");
            }
        }

        private void HandleInitialize(int requestSeq)
        {
            var response = new
            {
                type = "response",
                request_seq = requestSeq,
                success = true,
                command = "initialize",
                body = new
                {
                    supportsConfigurationDoneRequest = true,
                    supportsEvaluateForHovers = true,
                    supportsStepBack = false,
                    supportsSetVariable = false,
                    supportsRestartFrame = false,
                    supportsExceptionInfoRequest = true
                }
            };
            SendMessage(response);
            SendEvent("initialized", new { });
        }

        private void HandleSetBreakpoints(int requestSeq, JObject msg)
        {
            var args = msg["arguments"];
            string source = args["source"]?["path"]?.ToString();
            var breakpoints = args["breakpoints"]?.ToObject<JArray>();

            if (source != null)
            {
                source = source.Replace("\\", "/");

                Debug.Log($"[CL Debugger] Setting breakpoints for: {source}");  // ADD THIS

                _breakpoints[source] = new HashSet<int>();
                var verified = new List<object>();

                if (breakpoints != null)
                {
                    foreach (var bp in breakpoints)
                    {
                        int line = bp["line"]?.ToObject<int>() ?? 0;
                        _breakpoints[source].Add(line);
                        Debug.Log($"[CL Debugger] - Breakpoint at line {line}");  // ADD THIS
                        verified.Add(new { verified = true, line = line });
                    }
                }

                Debug.Log($"[CL Debugger] Total breakpoints stored: {_breakpoints.Count} files");  // ADD THIS

                var response = new
                {
                    type = "response",
                    request_seq = requestSeq,
                    success = true,
                    command = "setBreakpoints",
                    body = new { breakpoints = verified }
                };
                SendMessage(response);
            }
        }

        private void HandleConfigurationDone(int requestSeq)
        {
            SendResponse(requestSeq, "configurationDone", true, new { });
        }

        private void HandleLaunch(int requestSeq)
        {
            SendResponse(requestSeq, "launch", true, new { });
        }

        private void HandleThreads(int requestSeq)
        {
            var response = new
            {
                type = "response",
                request_seq = requestSeq,
                success = true,
                command = "threads",
                body = new
                {
                    threads = new[]
                    {
                        new { id = 1, name = "Main Thread" }
                    }
                }
            };
            SendMessage(response);
        }

        private void HandleStackTrace(int requestSeq)
        {
            var frames = new List<object>();
            int frameId = 0;

            // Current frame - use actual file line number, not AST line
            if (_currentStatement != null)
            {
                frames.Add(new
                {
                    id = frameId++,
                    name = _callStack.Count > 0 ? _callStack.Peek().MethodName : "main",
                    source = new { path = _currentFileName },
                    line = _currentLineNumber, // Use actual file line number
                    column = 1
                });
            }

            // Call stack frames - these already have actual line numbers from PushStackFrame
            foreach (var frame in _callStack)
            {
                frames.Add(new
                {
                    id = frameId++,
                    name = $"{frame.ClassName}.{frame.MethodName}",
                    source = new { path = frame.FileName },
                    line = frame.Line, // Already converted in PushStackFrame
                    column = 1
                });
            }

            var response = new
            {
                type = "response",
                request_seq = requestSeq,
                success = true,
                command = "stackTrace",
                body = new
                {
                    stackFrames = frames,
                    totalFrames = frames.Count
                }
            };
            SendMessage(response);
        }

        private void HandleScopes(int requestSeq, JObject msg)
        {
            var scopesList = new List<object>();

            // Clear old variable handles for this pause
            _variableHandles.Clear();
            _nextVariableHandle = 100;

            // Debug: Log what we have when paused
            Debug.Log($"[CL Debugger] HandleScopes called:");
            Debug.Log($"  - Local vars: {(_currentLocalVariables != null ? _currentLocalVariables.Count.ToString() : "null")}");
            Debug.Log($"  - Current instance: {(_currentInstance != null ? _currentInstance.ClassName : "null")}");
            Debug.Log($"  - Globals: {(_globalStaticClasses != null ? _globalStaticClasses.Count.ToString() : "null")}");

            // Register locals as a handle if we have locals
            int localHandle = 0;
            if (_currentLocalVariables != null && _currentLocalVariables.Count > 0)
            {
                localHandle = RegisterVariableHandle(_currentLocalVariables);
                Debug.Log($"  - Registered Local handle: {localHandle}");
                foreach (var kvp in _currentLocalVariables)
                    Debug.Log($"    - {kvp.Key}: {kvp.Value?.GetType().Name ?? "null"}");
            }

            // Register class instance variables as a handle
            int classHandle = 0;
            if (_currentInstance != null)
            {
                classHandle = RegisterVariableHandle(_currentInstance);
                Debug.Log($"  - Registered Class handle: {classHandle} ({_currentInstance.ClassName})");
                Debug.Log($"    - Instance has {_currentInstance.Variables.Count} variables");
            }

            // Register globals (static Main etc.) as a handle if available
            int globalHandle = 0;
            if (_globalStaticClasses != null && _globalStaticClasses.Count > 0)
            {
                globalHandle = RegisterVariableHandle(_globalStaticClasses);
                Debug.Log($"  - Registered Globals handle: {globalHandle}");
                foreach (var kvp in _globalStaticClasses)
                    Debug.Log($"    - {kvp.Key}: {kvp.Value.ClassName} ({kvp.Value.Variables.Count} vars)");
            }

            // Provide scopes: Local, Class, Globals (if present)
            if (localHandle != 0)
                scopesList.Add(new { name = "Local", variablesReference = localHandle, expensive = false });
            if (classHandle != 0)
                scopesList.Add(new { name = "Class", variablesReference = classHandle, expensive = false });
            if (globalHandle != 0)
                scopesList.Add(new { name = "Globals", variablesReference = globalHandle, expensive = false });

            Debug.Log($"  - Returning {scopesList.Count} scopes");

            var response = new
            {
                type = "response",
                request_seq = requestSeq,
                success = true,
                command = "scopes",
                body = new { scopes = scopesList }
            };
            SendMessage(response);
        }

        private void HandleVariables(int requestSeq, JObject msg)
        {
            int variablesRef = msg["arguments"]?["variablesReference"]?.ToObject<int>() ?? 0;
            var variables = new List<object>();

            Debug.Log($"[CL Debugger] HandleVariables for ref {variablesRef}");

            if (!TryGetVariableHandle(variablesRef, out object container))
            {
                Debug.Log($"  - No container found for ref {variablesRef}");
                // unknown reference -> empty
            }
            else
            {
                Debug.Log($"  - Container type: {container.GetType().Name}");

                // Locals: Dictionary<string, object>
                if (container is Dictionary<string, object> locals)
                {
                    Debug.Log($"  - Processing {locals.Count} local variables");
                    foreach (var kvp in locals)
                    {
                        int childRef = GetChildHandleIfComplex(kvp.Value);
                        variables.Add(new
                        {
                            name = kvp.Key,
                            value = FormatValue(kvp.Value),
                            type = GetTypeName(kvp.Value),
                            variablesReference = childRef
                        });
                        Debug.Log($"    - {kvp.Key}: {GetTypeName(kvp.Value)} = {FormatValue(kvp.Value)} (childRef: {childRef})");
                    }
                }
                // Class instance: CustomLogicClassInstance (expose its Variables, filtering internal ones)
                else if (container is CustomLogicClassInstance instance)
                {
                    Debug.Log($"  - Processing class instance: {instance.ClassName} with {instance.Variables.Count} variables");
                    int filteredCount = 0;
                    foreach (var kvp in instance.Variables)
                    {
                        // Filter out methods and internal variables
                        if (kvp.Value is UserMethod || kvp.Value is CLMethodBinding)
                        {
                            Debug.Log($"    - Filtered method: {kvp.Key}");
                            continue;
                        }
                        if (ShouldFilterVariable(kvp.Key))
                        {
                            Debug.Log($"    - Filtered internal: {kvp.Key}");
                            filteredCount++;
                            continue;
                        }

                        int childRef = GetChildHandleIfComplex(kvp.Value);
                        variables.Add(new
                        {
                            name = kvp.Key,
                            value = FormatValue(kvp.Value),
                            type = GetTypeName(kvp.Value),
                            variablesReference = childRef
                        });
                        Debug.Log($"    - {kvp.Key}: {GetTypeName(kvp.Value)} = {FormatValue(kvp.Value)} (childRef: {childRef})");
                    }
                    Debug.Log($"  - Filtered {filteredCount} internal variables, returning {variables.Count} variables");
                }
                // Globals: Dictionary<string, CustomLogicClassInstance>
                else if (container is Dictionary<string, CustomLogicClassInstance> globals)
                {
                    Debug.Log($"  - Processing {globals.Count} global classes");
                    foreach (var kvp in globals)
                    {
                        int childRef = RegisterVariableHandle(kvp.Value);
                        variables.Add(new
                        {
                            name = kvp.Key,
                            value = $"{kvp.Value.ClassName} (static)",
                            type = "class",
                            variablesReference = childRef
                        });
                        Debug.Log($"    - {kvp.Key}: {kvp.Value.ClassName} (childRef: {childRef})");
                    }
                }
                // Lists: CustomLogicListBuiltin
                else if (container is CustomLogicListBuiltin listBuiltin)
                {
                    Debug.Log($"  - Processing list with {listBuiltin.List.Count} items");
                    int index = 0;
                    foreach (var item in listBuiltin.List)
                    {
                        int childRef = GetChildHandleIfComplex(item);
                        variables.Add(new
                        {
                            name = $"[{index}]",
                            value = FormatValue(item),
                            type = GetTypeName(item),
                            variablesReference = childRef
                        });
                        index++;
                    }
                }
                // Dictionary-like builtins (CustomLogicDictBuiltin)
                else if (container is CustomLogicDictBuiltin dictBuiltin)
                {
                    Debug.Log($"  - Processing dict with {dictBuiltin.Dict.Count} items");
                    foreach (var kvp in dictBuiltin.Dict)
                    {
                        int childRef = GetChildHandleIfComplex(kvp.Value);
                        variables.Add(new
                        {
                            name = $"[{FormatValue(kvp.Key)}]",
                            value = FormatValue(kvp.Value),
                            type = GetTypeName(kvp.Value),
                            variablesReference = childRef
                        });
                    }
                }
            }

            Debug.Log($"  - Returning {variables.Count} variables");

            var response = new
            {
                type = "response",
                request_seq = requestSeq,
                success = true,
                command = "variables",
                body = new { variables }
            };
            SendMessage(response);
        }

        // Filter out internal/builtin variables that clutter the debug view
        private bool ShouldFilterVariable(string name)
        {
            // Filter out Type, Init, IsCharacter and other internal markers
            if (name == "Type" || name == "Init" || name == "IsCharacter")
                return true;
            // Don't filter variables starting with underscore - these are user variables
            // if (name.StartsWith("_"))
            //     return true;
            return false;
        }

        // Determine if a value should get its own variablesReference for expansion
        private int GetChildHandleIfComplex(object value)
        {
            if (value == null) return 0;
            if (value is CustomLogicClassInstance) return RegisterVariableHandle(value);
            if (value is CustomLogicListBuiltin) return RegisterVariableHandle(value);
            if (value is CustomLogicDictBuiltin) return RegisterVariableHandle(value);
            if (value is Dictionary<string, object>) return RegisterVariableHandle(value);
            return 0;
        }

        private string GetTypeName(object value)
        {
            if (value == null) return "null";
            if (value is CustomLogicClassInstance cli) return cli.ClassName;
            if (value is string) return "string";
            if (value is int) return "int";
            if (value is float) return "float";
            if (value is bool) return "bool";
            return value.GetType().Name;
        }

        private string FormatValue(object value)
        {
            if (value == null) return "null";
            if (value is string str) return $"\"{str}\"";
            if (value is bool b) return b.ToString().ToLower();
            if (value is CustomLogicClassInstance instance)
            {
                // Try to get a better representation for common builtins
                if (instance is CustomLogicVector3Builtin vec3)
                    return $"({vec3.Value.x}, {vec3.Value.y}, {vec3.Value.z})";
                if (instance is CustomLogicColorBuiltin color)
                    return $"({color.R}, {color.G}, {color.B}, {color.A})";
                if (instance is CustomLogicListBuiltin list)
                    return $"List[{list.List.Count}]";
                if (instance is CustomLogicDictBuiltin dict)
                    return $"Dict[{dict.Dict.Count}]";
                
                // Try to find a ToString or display value
                if (instance.TryGetVariable("ToString", out var toStr) && toStr is CLMethodBinding)
                {
                    try
                    {
                        var result = ((CLMethodBinding)toStr).Call(instance, new object[0]);
                        if (result is string s) return s;
                    }
                    catch { }
                }
                
                return $"{instance.ClassName}";
            }
            return value.ToString();
        }

        private void HandleEvaluate(int requestSeq, JObject msg)
        {
            var args = msg["arguments"];
            string expression = args?["expression"]?.ToString();
            string context = args?["context"]?.ToString(); // "watch", "hover", "repl"

            object resultVal = null;
            try
            {
                if (string.IsNullOrEmpty(expression))
                {
                    resultVal = null;
                }
                else
                {
                    // Support dotted path: "obj.field.subfield"
                    string[] parts = expression.Split('.');
                    object cur = null;
                    bool found = false;

                    // Try first part in locals
                    if (_currentLocalVariables != null && _currentLocalVariables.TryGetValue(parts[0], out cur))
                        found = true;
                    // Try current instance
                    else if (_currentInstance != null && _currentInstance.TryGetVariable(parts[0], out var v))
                    {
                        cur = v is CLPropertyBinding pb ? pb.GetValue(_currentInstance) : v;
                        found = true;
                    }
                    // Try globals
                    else if (_globalStaticClasses != null && _globalStaticClasses.TryGetValue(parts[0], out var g))
                    {
                        cur = g;
                        found = true;
                    }

                    if (found)
                    {
                        // Traverse dotted path
                        for (int i = 1; i < parts.Length; i++)
                        {
                            if (cur == null) { cur = null; break; }
                            if (cur is CustomLogicClassInstance cli)
                            {
                                if (!cli.TryGetVariable(parts[i], out var tmp)) { cur = null; break; }
                                cur = tmp is CLPropertyBinding pb ? pb.GetValue(cli) : tmp;
                            }
                            else if (cur is CustomLogicListBuiltin lb)
                            {
                                // Support [index] syntax
                                if (int.TryParse(parts[i].Trim('[', ']'), out int idx) && idx >= 0 && idx < lb.List.Count)
                                    cur = lb.List[idx];
                                else
                                    cur = null;
                            }
                            else if (cur is CustomLogicDictBuiltin db)
                            {
                                // Basic dict key lookup
                                string key = parts[i].Trim('[', ']', '"');
                                if (db.Dict.ContainsKey(key))
                                    cur = db.Dict[key];
                                else
                                    cur = null;
                            }
                            else
                            {
                                // Not traversable
                                cur = null;
                                break;
                            }
                        }
                    }
                    else
                    {
                        cur = null;
                    }
                    resultVal = cur;
                }
            }
            catch (Exception ex)
            {
                var responseErr = new
                {
                    type = "response",
                    request_seq = requestSeq,
                    success = false,
                    command = "evaluate",
                    message = ex.Message
                };
                SendMessage(responseErr);
                return;
            }

            int childRef = GetChildHandleIfComplex(resultVal);
            var response = new
            {
                type = "response",
                request_seq = requestSeq,
                success = true,
                command = "evaluate",
                body = new
                {
                    result = FormatValue(resultVal),
                    type = GetTypeName(resultVal),
                    variablesReference = childRef
                }
            };
            SendMessage(response);
        }

        private void HandleContinue(int requestSeq)
        {
            _stepMode = StepMode.None;
            Continue();
            SendResponse(requestSeq, "continue", true, new { allThreadsContinued = true });
        }

        private void HandleNext(int requestSeq)
        {
            _stepMode = StepMode.Over;
            _stepDepth = _callStack.Count;
            Continue();
            SendResponse(requestSeq, "next", true, new { });
        }

        private void HandleStepIn(int requestSeq)
        {
            _stepMode = StepMode.In;
            Continue();
            SendResponse(requestSeq, "stepIn", true, new { });
        }

        private void HandleStepOut(int requestSeq)
        {
            _stepMode = StepMode.Out;
            _stepDepth = _callStack.Count - 1;
            Continue();
            SendResponse(requestSeq, "stepOut", true, new { });
        }

        private void HandlePause(int requestSeq)
        {
            Pause();
            SendResponse(requestSeq, "pause", true, new { });
        }

        private void HandleDisconnect(int requestSeq)
        {
            SendResponse(requestSeq, "disconnect", true, new { });
            IsEnabled = false;
            _isPaused = false;
            _pauseEvent.Set();
        }

        private void SendResponse(int requestSeq, string command, bool success, object body)
        {
            var response = new
            {
                type = "response",
                request_seq = requestSeq,
                success,
                command,
                body
            };
            SendMessage(response);
        }

        private void SendEvent(string eventName, object body)
        {
            var evt = new
            {
                type = "event",
                @event = eventName,
                body
            };
            SendMessage(evt);
        }

        private void SendMessage(object message)
        {
            try
            {
                if (_stream == null || !_client.Connected) return;

                string json = JsonConvert.SerializeObject(message);
                string header = $"Content-Length: {json.Length}\r\n\r\n";
                byte[] data = Encoding.UTF8.GetBytes(header + json);

                _stream.Write(data, 0, data.Length);
                _stream.Flush();
            }
            catch (Exception e)
            {
                Debug.LogError($"[CL Debugger] Error sending message: {e.Message}");
            }
        }

        public void DumpDebugState()
        {
            Debug.Log($"[CL Debugger] === Debug State ===");
            Debug.Log($"Enabled: {IsEnabled}");
            Debug.Log($"Paused: {_isPaused}");
            Debug.Log($"Current file: {_currentFileName}");
            Debug.Log($"Breakpoints registered:");
            foreach (var kvp in _breakpoints)
            {
                Debug.Log($"  {kvp.Key}: {string.Join(", ", kvp.Value)}");
            }
            Debug.Log($"===================");
        }

        /// <summary>
        /// Called before executing each statement. Checks for breakpoints and pauses if needed.
        /// </summary>
        internal void OnBeforeStatement(CustomLogicBaseAst statement, string fileName, int actualLineNumber,
            CustomLogicClassInstance instance, Dictionary<string, object> localVariables)
        {
            if (!IsEnabled) return;

            _currentStatement = statement;
            _currentInstance = instance;
            _currentLocalVariables = localVariables;
            _currentFileName = fileName;
            _currentLineNumber = actualLineNumber; // Store the actual file line number

            bool shouldPause = false;
            string reason = "step";

            // Check step mode
            if (_stepMode == StepMode.Over)
            {
                if (_callStack.Count <= _stepDepth)
                {
                    shouldPause = true;
                    _stepMode = StepMode.None;
                }
            }
            else if (_stepMode == StepMode.In)
            {
                shouldPause = true;
                _stepMode = StepMode.None;
            }
            else if (_stepMode == StepMode.Out)
            {
                if (_callStack.Count < _stepDepth)
                {
                    shouldPause = true;
                    _stepMode = StepMode.None;
                }
            }

            // Check breakpoint using actual file line number
            if (HasBreakpoint(fileName, actualLineNumber))
            {
                shouldPause = true;
                reason = "breakpoint";
                Debug.Log($"[CL Debugger] Hit breakpoint at {fileName}:{actualLineNumber}");
                Debug.Log($"  - Instance: {instance?.ClassName ?? "null"}");
                Debug.Log($"  - Local vars count: {localVariables?.Count ?? 0}");
                if (localVariables != null)
                {
                    foreach (var kvp in localVariables)
                        Debug.Log($"    - {kvp.Key}: {kvp.Value?.GetType().Name ?? "null"}");
                }
            }

            if (shouldPause)
            {
                Pause();
                SendStoppedEvent(reason, fileName, actualLineNumber);
            }

            // Wait if paused
            _pauseEvent.WaitOne();
        }

        /// <summary>
        /// Called when an exception occurs in CL execution.
        /// </summary>
        internal void OnException(Exception exception, CustomLogicBaseAst statement, string fileName)
        {
            if (!IsEnabled) return;

            _currentStatement = statement;
            _currentFileName = fileName;

            Pause();
            SendStoppedEvent("exception", fileName, statement?.Line ?? 0, exception.Message);
        }

        private void SendStoppedEvent(string reason, string fileName, int line, string text = null)
        {
            var body = new
            {
                reason,
                threadId = 1,
                text,
                allThreadsStopped = true
            };
            SendEvent("stopped", body);
        }

        /// <summary>
        /// Sets a breakpoint at the specified file and line.
        /// </summary>
        public void SetBreakpoint(string fileName, int line)
        {
            if (!_breakpoints.ContainsKey(fileName))
                _breakpoints[fileName] = new HashSet<int>();

            _breakpoints[fileName].Add(line);
        }

        /// <summary>
        /// Removes a breakpoint.
        /// </summary>
        public void ClearBreakpoint(string fileName, int line)
        {
            if (_breakpoints.ContainsKey(fileName))
                _breakpoints[fileName].Remove(line);
        }

        /// <summary>
        /// Checks if a breakpoint exists at the given location.
        /// </summary>
        private bool HasBreakpoint(string fileName, int line)
        {
            fileName = fileName.Replace("\\", "/");

            foreach (var kvp in _breakpoints)
            {
                string bpFile = kvp.Key.Replace("\\", "/");

                // Try exact match first
                if (string.Equals(bpFile, fileName, StringComparison.OrdinalIgnoreCase))
                {
                    if (kvp.Value.Contains(line))
                    {
                        Debug.Log($"[CL Debugger] Breakpoint HIT at {fileName}:{line}");
                        return true;
                    }
                }

                // Then try if one is a suffix of the other (for relative vs absolute paths)
                if (bpFile.EndsWith(fileName, StringComparison.OrdinalIgnoreCase) ||
                    fileName.EndsWith(bpFile, StringComparison.OrdinalIgnoreCase))
                {
                    if (kvp.Value.Contains(line))
                    {
                        Debug.Log($"[CL Debugger] Breakpoint HIT (partial match) at {fileName}:{line}");
                        return true;
                    }
                }
            }

            // Debug.Log($"[CL Debugger] No breakpoint at {fileName}:{line}");
            return false;
        }

        /// <summary>
        /// Pauses execution.
        /// </summary>
        public void Pause()
        {
            _isPaused = true;
            _pauseEvent.Reset();
        }

        /// <summary>
        /// Continues execution.
        /// </summary>
        public void Continue()
        {
            _isPaused = false;
            _pauseEvent.Set();
        }

        /// <summary>
        /// Pushes a new frame onto the call stack.
        /// </summary>
        public void PushStackFrame(string methodName, string className, string fileName, int line)
        {
            if (!IsEnabled) return;
            
            _callStack.Push(new DebugStackFrame
            {
                MethodName = methodName,
                ClassName = className,
                FileName = fileName,
                Line = line
            });
        }

        /// <summary>
        /// Pops a frame from the call stack.
        /// </summary>
        public void PopStackFrame()
        {
            if (!IsEnabled) return;
            
            if (_callStack.Count > 0)
                _callStack.Pop();
        }

        public class DebugStackFrame
        {
            public string MethodName { get; set; }
            public string ClassName { get; set; }
            public string FileName { get; set; }
            public int Line { get; set; }
        }

        private enum StepMode
        {
            None,
            Over,
            In,
            Out
        }

        // Helper: register a container and return an id for expansion
        private int RegisterVariableHandle(object container)
        {
            if (container == null) return 0;
            int id = _nextVariableHandle++;
            _variableHandles[id] = container;
            return id;
        }

        // Helper: get container by variablesReference id
        private bool TryGetVariableHandle(int variablesReference, out object container)
        {
            if (variablesReference == 0)
            {
                container = null;
                return false;
            }
            return _variableHandles.TryGetValue(variablesReference, out container);
        }

        // Called by evaluator during Init to expose static classes
        internal void SetGlobals(Dictionary<string, CustomLogicClassInstance> staticClasses)
        {
            _globalStaticClasses = staticClasses;
        }
    }
}
