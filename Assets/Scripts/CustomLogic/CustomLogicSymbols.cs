using System;
using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicSymbols
    {
        public static Dictionary<string, int> Symbols = new Dictionary<string, int>();
        public static HashSet<string> SpecialSymbolNames = new HashSet<string>();
        public static HashSet<string> AlphaSymbolNames = new HashSet<string>();
        public static HashSet<int> BinopSymbols = new HashSet<int>();
        public static HashSet<int> ClassSymbols = new HashSet<int>();
        public static HashSet<int> ConditionalSymbols = new HashSet<int>();

        public static void Init()
        {
            AddSymbols();
            CategorizeSymbols();
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
            Symbols.Add(",", (int)CustomLogicSymbol.Comma);
            Symbols.Add(".", (int)CustomLogicSymbol.Dot);
            Symbols.Add("||", (int)CustomLogicSymbol.Or);
            Symbols.Add("&&", (int)CustomLogicSymbol.And);
            Symbols.Add("+", (int)CustomLogicSymbol.Plus);
            Symbols.Add("-", (int)CustomLogicSymbol.Minus);
            Symbols.Add("*", (int)CustomLogicSymbol.Times);
            Symbols.Add("/", (int)CustomLogicSymbol.Divide);
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
                "return", "if", "else", "for", "while", "elif", "in" })
                AlphaSymbolNames.Add(symbolName);
            foreach (string symbolName in Symbols.Keys)
            {
                if (!AlphaSymbolNames.Contains(symbolName))
                    SpecialSymbolNames.Add(symbolName);
            }
            foreach (CustomLogicSymbol symbol in new CustomLogicSymbol[] { CustomLogicSymbol.Or, CustomLogicSymbol.And, CustomLogicSymbol.Plus,
                CustomLogicSymbol.Minus, CustomLogicSymbol.Times, CustomLogicSymbol.Divide, CustomLogicSymbol.Equals, CustomLogicSymbol.NotEquals,
                CustomLogicSymbol.LessThan, CustomLogicSymbol.GreaterThan, CustomLogicSymbol.LessThanOrEquals, CustomLogicSymbol.GreaterThanOrEquals})
            {
                BinopSymbols.Add((int)symbol);
            }
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
        Not,
        SetEquals,
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
