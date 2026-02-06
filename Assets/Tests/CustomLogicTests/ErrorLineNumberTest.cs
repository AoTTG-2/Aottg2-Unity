using NUnit.Framework;
using CustomLogic;
using CustomLogic.OfflineEvaluator;
using UnityEngine;
using System.Linq;

namespace Test.CustomLogic
{
    [TestFixture]
    public class ErrorLineNumberTest
    {
        [Test]
        public void TestErrorLineNumbers_MultipleFiles()
        {
            // BaseLogic file with error on line 5
            string baseLogic = @"
extension BaseExtension
{
    function CauseErrorLine5()
    {
        result = 10 / 0;
        return result;
    }
}";

            // Addon file with error on line 8
            string addonLogic = @"
extension AddonExtension
{
    function DoSomething()
    {
        x = 5;
    }
    
    function CauseErrorLine8()
    {
        undefinedVariable = undefinedVariable + 1;
    }
}";

            // MapLogic file with error on line 8
            string mapLogic = @"
class MapHelper
{
    function Init()
    {
        x = 10;
        y = null;
        result = x / y;
    }
}";

            // Mode logic with error on line 12
            string modeLogic = @"
class Main
{
    function Init()
    {
    }
    
    function TestMethod()
    {
        a = 5;
        b = 10;
        c = String.Substring(""test"", 100);
        return c;
    }
    
    function CallBaseExtension()
    {
        return BaseExtension.CauseErrorLine5();
    }
    
    function CallAddonExtension()
    {
        return AddonExtension.CauseErrorLine8();
    }
}";

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Addon.cl", addonLogic, CustomLogicSourceType.Addon));
            compiler.AddSourceFile(new CustomLogicSourceFile("Map.cl", mapLogic, CustomLogicSourceType.MapLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Mode.cl", modeLogic, CustomLogicSourceType.ModeLogic));
            
            var offlineEvaluator = new OfflineCustomLogicEvaluator(compiler);
            var mainInstance = offlineEvaluator.GetMainInstance();
            
            // Test 1: Error in MapLogic Init (line 8)
            // Create instance without calling Init, then call it explicitly to avoid double execution
            offlineEvaluator.ClearErrors();
            var mapHelperInstance = offlineEvaluator.CreateClassInstance("MapHelper", init: false);
            offlineEvaluator.EvaluateMethod(mapHelperInstance, "Init");
            
            var mapErrors = offlineEvaluator.GetCapturedErrors();
            Debug.Log($"MapHelper errors count: {mapErrors.Count}");
            if (mapErrors.Count > 0)
            {
                var error = mapErrors[0];
                Debug.Log($"MapHelper error: Line={error.LineNumber}, FormattedLine='{error.FormattedLineNumber}', Namespace={error.Namespace}, Message={error.Message}");
            }
            
            Assert.AreEqual(1, mapErrors.Count, "Should have captured 1 error from MapHelper.Init");
            Assert.AreEqual("MapHelper", mapErrors[0].ClassName, "Error should be in MapHelper class");
            Assert.AreEqual("Init", mapErrors[0].MethodName, "Error should be in Init method");
            Assert.AreEqual(8, mapErrors[0].LineNumber, "Error should be on line 8 (x / y)");
            Assert.IsTrue(mapErrors[0].FormattedLineNumber.Contains("Map.cl"), "Error should reference Map.cl file");
            Assert.AreEqual(CustomLogicSourceType.MapLogic, mapErrors[0].Namespace, "Error should have MapLogic namespace");
            
            // Test 2: Error in ModeLogic TestMethod (line 12)
            offlineEvaluator.ClearErrors();
            offlineEvaluator.EvaluateMethod(mainInstance, "TestMethod");
            
            var modeErrors = offlineEvaluator.GetCapturedErrors();
            Debug.Log($"TestMethod errors count: {modeErrors.Count}");
            if (modeErrors.Count > 0)
            {
                var error = modeErrors[0];
                Debug.Log($"TestMethod error: Line={error.LineNumber}, FormattedLine='{error.FormattedLineNumber}', Namespace={error.Namespace}, Message={error.Message}");
            }
            
            Assert.AreEqual(1, modeErrors.Count, "Should have captured 1 error from TestMethod");
            Assert.AreEqual("Main", modeErrors[0].ClassName, "Error should be in Main class");
            Assert.AreEqual("TestMethod", modeErrors[0].MethodName, "Error should be in TestMethod");
            Assert.AreEqual(12, modeErrors[0].LineNumber, "Error should be on line 12 (String.Substring)");
            Assert.IsTrue(modeErrors[0].FormattedLineNumber.Contains("Mode.cl"), "Error should reference Mode.cl file");
            Assert.AreEqual(CustomLogicSourceType.ModeLogic, modeErrors[0].Namespace, "Error should have ModeLogic namespace");
            
            // Test 3: Error in BaseLogic via call from Main (line 6 in BaseLogic)
            offlineEvaluator.ClearErrors();
            offlineEvaluator.EvaluateMethod(mainInstance, "CallBaseExtension");
            
            var baseErrors = offlineEvaluator.GetCapturedErrors();
            Debug.Log($"BaseExtension errors count: {baseErrors.Count}");
            if (baseErrors.Count > 0)
            {
                var error = baseErrors[0];
                Debug.Log($"BaseExtension error: Line={error.LineNumber}, FormattedLine='{error.FormattedLineNumber}', Namespace={error.Namespace}, Message={error.Message}");
            }
            
            Assert.AreEqual(1, baseErrors.Count, "Should have captured 1 error from BaseExtension");
            Assert.AreEqual("BaseExtension", baseErrors[0].ClassName, "Error should be in BaseExtension class");
            Assert.AreEqual("CauseErrorLine5", baseErrors[0].MethodName, "Error should be in CauseErrorLine5 method");
            Assert.AreEqual(6, baseErrors[0].LineNumber, "Error should be on line 6 (division by zero)");
            Assert.IsTrue(baseErrors[0].FormattedLineNumber.Contains("BaseLogic.cl"), "Error should reference BaseLogic.cl file");
            Assert.AreEqual(CustomLogicSourceType.BaseLogic, baseErrors[0].Namespace, "Error should have BaseLogic namespace");
            
            // Test 4: Error in Addon via call from Main (line 11 in Addon)
            offlineEvaluator.ClearErrors();
            offlineEvaluator.EvaluateMethod(mainInstance, "CallAddonExtension");
            
            var addonErrors = offlineEvaluator.GetCapturedErrors();
            Debug.Log($"AddonExtension errors count: {addonErrors.Count}");
            if (addonErrors.Count > 0)
            {
                var error = addonErrors[0];
                Debug.Log($"AddonExtension error: Line={error.LineNumber}, FormattedLine='{error.FormattedLineNumber}', Namespace={error.Namespace}, Message={error.Message}");
            }
            
            Assert.AreEqual(1, addonErrors.Count, "Should have captured 1 error from AddonExtension");
            Assert.AreEqual("AddonExtension", addonErrors[0].ClassName, "Error should be in AddonExtension class");
            Assert.AreEqual("CauseErrorLine8", addonErrors[0].MethodName, "Error should be in CauseErrorLine8 method");
            Assert.AreEqual(11, addonErrors[0].LineNumber, "Error should be on line 11 (undefinedVariable)");
            Assert.IsTrue(addonErrors[0].FormattedLineNumber.Contains("Addon.cl"), "Error should reference Addon.cl file");
            Assert.AreEqual(CustomLogicSourceType.Addon, addonErrors[0].Namespace, "Error should have Addon namespace");
        }
        
        [Test]
        public void TestErrorLineNumbers_NestedCalls()
        {
            // Test that errors in nested method calls report the correct line number
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function Level1()
    {
        return self.Level2();
    }
    
    function Level2()
    {
        return self.Level3();
    }
    
    function Level3()
    {
        value = List([1, 2, 3]);
        return value.Get(100);
    }
}";

            var offlineEvaluator = new OfflineCustomLogicEvaluator(script);
            var mainInstance = offlineEvaluator.GetMainInstance();
            
            offlineEvaluator.ClearErrors();
            offlineEvaluator.EvaluateMethod(mainInstance, "Level1");
            
            var errors = offlineEvaluator.GetCapturedErrors();
            Debug.Log($"Nested call errors count: {errors.Count}");
            if (errors.Count > 0)
            {
                var error = errors[0];
                Debug.Log($"Nested error: Line={error.LineNumber}, Class={error.ClassName}, Method={error.MethodName}, Message={error.Message}");
            }
            
            Assert.AreEqual(1, errors.Count, "Should have captured 1 error");
            Assert.AreEqual("Main", errors[0].ClassName, "Error should be in Main class");
            Assert.AreEqual("Level3", errors[0].MethodName, "Error should be in Level3 method (where actual error occurred)");
            // Now builtin method errors should report the correct line number!
            Assert.AreEqual(20, errors[0].LineNumber, "Error should be on line 20 (value.Get(100))");
            Assert.IsTrue(errors[0].LineNumber > 0, "Builtin method error should have non-zero line number");
            Assert.IsNotEmpty(errors[0].FormattedLineNumber, "Error should have formatted line number");
        }
        
        [Test]
        public void TestErrorLineNumbers_MultipleErrorsInSameMethod()
        {
            // Test multiple errors in the same method
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function HasMultipleErrors()
    {
        list1 = List([1, 2]);
        item1 = list1.Get(999);
        
        list2 = List([3, 4]);
        item2 = list2.Get(888);
        
        return item1 + item2;
    }
}";

            var offlineEvaluator = new OfflineCustomLogicEvaluator(script);
            var mainInstance = offlineEvaluator.GetMainInstance();
            
            offlineEvaluator.ClearErrors();
            offlineEvaluator.EvaluateMethod(mainInstance, "HasMultipleErrors");
            
            var errors = offlineEvaluator.GetCapturedErrors();
            Debug.Log($"Multiple errors count: {errors.Count}");
            foreach (var error in errors)
            {
                Debug.Log($"Error: Line={error.LineNumber}, Message={error.Message}");
            }
            
            // Should have at least 1 error (execution stops at first error)
            Assert.GreaterOrEqual(errors.Count, 1, "Should have captured at least 1 error");
            Assert.AreEqual(10, errors[0].LineNumber, "First error should be on line 10 (list1.Get(999))");
        }
        
        [Test]
        public void TestErrorLineNumbers_FormatContainsFilename()
        {
            // Verify that FormattedLineNumber includes the filename
            string baseLogic = @"
extension Helper
{
    function CauseError()
    {
        x = 1 / 0;
    }
}";

            string modeLogic = @"
class Main
{
    function Init()
    {
        Helper.CauseError();
    }
}";

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Mode.cl", modeLogic, CustomLogicSourceType.ModeLogic));
            
            var offlineEvaluator = new OfflineCustomLogicEvaluator(compiler);
            var mainInstance = offlineEvaluator.GetMainInstance();
            
            var errors = offlineEvaluator.GetCapturedErrors();
            Debug.Log($"Filename test errors count: {errors.Count}");
            if (errors.Count > 0)
            {
                var error = errors[0];
                Debug.Log($"Error formatted line: '{error.FormattedLineNumber}'");
            }
            
            Assert.GreaterOrEqual(errors.Count, 1, "Should have captured at least 1 error");
            Assert.IsNotEmpty(errors[0].FormattedLineNumber, "FormattedLineNumber should not be empty");
            Assert.IsTrue(
                errors[0].FormattedLineNumber.Contains("BaseLogic.cl") || 
                errors[0].FormattedLineNumber.Contains("Mode.cl"),
                $"FormattedLineNumber should contain filename, got: '{errors[0].FormattedLineNumber}'"
            );
        }
        
        [Test]
        public void TestErrorLineNumbers_BuiltinMethods()
        {
            // Specifically test that builtin method errors report correct line numbers
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function TestListGet()
    {
        myList = List([1, 2, 3]);
        value = myList.Get(999);
        return value;
    }
    
    function TestStringSubstring()
    {
        text = ""hello"";
        sub = String.Substring(text, 100);
        return sub;
    }
    
    function TestDictGet()
    {
        myDict = Dict();
        value = myDict.Get(""nonexistent"");
        return value;
    }
}";

            var offlineEvaluator = new OfflineCustomLogicEvaluator(script);
            var mainInstance = offlineEvaluator.GetMainInstance();
            
            // Test 1: List.Get error
            offlineEvaluator.ClearErrors();
            offlineEvaluator.EvaluateMethod(mainInstance, "TestListGet");
            
            var listErrors = offlineEvaluator.GetCapturedErrors();
            Debug.Log($"List.Get errors count: {listErrors.Count}");
            if (listErrors.Count > 0)
            {
                var error = listErrors[0];
                Debug.Log($"List.Get error: Line={error.LineNumber}, FormattedLine='{error.FormattedLineNumber}', Message={error.Message}");
            }
            
            Assert.AreEqual(1, listErrors.Count, "Should have captured 1 error from List.Get");
            Assert.AreEqual(10, listErrors[0].LineNumber, "Error should be on line 10 (myList.Get(999))");
            Assert.IsNotEmpty(listErrors[0].FormattedLineNumber, "Error should have formatted line number");
            Assert.AreEqual("Get", listErrors[0].MethodName, "Error method should be Get");
            
            // Test 2: String.Substring error  
            offlineEvaluator.ClearErrors();
            offlineEvaluator.EvaluateMethod(mainInstance, "TestStringSubstring");
            
            var stringErrors = offlineEvaluator.GetCapturedErrors();
            Debug.Log($"String.Substring errors count: {stringErrors.Count}");
            if (stringErrors.Count > 0)
            {
                var error = stringErrors[0];
                Debug.Log($"String.Substring error: Line={error.LineNumber}, FormattedLine='{error.FormattedLineNumber}', Message={error.Message}");
            }
            
            Assert.AreEqual(1, stringErrors.Count, "Should have captured 1 error from String.Substring");
            Assert.AreEqual(17, stringErrors[0].LineNumber, "Error should be on line 17 (String.Substring call)");
            Assert.IsNotEmpty(stringErrors[0].FormattedLineNumber, "Error should have formatted line number");
            Assert.AreEqual("Substring", stringErrors[0].MethodName, "Error method should be Substring");
        }
    }
}
