using ApplicationManagers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicLexer
    {
        protected string _source;
        protected List<CustomLogicToken> _tokens = new List<CustomLogicToken>();
        protected int _lineOffset;
        public string Error = "";

        public CustomLogicLexer(string source, int lineOffset)
        {
            _source = source;
            _lineOffset = lineOffset;
        }

        public List<CustomLogicToken> GetTokens()
        {
            _tokens.Clear();
            string[] sourceArr = _source.Split('\n');
            List<char> chars = new List<char>();
            List<int> lines = new List<int>();
            for (int i = 0; i < sourceArr.Length; i++)
            {
                string line = sourceArr[i];
                for (int j = 0; j < line.Length; j++)
                {
                    chars.Add(line[j]);
                    lines.Add(i);
                }
            }
            for (int i = 0; i < chars.Count; i++)
            {
                char c = chars[i];
                int line = lines[i];
                try
                {
                    string twoCharToken = c.ToString();
                    if (i < chars.Count - 1)
                        twoCharToken += chars[i + 1];
                    if (char.IsLetter(c) || c == '_')
                    {
                        string boolStr = ScanBool(i, chars);
                        if (boolStr != "")
                        {
                            AddToken(CustomLogicTokenType.Primitive, boolStr == "true", line);
                            i += boolStr.Length - 1;
                        }
                        else
                        {
                            string alphaSymbol = ScanAlphaSymbol(i, chars);
                            if (alphaSymbol != "")
                            {
                                if (alphaSymbol == "null")
                                    AddToken(CustomLogicTokenType.Primitive, null, line);
                                else
                                    AddToken(CustomLogicTokenType.Symbol, CustomLogicSymbols.Symbols[alphaSymbol], line);
                                i += alphaSymbol.Length - 1;
                            }
                            else
                            {
                                string name = ScanName(i, chars);
                                AddToken(CustomLogicTokenType.Name, name, line);
                                i += name.Length - 1;
                            }
                        }
                    }
                    else if (char.IsDigit(c) || (c == '-' && i < chars.Count - 1 && char.IsDigit(chars[i + 1])))
                    {
                        string numberStr = ScanNumber(i, chars);
                        if (numberStr.Contains("."))
                            AddToken(CustomLogicTokenType.Primitive, float.Parse(numberStr), line);
                        else
                            AddToken(CustomLogicTokenType.Primitive, int.Parse(numberStr), line);
                        i += numberStr.Length - 1;
                    }
                    else if (c == '\"')
                    {
                        string strLiteral = ScanStringLiteral(i, chars);
                        AddToken(CustomLogicTokenType.Primitive, strLiteral, line);
                        i += strLiteral.Length + 1;
                    }
                    else if (c == '#')
                    {
                        string comment = ScanComment(i, chars);
                        i += comment.Length + 1;
                    }
                    else if (CustomLogicSymbols.SpecialSymbolNames.Contains(twoCharToken))
                    {
                        AddToken(CustomLogicTokenType.Symbol, CustomLogicSymbols.Symbols[twoCharToken], line);
                        i += 1;
                    }
                    else if (CustomLogicSymbols.SpecialSymbolNames.Contains(c.ToString()))
                    {
                        AddToken(CustomLogicTokenType.Symbol, CustomLogicSymbols.Symbols[c.ToString()], line);
                    }
                }
                catch (Exception e)
                {
                    line = line + 1 + _lineOffset;
                    Error = "Error parsing custom logic at line " + line.ToString() + ": " + e.Message;
                    DebugConsole.Log(Error, true);
                    return new List<CustomLogicToken>();
                }
            }
            return _tokens;
        }

        private void AddToken(CustomLogicTokenType type, object value, int line)
        {
            _tokens.Add(new CustomLogicToken(type, value, line + 1 - _lineOffset ));
        }

        private string ScanAlphaSymbol(int startIndex, List<char> chars)
        {
            string currentLexeme = "";
            for (int i = startIndex; i < chars.Count; i++)
            {
                if (!char.IsLetter(chars[i]) && chars[i] != '_')
                {
                    if (CustomLogicSymbols.AlphaSymbolNames.Contains(currentLexeme))
                        return currentLexeme;
                    return "";
                }
                currentLexeme += chars[i];
            }
            return "";
        }

        private string ScanBool(int startIndex, List<char> chars)
        {
            string currentLexeme = "";
            for (int i = startIndex; i < chars.Count; i++)
            {
                if (!char.IsLetter(chars[i]))
                {
                    if (currentLexeme == "true" || currentLexeme == "false")
                        return currentLexeme;
                    return "";
                }
                currentLexeme += chars[i];
            }
            return "";
        }

        private string ScanNumber(int startIndex, List<char> chars)
        {
            string currentLexeme = "";
            for (int i = startIndex; i < chars.Count; i++)
            {
                if (!char.IsDigit(chars[i]) && chars[i] != '.' && !(i == startIndex && chars[i] == '-'))
                    return currentLexeme;
                currentLexeme += chars[i];
            }
            return currentLexeme;
        }

        private string ScanName(int startIndex, List<char> chars)
        {
            string currentLexeme = "";
            for (int i = startIndex; i < chars.Count; i++)
            {
                if (!char.IsLetterOrDigit(chars[i]) && chars[i] != '_')
                    return currentLexeme;
                currentLexeme += chars[i];
            }
            return currentLexeme;
        }

        private string ScanStringLiteral(int startIndex, List<char> chars)
        {
            string currentLexeme = "";
            for (int i = startIndex + 1; i < chars.Count; i++)
            {
                if (chars[i] == '\"')
                    return currentLexeme;
                currentLexeme += chars[i];
            }
            throw new Exception("Unclosed string literal");
        }

        private string ScanComment(int startIndex, List<char> chars)
        {
            string currentLexeme = "";
            for (int i = startIndex + 1; i < chars.Count; i++)
            {
                if (chars[i] == '#')
                    return currentLexeme;
                currentLexeme += chars[i];
            }
            throw new Exception("Unclosed comment");
        }
    }
}