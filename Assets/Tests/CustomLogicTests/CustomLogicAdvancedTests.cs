using NUnit.Framework;
using CustomLogic;
using CustomLogic.OfflineEvaluator;
using System;

namespace Tests.CustomLogic
{
    [TestFixture]
    public class CustomLogicAdvancedTests
    {
        [Test]
        public void TestRecursiveFunctions()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function Factorial(n)
    {
        if (n <= 1)
        {
            return 1;
        }
        return n * self.Factorial(n - 1);
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.AreEqual(1, evaluator.EvaluateMainMethod("Factorial", 1));
            Assert.AreEqual(120, evaluator.EvaluateMainMethod("Factorial", 5));
            Assert.AreEqual(3628800, evaluator.EvaluateMainMethod("Factorial", 10));
        }

        [Test]
        public void TestListOperations()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function FilterEven(value)
    {
        return value % 2 == 0;
    }
    
    function Double(value)
    {
        return value * 2;
    }
    
    function Sum(acc, value)
    {
        return acc + value;
    }
    
    function TestListOps()
    {
        numbers = List(1, 2, 3, 4, 5, 6);
        
        # Filter to get even numbers
        evens = numbers.Filter(self.FilterEven);
        
        # Double each value
        doubled = evens.Map(self.Double);
        
        # Sum all values
        total = doubled.Reduce(self.Sum, 0);
        
        return total;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            // Even numbers: 2, 4, 6
            // Doubled: 4, 8, 12
            // Sum: 24
            Assert.AreEqual(24, evaluator.EvaluateMainMethod("TestListOps"));
        }

        [Test]
        public void TestNestedClasses()
        {
            string script = @"
class Point
{
    _x = 0;
    _y = 0;
    
    function Init(x, y)
    {
        self._x = x;
        self._y = y;
    }
    
    function DistanceFrom(other)
    {
        dx = self._x - other._x;
        dy = self._y - other._y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}

class Main
{
    function Init()
    {
    }
    
    function TestPoints()
    {
        p1 = Point(0, 0);
        p2 = Point(3, 4);
        return p1.DistanceFrom(p2);
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            var result = evaluator.EvaluateMainMethod("TestPoints");
            Assert.AreEqual(5.0f, (float)result, 0.001f); // Distance is 5 (3-4-5 triangle)
        }

        [Test]
        public void TestStringOperations()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function TestStrings()
    {
        str1 = ""Hello"";
        str2 = "" World"";
        combined = str1 + str2;
        length = String.Length(combined);
        return length;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.AreEqual(11, evaluator.EvaluateMainMethod("TestStrings"));
        }

        [Test]
        public void TestCompoundAssignmentOperators()
        {
            string script = @"
class Main
{
    _value = 10;
    
    function Init()
    {
    }
    
    function TestCompoundAssignment()
    {
        self._value += 5;  # 15
        self._value *= 2;  # 30
        self._value -= 10; # 20
        self._value /= 4;  # 5
        return self._value;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.AreEqual(5, evaluator.EvaluateMainMethod("TestCompoundAssignment"));
        }

        [Test]
        public void TestComplexConditionals()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function Classify(value)
    {
        if (value < 0)
        {
            return ""negative"";
        }
        elif (value == 0)
        {
            return ""zero"";
        }
        elif (value < 10)
        {
            return ""small"";
        }
        elif (value < 100)
        {
            return ""medium"";
        }
        else
        {
            return ""large"";
        }
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.AreEqual("negative", evaluator.EvaluateMainMethod("Classify", -5));
            Assert.AreEqual("zero", evaluator.EvaluateMainMethod("Classify", 0));
            Assert.AreEqual("small", evaluator.EvaluateMainMethod("Classify", 5));
            Assert.AreEqual("medium", evaluator.EvaluateMainMethod("Classify", 50));
            Assert.AreEqual("large", evaluator.EvaluateMainMethod("Classify", 500));
        }

        [Test]
        public void TestBreakAndContinue()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function SumUntilNegative()
    {
        numbers = List(1, 2, 3, -1, 4, 5);
        sum = 0;
        for (num in numbers)
        {
            if (num < 0)
            {
                break;
            }
            sum = sum + num;
        }
        return sum;
    }
    
    function SumPositives()
    {
        numbers = List(1, -2, 3, -4, 5);
        sum = 0;
        for (num in numbers)
        {
            if (num < 0)
            {
                continue;
            }
            sum = sum + num;
        }
        return sum;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.AreEqual(6, evaluator.EvaluateMainMethod("SumUntilNegative")); // 1+2+3
            Assert.AreEqual(9, evaluator.EvaluateMainMethod("SumPositives")); // 1+3+5
        }

        [Test]
        public void TestSet()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function TestSet()
    {
        set = Set();
        set.Add(1);
        set.Add(2);
        set.Add(2);  # Duplicate
        set.Add(3);
        return set.Count;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.AreEqual(3, evaluator.EvaluateMainMethod("TestSet")); // Set should have 3 unique values
        }

        [Test]
        public void TestJsonSerialization()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function TestJson()
    {
        dict = Dict();
        dict.Set(""name"", ""Test"");
        dict.Set(""value"", 42);
        
        json = Json.SaveToString(dict);
        loaded = Json.LoadFromString(json);
        
        return loaded.Get(""value"");
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.AreEqual(42, evaluator.EvaluateMainMethod("TestJson"));
        }

        [Test]
        public void TestMultipleClassInteraction()
        {
            string script = @"
class Calculator
{
    function Init()
    {
    }
    
    function Add(a, b)
    {
        return a + b;
    }
    
    function Multiply(a, b)
    {
        return a * b;
    }
}

class Main
{
    _calculator = null;
    
    function Init()
    {
        self._calculator = Calculator();
    }
    
    function Calculate()
    {
        result = self._calculator.Add(5, 3);
        result = self._calculator.Multiply(result, 2);
        return result;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.AreEqual(16, evaluator.EvaluateMainMethod("Calculate")); // (5+3)*2 = 16
        }
    }
}
