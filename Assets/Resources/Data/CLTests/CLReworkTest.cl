class Main {

    _tester = null;

    function Init()
    {
        self._tester = Tester(self.DisplayInConsole);

        baseList = self.GetList("1,2,1,4,115");
        uniqueList = self.GetList("1,2,4,115");
        filteredList = self.GetList("115");
        transformedList = self.GetList("2,4,2,8,230");
        values = baseList;
        uniques = List(values.ToSet());

        self._tester.Assert("ToSet", self.ListCompare(uniques, uniqueList));
        self._tester.AssertEqual("reduce", values.Reduce(self.Sum2, 0), 123);
        self._tester.Assert("Filter", self.ListCompare(values.Filter(self.Filter), filteredList));
        self._tester.Assert("Transform", self.ListCompare(values.Map(self.TransformData), transformedList));
        self._tester.Assert("ExampleFail", false);

        self._tester.ShowResults();
    }

    function Sum2(a, b) {
        return a + b;
    }

    function Filter(a)
    {
        return a > 20;
    }

    function TransformData(a) {
        return a * 2;
    }

    function DisplayInChat(message) {
        Game.Print(message);
    }

    function DisplayInConsole(message) {
        Game.Debug(message);
    }

    function ListCompare(a, b) {
        if (a.Count != b.Count) {
            return false;
        }
        for (i in Range(0, a.Count, 1)) {
            if (a.Get(i) != b.Get(i)) {
                return false;
            }
        }
        return true;
    }

    function GetList(a) {
        v = String.Split(a, ",", true);
        for (i in Range(0, v.Count, 1)) {
            v.Set(i, Convert.ToInt(v.Get(i)));
        }
        return v;
    }
}

class Tester {
    _passingTests = List();
    _failingTests = List();
    _failMessage = "";
    _passMessage = "";
    _displayMethod = null;

    function Init(displayMethod)
    {
        self._failMessage = UI.WrapStyleTag("Test failed: ", "color", "red");
        self._passMessage = UI.WrapStyleTag("Test passed: ", "color", "green");
        self._displayMethod = displayMethod;
    }

    function ShowResults()
    {
        passingTests = self._passingTests.Count;
        failingTests = self._failingTests.Count;
        totalTests = passingTests + failingTests;

        # Print number of passing tests and then print the lines for the failingTests
        if (failingTests > 0) {
            self._displayMethod(UI.WrapStyleTag("Test failed: ", "color", "red") + failingTests + " out of " + totalTests + " tests failed");
            for (i in Range(0, self._failingTests.Count, 1)) {
                self._displayMethod(self._failingTests.Get(i));
            }
        } else {
           self._displayMethod(UI.WrapStyleTag("Test passed: ", "color", "green") + passingTests + " out of " + totalTests + " tests passed");
        }
    }

    function Assert(method, value) {
        if (value) {
            self._passingTests.Add(self._passMessage + UI.WrapStyleTag(method + " returned " + value, "color", "white"));
        } else {
            self._failingTests.Add(self._failMessage + UI.WrapStyleTag(method + " returned " + value, "color", "white"));
        }
    }

    function AssertEqual(method, result, expected) {
        if (result == expected) {
            self._passingTests.Add(self._passMessage + UI.WrapStyleTag(method + " returned " + result + " expected " + expected, "color", "white"));
        } else {
            self._failingTests.Add(self._failMessage + UI.WrapStyleTag(method + " returned " + result + " expected " + expected, "color", "white"));
        }
    }

    function AssertNotEqual(method, result, expected) {
        if (result != expected) {
            self._passingTests.Add(self._passMessage + UI.WrapStyleTag(method + " returned " + result + " expected not " + expected, "color", "white"));
        } else {
            self._failingTests.Add(self._failMessage + UI.WrapStyleTag(method + " returned " + result + " expected not  " + expected, "color", "white"));
        }
    }
}

class Int2TestSuite
{
    _tester = null;
    function Init(tester)
    {
        self._tester = tester;

        self._tester.AssertEqual("Int2.X", Int2(1, 2).X, 1);
        self._tester.AssertEqual("Int2.Y", Int2(1, 2).Y, 2);

        self._tester.AssertEqual("Int2.__eq__", Int2(1, 2), Int2(1,2));
        self._tester.AssertEqual("Int2.__add__", Int2(1,2) + Int2(1,3), Int2(2, 5));
        self._tester.AssertEqual("Int2.__sub__", Int2(1, 2) - Int2(1,3), Int2(0, -1));
        self._tester.AssertEqual("Int2.__mul__", Int2(1, 2) * Int2(1,3), Int2(1, 6));
        self._tester.AssertEqual("Int2.__div__", Int2(10, 20) / Int2(2, 4), Int2(5, 5));
        self._tester.AssertEqual("Int2.__copy__", Int2(1, 2).__Copy__(), Int2(1, 2));
        self._tester.AssertEqual("Int2.__str__", Int2(1, 2).__Str__(), "(1, 2)");
    }
}

# Example of a class which overrides default methods.
class Int2
{
    X = 0;
    Y = 0;

    function Init(x, y)
    {
        self.X = x;
        self.Y = y;
    }

    function __Add__(this, other)
    {
        if (other.Type != "Int2")
        {
            return this;
        }

        return Int2(this.X + other.X, this.Y + other.Y);
    }

    function __Sub__(this, other)
    {
        if (other.Type != "Int2")
        {
            return null;
        }

        return Int2(this.X - other.X, this.Y - other.Y);
    }

    function __Mul__(this, other)
    {
        if (other.Type != "Int2")
        {
            return null;
        }

        return Int2(this.X * other.X, this.Y * other.Y);
    }

    function __Div__(this, other)
    {
        if (other.Type != "Int2")
        {
            return null;
        }

        return Int2(this.X / other.X, this.Y / other.Y);
    }

    function __Eq__(this, other)
    {
        if (other == null)
        {
            return false;
        }

        if (other.Type != "Int2")
        {
            return false;
        }

        return this.X == other.X && this.Y == other.Y;
    }

    function __Copy__()
    {
        return Int2(self.X, self.Y);
    }

    function __Str__()
    {
        return "(" + self.X + ", " + self.Y + ")";
    }
}