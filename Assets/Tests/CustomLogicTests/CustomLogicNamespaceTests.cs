using NUnit.Framework;
using CustomLogic;
using CustomLogic.OfflineEvaluator;
using System;

namespace Test.CustomLogic
{
    [TestFixture]
    public class CustomLogicNamespaceTests
    {
        [Test]
        public void TestBuiltinIsolation_Vector3OverrideInUserCode()
        {
            // BaseLogic uses Vector3 builtin
            string baseLogic = @"
extension BaseExtension
{
    function UseVector3()
    {
        # This should use the C# Vector3 builtin
        v = Vector3(1, 2, 3);
        return v.X + v.Y + v.Z;
    }
}";

            // User code overrides Vector3
            string userCode = @"
class Vector3
{
    _value = 0;
    
    function Init(x)
    {
        self._value = x;
    }
    
    function Add()
    {
        return self._value + 100;
    }
}

class Main
{
    function Init()
    {
    }
    
    function TestUserVector()
    {
        # This should use the user-defined Vector3
        v = Vector3(5);
        return v.Add();
    }
    
    function TestBaseLogicVector()
    {
        # This should still use the builtin Vector3 from BaseExtension
        return BaseExtension.UseVector3();
    }
}";

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("UserMode.cl", userCode, CustomLogicSourceType.ModeLogic));
            
            var offlineEvaluator = new OfflineCustomLogicEvaluator(compiler);
            var mainInstance = offlineEvaluator.GetMainInstance();
            
            // User code uses user-defined Vector3
            var userResult = offlineEvaluator.EvaluateMethod(mainInstance, "TestUserVector");
            Assert.AreEqual(105, userResult); // 5 + 100
            
            // BaseLogic uses builtin Vector3
            var baseResult = offlineEvaluator.EvaluateMethod(mainInstance, "TestBaseLogicVector");
            Assert.AreEqual(6, baseResult); // 1 + 2 + 3
        }

        [Test]
        public void TestNamespaceIsolation_ComponentOverride()
        {
            // BaseLogic defines a Healthbar component
            string baseLogic = @"
# general
component HealthBar
{
    HP = 100;
    
    function GetHP()
    {
        return self.HP;
    }
}";

            // User code overrides Healthbar
            string userCode = @"
component HealthBar
{
    HP = 200;
    MaxHP = 300;
    
    function GetHP()
    {
        return self.HP + self.MaxHP;
    }
}

class Main
{
    function Init()
    {
    }
    
    function TestUserHealthBar()
    {
        hb = HealthBar();
        return hb.GetHP();
    }
}";

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("UserMode.cl", userCode, CustomLogicSourceType.ModeLogic));
            
            var offlineEvaluator = new OfflineCustomLogicEvaluator(compiler);
            var mainInstance = offlineEvaluator.GetMainInstance();
            
            // User code should use its own HealthBar definition (200 + 300 = 500)
            var result = offlineEvaluator.EvaluateMethod(mainInstance, "TestUserHealthBar");
            Assert.AreEqual(500, result);
        }

        [Test]
        public void TestNamespaceIsolation_StaticBuiltinOverride()
        {
            // BaseLogic uses Convert builtin
            string baseLogic = @"
extension BaseExtension
{
    function UseConvertBuiltin()
    {
        # This should use the C# Convert builtin
        # ToInt converts float to int
        result = Convert.ToInt(5.7);
        return result;
    }
}";

            // Mode logic overrides Convert with custom implementation
            string modeLogic = @"
extension Convert
{
    function ToInt(value)
    {
        # Custom implementation that adds 100
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
        # This should use the user-defined Convert
        return Convert.ToInt(5.7);
    }
    
    function TestBaseLogicConvert()
    {
        # This should still use the builtin Convert from BaseExtension
        return BaseExtension.UseConvertBuiltin();
    }
}";

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Mode.cl", modeLogic, CustomLogicSourceType.ModeLogic));
            
            var offlineEvaluator = new OfflineCustomLogicEvaluator(compiler);
            var mainInstance = offlineEvaluator.GetMainInstance();
            
            // User code uses user-defined Convert
            var userResult = offlineEvaluator.EvaluateMethod(mainInstance, "TestUserConvert");
            Assert.AreEqual(105.7f, userResult); // 100 + 5.7
            
            // BaseLogic uses builtin Convert
            var baseResult = offlineEvaluator.EvaluateMethod(mainInstance, "TestBaseLogicConvert");
            Assert.AreEqual(5, baseResult); // ToInt(5.7) = 5
        }

        [Test]
        public void TestCrossFileReferences_AddonVisible()
        {
            // Addon defines a shared class
            string addonLogic = @"
class SharedUtility
{
    function Init()
    {
    }
    
    function Double(x)
    {
        return x * 2;
    }
}";

            // Map logic uses addon
            string mapLogic = @"
class MapHelper
{
    function Init()
    {
    }
    
    function UseShared(x)
    {
        util = SharedUtility();
        return util.Double(x);
    }
}";

            // Mode logic uses both addon and map logic classes
            string modeLogic = @"
class Main
{
    function Init()
    {
    }
    
    function TestBoth()
    {
        # Should be able to use addon
        util = SharedUtility();
        a = util.Double(3);
        
        # Should be able to use map logic
        helper = MapHelper();
        b = helper.UseShared(4);
        
        return a + b;
    }
}";

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("Addon.cl", addonLogic, CustomLogicSourceType.Addon));
            compiler.AddSourceFile(new CustomLogicSourceFile("Map.maplogic", mapLogic, CustomLogicSourceType.MapLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Mode.cl", modeLogic, CustomLogicSourceType.ModeLogic));
            
            var offlineEvaluator = new OfflineCustomLogicEvaluator(compiler);
            var mainInstance = offlineEvaluator.GetMainInstance();
            
            // 3*2 + 4*2 = 6 + 8 = 14
            var result = offlineEvaluator.EvaluateMethod(mainInstance, "TestBoth");
            Assert.AreEqual(14, result);
        }

        [Test]
        public void TestCrossNamespaceMethodCheck_ConvertHasMethod()
        {
            // BaseLogic receives class instance from ModeLogic and checks for methods
            string baseLogic = @"
extension InspectionUtility
{
    function InspectInstance(instance)
    {
        # Check if instance has specific methods
        hasProcess = Convert.HasMethod(instance, ""Process"");
        hasValidate = Convert.HasMethod(instance, ""Validate"");
        hasNonExistent = Convert.HasMethod(instance, ""NonExistentMethod"");
        
        # Return count of methods found
        count = 0;
        if (hasProcess) {
            count = count + 1;
        }
        if (hasValidate) {
            count = count + 1;
        }
        if (hasNonExistent) {
            count = count + 1;
        }
            
        return count;
    }
    
    function CallMethodIfExists(instance, methodName)
    {
        if (Convert.HasMethod(instance, methodName))
        {
            # If method exists, call it
            result = instance.Process();
            return result;
        }
        return -1;
    }
}";

            // ModeLogic defines Main class with specific methods
            string modeLogic = @"
class Main
{
    _value = 42;
    
    function Init()
    {
    }
    
    function Process()
    {
        return self._value * 2;
    }
    
    function Validate()
    {
        return self._value > 0;
    }
    
    function TestInspection()
    {
        # Pass this Main instance to BaseLogic for inspection
        methodCount = InspectionUtility.InspectInstance(self);
        return methodCount;
    }
    
    function TestCallIfExists()
    {
        # BaseLogic checks if method exists and calls it
        result = InspectionUtility.CallMethodIfExists(self, ""Process"");
        return result;
    }
}";

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Mode.cl", modeLogic, CustomLogicSourceType.ModeLogic));
            
            var offlineEvaluator = new OfflineCustomLogicEvaluator(compiler);
            var mainInstance = offlineEvaluator.GetMainInstance();
            
            // Should find 2 methods (Process and Validate), not NonExistentMethod
            var methodCount = offlineEvaluator.EvaluateMethod(mainInstance, "TestInspection");
            Assert.AreEqual(2, methodCount, "Should find exactly 2 methods: Process and Validate");
            
            // Should successfully call Process() and return 84 (42 * 2)
            var processResult = offlineEvaluator.EvaluateMethod(mainInstance, "TestCallIfExists");
            Assert.AreEqual(84, processResult, "Process() should return 84 (42 * 2)");
        }

        [Test]
        public void TestCrossNamespaceMethodCheck_DifferentClassTypes()
        {
            // BaseLogic utility that inspects various class types
            string baseLogic = @"
extension TypeInspector
{
    function InspectAndDescribe(instance)
    {
        # Check for various common methods
        hasInit = Convert.HasMethod(instance, ""Init"");
        hasGetValue = Convert.HasMethod(instance, ""GetValue"");
        hasSetValue = Convert.HasMethod(instance, ""SetValue"");
        hasToString = Convert.HasMethod(instance, ""ToString"");
        
        # Build a description code
        code = 0;
        if (hasInit) {
            code = code + 1;
        }
        if (hasGetValue) {
            code = code + 10;
        }
        if (hasSetValue) {
            code = code + 100;
        }   
        if (hasToString) {
            code = code + 1000;
        }
            
        return code;
    }
}";

            // ModeLogic with various classes
            string modeLogic = @"
class SimpleData
{
    _value = 0;
    
    function Init(val)
    {
        self._value = val;
    }
    
    function GetValue()
    {
        return self._value;
    }
}

class ComplexData
{
    _value = 0;
    
    function Init(val)
    {
        self._value = val;
    }
    
    function GetValue()
    {
        return self._value;
    }
    
    function SetValue(val)
    {
        self._value = val;
    }
    
    function ToString()
    {
        return self._value;
    }
}

class Main
{
    function Init()
    {
    }
    
    function TestSimpleData()
    {
        obj = SimpleData(5);
        # SimpleData has Init and GetValue
        # Expected code: 1 (Init) + 10 (GetValue) = 11
        return TypeInspector.InspectAndDescribe(obj);
    }
    
    function TestComplexData()
    {
        obj = ComplexData(10);
        # ComplexData has Init, GetValue, SetValue, ToString
        # Expected code: 1 + 10 + 100 + 1000 = 1111
        return TypeInspector.InspectAndDescribe(obj);
    }
    
    function TestMainInstance()
    {
        # Main only has Init
        # Expected code: 1
        return TypeInspector.InspectAndDescribe(self);
    }
}";

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Mode.cl", modeLogic, CustomLogicSourceType.ModeLogic));
            
            var offlineEvaluator = new OfflineCustomLogicEvaluator(compiler);
            var mainInstance = offlineEvaluator.GetMainInstance();
            
            // SimpleData: Init + GetValue = 1 + 10 = 11
            var simpleResult = offlineEvaluator.EvaluateMethod(mainInstance, "TestSimpleData");
            Assert.AreEqual(11, simpleResult, "SimpleData should have Init (1) + GetValue (10) = 11");
            
            // ComplexData: Init + GetValue + SetValue + ToString = 1 + 10 + 100 + 1000 = 1111
            var complexResult = offlineEvaluator.EvaluateMethod(mainInstance, "TestComplexData");
            Assert.AreEqual(1111, complexResult, "ComplexData should have all methods = 1111");
            
            // Main: Only Init = 1
            var mainResult = offlineEvaluator.EvaluateMethod(mainInstance, "TestMainInstance");
            Assert.AreEqual(1, mainResult, "Main should only have Init (1)");
        }

        [Test]
        public void TestCrossNamespaceMethodCheck_ComponentInstance()
        {
            // BaseLogic receives component instance and validates its methods
            string baseLogic = @"
extension ComponentValidator
{
    function ValidateComponent(comp)
    {
        # Components should have Init
        if (!Convert.HasMethod(comp, ""Init"")) {
            return -1;
        }
            
        # Check for OnUpdate (common component callback)
        hasOnUpdate = Convert.HasMethod(comp, ""OnUpdate"");
        
        # Check for custom method
        hasCustom = Convert.HasMethod(comp, ""CustomBehavior"");
        
        # Return status code
        if (hasOnUpdate && hasCustom) {
            return 2;  # Full featured component
        }
        if (hasOnUpdate || hasCustom) {
            return 1;  # Partial component
        }
        return 0;  # Basic component
    }
}";

            // ModeLogic with different component types
            string modeLogic = @"
component BasicComponent
{
    function Init()
    {
    }
}

component UpdateComponent
{
    function Init()
    {
    }
    
    function OnUpdate()
    {
        # Update logic
    }
}

component FullComponent
{
    function Init()
    {
    }
    
    function OnUpdate()
    {
        # Update logic
    }
    
    function CustomBehavior()
    {
        # Custom behavior
    }
}

class Main
{
    function Init()
    {
    }
    
    function TestBasicComponent()
    {
        comp = BasicComponent();
        return ComponentValidator.ValidateComponent(comp);
    }
    
    function TestUpdateComponent()
    {
        comp = UpdateComponent();
        return ComponentValidator.ValidateComponent(comp);
    }
    
    function TestFullComponent()
    {
        comp = FullComponent();
        return ComponentValidator.ValidateComponent(comp);
    }
}";

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", baseLogic, CustomLogicSourceType.BaseLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Mode.cl", modeLogic, CustomLogicSourceType.ModeLogic));
            
            var offlineEvaluator = new OfflineCustomLogicEvaluator(compiler);
            var mainInstance = offlineEvaluator.GetMainInstance();
            
            // BasicComponent: Only has Init, no OnUpdate or Custom = 0
            var basicResult = offlineEvaluator.EvaluateMethod(mainInstance, "TestBasicComponent");
            Assert.AreEqual(0, basicResult, "BasicComponent should return 0");
            
            // UpdateComponent: Has OnUpdate but not Custom = 1
            var updateResult = offlineEvaluator.EvaluateMethod(mainInstance, "TestUpdateComponent");
            Assert.AreEqual(1, updateResult, "UpdateComponent should return 1");
            
            // FullComponent: Has both OnUpdate and Custom = 2
            var fullResult = offlineEvaluator.EvaluateMethod(mainInstance, "TestFullComponent");
            Assert.AreEqual(2, fullResult, "FullComponent should return 2");
        }
    }
}
