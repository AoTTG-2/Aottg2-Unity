using System;
using System.Collections.Generic;

namespace CustomLogic.OfflineEvaluator
{
    /// <summary>
    /// Offline evaluator for Custom Logic scripts that can run without Unity runtime dependencies.
    /// Supports multi-file compilation and namespace isolation testing.
    /// </summary>
    public class OfflineCustomLogicEvaluator
    {
        private CustomLogicEvaluator _evaluator;
        private CustomLogicCompiler _compiler;
        private CustomLogicClassInstance _mainInstance;

        /// <summary>
        /// Static builtin types that work offline (no Unity runtime dependencies)
        /// </summary>
        private static readonly HashSet<string> OfflineCompatibleBuiltins = new HashSet<string>
        {
            "Math",
            "Convert",
            "Json",
            "Random",
            "String",
            "List",
            "Set",
            "Dict",
            "Range",
            "Vector2",
            "Vector3",
            "Quaternion",
            "Color"
        };

        /// <summary>
        /// Creates an offline evaluator from a single script file.
        /// </summary>
        public OfflineCustomLogicEvaluator(string script)
        {
            InitializeSymbols();
            
            _compiler = new CustomLogicCompiler();
            _compiler.AddSourceFile(new CustomLogicSourceFile("TestScript.cl", script, CustomLogicSourceType.ModeLogic));
            
            CompileAndInitialize();
        }

        /// <summary>
        /// Creates an offline evaluator from a compiler with multiple source files.
        /// This allows testing namespace isolation with BaseLogic, Addon, MapLogic, and ModeLogic files.
        /// </summary>
        public OfflineCustomLogicEvaluator(CustomLogicCompiler compiler)
        {
            InitializeSymbols();
            _compiler = compiler;
            CompileAndInitialize();
        }

        private void InitializeSymbols()
        {
            // Initialize CustomLogic symbols (required before lexing/parsing)
            if (CustomLogicSymbols.Symbols.Count == 0)
            {
                CustomLogicSymbols.Init();
            }
        }

        private void CompileAndInitialize()
        {
            // Compile the script(s)
            string combinedSource = _compiler.Compile();

            // Parse the script
            var lexer = new CustomLogicLexer(combinedSource, _compiler);
            var tokens = lexer.GetTokens();
            if (!string.IsNullOrEmpty(lexer.Error))
                throw new Exception($"Lexer error: {lexer.Error}");

            var parser = new CustomLogicParser(tokens, _compiler);
            var startAst = parser.GetStartAst();
            if (!string.IsNullOrEmpty(parser.Error))
                throw new Exception($"Parser error: {parser.Error}");

            // Create evaluator
            _evaluator = new CustomLogicEvaluator(startAst, _compiler);
            
            // IMPORTANT: Set the global evaluator reference for builtin classes that need it
            // (e.g., List.Filter, List.Map, List.Reduce)
            CustomLogicManager.Evaluator = _evaluator;

            // Initialize the evaluator's static classes using the same pattern as Init()
            InitializeStaticClasses();

            // Call Init() on all static classes (like Start() does)
            CallInitOnStaticClasses();

            // Get the Main instance (it was created during initialization)
            if (HasMainClass())
            {
                _mainInstance = GetStaticClass("Main");
            }
        }

        /// <summary>
        /// Initialize static classes using the same pattern as CustomLogicEvaluator.Init()
        /// This creates C# builtins first, then user-defined classes with namespace isolation.
        /// Only offline-compatible builtins are initialized.
        /// </summary>
        private void InitializeStaticClasses()
        {
            var startAst = GetStartAst();
            var staticClasses = GetStaticClassesDictionary();
            var namespacedStaticClasses = _evaluator.GetNamespacedStaticClasses();

            // First, create C# builtin static classes (these are the default and always accessible)
            foreach (var staticType in CustomLogicBuiltinTypes.StaticTypeNames)
            {
                if (OfflineCompatibleBuiltins.Contains(staticType))
                {
                    var instance = CustomLogicBuiltinTypes.CreateClassInstance(staticType, CustomLogicEvaluator.EmptyArgs);
                    staticClasses[staticType] = instance;
                }
            }

            // Then create user-defined static classes (Main and extensions)
            foreach (string className in startAst.Classes.Keys)
            {
                if (className == "Main")
                {
                    // Always create Main from user code if it exists
                    CreateStaticClassInternal(className, staticClasses);
                }
                else if ((int)startAst.Classes[className].Token.Value == (int)CustomLogicSymbol.Extension)
                {
                    // Get the namespace for this extension
                    CustomLogicSourceType? classNamespace = null;
                    if (startAst.ClassNamespaces.TryGetValue(className, out var ns))
                        classNamespace = ns;

                    var instance = _evaluator.CreateClassInstance(className, CustomLogicEvaluator.EmptyArgs, false, classNamespace);
                    instance.Namespace = classNamespace;
                    
                    // Check if this extension name conflicts with a C# builtin static class
                    bool conflictsWithBuiltin = CustomLogicBuiltinTypes.StaticTypeNames.Contains(className);
                    
                    if (conflictsWithBuiltin)
                    {
                        // Store in namespaced dictionary, do NOT add to staticClasses (preserve builtin)
                        if (!namespacedStaticClasses.ContainsKey(className))
                            namespacedStaticClasses[className] = new Dictionary<CustomLogicSourceType, CustomLogicClassInstance>();
                            
                        if (classNamespace.HasValue)
                            namespacedStaticClasses[className][classNamespace.Value] = instance;
                        // NOTE: We intentionally do NOT add to staticClasses to preserve the builtin
                    }
                    else
                    {
                        // No conflict with builtin, store as default
                        staticClasses[className] = instance;
                    }
                }
            }

            // Run assignments for all class instances in staticClasses
            foreach (CustomLogicClassInstance instance in staticClasses.Values)
            {
                if (instance is not BuiltinClassInstance)
                    RunAssignmentsClassInstance(instance);
            }
            
            // Run assignments for all namespace-specific class instances
            foreach (var namespaceDict in namespacedStaticClasses.Values)
            {
                foreach (var instance in namespaceDict.Values)
                {
                    if (instance is not BuiltinClassInstance)
                        RunAssignmentsClassInstance(instance);
                }
            }
        }

        /// <summary>
        /// Call Init() on all static classes, mirroring the behavior in CustomLogicEvaluator.Start()
        /// </summary>
        private void CallInitOnStaticClasses()
        {
            var staticClasses = GetStaticClassesDictionary();
            
            // Call Init() on all static classes
            foreach (var instance in staticClasses.Values)
            {
                _evaluator.EvaluateMethod(instance, "Init");
                instance.Inited = true;
            }
        }

        /// <summary>
        /// Create a static class instance (used for Main and extension classes)
        /// </summary>
        private void CreateStaticClassInternal(string className, Dictionary<string, CustomLogicClassInstance> staticClasses)
        {
            if (!staticClasses.ContainsKey(className))
            {
                var startAst = GetStartAst();
                CustomLogicSourceType? classNamespace = null;
                
                // Get the namespace for this class if it exists
                if (startAst.ClassNamespaces.TryGetValue(className, out var ns))
                {
                    classNamespace = ns;
                }
                
                // Use init: false because we'll run assignments and call Init manually later in the correct order
                var instance = _evaluator.CreateClassInstance(className, CustomLogicEvaluator.EmptyArgs, false, classNamespace);
                instance.Namespace = classNamespace;
                staticClasses.Add(className, instance);
            }
        }

        /// <summary>
        /// Run assignments for a class instance (sets up variables and methods)
        /// </summary>
        private void RunAssignmentsClassInstance(CustomLogicClassInstance classInstance)
        {
            _evaluator.RunAssignmentsClassInstance(classInstance);
        }

        /// <summary>
        /// Get the AST from the evaluator
        /// </summary>
        private CustomLogicStartAst GetStartAst()
        {
            return _evaluator.GetStartAst();
        }

        /// <summary>
        /// Get the static classes dictionary from the evaluator
        /// </summary>
        private Dictionary<string, CustomLogicClassInstance> GetStaticClassesDictionary()
        {
            return _evaluator.GetStaticClasses();
        }

        /// <summary>
        /// Get a static class instance from the evaluator
        /// </summary>
        public CustomLogicClassInstance GetStaticClass(string className)
        {
            var staticClasses = GetStaticClassesDictionary();
            if (staticClasses != null && staticClasses.ContainsKey(className))
            {
                return staticClasses[className];
            }
            return null;
        }

        /// <summary>
        /// Create a static class by name (for advanced testing scenarios).
        /// </summary>
        public void CreateStaticClass(string className)
        {
            var staticClasses = GetStaticClassesDictionary();
            CreateStaticClassInternal(className, staticClasses);
        }

        public bool HasMainClass()
        {
            var startAst = GetStartAst();
            return startAst?.Classes.ContainsKey("Main") ?? false;
        }

        public CustomLogicClassInstance GetMainInstance()
        {
            return _mainInstance;
        }

        /// <summary>
        /// Evaluate a method on a class instance.
        /// </summary>
        public object EvaluateMethod(CustomLogicClassInstance instance, string methodName, params object[] parameters)
        {
            return _evaluator.EvaluateMethod(instance, methodName, parameters);
        }

        /// <summary>
        /// Create and evaluate a method on a new instance of the specified class.
        /// </summary>
        public object EvaluateMethod(string className, string methodName, params object[] parameters)
        {
            var instance = _evaluator.CreateClassInstance(className, CustomLogicEvaluator.EmptyArgs, init: true);
            return _evaluator.EvaluateMethod(instance, methodName, parameters);
        }

        public object EvaluateMainMethod(string methodName, params object[] parameters)
        {
            if (_mainInstance == null)
                throw new Exception("Main class not found or not initialized");
            return _evaluator.EvaluateMethod(_mainInstance, methodName, parameters);
        }

        public object GetMainVariable(string variableName)
        {
            if (_mainInstance == null)
                throw new Exception("Main class not found or not initialized");
            return _mainInstance.GetVariable(variableName);
        }

        public void SetMainVariable(string variableName, object value)
        {
            if (_mainInstance == null)
                throw new Exception("Main class not found or not initialized");
            _mainInstance.Variables[variableName] = value;
        }

        /// <summary>
        /// Get the compiler used to compile the scripts.
        /// </summary>
        public CustomLogicCompiler GetCompiler()
        {
            return _compiler;
        }
    }
}
