using ApplicationManagers;
using System;
using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicLexer
    {
        protected string _source;
        protected List<CustomLogicToken> _tokens = new List<CustomLogicToken>();
        protected int _lineOffset;
        public string Error = "";

        private int _line;
        private char[] _chars;
        
        public CustomLogicLexer(string source, int lineOffset)
        {
            _source = source;
            _lineOffset = lineOffset;
        }

        public List<CustomLogicToken> GetTokens()
        {
            _line = 0;
            _tokens.Clear();
            _chars = _source.ToCharArray();
            
            for (int i = 0; i < _chars.Length; i++)
            {
                char c = _chars[i];
                try
                {
                    if (c == '\n')
                    {
                        _line++;
                        continue;
                    }
                    
                    string twoCharToken = c.ToString();
                    if (i < _chars.Length - 1)
                        twoCharToken += _chars[i + 1];
                    if (char.IsLetter(c) || c == '_')
                    {
                        string boolStr = ScanBool(i);
                        if (boolStr != "")
                        {
                            AddToken(CustomLogicTokenType.Primitive, boolStr == "true", _line);
                            i += boolStr.Length - 1;
                        }
                        else
                        {
                            string alphaSymbol = ScanAlphaSymbol(i);
                            if (alphaSymbol != "")
                            {
                                if (alphaSymbol == "null")
                                    AddToken(CustomLogicTokenType.Primitive, null, _line);
                                else
                                    AddToken(CustomLogicTokenType.Symbol, CustomLogicSymbols.Symbols[alphaSymbol], _line);
                                i += alphaSymbol.Length - 1;
                            }
                            else
                            {
                                string name = ScanName(i);
                                AddToken(CustomLogicTokenType.Name, name, _line);
                                i += name.Length - 1;
                            }
                        }
                    }
                    else if (char.IsDigit(c) || (c == '-' && i < _chars.Length - 1 && char.IsDigit(_chars[i + 1])))
                    {
                        string numberStr = ScanNumber(i);
                        if (numberStr.Contains("."))
                            AddToken(CustomLogicTokenType.Primitive, float.Parse(numberStr), _line);
                        else
                            AddToken(CustomLogicTokenType.Primitive, int.Parse(numberStr), _line);
                        i += numberStr.Length - 1;
                    }
                    else if (c == '\"')
                    {
                        string strLiteral = ScanStringLiteral(i);
                        AddToken(CustomLogicTokenType.Primitive, strLiteral, _line);
                        i += strLiteral.Length + 1;
                    }
                    else if (c == '#')
                    {
                        string comment = ScanComment(i);
                        i += comment.Length + 1;
                    }
                    else if (CustomLogicSymbols.SpecialSymbolNames.Contains(twoCharToken))
                    {
                        AddToken(CustomLogicTokenType.Symbol, CustomLogicSymbols.Symbols[twoCharToken], _line);
                        i += 1;
                    }
                    else if (CustomLogicSymbols.SpecialSymbolNames.Contains(c.ToString()))
                    {
                        if (c == '/' && i + 1 < _chars.Length && _chars[i + 1] == '*')
                        {
                            var comment = ScanBlockComment(i + 1);
                            i += comment.Length + 3;
                        }
                        else
                            AddToken(CustomLogicTokenType.Symbol, CustomLogicSymbols.Symbols[c.ToString()], _line);
                    }
                }
                catch (Exception e)
                {
                    _line = _line + 1 + _lineOffset;
                    Error = "Error parsing custom logic at line " + _line.ToString() + ": " + e.Message;
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

        private string ScanAlphaSymbol(int startIndex)
        {
            string currentLexeme = "";
            for (int i = startIndex; i < _chars.Length; i++)
            {
                if (!char.IsLetter(_chars[i]) && _chars[i] != '_')
                {
                    if (CustomLogicSymbols.AlphaSymbolNames.Contains(currentLexeme))
                        return currentLexeme;
                    return "";
                }
                currentLexeme += _chars[i];
            }
            return "";
        }

        private string ScanBool(int startIndex)
        {
            string currentLexeme = "";
            for (int i = startIndex; i < _chars.Length; i++)
            {
                if (!char.IsLetter(_chars[i]))
                {
                    if (currentLexeme == "true" || currentLexeme == "false")
                        return currentLexeme;
                    return "";
                }
                currentLexeme += _chars[i];
            }
            return "";
        }

        private string ScanNumber(int startIndex)
        {
            string currentLexeme = "";
            for (int i = startIndex; i < _chars.Length; i++)
            {
                if (!char.IsDigit(_chars[i]) && _chars[i] != '.' && !(i == startIndex && _chars[i] == '-'))
                    return currentLexeme;
                currentLexeme += _chars[i];
            }
            return currentLexeme;
        }

        private string ScanName(int startIndex)
        {
            string currentLexeme = "";
            for (int i = startIndex; i < _chars.Length; i++)
            {
                if (!char.IsLetterOrDigit(_chars[i]) && _chars[i] != '_')
                    return currentLexeme;
                currentLexeme += _chars[i];
            }
            return currentLexeme;
        }

        private string ScanStringLiteral(int startIndex)
        {
            string currentLexeme = "";
            for (int i = startIndex + 1; i < _chars.Length; i++)
            {
                if (_chars[i] == '\n')
                    _line++;
                
                if (_chars[i] == '\"')
                    return currentLexeme;
                currentLexeme += _chars[i];
            }
            throw new Exception("Unclosed string literal");
        }

        private string ScanComment(int startIndex)
        {
            string currentLexeme = "";
            for (int i = startIndex + 1; i < _chars.Length; i++)
            {
                if (_chars[i] == '\n')
                {
                    _line++;
                    return currentLexeme;
                }
                currentLexeme += _chars[i];
            }
            throw new Exception("Unclosed comment");
        }
        
        private string ScanBlockComment(int startIndex)
        {
            string currentLexeme = "";
            for (int i = startIndex + 1; i < _chars.Length; i++)
            {
                if (_chars[i] == '\n')
                    _line++;
                
                if (_chars[i] == '*' && i + 1 < _chars.Length && _chars[i + 1] == '/')
                    return currentLexeme;

                currentLexeme += _chars[i];
            }

            throw new Exception("Unclosed block comment");
        }
    }
}