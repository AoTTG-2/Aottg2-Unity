using System;
using System.Collections.Generic;

namespace CustomLogic.OfflineEvaluator
{
    /// <summary>
    /// Offline evaluator for Custom Logic scripts that can run without Unity runtime dependencies.
    /// Automatically calls Init on the Main class.
    /// </summary>
    public class OfflineCustomLogicEvaluator
    {
        private CustomLogicEvaluator _evaluator;
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

        public OfflineCustomLogicEvaluator(string script)
        {
            // Initialize CustomLogic symbols (required before lexing/parsing)
            if (CustomLogicSymbols.Symbols.Count == 0)
            {
                CustomLogicSymbols.Init();
            }

            // Compile the script
            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("TestScript.cl", script, CustomLogicSourceType.ModeLogic));
            string combinedSource = compiler.Compile();

            // Parse the script
            var lexer = new CustomLogicLexer(combinedSource, compiler);
            var tokens = lexer.GetTokens();
            if (!string.IsNullOrEmpty(lexer.Error))
                throw new Exception($"Lexer error: {lexer.Error}");

            var parser = new CustomLogicParser(tokens, compiler);
            var startAst = parser.GetStartAst();
            if (!string.IsNullOrEmpty(parser.Error))
                throw new Exception($"Parser error: {parser.Error}");

            // Create evaluator
            _evaluator = new CustomLogicEvaluator(startAst, compiler);
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
        /// This creates user-defined classes first, then builtin classes, then runs assignments.
        /// Only offline-compatible builtins are initialized.
        /// </summary>
        private void InitializeStaticClasses()
        {
            var startAst = GetStartAst();
            var staticClasses = GetStaticClassesDictionary();

            // First, create user-defined static classes (Main and extensions)
            // This must happen BEFORE C# bindings so user code can override
            foreach (string className in startAst.Classes.Keys)
            {
                if (className == "Main")
                    CreateStaticClass(className, staticClasses);
                else if ((int)startAst.Classes[className].Token.Value == (int)CustomLogicSymbol.Extension)
                    CreateStaticClass(className, staticClasses);
            }

            // Then create C# builtin static classes ONLY if:
            // 1. Not already defined by user
            // 2. Compatible with offline mode (no Unity runtime dependencies)
            foreach (var staticType in CustomLogicBuiltinTypes.StaticTypeNames)
            {
                if (!staticClasses.ContainsKey(staticType) && OfflineCompatibleBuiltins.Contains(staticType))
                {
                    var instance = CustomLogicBuiltinTypes.CreateClassInstance(staticType, CustomLogicEvaluator.EmptyArgs);
                    staticClasses[staticType] = instance;
                }
            }

            // Run assignments for all class instances
            foreach (CustomLogicClassInstance instance in staticClasses.Values)
            {
                if (instance is not BuiltinClassInstance)
                    RunAssignmentsClassInstance(instance);
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
        private void CreateStaticClass(string className, Dictionary<string, CustomLogicClassInstance> staticClasses)
        {
            if (!staticClasses.ContainsKey(className))
            {
                var instance = _evaluator.CreateClassInstance(className, CustomLogicEvaluator.EmptyArgs, false);
                staticClasses.Add(className, instance);
            }
        }

        /// <summary>
        /// Run assignments for a class instance (sets up variables and methods)
        /// </summary>
        private void RunAssignmentsClassInstance(CustomLogicClassInstance classInstance)
        {
            var runAssignmentsMethod = _evaluator.GetType()
                .GetMethod("RunAssignmentsClassInstance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (runAssignmentsMethod != null)
            {
                runAssignmentsMethod.Invoke(_evaluator, new object[] { classInstance });
            }
        }

        /// <summary>
        /// Get the AST from the evaluator
        /// </summary>
        private CustomLogicStartAst GetStartAst()
        {
            var startField = _evaluator.GetType()
                .GetField("_start", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return startField?.GetValue(_evaluator) as CustomLogicStartAst;
        }

        /// <summary>
        /// Get the static classes dictionary from the evaluator
        /// </summary>
        private Dictionary<string, CustomLogicClassInstance> GetStaticClassesDictionary()
        {
            var staticClassesField = _evaluator.GetType()
                .GetField("_staticClasses", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return staticClassesField?.GetValue(_evaluator) as Dictionary<string, CustomLogicClassInstance>;
        }

        /// <summary>
        /// Get a static class instance from the evaluator
        /// </summary>
        private CustomLogicClassInstance GetStaticClass(string className)
        {
            var staticClasses = GetStaticClassesDictionary();
            if (staticClasses != null && staticClasses.ContainsKey(className))
            {
                return staticClasses[className];
            }
            return null;
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
    }
}
