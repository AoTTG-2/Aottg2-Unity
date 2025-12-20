using NUnit.Framework;
using CustomLogic.OfflineEvaluator;

namespace Tests.CustomLogic
{
    /// <summary>
    /// Example test demonstrating a concise testing pattern using the helper class.
    /// Shows how to test the example BossHealthBar script from the user's requirements.
    /// </summary>
    [TestFixture]
    public class ExampleBossHealthBarTests
    {
        [Test]
        public void TestBossHealthBarExample()
        {
            // This is a simplified version of the BossHealthBar class from the user's example
            // Testing the core logic without UI dependencies
            string script = @"
class BossHealthBar
{
    _bossName = ""Boss"";
    _maxHealth = 100.0;
    _currentHealth = 100.0;
    _colorThresholds = null;
    
    function Init(bossName, maxHealth)
    {
        self._bossName = bossName;
        self._maxHealth = maxHealth;
        self._currentHealth = maxHealth;
        self._colorThresholds = List();
        
        # Add default color thresholds
        self.AddColorThreshold(75.0, ""Green"");
        self.AddColorThreshold(50.0, ""Yellow"");
        self.AddColorThreshold(25.0, ""Orange"");
        self.AddColorThreshold(0.0, ""Red"");
    }
    
    function AddColorThreshold(percent, color)
    {
        threshold = Dict();
        threshold.Set(""Percent"", percent);
        threshold.Set(""Color"", color);
        self._colorThresholds.Add(threshold);
        self.SortThresholds();
    }
    
    function SortThresholds()
    {
        # Bubble sort in descending order
        n = self._colorThresholds.Count;
        i = 0;
        while (i < n - 1)
        {
            j = 0;
            while (j < n - i - 1)
            {
                current = self._colorThresholds.Get(j);
                next = self._colorThresholds.Get(j + 1);
                
                if (current.Get(""Percent"") < next.Get(""Percent""))
                {
                    temp = self._colorThresholds.Get(j);
                    self._colorThresholds.Set(j, self._colorThresholds.Get(j + 1));
                    self._colorThresholds.Set(j + 1, temp);
                }
                j = j + 1;
            }
            i = i + 1;
        }
    }
    
    function GetCurrentColor()
    {
        percentage = self.GetHealthPercentage();
        i = 0;
        while (i < self._colorThresholds.Count)
        {
            threshold = self._colorThresholds.Get(i);
            if (percentage >= threshold.Get(""Percent""))
            {
                return threshold.Get(""Color"");
            }
            i = i + 1;
        }
        return ""Gray"";
    }
    
    function GetHealthPercentage()
    {
        if (self._maxHealth <= 0.0)
        {
            return 0.0;
        }
        return (self._currentHealth / self._maxHealth) * 100.0;
    }
    
    function RemoveHealth(amount)
    {
        self._currentHealth = Math.Max(0.0, self._currentHealth - amount);
    }
    
    function AddHealth(amount)
    {
        previousHealth = self._currentHealth;
        self._currentHealth = Math.Min(self._maxHealth, self._currentHealth + amount);
        return self._currentHealth - previousHealth;
    }
    
    function IsDead()
    {
        return self._currentHealth <= 0.0;
    }
    
    function IsFull()
    {
        return self._currentHealth >= self._maxHealth;
    }
    
    function Reset()
    {
        self._currentHealth = self._maxHealth;
    }
}

class Main
{
    _bossHealthBar = null;
    
    function Init()
    {
        # Create boss health bar with 1000 max health
        self._bossHealthBar = BossHealthBar(""Colossal Titan"", 1000.0);
    }
    
    function DealDamage(amount)
    {
        self._bossHealthBar.RemoveHealth(amount);
        return self._bossHealthBar._currentHealth;
    }
    
    function HealBoss(amount)
    {
        return self._bossHealthBar.AddHealth(amount);
    }
    
    function GetHealthPercentage()
    {
        return self._bossHealthBar.GetHealthPercentage();
    }
    
    function GetCurrentColor()
    {
        return self._bossHealthBar.GetCurrentColor();
    }
    
    function IsDead()
    {
        return self._bossHealthBar.IsDead();
    }
    
    function Reset()
    {
        self._bossHealthBar.Reset();
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            
            // Test initial state
            Assert.AreEqual(100.0f, (float)evaluator.EvaluateMainMethod("GetHealthPercentage"), 0.001f);
            Assert.AreEqual("Green", evaluator.EvaluateMainMethod("GetCurrentColor"));
            
            // Deal damage - should change to yellow
            evaluator.EvaluateMainMethod("DealDamage", 300.0f);
            Assert.AreEqual(70.0f, (float)evaluator.EvaluateMainMethod("GetHealthPercentage"), 0.001f);
            Assert.AreEqual("Yellow", evaluator.EvaluateMainMethod("GetCurrentColor"));
            
            // Deal more damage - should change to orange
            evaluator.EvaluateMainMethod("DealDamage", 300.0f);
            Assert.AreEqual(40.0f, (float)evaluator.EvaluateMainMethod("GetHealthPercentage"), 0.001f);
            Assert.AreEqual("Orange", evaluator.EvaluateMainMethod("GetCurrentColor"));
            
            // Deal more damage - should change to red
            evaluator.EvaluateMainMethod("DealDamage", 300.0f);
            Assert.AreEqual(10.0f, (float)evaluator.EvaluateMainMethod("GetHealthPercentage"), 0.001f);
            Assert.AreEqual("Red", evaluator.EvaluateMainMethod("GetCurrentColor"));
            
            // Kill the boss
            evaluator.EvaluateMainMethod("DealDamage", 100.0f);
            Assert.AreEqual(true, evaluator.EvaluateMainMethod("IsDead"));
            Assert.AreEqual(0.0f, (float)evaluator.EvaluateMainMethod("GetHealthPercentage"), 0.001f);
            
            // Test healing
            evaluator.EvaluateMainMethod("Reset");
            var actualHeal = evaluator.EvaluateMainMethod("HealBoss", 100.0f);
            Assert.AreEqual(0.0f, (float)actualHeal, 0.001f); // Can't heal above max
            
            // Deal damage then heal
            evaluator.EvaluateMainMethod("DealDamage", 500.0f);
            actualHeal = evaluator.EvaluateMainMethod("HealBoss", 200.0f);
            Assert.AreEqual(200.0f, (float)actualHeal, 0.001f);
            Assert.AreEqual(70.0f, (float)evaluator.EvaluateMainMethod("GetHealthPercentage"), 0.001f);
        }

        [Test]
        public void TestColorThresholdsAreSorted()
        {
            // Test that color thresholds are properly sorted in descending order
            string script = @"
class Main
{
    _thresholds = null;
    
    function Init()
    {
        self._thresholds = List();
    }
    
    function AddThreshold(percent, color)
    {
        threshold = Dict();
        threshold.Set(""Percent"", percent);
        threshold.Set(""Color"", color);
        self._thresholds.Add(threshold);
    }
    
    function SortThresholds()
    {
        n = self._thresholds.Count;
        i = 0;
        while (i < n - 1)
        {
            j = 0;
            while (j < n - i - 1)
            {
                current = self._thresholds.Get(j);
                next = self._thresholds.Get(j + 1);
                
                if (current.Get(""Percent"") < next.Get(""Percent""))
                {
                    temp = self._thresholds.Get(j);
                    self._thresholds.Set(j, self._thresholds.Get(j + 1));
                    self._thresholds.Set(j + 1, temp);
                }
                j = j + 1;
            }
            i = i + 1;
        }
    }
    
    function GetThresholdAt(index)
    {
        threshold = self._thresholds.Get(index);
        return threshold.Get(""Percent"");
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            
            // Add thresholds in random order
            evaluator.EvaluateMainMethod("AddThreshold", 25.0f, "Orange");
            evaluator.EvaluateMainMethod("AddThreshold", 75.0f, "Green");
            evaluator.EvaluateMainMethod("AddThreshold", 0.0f, "Red");
            evaluator.EvaluateMainMethod("AddThreshold", 50.0f, "Yellow");
            
            // Sort them
            evaluator.EvaluateMainMethod("SortThresholds");
            
            // Verify they're in descending order
            Assert.AreEqual(75.0f, (float)evaluator.EvaluateMainMethod("GetThresholdAt", 0), 0.001f);
            Assert.AreEqual(50.0f, (float)evaluator.EvaluateMainMethod("GetThresholdAt", 1), 0.001f);
            Assert.AreEqual(25.0f, (float)evaluator.EvaluateMainMethod("GetThresholdAt", 2), 0.001f);
            Assert.AreEqual(0.0f, (float)evaluator.EvaluateMainMethod("GetThresholdAt", 3), 0.001f);
        }
    }
}
