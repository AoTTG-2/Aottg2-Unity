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
    }
}
