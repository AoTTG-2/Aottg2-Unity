using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicSymbols
    {
        public static Dictionary<string, int> Symbols = new Dictionary<string, int>();
        public static HashSet<string> SpecialSymbolNames = new HashSet<string>();
        public static HashSet<string> AlphaSymbolNames = new HashSet<string>();
        public static Dictionary<int, int> BinopSymbolPriorities = new Dictionary<int, int>();
        public static HashSet<int> ClassSymbols = new HashSet<int>();
        public static HashSet<int> ConditionalSymbols = new HashSet<int>();
        private static bool _loaded = false;

        public static void Init()
        {
            if (_loaded)
                return;
            ClearSymbols();
            AddSymbols();
            CategorizeSymbols();
            _loaded = true;
        }

        private static void ClearSymbols()
        {
            Symbols.Clear();
            SpecialSymbolNames.Clear();
            AlphaSymbolNames.Clear();
            BinopSymbolPriorities.Clear();
            ClassSymbols.Clear();
            ConditionalSymbols.Clear();
        }

        private static void AddSymbols()
        {
            Symbols.Add("class", (int)CustomLogicSymbol.Class);
            Symbols.Add("extension", (int)CustomLogicSymbol.Extension);
            Symbols.Add("component", (int)CustomLogicSymbol.Component);
            Symbols.Add("cutscene", (int)CustomLogicSymbol.Cutscene);
            Symbols.Add("function", (int)CustomLogicSymbol.Function);
            Symbols.Add("coroutine", (int)CustomLogicSymbol.Coroutine);
            Symbols.Add("wait", (int)CustomLogicSymbol.Wait);
            Symbols.Add("null", (int)CustomLogicSymbol.Null);
            Symbols.Add("return", (int)CustomLogicSymbol.Return);
            Symbols.Add("break", (int)CustomLogicSymbol.Break);
            Symbols.Add("continue", (int)CustomLogicSymbol.Continue);
            Symbols.Add("if", (int)CustomLogicSymbol.If);
            Symbols.Add("else", (int)CustomLogicSymbol.Else);
            Symbols.Add("elif", (int)CustomLogicSymbol.ElseIf);
            Symbols.Add("for", (int)CustomLogicSymbol.For);
            Symbols.Add("while", (int)CustomLogicSymbol.While);
            Symbols.Add("in", (int)CustomLogicSymbol.In);
            Symbols.Add("{", (int)CustomLogicSymbol.LeftCurly);
            Symbols.Add("}", (int)CustomLogicSymbol.RightCurly);
            Symbols.Add("(", (int)CustomLogicSymbol.LeftParen);
            Symbols.Add(")", (int)CustomLogicSymbol.RightParen);
            Symbols.Add(";", (int)CustomLogicSymbol.Semicolon);
            Symbols.Add("\"", (int)CustomLogicSymbol.DoubleQuote);
            Symbols.Add("=", (int)CustomLogicSymbol.SetEquals);
            Symbols.Add("+=", (int)CustomLogicSymbol.PlusEquals);
            Symbols.Add("-=", (int)CustomLogicSymbol.MinusEquals);
            Symbols.Add("*=", (int)CustomLogicSymbol.TimesEquals);
            Symbols.Add("/=", (int)CustomLogicSymbol.DivideEquals);
            Symbols.Add(",", (int)CustomLogicSymbol.Comma);
            Symbols.Add(".", (int)CustomLogicSymbol.Dot);
            Symbols.Add("||", (int)CustomLogicSymbol.Or);
            Symbols.Add("&&", (int)CustomLogicSymbol.And);
            Symbols.Add("+", (int)CustomLogicSymbol.Plus);
            Symbols.Add("-", (int)CustomLogicSymbol.Minus);
            Symbols.Add("*", (int)CustomLogicSymbol.Times);
            Symbols.Add("/", (int)CustomLogicSymbol.Divide);
            Symbols.Add("%", (int)CustomLogicSymbol.Modulo);
            Symbols.Add("==", (int)CustomLogicSymbol.Equals);
            Symbols.Add("!=", (int)CustomLogicSymbol.NotEquals);
            Symbols.Add("<", (int)CustomLogicSymbol.LessThan);
            Symbols.Add(">", (int)CustomLogicSymbol.GreaterThan);
            Symbols.Add("<=", (int)CustomLogicSymbol.LessThanOrEquals);
            Symbols.Add(">=", (int)CustomLogicSymbol.GreaterThanOrEquals);
            Symbols.Add("!", (int)CustomLogicSymbol.Not);
        }

        private static void CategorizeSymbols()
        {
            foreach (string symbolName in new string[] { "class", "component", "extension", "cutscene", "function", "coroutine", "wait", "null",
                "return", "break", "continue", "if", "else", "for", "while", "elif", "in" })
                AlphaSymbolNames.Add(symbolName);
            foreach (string symbolName in Symbols.Keys)
            {
                if (!AlphaSymbolNames.Contains(symbolName))
                    SpecialSymbolNames.Add(symbolName);
            }
            BinopSymbolPriorities.Add((int)CustomLogicSymbol.SetEquals, 0);
            foreach (CustomLogicSymbol symbol in new CustomLogicSymbol[] { CustomLogicSymbol.PlusEquals, CustomLogicSymbol.MinusEquals, CustomLogicSymbol.TimesEquals, CustomLogicSymbol.DivideEquals })
                BinopSymbolPriorities.Add((int)symbol, 0);
            foreach (CustomLogicSymbol symbol in new CustomLogicSymbol[] { CustomLogicSymbol.Or, CustomLogicSymbol.And })
                BinopSymbolPriorities.Add((int)symbol, 1);
            foreach (CustomLogicSymbol symbol in new CustomLogicSymbol[] { CustomLogicSymbol.Equals, CustomLogicSymbol.NotEquals,
                CustomLogicSymbol.LessThan, CustomLogicSymbol.GreaterThan, CustomLogicSymbol.LessThanOrEquals, CustomLogicSymbol.GreaterThanOrEquals })
                BinopSymbolPriorities.Add((int)symbol, 2);
            foreach (CustomLogicSymbol symbol in new CustomLogicSymbol[] { CustomLogicSymbol.Plus, CustomLogicSymbol.Minus })
                BinopSymbolPriorities.Add((int)symbol, 3);
            foreach (CustomLogicSymbol symbol in new CustomLogicSymbol[] { CustomLogicSymbol.Times, CustomLogicSymbol.Divide, CustomLogicSymbol.Modulo })
                BinopSymbolPriorities.Add((int)symbol, 4);
            foreach (CustomLogicSymbol symbol in new CustomLogicSymbol[] { CustomLogicSymbol.Class, CustomLogicSymbol.Component, CustomLogicSymbol.Extension,
            CustomLogicSymbol.Cutscene})
                ClassSymbols.Add((int)symbol);
            foreach (CustomLogicSymbol symbol in new CustomLogicSymbol[] { CustomLogicSymbol.If, CustomLogicSymbol.While, CustomLogicSymbol.Else, CustomLogicSymbol.ElseIf })
                ConditionalSymbols.Add((int)symbol);
        }
    }

    public enum CustomLogicSymbol
    {
        Component,
        Class,
        Extension,
        Cutscene,
        Function,
        Coroutine,
        Wait,
        Null,
        LeftCurly,
        RightCurly,
        LeftParen,
        RightParen,
        Return,
        Continue,
        Break,
        Not,
        SetEquals,
        PlusEquals,
        MinusEquals,
        TimesEquals,
        DivideEquals,
        And,
        Or,
        LessThan,
        GreaterThan,
        LessThanOrEquals,
        GreaterThanOrEquals,
        Equals,
        NotEquals,
        Plus,
        Minus,
        Times,
        Divide,
        Modulo,
        Semicolon,
        DoubleQuote,
        Comma,
        Dot,
        If,
        Else,
        ElseIf,
        While,
        For,
        In
    }
}
