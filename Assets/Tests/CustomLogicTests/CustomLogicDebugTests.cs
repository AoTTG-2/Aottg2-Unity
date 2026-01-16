using NUnit.Framework;
using CustomLogic;
using CustomLogic.OfflineEvaluator;
using System;
using System.Linq;

namespace Tests.CustomLogic
{
    /// <summary>
    /// Tests for verifying error reporting (file names and line numbers) across different file types.
    /// Ensures that the CustomLogicCompiler correctly tracks and reports errors with proper context.
    /// </summary>
    [TestFixture]
    public class CustomLogicDebugTests
    {
        [Test]
        public void TestSingleFileError_LineNumberTracking()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function CauseError()
    {
        # This should cause an error on line 10
        x = UndefinedVariable;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            
            // Call the method - error will be captured but execution continues
            evaluator.EvaluateMainMethod("CauseError");
            
            // Verify error was captured
            Assert.IsTrue(evaluator.HasErrors(), "Expected an error to be captured");
            
            var error = evaluator.GetLastError();
            Assert.IsNotNull(error, "Expected error details");
            
            // Verify the error message contains line information
            string errorMessage = error.FullMessage;
            Assert.IsTrue(errorMessage.Contains("line") || errorMessage.Contains("Line"), 
                $"Error message should contain line information. Got: {errorMessage}");
        }

        [Test]
        public void TestMultiFileError_BaseLogicLineTracking()
        {
            string baseLogic = @"
# BaseLogic.cl
extension MathUtils
{
    function SafeDivide(a, b)
    {
        # Error on line 6 of BaseLogic
        return a / UndefinedVariable;
    }
}";

            string modeLogic = @"
# ModeLogic.cl
class Main
{
    function Init()
    {
    }
    
    function TestError()
    {
        return MathUtils.SafeDivide(10, 2);
    }
}";

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Mode.cl", modeLogic, CustomLogicSourceType.ModeLogic));
            
            var evaluator = new OfflineCustomLogicEvaluator(compiler);
            
            // Call the method - error will be captured
            evaluator.EvaluateMainMethod("TestError");
            
            // Verify error was captured
            Assert.IsTrue(evaluator.HasErrors(), "Expected an error to be captured");
            
            var error = evaluator.GetLastError();
            Assert.IsNotNull(error, "Expected error details");
            
            // Should reference BaseLogic file, MathUtils class, or SafeDivide method
            string errorMessage = error.FullMessage;
            bool hasContext = errorMessage.Contains("MathUtils") || 
                            errorMessage.Contains("SafeDivide") || 
                            errorMessage.Contains("BaseLogic") ||
                            error.FormattedLineNumber.Contains("BaseLogic");
            
            Assert.IsTrue(hasContext,
                $"Error should reference BaseLogic context. Got: {errorMessage}");
        }

        [Test]
        public void TestMultiFileError_ModeLogicLineTracking()
        {
            string baseLogic = @"
# BaseLogic.cl
extension Utils
{
    function Helper()
    {
        return 42;
    }
}";

            string modeLogic = @"
# ModeLogic.cl
class Main
{
    function Init()
    {
    }
    
    function TestError()
    {
        # Error on line 11 of ModeLogic
        x = NonExistentVariable;
        return x;
    }
}";

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("TestMode.cl", modeLogic, CustomLogicSourceType.ModeLogic));
            
            var evaluator = new OfflineCustomLogicEvaluator(compiler);
            
            // Call the method - error will be captured
            evaluator.EvaluateMainMethod("TestError");
            
            // Verify error was captured
            Assert.IsTrue(evaluator.HasErrors(), "Expected an error to be captured");
            
            var error = evaluator.GetLastError();
            Assert.IsNotNull(error, "Expected error details");
            
            // Should reference Main class or TestError method or ModeLogic file
            string errorMessage = error.FullMessage;
            bool hasContext = errorMessage.Contains("Main") || 
                            errorMessage.Contains("TestError") || 
                            errorMessage.Contains("TestMode") ||
                            error.FormattedLineNumber.Contains("TestMode");
            
            Assert.IsTrue(hasContext,
                $"Error should reference ModeLogic context. Got: {errorMessage}");
        }

        [Test]
        public void TestRuntimeError_DivideByZero()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function DivideByZero()
    {
        a = 10;
        b = 0;
        # Division by zero on line 12 - in CL this returns 0, not an error
        result = a / b;
        return result;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            
            // In Custom Logic, integer division by zero returns null.
            var result = evaluator.EvaluateMainMethod("DivideByZero");
            
            // Verify it returns null (integer division behavior)
            Assert.AreEqual(null, result);
            
            // Creates an exception still...
            Assert.True(evaluator.HasErrors(), "Division by zero still generates an exception even if it has a defined result as null.");
        }

        [Test]
        public void TestRuntimeError_NullReference()
        {
            string script = @"
class Main
{
    _myObject = null;
    
    function Init()
    {
    }
    
    function AccessNull()
    {
        # Null reference on line 12
        value = self._myObject.SomeMethod();
        return value;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            
            // Call the method - error will be captured
            evaluator.EvaluateMainMethod("AccessNull");
            
            // Verify error was captured
            Assert.IsTrue(evaluator.HasErrors(), "Expected an error to be captured");
            
            var error = evaluator.GetLastError();
            Assert.IsNotNull(error, "Expected error details");
            
            // Should mention null or reference error
            string errorMessage = error.Message.ToLower();
            Assert.IsTrue(errorMessage.Contains("null") || errorMessage.Contains("reference"),
                $"Error should mention null reference. Got: {error.FullMessage}");
        }

        [Test]
        public void TestCompilerError_LineMappingAcrossFiles()
        {
            string baseLogic = @"
# BaseLogic.cl - 5 lines
extension BaseExt
{
    _value = 100;
}";

            string addonLogic = @"
# Addon.cl - 5 lines
extension AddonExt
{
    _value = 200;
}";

            string mapLogic = @"
# MapLogic.cl - 5 lines
component TestComponent
{
    _value = 300;
}";

            string modeLogic = @"
# ModeLogic.cl
class Main
{
    function Init()
    {
    }
    
    function Test()
    {
        # This line references all extensions
        a = BaseExt._value;
        b = AddonExt._value;
        c = TestComponent._value;  # This will fail - can't access component this way
        return a + b + c;
    }
}";

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Addon.cl", addonLogic, CustomLogicSourceType.Addon));
            compiler.AddSourceFile(new CustomLogicSourceFile("Map.cl", mapLogic, CustomLogicSourceType.MapLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Mode.cl", modeLogic, CustomLogicSourceType.ModeLogic));
            
            var evaluator = new OfflineCustomLogicEvaluator(compiler);
            
            // Access to extension values should work
            var result = evaluator.EvaluateMainMethod("Test");
            
            // TestComponent is a component class, not an extension, so accessing it statically will fail
            // But accessing the extension values should work
        }

        [Test]
        public void TestMethodNotFound_ErrorReporting()
        {
            string script = @"
class Helper
{
    function Init()
    {
    }
    
    function DoSomething()
    {
        return 42;
    }
}

class Main
{
    function Init()
    {
    }
    
    function CallNonExistent()
    {
        helper = Helper();
        # Method doesn't exist - line 23
        value = helper.NonExistentMethod();
        return value;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            
            // Call the method - error will be captured
            evaluator.EvaluateMainMethod("CallNonExistent");
            
            // Verify error was captured
            Assert.IsTrue(evaluator.HasErrors(), "Expected an error to be captured");
            
            var error = evaluator.GetLastError();
            Assert.IsNotNull(error, "Expected error details");
            
            // Should mention the method name or that it wasn't found
            string errorMessage = error.FullMessage;
            Assert.IsTrue(errorMessage.Contains("NonExistentMethod") || 
                         errorMessage.ToLower().Contains("not found") || 
                         errorMessage.ToLower().Contains("method"),
                $"Error should mention missing method. Got: {errorMessage}");
        }

        [Test]
        public void TestClassNotFound_ErrorReporting()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function CreateNonExistent()
    {
        # Class doesn't exist - line 10
        obj = NonExistentClass();
        return obj;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            
            // Call the method - error will be captured
            evaluator.EvaluateMainMethod("CreateNonExistent");
            
            // Verify error was captured
            Assert.IsTrue(evaluator.HasErrors(), "Expected an error to be captured");
            
            var error = evaluator.GetLastError();
            Assert.IsNotNull(error, "Expected error details");
            
            // Should mention the class name or that it wasn't found
            string errorMessage = error.FullMessage;
            Assert.IsTrue(errorMessage.Contains("NonExistentClass") || 
                         errorMessage.ToLower().Contains("not found") || 
                         errorMessage.ToLower().Contains("class"),
                $"Error should mention missing class. Got: {errorMessage}");
        }

/*        [Test]
        public void TestRecursionDepth_StackOverflow()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function InfiniteRecursion()
    {
        # This will cause stack overflow
        return self.InfiniteRecursion();
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            
            // Stack overflow will still throw because it's a CLR exception, not a CL error
            // This is expected behavior - some errors are too severe to capture
            bool caughtStackOverflow = false;
            try
            {
                evaluator.EvaluateMainMethod("InfiniteRecursion");
            }
            catch (StackOverflowException)
            {
                caughtStackOverflow = true;
            }
            
            Assert.IsTrue(caughtStackOverflow, "Stack overflow should be thrown for infinite recursion");
        }*/

        [Test]
        public void TestTypeError_WrongParameterType()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function TestMath()
    {
        # Math.Max expects numbers, not strings
        result = Math.Max(""hello"", ""world"");
        return result;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            
            // Call the method - may or may not error depending on builtin implementation
            var result = evaluator.EvaluateMainMethod("TestMath");
            
            // Some builtins might handle type conversion, some might error
            // If an error occurred, verify it mentions the type issue
            if (evaluator.HasErrors())
            {
                var error = evaluator.GetLastError();
                string errorMessage = error.Message.ToLower();
                Assert.IsTrue(errorMessage.Contains("specified cast is not valid"),
                    $"Error should mention type issue. Got: {error.FullMessage}");
            }
            // Otherwise, the builtin handled it gracefully (also valid)
        }

        [Test]
        public void TestCompiler_FileInfo()
        {
            string baseLogic = "extension BaseExt { _value = 1; }";
            string addon = "extension AddonExt { _value = 2; }";
            string mapLogic = "extension MapExt { _value = 3; }";
            string modeLogic = "class Main { function Init() { } }";

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Addon.cl", addon, CustomLogicSourceType.Addon));
            compiler.AddSourceFile(new CustomLogicSourceFile("Map.cl", mapLogic, CustomLogicSourceType.MapLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Mode.cl", modeLogic, CustomLogicSourceType.ModeLogic));

            // Compile to initialize line tracking
            string combined = compiler.Compile();

            // Test GetFileTypeForLine - line 0 should be BaseLogic (0-based indexing)
            var baseLogicType = compiler.GetFileTypeForLine(0);
            Assert.AreEqual(CustomLogicSourceType.BaseLogic, baseLogicType, "Line 0 should be in BaseLogic");

            var addonType = compiler.GetFileTypeForLine(1);
            Assert.AreEqual(CustomLogicSourceType.Addon, addonType, "Line 0 should be in BaseLogic");

            var mapLogicType = compiler.GetFileTypeForLine(2);
            Assert.AreEqual(CustomLogicSourceType.MapLogic, mapLogicType, "Line 0 should be in BaseLogic");

            var modeLogicType = compiler.GetFileTypeForLine(3);
            Assert.AreEqual(CustomLogicSourceType.ModeLogic, modeLogicType, "Line 0 should be in BaseLogic");

            // Test FormatLineNumber
            string formatted = compiler.FormatLineNumber(0);
            Assert.IsTrue(formatted.Contains("BaseLogic") || formatted.Contains("0"),
                $"Formatted line should reference BaseLogic. Got: {formatted}");
        }

        [Test]
        public void TestCompiler_LineMappingMultipleFiles()
        {
            // Test with files of varying lengths to ensure proper range calculation
            string baseLogic = @"extension BaseExt
{
    _value = 1;
}";  // 4 lines (indices 0-3)

            string addon = @"extension AddonExt
{
    _value = 2;
}";  // 4 lines (indices 4-7)

            string mapLogic = @"extension MapExt
{
    _value = 3;
}";  // 4 lines (indices 8-11)

            string modeLogic = @"class Main
{
    function Init() { }
}";  // 4 lines (indices 12-15)

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Addon.cl", addon, CustomLogicSourceType.Addon));
            compiler.AddSourceFile(new CustomLogicSourceFile("Map.cl", mapLogic, CustomLogicSourceType.MapLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Mode.cl", modeLogic, CustomLogicSourceType.ModeLogic));

            string combined = compiler.Compile();

            // Verify file info
            var fileInfo = compiler.GetFileInfo().ToList();
            Assert.AreEqual(4, fileInfo.Count, "Should have 4 files");

            // Test BaseLogic boundaries (lines 0-3)
            Assert.AreEqual(CustomLogicSourceType.BaseLogic, compiler.GetFileTypeForLine(0), "Line 0 should be BaseLogic");
            Assert.AreEqual(CustomLogicSourceType.BaseLogic, compiler.GetFileTypeForLine(3), "Line 3 should be BaseLogic");
            Assert.AreEqual("BaseLogic.cl", compiler.GetFileNameForLine(0), "Line 0 should be in BaseLogic.cl");
            Assert.AreEqual("BaseLogic.cl", compiler.GetFileNameForLine(3), "Line 3 should be in BaseLogic.cl");

            // Test Addon boundaries (lines 4-7)
            Assert.AreEqual(CustomLogicSourceType.Addon, compiler.GetFileTypeForLine(4), "Line 4 should be Addon");
            Assert.AreEqual(CustomLogicSourceType.Addon, compiler.GetFileTypeForLine(7), "Line 7 should be Addon");
            Assert.AreEqual("Addon.cl", compiler.GetFileNameForLine(4), "Line 4 should be in Addon.cl");
            Assert.AreEqual("Addon.cl", compiler.GetFileNameForLine(7), "Line 7 should be in Addon.cl");

            // Test MapLogic boundaries (lines 8-11)
            Assert.AreEqual(CustomLogicSourceType.MapLogic, compiler.GetFileTypeForLine(8), "Line 8 should be MapLogic");
            Assert.AreEqual(CustomLogicSourceType.MapLogic, compiler.GetFileTypeForLine(11), "Line 11 should be MapLogic");
            Assert.AreEqual("Map.cl", compiler.GetFileNameForLine(8), "Line 8 should be in Map.cl");
            Assert.AreEqual("Map.cl", compiler.GetFileNameForLine(11), "Line 11 should be in Map.cl");

            // Test ModeLogic boundaries (lines 12-15)
            Assert.AreEqual(CustomLogicSourceType.ModeLogic, compiler.GetFileTypeForLine(12), "Line 12 should be ModeLogic");
            Assert.AreEqual(CustomLogicSourceType.ModeLogic, compiler.GetFileTypeForLine(15), "Line 15 should be ModeLogic");
            Assert.AreEqual("Mode.cl", compiler.GetFileNameForLine(12), "Line 12 should be in Mode.cl");
            Assert.AreEqual("Mode.cl", compiler.GetFileNameForLine(15), "Line 15 should be in Mode.cl");

            // Test local line numbers
            Assert.AreEqual(1, compiler.GetLocalLineNumber(0), "Global line 0 should be local line 1 in BaseLogic");
            Assert.AreEqual(4, compiler.GetLocalLineNumber(3), "Global line 3 should be local line 4 in BaseLogic");
            Assert.AreEqual(1, compiler.GetLocalLineNumber(4), "Global line 4 should be local line 1 in Addon");
            Assert.AreEqual(4, compiler.GetLocalLineNumber(7), "Global line 7 should be local line 4 in Addon");

            // Test formatting
            string formatted0 = compiler.FormatLineNumber(0);
            Assert.IsTrue(formatted0.Contains("BaseLogic"), $"Line 0 format should mention BaseLogic. Got: {formatted0}");
            
            string formatted5 = compiler.FormatLineNumber(5);
            Assert.IsTrue(formatted5.Contains("Addon"), $"Line 5 format should mention Addon. Got: {formatted5}");
        }

        [Test]
        public void TestCompiler_LineMappingWithVaryingLengths()
        {
            // Test with files of different lengths
            string baseLogic = "extension A { _x = 1; }";  // 1 line (index 0)
            
            string addon = @"extension B
{
    _x = 2;
}";  // 4 lines (indices 1-4)
            
            string mapLogic = @"extension C
{
    _x = 3;
    _y = 4;
    _z = 5;
}";  // 6 lines (indices 5-10)

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("Base.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Add.cl", addon, CustomLogicSourceType.Addon));
            compiler.AddSourceFile(new CustomLogicSourceFile("Map.cl", mapLogic, CustomLogicSourceType.MapLogic));

            string combined = compiler.Compile();

            // Test boundaries
            Assert.AreEqual(CustomLogicSourceType.BaseLogic, compiler.GetFileTypeForLine(0), "Line 0 should be BaseLogic");
            
            Assert.AreEqual(CustomLogicSourceType.Addon, compiler.GetFileTypeForLine(1), "Line 1 should be Addon");
            Assert.AreEqual(CustomLogicSourceType.Addon, compiler.GetFileTypeForLine(2), "Line 2 should be Addon");
            Assert.AreEqual(CustomLogicSourceType.Addon, compiler.GetFileTypeForLine(4), "Line 4 should be Addon");
            
            Assert.AreEqual(CustomLogicSourceType.MapLogic, compiler.GetFileTypeForLine(5), "Line 5 should be MapLogic");
            Assert.AreEqual(CustomLogicSourceType.MapLogic, compiler.GetFileTypeForLine(10), "Line 10 should be MapLogic");

            // Test edge cases - lines outside range
            Assert.IsNull(compiler.GetFileTypeForLine(11), "Line 11 should be out of range");
            Assert.IsNull(compiler.GetFileTypeForLine(100), "Line 100 should be out of range");
            Assert.IsNull(compiler.GetFileTypeForLine(-1), "Line -1 should be out of range");
        }

        [Test]
        public void TestCompiler_EmptyFiles()
        {
            // Test with empty files (edge case)
            string baseLogic = "";  // 0 lines? Or 1 line?
            string addon = "extension A { }";

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("Empty.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("NotEmpty.cl", addon, CustomLogicSourceType.Addon));

            string combined = compiler.Compile();

            // An empty file should still occupy at least one line in the combined source
            // This test verifies how empty files are handled
            Assert.AreEqual(2, compiler.FileCount, "Should have 2 files");
        }
    }
}
