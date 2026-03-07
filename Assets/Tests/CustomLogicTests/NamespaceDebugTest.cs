using NUnit.Framework;
using CustomLogic;
using CustomLogic.OfflineEvaluator;
using UnityEngine;
using System.Linq;

namespace Test.CustomLogic
{
    [TestFixture]
    public class NamespaceDebugTest
    {
        [Test]
        public void DebugNamespaceResolution()
        {
            Debug.Log("====== STARTING DEBUG TEST ======");
            
            // BaseLogic uses Convert builtin
            string baseLogic = @"
extension BaseExtension
{
    function UseConvertBuiltin()
    {
        result = Convert.ToInt(5.7);
        return result;
    }
}";

            // Mode logic overrides Convert
            string modeLogic = @"
extension Convert
{
    function ToInt(value)
    {
        return 100 + value;
    }
}

class Main
{
    function Init()
    {
    }
    
    function TestUserConvert()
    {
        return Convert.ToInt(5.7);
    }
    
    function TestBaseLogicConvert()
    {
        return BaseExtension.UseConvertBuiltin();
    }
}";

            Debug.Log("====== COMPILING ======");
            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Mode.cl", modeLogic, CustomLogicSourceType.ModeLogic));
            
            Debug.Log("====== CREATING EVALUATOR ======");
            var offlineEvaluator = new OfflineCustomLogicEvaluator(compiler);
            
            // Access internal state for debugging (using reflection if needed)
            var evaluatorField = typeof(OfflineCustomLogicEvaluator).GetField("_evaluator", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (evaluatorField != null)
            {
                var evaluator = evaluatorField.GetValue(offlineEvaluator);
                Debug.Log($"Evaluator type: {evaluator?.GetType().Name}");
                
                // Try to access _staticClasses and _namespacedStaticClasses
                var staticClassesField = evaluator?.GetType().GetField("_staticClasses",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var namespacedField = evaluator?.GetType().GetField("_namespacedStaticClasses",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (staticClassesField != null)
                {
                    var staticClasses = staticClassesField.GetValue(evaluator) as System.Collections.IDictionary;
                    Debug.Log($"_staticClasses count: {staticClasses?.Count ?? 0}");
                    
                    if (staticClasses != null && staticClasses.Contains("Convert"))
                    {
                        var convertInstance = staticClasses["Convert"];
                        Debug.Log($"Convert in _staticClasses: type={convertInstance?.GetType().Name}, isBuiltin={convertInstance is BuiltinClassInstance}");
                    }
                    
                    if (staticClasses != null && staticClasses.Contains("BaseExtension"))
                    {
                        var baseExt = staticClasses["BaseExtension"] as CustomLogicClassInstance;
                        Debug.Log($"BaseExtension in _staticClasses: namespace={baseExt?.Namespace}");
                    }
                }
                
                if (namespacedField != null)
                {
                    var namespacedClasses = namespacedField.GetValue(evaluator) as System.Collections.IDictionary;
                    Debug.Log($"_namespacedStaticClasses count: {namespacedClasses?.Count ?? 0}");
                    
                    if (namespacedClasses != null && namespacedClasses.Contains("Convert"))
                    {
                        var convertDict = namespacedClasses["Convert"] as System.Collections.IDictionary;
                        Debug.Log($"Convert namespaced versions: {convertDict?.Count ?? 0}");
                    }
                }
            }
            
            var mainInstance = offlineEvaluator.GetMainInstance();
            Debug.Log($"Main instance namespace: {mainInstance?.Namespace}");
            
            Debug.Log("====== TEST 1: User code using Convert ======");
            var userResult = offlineEvaluator.EvaluateMethod(mainInstance, "TestUserConvert");
            Debug.Log($"User result: {userResult} (expected: 105.7)");
            
            Debug.Log("====== TEST 2: BaseLogic using Convert ======");
            var baseResult = offlineEvaluator.EvaluateMethod(mainInstance, "TestBaseLogicConvert");
            Debug.Log($"Base result: {baseResult} (expected: 5)");
            
            Debug.Log("====== TEST COMPLETE ======");
            
            // Provide helpful failure messages
            if (baseResult != null && baseResult is float && (float)baseResult != 5)
            {
                Assert.Fail($"BaseExtension is using wrong Convert version. " +
                    $"Got {baseResult} but expected 5. " +
                    $"This means BaseExtension is using the user-defined Convert instead of the builtin. " +
                    $"Check debug logs to see if BaseExtension has namespace=BaseLogic set correctly.");
            }
            
            if (userResult != null && userResult is float && (float)userResult != 105.7f)
            {
                Assert.Fail($"Main is using wrong Convert version. " +
                    $"Got {userResult} but expected 105.7. " +
                    $"This means Main is NOT using the user-defined Convert.");
            }
            
            // Assertions
            Assert.AreEqual(105.7f, userResult, "User code should use user-defined Convert");
            Assert.AreEqual(5, baseResult, "BaseLogic should use builtin Convert");
        }
    }
}
