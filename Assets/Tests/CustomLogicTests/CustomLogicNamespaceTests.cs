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
        public void TestNamespaceIsolation_MapLogicAddonIsolation()
        {
            // Addon defines a utility class
            string addonLogic = @"
class AddonHelper
{
    function Init()
    {
    }
    
    function Calculate(x)
    {
        return x * 2;
    }
}";

            // Map logic uses addon but has its own Main
            string mapLogic = @"
class Main
{
    function Init()
    {
    }
    
    function TestAddonHelper()
    {
        helper = AddonHelper();
        return helper.Calculate(5);
    }
}";

            // Mode logic has its own Main that shouldn't see map's Main
            string modeLogic = @"
class Main
{
    _value = 999;
    
    function Init()
    {
    }
    
    function GetValue()
    {
        return self._value;
    }
}";

            var compiler = new CustomLogicCompiler();
            compiler.AddSourceFile(new CustomLogicSourceFile("Addon.cl", addonLogic, CustomLogicSourceType.Addon));
            compiler.AddSourceFile(new CustomLogicSourceFile("Map.maplogic", mapLogic, CustomLogicSourceType.MapLogic));
            compiler.AddSourceFile(new CustomLogicSourceFile("Mode.cl", modeLogic, CustomLogicSourceType.ModeLogic));
            
            var offlineEvaluator = new OfflineCustomLogicEvaluator(compiler);
            var mainInstance = offlineEvaluator.GetMainInstance();
            
            // Should use mode logic's Main (last loaded wins for Main)
            var result = offlineEvaluator.EvaluateMethod(mainInstance, "GetValue");
            Assert.AreEqual(999, result);
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
