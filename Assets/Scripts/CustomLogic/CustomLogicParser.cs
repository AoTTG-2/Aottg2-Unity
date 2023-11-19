using ApplicationManagers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicParser
    {
        protected List<CustomLogicToken> _tokens = new List<CustomLogicToken>();
        public string Error = "";

        public CustomLogicParser(List<CustomLogicToken> tokens)
        {
            _tokens = tokens;
        }

        public CustomLogicStartAst GetStartAst()
        {
            CustomLogicStartAst start = new CustomLogicStartAst();
            try
            {
                ParseAst(0, start);
                return start;
            }
            catch (Exception e)
            {
                Error = e.Message;
                DebugConsole.Log("Custom logic parsing error: " + e.Message, true);
                return new CustomLogicStartAst();
            }
        }

        public CustomLogicBaseExpressionAst ParseExpression(CustomLogicBaseExpressionAst prev, int startIndex, int endIndex)
        {
            CustomLogicToken currToken = _tokens[startIndex];
            CustomLogicToken nextToken = null;
            if (startIndex > endIndex)
                return prev;
            if (startIndex < _tokens.Count - 1)
                nextToken = _tokens[startIndex + 1];
            int lowestBinopIndex = FindLowestBinop(startIndex, endIndex);
            if (lowestBinopIndex > 0)
            {
                var binopToken = _tokens[lowestBinopIndex];
                var left = ParseExpression(null, startIndex, lowestBinopIndex - 1);
                var right = ParseExpression(null, lowestBinopIndex + 1, endIndex);
                if (IsSymbolValue(binopToken, (int)CustomLogicSymbol.SetEquals))
                {
                    var assignmentAst = new CustomLogicAssignmentExpressionAst(left, currToken.Line);
                    assignmentAst.Right = right;
                    return assignmentAst;
                }
                else
                {
                    var binopAst = new CustomLogicBinopExpressionAst(binopToken, binopToken.Line);
                    binopAst.Left = left;
                    binopAst.Right = right;
                    return binopAst;
                }
            }
            else if (IsSymbolValue(currToken, (int)CustomLogicSymbol.LeftParen))
            {
                int end = FindClosingParen(startIndex);
                var expression = ParseExpression(null, startIndex + 1, end - 1);
                return ParseExpression(expression, end + 1, endIndex);
            }
            else if (currToken.Type == CustomLogicTokenType.Primitive)
            {
                CustomLogicPrimitiveExpressionAst primitiveExpressionAst = new CustomLogicPrimitiveExpressionAst(currToken.Value, currToken.Line);
                return ParseExpression(primitiveExpressionAst, startIndex + 1, endIndex);
            }
            else if (IsSymbolValue(currToken, (int)CustomLogicSymbol.Not))
            {
                CustomLogicNotExpressionAst notExpressionAst = new CustomLogicNotExpressionAst(nextToken.Line);
                var right = ParseExpression(notExpressionAst, startIndex + 1, endIndex);
                notExpressionAst.Next = right;
                return notExpressionAst;
            }
            else if (IsSymbolValue(currToken, (int)CustomLogicSymbol.Dot))
            {
                AssertTokenType(nextToken, CustomLogicTokenType.Name);
                CustomLogicToken peekToken = _tokens[startIndex + 2];
                if (IsSymbolValue(peekToken, (int)CustomLogicSymbol.LeftParen))
                {
                    CustomLogicMethodCallExpressionAst methodCallExpressionAst = new CustomLogicMethodCallExpressionAst((string)nextToken.Value, currToken.Line);
                    methodCallExpressionAst.Left = prev;
                    int start = startIndex + 2;
                    int end = FindClosingParen(start);
                    int[] commas = FindCommas(start + 1, end);
                    int startParam = start + 1;
                    if (commas.Length > 0)
                    {
                        foreach (int comma in commas)
                        {
                            var commaExpression = ParseExpression(null, startParam, comma - 1);
                            methodCallExpressionAst.Parameters.Add(commaExpression);
                            startParam = comma + 1;
                        }
                    }
                    var lastExpression = ParseExpression(null, startParam, end - 1);
                    if (lastExpression != null)
                        methodCallExpressionAst.Parameters.Add(lastExpression);
                    return ParseExpression(methodCallExpressionAst, end + 1, endIndex);
                }
                else
                {
                    CustomLogicFieldExpressionAst fieldExpressionAst = new CustomLogicFieldExpressionAst((string)nextToken.Value, currToken.Line);
                    fieldExpressionAst.Left = prev;
                    return ParseExpression(fieldExpressionAst, startIndex + 2, endIndex);
                }
            }
            else if (currToken.Type == CustomLogicTokenType.Name)
            {
                if (IsSymbolValue(nextToken, (int)CustomLogicSymbol.LeftParen))
                {
                    CustomLogicClassInstantiateExpressionAst classExpressionAst = new CustomLogicClassInstantiateExpressionAst((string)currToken.Value, currToken.Line);
                    classExpressionAst.Left = prev;
                    int start = startIndex + 1;
                    int end = FindClosingParen(start);
                    int[] commas = FindCommas(start + 1, end);
                    int startParam = start + 1;
                    if (commas.Length > 0)
                    {
                        foreach (int comma in commas)
                        {
                            var commaExpression = ParseExpression(null, startParam, comma - 1);
                            classExpressionAst.Parameters.Add(commaExpression);
                            startParam = comma + 1;
                        }
                    }
                    var lastExpression = ParseExpression(null, startParam, end - 1);
                    if (lastExpression != null)
                        classExpressionAst.Parameters.Add(lastExpression);
                    return ParseExpression(classExpressionAst, end + 1, endIndex);
                }
                else
                {
                    CustomLogicVariableExpressionAst variableExpressionAst = new CustomLogicVariableExpressionAst((string)currToken.Value, currToken.Line);
                    return ParseExpression(variableExpressionAst, startIndex + 1, endIndex);
                }
            }
            return null;
        }

        public object[] ParseExpressionAst(int startIndex)
        {
            int end = FindSemicolon(startIndex);
            return new object[] { end + 1, ParseExpression(null, startIndex, end - 1) };
        }

        public int ParseAst(int startIndex, CustomLogicBaseAst prev)
        {
            int lastIndex = startIndex;
            if (startIndex >= _tokens.Count)
                return startIndex;
            CustomLogicToken currToken = _tokens[startIndex];
            CustomLogicToken nextToken = null;
            if (startIndex < _tokens.Count - 1)
                nextToken = _tokens[startIndex + 1];
            if (prev.Type == CustomLogicAstType.Start)
            {
                if (IsSymbolIn(currToken, CustomLogicSymbols.ClassSymbols))
                {
                    CustomLogicClassDefinitionAst classAst = new CustomLogicClassDefinitionAst(currToken, currToken.Line);
                    AssertSymbolValue(_tokens[startIndex + 2], (int)CustomLogicSymbol.LeftCurly);
                    startIndex = ParseAst(startIndex + 3, classAst);
                    ((CustomLogicStartAst)prev).AddClass((string)nextToken.Value, classAst);
                }
                else
                    AssertFalse(currToken);
            }
            else if (prev.Type == CustomLogicAstType.ClassDefinition)
            {
                if (IsSymbolValue(currToken, (int)CustomLogicSymbol.Function) || IsSymbolValue(currToken, (int)CustomLogicSymbol.Coroutine))
                {
                    AssertTokenType(nextToken, CustomLogicTokenType.Name);
                    bool coroutine = IsSymbolValue(currToken, (int)CustomLogicSymbol.Coroutine);
                    CustomLogicMethodDefinitionAst methodAst = new CustomLogicMethodDefinitionAst(currToken.Line, coroutine);
                    int scanIndex = startIndex + 2;
                    CustomLogicToken scanToken = _tokens[scanIndex];
                    AssertSymbolValue(scanToken, (int)CustomLogicSymbol.LeftParen);
                    scanIndex += 1;
                    scanToken = _tokens[scanIndex];
                    while (!(scanToken.Type == CustomLogicTokenType.Symbol && (int)scanToken.Value == (int)CustomLogicSymbol.RightParen))
                    {
                        if (scanToken.Type == CustomLogicTokenType.Name)
                            methodAst.ParameterNames.Add((string)scanToken.Value);
                        else
                            AssertSymbolValue(scanToken, (int)CustomLogicSymbol.Comma);
                        scanIndex += 1;
                        scanToken = _tokens[scanIndex];
                    }
                    AssertSymbolValue(_tokens[scanIndex + 1], (int)CustomLogicSymbol.LeftCurly);
                    startIndex = ParseAst(scanIndex + 2, methodAst);
                    ((CustomLogicClassDefinitionAst)prev).AddMethod((string)nextToken.Value, methodAst);
                }
                else if (currToken.Type == CustomLogicTokenType.Name)
                {
                    AssertSymbolValue(nextToken, (int)CustomLogicSymbol.SetEquals);
                    CustomLogicVariableExpressionAst variableAst = new CustomLogicVariableExpressionAst((string)currToken.Value, currToken.Line);
                    CustomLogicAssignmentExpressionAst assignmentAst = new CustomLogicAssignmentExpressionAst(variableAst, currToken.Line);
                    int end = FindSemicolon(startIndex);
                    var expression = ParseExpression(null, startIndex + 2, end - 1);
                    assignmentAst.Right = expression;
                    startIndex = end + 1;
                    ((CustomLogicClassDefinitionAst)prev).Assignments.Add(assignmentAst);
                }
                else if (IsSymbolValue(currToken, (int)CustomLogicSymbol.RightCurly))
                {
                    return startIndex + 1;
                }
                else
                    AssertFalse(currToken);
            }
            else if (prev.Type == CustomLogicAstType.MethodDefinition || prev.Type == CustomLogicAstType.ConditionalExpression || 
                prev.Type == CustomLogicAstType.ForExpression)
            {
                if (IsSymbolValue(currToken, (int)CustomLogicSymbol.Return))
                {
                    CustomLogicBaseExpressionAst expressionAst;
                    if (IsSymbolValue(nextToken, (int)CustomLogicSymbol.Semicolon))
                    {
                        expressionAst = new CustomLogicPrimitiveExpressionAst(null, currToken.Line);
                        startIndex = startIndex + 2;
                    }
                    else
                    {
                        object[] expression = ParseExpressionAst(startIndex + 1);
                        startIndex = (int)expression[0];
                        expressionAst = (CustomLogicBaseExpressionAst)expression[1];
                    }
                    CustomLogicReturnExpressionAst returnExpression = new CustomLogicReturnExpressionAst(expressionAst, currToken.Line);
                    ((CustomLogicBlockAst)prev).Statements.Add(returnExpression);
                }
                else if (IsSymbolValue(currToken, (int)CustomLogicSymbol.Wait))
                {
                    object[] expression = ParseExpressionAst(startIndex + 1);
                    startIndex = (int)expression[0];
                    CustomLogicWaitExpressionAst returnExpression = new CustomLogicWaitExpressionAst((CustomLogicBaseExpressionAst)expression[1], currToken.Line);
                    ((CustomLogicBlockAst)prev).Statements.Add(returnExpression);
                }
                else if (currToken.Type == CustomLogicTokenType.Name)
                {
                    object[] expression = ParseExpressionAst(startIndex);
                    startIndex = (int)expression[0];
                    ((CustomLogicBlockAst)prev).Statements.Add((CustomLogicBaseExpressionAst)expression[1]);
                }
                else if (IsSymbolIn(currToken, CustomLogicSymbols.ConditionalSymbols))
                {
                    CustomLogicConditionalBlockAst conditionalAst = new CustomLogicConditionalBlockAst(currToken, currToken.Line);
                    if (IsSymbolValue(currToken, (int)CustomLogicSymbol.Else))
                    {
                        AssertSymbolValue(_tokens[startIndex + 1], (int)CustomLogicSymbol.LeftCurly);
                        startIndex = ParseAst(startIndex + 2, conditionalAst);
                    }
                    else
                    {
                        AssertSymbolValue(nextToken, (int)CustomLogicSymbol.LeftParen);
                        int end = FindClosingParen(startIndex + 1);
                        conditionalAst.Condition = ParseExpression(null, startIndex + 2, end - 1);
                        AssertSymbolValue(_tokens[end + 1], (int)CustomLogicSymbol.LeftCurly);
                        startIndex = ParseAst(end + 2, conditionalAst);
                    }
                    ((CustomLogicBlockAst)prev).Statements.Add(conditionalAst);
                }
                else if (IsSymbolValue(currToken, (int)CustomLogicSymbol.For))
                {
                    AssertSymbolValue(nextToken, (int)CustomLogicSymbol.LeftParen);
                    int end = FindClosingParen(startIndex);
                    CustomLogicForBlockAst foreachAst = new CustomLogicForBlockAst(currToken.Line);
                    int scanIndex = startIndex + 2;
                    AssertTokenType(_tokens[scanIndex], CustomLogicTokenType.Name);
                    CustomLogicVariableExpressionAst variableAst = new CustomLogicVariableExpressionAst((string)_tokens[scanIndex].Value, _tokens[scanIndex].Line);
                    foreachAst.Variable = variableAst;
                    AssertSymbolValue(_tokens[scanIndex + 1], (int)CustomLogicSymbol.In);
                    var expression = ParseExpression(null, scanIndex + 2, end - 1);
                    foreachAst.Iterable = expression;
                    AssertSymbolValue(_tokens[end + 1], (int)CustomLogicSymbol.LeftCurly);
                    startIndex = ParseAst(end + 2, foreachAst);
                    ((CustomLogicBlockAst)prev).Statements.Add(foreachAst);
                }
                else if (IsSymbolValue(currToken, (int)CustomLogicSymbol.RightCurly))
                {
                    return startIndex + 1;
                }
                else
                    AssertFalse(currToken);
            }
            if (startIndex == lastIndex)
            {
                AssertFalse(currToken);
            }
            return ParseAst(startIndex, prev);
        }

        private int FindLowestBinop(int startIndex, int endIndex)
        {
            int lowestBinopIndex = -1;
            int lowestBinopValue = int.MaxValue;
            int parenCount = 0;
            for (int i = startIndex; i < endIndex; i++)
            {
                var token = _tokens[i];
                if (IsSymbolValue(token, (int)CustomLogicSymbol.LeftParen))
                    parenCount++;
                else if (IsSymbolValue(token, (int)CustomLogicSymbol.RightParen))
                    parenCount--;
                if (parenCount > 0)
                    continue;
                if (IsSymbolIn(token, CustomLogicSymbols.BinopSymbols) || IsSymbolValue(token, (int)CustomLogicSymbol.SetEquals))
                {
                    if ((int)token.Value < lowestBinopValue)
                    {
                        lowestBinopIndex = i;
                        lowestBinopValue = (int)token.Value;
                    }
                }
            }
            return lowestBinopIndex;
        }

        private int[] FindCommas(int startIndex, int endIndex)
        {
            List<int> ints = new List<int>();
            int parenCount = 0;
            for (int i = startIndex; i < endIndex; i++)
            {
                var token = _tokens[i];
                if (IsSymbolValue(token, (int)CustomLogicSymbol.LeftParen))
                    parenCount++;
                if (IsSymbolValue(token, (int)CustomLogicSymbol.RightParen))
                    parenCount--;
                if (IsSymbolValue(token, (int)CustomLogicSymbol.Comma) && parenCount == 0)
                    ints.Add(i);
            }
            return ints.ToArray();
        }

        private int FindClosingParen(int startIndex)
        {
            int parenCount = 0;
            for (int i = startIndex; i < _tokens.Count; i++)
            {
                var token = _tokens[i];
                if (IsSymbolValue(token, (int)CustomLogicSymbol.LeftParen))
                    parenCount++;
                else if (IsSymbolValue(token, (int)CustomLogicSymbol.RightParen))
                {
                    parenCount--;
                    if (parenCount == 0)
                        return i;
                }
            }
            return -1;
        }

        private int FindSemicolon(int startIndex)
        {
            for (int i = startIndex; i < _tokens.Count; i++)
            {
                var token = _tokens[i];
                if (IsSymbolValue(token, (int)CustomLogicSymbol.Semicolon))
                {
                    return i;
                }
            }
            return -1;
        }

        private bool IsSymbolIn(CustomLogicToken token, HashSet<int> symbols)
        {
            return token != null && token.Type == CustomLogicTokenType.Symbol && symbols.Contains((int)token.Value);
        }

        private bool IsSymbolValue(CustomLogicToken token, int symbolValue)
        {
            return token != null && token.Type == CustomLogicTokenType.Symbol && (int)token.Value == symbolValue;
        }

        private void AssertSymbolValue(CustomLogicToken token, int symbolValue)
        {
            if (token == null || token.Type != CustomLogicTokenType.Symbol || (int)token.Value != symbolValue)
            {
                throw new Exception("Parsing error at line " + token.Line.ToString() + ", got " + GetTokenString(token)
                    + ", expected " + ((CustomLogicSymbol)symbolValue).ToString());
            }
                
        }

        private void AssertTokenType(CustomLogicToken token, CustomLogicTokenType type)
        {
            if (token == null || token.Type != type)
                throw new Exception("Parsing error at line " + token.Line.ToString() + ", got " + GetTokenString(token)
                    + ", expected " + type.ToString());
        }

        private void AssertFalse(CustomLogicToken token)
        {
            throw new Exception("Parsing error at line " + token.Line.ToString() + ", got " + GetTokenString(token));
        }

        private string GetTokenString(CustomLogicToken token)
        {
            if (token == null)
                return "null";
            if (token.Type == CustomLogicTokenType.Symbol)
                return ((CustomLogicSymbol)token.Value).ToString();
            return token.Value.ToString();
        }
    }
}