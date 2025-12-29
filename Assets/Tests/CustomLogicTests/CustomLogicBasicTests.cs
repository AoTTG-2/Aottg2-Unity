using NUnit.Framework;
using CustomLogic;
using CustomLogic.OfflineEvaluator;
using System;

namespace Tests.CustomLogic
{
    [TestFixture]
    public class CustomLogicBasicTests
    {
        [Test]
        public void TestMainClassInitialization()
        {
            string script = @"
class Main
{
    _value = 0;
    
    function Init()
    {
        self._value = 42;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.IsTrue(evaluator.HasMainClass());
            Assert.IsNotNull(evaluator.GetMainInstance());
            
            var value = evaluator.GetMainVariable("_value");
            Assert.AreEqual(42, value);
        }

        [Test]
        public void TestSimpleMethod()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function Add(a, b)
    {
        return a + b;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            var result = evaluator.EvaluateMainMethod("Add", 5, 10);
            Assert.AreEqual(15, result);
        }

        [Test]
        public void TestVariableAssignment()
        {
            string script = @"
class Main
{
    _counter = 0;
    
    function Init()
    {
        self._counter = 10;
    }
    
    function Increment()
    {
        self._counter = self._counter + 1;
        return self._counter;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            var result1 = evaluator.EvaluateMainMethod("Increment");
            Assert.AreEqual(11, result1);
            
            var result2 = evaluator.EvaluateMainMethod("Increment");
            Assert.AreEqual(12, result2);
        }

        [Test]
        public void TestConditionals()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function IsPositive(value)
    {
        if (value > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.AreEqual(true, evaluator.EvaluateMainMethod("IsPositive", 5));
            Assert.AreEqual(false, evaluator.EvaluateMainMethod("IsPositive", -5));
            Assert.AreEqual(false, evaluator.EvaluateMainMethod("IsPositive", 0));
        }

        [Test]
        public void TestLoops()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function SumToN(n)
    {
        sum = 0;
        i = 1;
        while (i <= n)
        {
            sum = sum + i;
            i = i + 1;
        }
        return sum;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.AreEqual(15, evaluator.EvaluateMainMethod("SumToN", 5)); // 1+2+3+4+5 = 15
            Assert.AreEqual(55, evaluator.EvaluateMainMethod("SumToN", 10)); // 1+2+...+10 = 55
        }

        [Test]
        public void TestLists()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function CreateAndSum()
    {
        list = List(1, 2, 3, 4, 5);
        sum = 0;
        for (item in list)
        {
            sum = sum + item;
        }
        return sum;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.AreEqual(15, evaluator.EvaluateMainMethod("CreateAndSum"));
        }

        [Test]
        public void TestDictionaries()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function UseDictionary()
    {
        dict = Dict();
        dict.Set(""key1"", 100);
        dict.Set(""key2"", 200);
        return dict.Get(""key1"") + dict.Get(""key2"");
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.AreEqual(300, evaluator.EvaluateMainMethod("UseDictionary"));
        }

        [Test]
        public void TestMathBuiltin()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function TestMath()
    {
        a = Math.Max(5, 10);
        b = Math.Min(5, 10);
        c = Math.Abs(-15);
        return a + b + c;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.AreEqual(30, evaluator.EvaluateMainMethod("TestMath")); // 10 + 5 + 15 = 30
        }

        [Test]
        public void TestConvertBuiltin()
        {
            string script = @"
class Main
{
    function Init()
    {
    }
    
    function TestConvert()
    {
        a = Convert.ToInt(""42"");
        b = Convert.ToFloat(""3.14"");
        c = Convert.ToString(100);
        d = Convert.ToBool(""true"");
        
        result = a;
        if (d)
        {
            result = result + 10;
        }
        return result;
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.AreEqual(52, evaluator.EvaluateMainMethod("TestConvert"));
        }

        [Test]
        public void TestCustomClass()
        {
            string script = @"
class Counter
{
    _value = 0;
    
    function Init(initialValue)
    {
        self._value = initialValue;
    }
    
    function Increment()
    {
        self._value = self._value + 1;
    }
    
    function GetValue()
    {
        return self._value;
    }
}

class Main
{
    function Init()
    {
    }
    
    function TestCounter()
    {
        counter = Counter(10);
        counter.Increment();
        counter.Increment();
        return counter.GetValue();
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            Assert.AreEqual(12, evaluator.EvaluateMainMethod("TestCounter"));
        }

        [Test]
        public void TestNullComparisonsWithCustomEquals()
        {
            string script = @"
class CustomObject
{
    _value = 0;
    _equalsCallCount = 0;
    
    function Init(value)
    {
        self._value = value;
    }
    
    function __Eq__(a, b)
    {
        self._equalsCallCount = self._equalsCallCount + 1;
        
        # Both are CustomObject instances
        return a._value == b._value;
    }
    
    function GetEqualsCallCount()
    {
        return self._equalsCallCount;
    }
}

class Main
{
    _obj1 = null;
    _obj2 = null;
    _obj3 = null;
    _nullRef = null;
    
    function Init()
    {
        self._obj1 = CustomObject(10);
        self._obj2 = CustomObject(20);
        self._obj3 = CustomObject(10);
    }
    
    function TestNullVsNull()
    {
        return self._nullRef == null;
    }
    
    function TestNullVsObject()
    {
        return self._nullRef == self._obj1;
    }
    
    function TestObjectVsNull()
    {
        return self._obj1 == self._nullRef;
    }
    
    function TestNullVsNullReverse()
    {
        return null == self._nullRef;
    }
    
    function TestObjectVsNullReverse()
    {
        return null == self._obj1;
    }
    
    function TestObjectVsObject_Equal()
    {
        return self._obj1 == self._obj3;
    }
    
    function TestObjectVsObject_NotEqual()
    {
        return self._obj1 == self._obj2;
    }
    
    function TestNotEqualsNull()
    {
        return self._obj1 != null;
    }
    
    function TestNotEqualsNullReverse()
    {
        return null != self._obj1;
    }
    
    function TestNotEqualsBothNull()
    {
        return self._nullRef != null;
    }
    
    function TestAssignmentAndComparison()
    {
        temp = self._obj1;
        return temp == self._obj1;
    }
    
    function TestReassignmentToNull()
    {
        temp = self._obj1;
        temp = null;
        return temp == null;
    }
    
    function TestEqualsCallCount()
    {
        # This should call __Eq__ once
        self._obj1 = CustomObject(10);
        self._obj2 = CustomObject(20);
        self._obj3 = CustomObject(10);
        result = self._obj1 == self._obj3;
        return self._obj1.GetEqualsCallCount();
    }
    
    function TestNullDoesNotCallEquals()
    {
        # Reset call count
        self._obj1 = CustomObject(10);
        # This should NOT call __Eq__ (should return false immediately)
        result = self._obj1 == null;
        return self._obj1.GetEqualsCallCount();
    }
}";

            var evaluator = new OfflineCustomLogicEvaluator(script);
            
            // Test null vs null
            Assert.AreEqual(true, evaluator.EvaluateMainMethod("TestNullVsNull"), 
                "null == null should be true");
            
            // Test null vs object
            Assert.AreEqual(false, evaluator.EvaluateMainMethod("TestNullVsObject"), 
                "null == object should be false");
            
            // Test object vs null
            Assert.AreEqual(false, evaluator.EvaluateMainMethod("TestObjectVsNull"), 
                "object == null should be false");
            
            // Test reverse comparisons
            Assert.AreEqual(true, evaluator.EvaluateMainMethod("TestNullVsNullReverse"), 
                "null == null (reverse) should be true");
            
            Assert.AreEqual(false, evaluator.EvaluateMainMethod("TestObjectVsNullReverse"), 
                "null == object (reverse) should be false");
            
            // Test object vs object comparisons
            Assert.AreEqual(true, evaluator.EvaluateMainMethod("TestObjectVsObject_Equal"), 
                "objects with same value should be equal");
            
            Assert.AreEqual(false, evaluator.EvaluateMainMethod("TestObjectVsObject_NotEqual"), 
                "objects with different values should not be equal");
            
            // Test != operator
            Assert.AreEqual(true, evaluator.EvaluateMainMethod("TestNotEqualsNull"), 
                "object != null should be true");
            
            Assert.AreEqual(true, evaluator.EvaluateMainMethod("TestNotEqualsNullReverse"), 
                "null != object should be true");
            
            Assert.AreEqual(false, evaluator.EvaluateMainMethod("TestNotEqualsBothNull"), 
                "null != null should be false");
            
            // Test assignment and comparison
            Assert.AreEqual(true, evaluator.EvaluateMainMethod("TestAssignmentAndComparison"), 
                "assigned reference should equal original");
            
            Assert.AreEqual(true, evaluator.EvaluateMainMethod("TestReassignmentToNull"), 
                "reassigned to null should equal null");
            
            // Test that __Eq__ is called when comparing objects
            Assert.AreEqual(1, evaluator.EvaluateMainMethod("TestEqualsCallCount"), 
                "__Eq__ should be called once when comparing two objects");
            
            // Test that __Eq__ is NOT called when comparing with null
            Assert.AreEqual(0, evaluator.EvaluateMainMethod("TestNullDoesNotCallEquals"), 
                "__Eq__ should NOT be called when comparing object with null");
        }
    }
}
