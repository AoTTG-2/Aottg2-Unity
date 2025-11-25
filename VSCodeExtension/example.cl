class Main {

    _tester = null;

    function OnGameStart()
    {

        self._tester = Tester(self.DisplayInConsole);

        self._tester.ShowResults()

        baseList = List(1,2,1,4,115); # self.GetList("1,2,1,4,115");
        uniqueList = List(1,2,4,115); # self.GetList("1,2,4,115");
        filteredList = List(115);  # self.GetList("115");
        transformedList = List(2,4,2,8,230); # self.GetList("2,4,2,8,230");
        values = baseList;
        uniques = values.ToSet().ToList();
        a = List();

        a = Vector3(1,2,3);

        self._tester.Assert("ToSet", self.ListCompare(uniques, uniqueList));
        self._tester.AssertEqual("reduce", values.Reduce(self.Sum2, 0), 123);
        self._tester.Assert("Filter", self.ListCompare(values.Filter(self.Filter), filteredList));
        self._tester.Assert("Transform", self.ListCompare(values.Map(self.TransformData), transformedList));
        self._tester.Assert("ExampleFail", false);

        self._tester.AssertEqual("Int2.X", Int2(1, 2).X, 1);
        self._tester.AssertEqual("Int2.Y", Int2(1, 2).Y, 2);

        a = Int2(1, 2);
        b = Int2(1, 3);
        c = Int2(10, 20);
        d = Int2(2, 4);

        self._tester.AssertEqual("Int2.__eq__", a, a);
        self._tester.AssertEqual("Int2.__add__", a+b, Int2(2, 5));
        self._tester.AssertEqual("Int2.__sub__", a-b, Int2(0, -1));
        self._tester.AssertEqual("Int2.__mul__", a*b, Int2(1, 6));
        self._tester.AssertEqual("Int2.__div__", c / d, Int2(5, 5));
        self._tester.AssertEqual("Int2.__copy__", a.__Copy__(), Int2(1, 2));
        self._tester.AssertEqual("Int2.__str__", a.__Str__(), "(1, 2)");

        self.UnaryAndBinopTests();

        self._tester.ShowResults();
    }

    function UnaryAndBinopTests()
    {
        # === Unary Tests ===
        a = 5;
        b = -a;              # -5
        c = +a;              # 5
        d = -(-a);           # 5
        e = +(-a);           # -5
        f = -(a -2);        # -3
        g = -a * 2;          # (-5)*2 = -10
        h = -(-a) * 3;       # (5)*3 = 15
        i = -(a + -a);       # -(0) = 0

        self._tester.AssertEqual("Unary Minus", b, -5);
        self._tester.AssertEqual("Unary Plus", c, 5);
        self._tester.AssertEqual("Double Negation", d, 5);
        self._tester.AssertEqual("Unary Plus/Minus Combo", e, -5);
        self._tester.AssertEqual("Unary on Expression", f, -3);
        self._tester.AssertEqual("Unary with Multiplication", g, -10);
        self._tester.AssertEqual("Nested Unary Multiply", h, 15);
        self._tester.AssertEqual("Negate Zero Result", i, 0);

        # === Binary Arithmetic Tests ===
        x = 10;
        y = 4;

        self._tester.AssertEqual("Addition", x + y, 14);
        self._tester.AssertEqual("Subtraction", x -y, 6);
        self._tester.AssertEqual("Multiplication", x * y, 40);
        self._tester.AssertEqual("Division", x / y, 2);   # assuming integer division
        self._tester.AssertEqual("Modulo", x % y, 2);

        # === Operator Precedence ===
        self._tester.AssertEqual("Precedence No Parens", 2 + 3 * 4, 14);
        self._tester.AssertEqual("Precedence With Parens", (2 + 3) * 4, 20);
        self._tester.AssertEqual("Unary Before Multiply", -2 * 3, -6);
        self._tester.AssertEqual("Unary Inside Parens", -(2 + 3) * 4, -20);

        # === Complex Expressions ===
        self._tester.AssertEqual("Chained Mix 1", 10 + -2 * 3, 4);
        self._tester.AssertEqual("Chained Mix 2", (10 + -2) * 3, 24);
        self._tester.AssertEqual("Chained Mix 3", -(10 - 2 * 3) / 2, -2);
        self._tester.AssertEqual("Unary and Binary Combined", 1 + -1 + +1 + -1, 0);
        self._tester.AssertEqual("Unary Negation of Result", -(1 + 2) * (3 + 4), -21);

        # === Edge Cases ===
        self._tester.AssertEqual("Zero Negation", -0, 0);
        self._tester.AssertEqual("Double Unary Zero", -(-0), 0);
        self._tester.AssertEqual("Unary Plus Zero", +0, 0);
        self._tester.AssertEqual("Negative Multiplication Symmetry", -2 * -3, 6);
        self._tester.AssertEqual("Division Negative Result", 6 / -2, -3);

        # === Logical NOT Check ===
        self._tester.AssertEqual("Logical NOT True", !false, true);
        self._tester.AssertEqual("Logical NOT False", !true, false);
        self._tester.AssertEqual("Logical NOT with Comparison", !(1 < 2), false);
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
        for (i in Range(a.Count)) {
            if (a.Get(i) != b.Get(i)) {
                return false;
            }
        }
        return true;
    }

    function GetList(a) {
        v = String.Split(a, ",", true);
        for (i in Range(v.Count)) {
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