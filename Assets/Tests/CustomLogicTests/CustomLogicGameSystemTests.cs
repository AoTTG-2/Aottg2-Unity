using NUnit.Framework;
using CustomLogic;
using CustomLogic.OfflineEvaluator;
using System;

namespace Tests.CustomLogic
{
    /// <summary>
    /// Tests demonstrating how to test complex game logic scripts.
    /// This example tests a simplified version of a boss health system.
    /// </summary>
    [TestFixture]
    public class CustomLogicGameSystemTests
    {
        [Test]
        public void TestBossHealthSystem()
        {
            string script = @"
class BossHealth
{
    _maxHealth = 100.0;
    _currentHealth = 100.0;
    _name = """";
    
    function Init(name, maxHealth)
    {
        self._name = name;
        self._maxHealth = maxHealth;
        self._currentHealth = maxHealth;
    }
    
    function TakeDamage(amount)
    {
        self._currentHealth = Math.Max(0.0, self._currentHealth - amount);
        return self._currentHealth;
    }
    
    function Heal(amount)
    {
        previousHealth = self._currentHealth;
        self._currentHealth = Math.Min(self._maxHealth, self._currentHealth + amount);
        return self._currentHealth - previousHealth;
    }
    
    function GetHealthPercentage()
    {
        if (self._maxHealth <= 0.0)
        {
            return 0.0;
        }
        return (self._currentHealth / self._maxHealth) * 100.0;
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
    _boss = null;
    
    function Init()
    {
        self._boss = BossHealth(""Colossal Titan"", 1000.0);
    }
    
    function GetBossHealth()
    {
        return self._boss._currentHealth;
    }
    
    function DealDamage(amount)
    {
        return self._boss.TakeDamage(amount);
    }
    
    function HealBoss(amount)
    {
        return self._boss.Heal(amount);
    }
    
    function GetHealthPercentage()
    {
        return self._boss.GetHealthPercentage();
    }
    
    function IsBossDead()
    {
        return self._boss.IsDead();
    }
    
    function ResetBoss()
    {
        self._boss.Reset();
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            
            // Test initial health
            var initialHealth = evaluator.EvaluateMainMethod("GetBossHealth");
            Assert.AreEqual(1000.0f, (float)initialHealth, 0.001f);
            
            // Test damage
            var healthAfterDamage = evaluator.EvaluateMainMethod("DealDamage", 50.0f);
            Assert.AreEqual(950.0f, (float)healthAfterDamage, 0.001f);
            
            // Test health percentage
            var percentage = evaluator.EvaluateMainMethod("GetHealthPercentage");
            Assert.AreEqual(95.0f, (float)percentage, 0.001f);
            
            // Test healing
            var actualHeal = evaluator.EvaluateMainMethod("HealBoss", 100.0f);
            Assert.AreEqual(50.0f, (float)actualHeal, 0.001f); // Can only heal back to max
            
            // Test full health
            var currentHealth = evaluator.EvaluateMainMethod("GetBossHealth");
            Assert.AreEqual(1000.0f, (float)currentHealth, 0.001f);
            
            // Test killing the boss
            evaluator.EvaluateMainMethod("DealDamage", 1000.0f);
            var isDead = evaluator.EvaluateMainMethod("IsBossDead");
            Assert.AreEqual(true, isDead);
            
            // Test reset
            evaluator.EvaluateMainMethod("ResetBoss");
            var healthAfterReset = evaluator.EvaluateMainMethod("GetBossHealth");
            Assert.AreEqual(1000.0f, (float)healthAfterReset, 0.001f);
        }

        [Test]
        public void TestColorThresholdSystem()
        {
            string script = @"
class ColorThreshold
{
    _thresholds = null;
    
    function Init()
    {
        self._thresholds = List();
    }
    
    function AddThreshold(percent, colorName)
    {
        threshold = Dict();
        threshold.Set(""Percent"", percent);
        threshold.Set(""Color"", colorName);
        self._thresholds.Add(threshold);
        self.Sort();
    }
    
    function Sort()
    {
        # Bubble sort in descending order
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
                    # Swap
                    temp = self._thresholds.Get(j);
                    self._thresholds.Set(j, self._thresholds.Get(j + 1));
                    self._thresholds.Set(j + 1, temp);
                }
                j = j + 1;
            }
            i = i + 1;
        }
    }
    
    function GetColorForPercent(percent)
    {
        i = 0;
        while (i < self._thresholds.Count)
        {
            threshold = self._thresholds.Get(i);
            if (percent >= threshold.Get(""Percent""))
            {
                return threshold.Get(""Color"");
            }
            i = i + 1;
        }
        return ""gray"";
    }
}

class Main
{
    _thresholds = null;
    
    function Init()
    {
        self._thresholds = ColorThreshold();
        self._thresholds.AddThreshold(75.0, ""green"");
        self._thresholds.AddThreshold(50.0, ""yellow"");
        self._thresholds.AddThreshold(25.0, ""orange"");
        self._thresholds.AddThreshold(0.0, ""red"");
    }
    
    function GetColor(percent)
    {
        return self._thresholds.GetColorForPercent(percent);
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            
            Assert.AreEqual("green", evaluator.EvaluateMainMethod("GetColor", 100.0f));
            Assert.AreEqual("green", evaluator.EvaluateMainMethod("GetColor", 75.0f));
            Assert.AreEqual("yellow", evaluator.EvaluateMainMethod("GetColor", 60.0f));
            Assert.AreEqual("yellow", evaluator.EvaluateMainMethod("GetColor", 50.0f));
            Assert.AreEqual("orange", evaluator.EvaluateMainMethod("GetColor", 30.0f));
            Assert.AreEqual("orange", evaluator.EvaluateMainMethod("GetColor", 25.0f));
            Assert.AreEqual("red", evaluator.EvaluateMainMethod("GetColor", 10.0f));
            Assert.AreEqual("red", evaluator.EvaluateMainMethod("GetColor", 0.0f));
        }

        [Test]
        public void TestInventorySystem()
        {
            string script = @"
class Item
{
    _name = """";
    _quantity = 0;
    
    function Init(name, quantity)
    {
        self._name = name;
        self._quantity = quantity;
    }
    
    function Add(amount)
    {
        self._quantity = self._quantity + amount;
    }
    
    function Remove(amount)
    {
        if (self._quantity >= amount)
        {
            self._quantity = self._quantity - amount;
            return true;
        }
        return false;
    }
}

class Inventory
{
    _items = null;
    
    function Init()
    {
        self._items = Dict();
    }
    
    function AddItem(name, quantity)
    {
        if (self._items.Contains(name))
        {
            item = self._items.Get(name);
            item.Add(quantity);
        }
        else
        {
            item = Item(name, quantity);
            self._items.Set(name, item);
        }
    }
    
    function RemoveItem(name, quantity)
    {
        if (self._items.Contains(name))
        {
            item = self._items.Get(name);
            return item.Remove(quantity);
        }
        return false;
    }
    
    function GetQuantity(name)
    {
        if (self._items.Contains(name))
        {
            item = self._items.Get(name);
            return item._quantity;
        }
        return 0;
    }
}

class Main
{
    _inventory = null;
    
    function Init()
    {
        self._inventory = Inventory();
    }
    
    function AddItem(name, quantity)
    {
        self._inventory.AddItem(name, quantity);
    }
    
    function RemoveItem(name, quantity)
    {
        return self._inventory.RemoveItem(name, quantity);
    }
    
    function GetItemCount(name)
    {
        return self._inventory.GetQuantity(name);
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            
            // Add items
            evaluator.EvaluateMainMethod("AddItem", "Potion", 5);
            Assert.AreEqual(5, evaluator.EvaluateMainMethod("GetItemCount", "Potion"));
            
            // Add more of same item
            evaluator.EvaluateMainMethod("AddItem", "Potion", 3);
            Assert.AreEqual(8, evaluator.EvaluateMainMethod("GetItemCount", "Potion"));
            
            // Remove items
            var success = evaluator.EvaluateMainMethod("RemoveItem", "Potion", 5);
            Assert.AreEqual(true, success);
            Assert.AreEqual(3, evaluator.EvaluateMainMethod("GetItemCount", "Potion"));
            
            // Try to remove more than available
            success = evaluator.EvaluateMainMethod("RemoveItem", "Potion", 10);
            Assert.AreEqual(false, success);
            Assert.AreEqual(3, evaluator.EvaluateMainMethod("GetItemCount", "Potion"));
            
            // Check non-existent item
            Assert.AreEqual(0, evaluator.EvaluateMainMethod("GetItemCount", "Sword"));
        }

        [Test]
        public void TestStateMachine()
        {
            string script = @"
class StateMachine
{
    _currentState = """";
    _validTransitions = null;
    
    function Init(initialState)
    {
        self._currentState = initialState;
        self._validTransitions = Dict();
    }
    
    function AddTransition(fromState, toState)
    {
        if (!self._validTransitions.Contains(fromState))
        {
            self._validTransitions.Set(fromState, List());
        }
        transitions = self._validTransitions.Get(fromState);
        transitions.Add(toState);
    }
    
    function CanTransition(toState)
    {
        if (!self._validTransitions.Contains(self._currentState))
        {
            return false;
        }
        transitions = self._validTransitions.Get(self._currentState);
        return transitions.Contains(toState);
    }
    
    function Transition(toState)
    {
        if (self.CanTransition(toState))
        {
            self._currentState = toState;
            return true;
        }
        return false;
    }
    
    function GetState()
    {
        return self._currentState;
    }
}

class Main
{
    _fsm = null;
    
    function Init()
    {
        self._fsm = StateMachine(""Idle"");
        self._fsm.AddTransition(""Idle"", ""Walking"");
        self._fsm.AddTransition(""Walking"", ""Running"");
        self._fsm.AddTransition(""Running"", ""Idle"");
    }
    
    function GetState()
    {
        return self._fsm.GetState();
    }
    
    function TransitionTo(state)
    {
        return self._fsm.Transition(state);
    }
    
    function CanTransitionTo(state)
    {
        return self._fsm.CanTransition(state);
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            
            // Initial state
            Assert.AreEqual("Idle", evaluator.EvaluateMainMethod("GetState"));
            
            // Test invalid transition from Idle (cannot go to Running directly)
            Assert.AreEqual(false, evaluator.EvaluateMainMethod("CanTransitionTo", "Running"));
            
            // Valid transition
            Assert.AreEqual(true, evaluator.EvaluateMainMethod("TransitionTo", "Walking"));
            Assert.AreEqual("Walking", evaluator.EvaluateMainMethod("GetState"));
            
            // Valid transition from Walking to Running
            Assert.AreEqual(true, evaluator.EvaluateMainMethod("TransitionTo", "Running"));
            Assert.AreEqual("Running", evaluator.EvaluateMainMethod("GetState"));
            
            // Valid transition from Running to Idle
            Assert.AreEqual(true, evaluator.EvaluateMainMethod("TransitionTo", "Idle"));
            Assert.AreEqual("Idle", evaluator.EvaluateMainMethod("GetState"));
        }
    }
}
