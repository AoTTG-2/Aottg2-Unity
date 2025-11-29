using ApplicationManagers;
using Characters;
using GameManagers;
using Map;
using Photon.Pun;
using Photon.Realtime;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using Utility;


namespace CustomLogic
{
    partial class CustomLogicEvaluator
    {
        public float CurrentTime;
        public bool HasSetMusic = false;
        public Dictionary<int, CustomLogicNetworkViewBuiltin> IdToNetworkView = new Dictionary<int, CustomLogicNetworkViewBuiltin>();
        public Dictionary<int, CustomLogicMapObjectBuiltin> IdToMapObjectBuiltin = new Dictionary<int, CustomLogicMapObjectBuiltin>();
        protected CustomLogicStartAst _start;
        protected Dictionary<string, CustomLogicClassInstance> _staticClasses = new Dictionary<string, CustomLogicClassInstance>();
        protected Dictionary<string, List<CustomLogicClassInstance>> _callbacks = new Dictionary<string, List<CustomLogicClassInstance>>();
        public Dictionary<int, Dictionary<string, float>> PlayerIdToLastPropertyChanges = new Dictionary<int, Dictionary<string, float>>();
        public string ScoreboardHeader = "Kills / Deaths / Max / Total";
        public string ScoreboardProperty = "";
        public static readonly object[] EmptyArgs = Array.Empty<object>();
        public bool DefaultShowKillScore = true;
        public bool DefaultShowKillFeed = true;
        public bool DefaultAddKillScore = true;
        public bool ShowScoreboardLoadout = true;
        public bool ShowScoreboardStatus = true;
        public string ForcedCharacterType = string.Empty;
        public string ForcedLoadout = string.Empty;
        private int _baseLogicOffset = 0;

        public CustomLogicEvaluator(CustomLogicStartAst start, int baseLogicOffset = 0)
        {
            _start = start;
            _baseLogicOffset = baseLogicOffset;
        }

        #region Evaluator
        public CustomLogicClassInstance CreateClassInstance(string className, object[] parameterValues, bool init = true)
        {
            if (CustomLogicBuiltinTypes.IsBuiltinType(className))
            {
                if (CustomLogicBuiltinTypes.IsAbstract(className))
                    throw new Exception("Cannot instantiate abstract type " + className);

                return CustomLogicBuiltinTypes.CreateClassInstance(className, parameterValues);
            }

            var classInstance = new UserClassInstance(className);
            if (init)
            {
                RunAssignmentsClassInstance(classInstance);
                EvaluateMethod(classInstance, "Init", parameterValues);
                classInstance.Inited = true;
            }
            return classInstance;
        }

        private void RunAssignmentsClassInstance(CustomLogicClassInstance classInstance)
        {
            CustomLogicClassDefinitionAst classAst = _start.Classes[classInstance.ClassName];
            foreach (CustomLogicAssignmentExpressionAst assignment in classAst.Assignments)
            {
                string variableName = ((CustomLogicVariableExpressionAst)assignment.Left).Name;
                object value = EvaluateExpression(classInstance, new Dictionary<string, object>(), assignment.Right);
                classInstance.Variables[variableName] = value;
            }

            foreach (var (name, methodAst) in classAst.Methods)
            {
                var method = new UserMethod(classInstance, methodAst);
                classInstance.Variables[name] = method;
            }
        }

        private IEnumerator EvaluateBlockCoroutine(CustomLogicClassInstance classInstance, Dictionary<string, object> localVariables,
            List<CustomLogicBaseAst> statements)
        {
            ConditionalEvalState conditionalState = ConditionalEvalState.None;
            foreach (CustomLogicBaseAst statement in statements)
            {
                if (statement is CustomLogicAssignmentExpressionAst assignment)
                {
                    EvaluateAssignmentExpression(classInstance, localVariables, assignment);
                }
                else if (statement is CustomLogicReturnExpressionAst || statement is CustomLogicBreakExpressionAst || statement is CustomLogicContinueExpressionAst)
                {
                    yield return statement;
                    yield break;
                }
                else if (statement is CustomLogicWaitExpressionAst waitExpressionAst)
                {
                    object value = EvaluateExpression(classInstance, localVariables, waitExpressionAst.WaitTime);
                    bool isCutscene = (int)_start.Classes[classInstance.ClassName].Token.Value == (int)CustomLogicSymbol.Cutscene;

                    if (value is null)
                        yield return null;
                    else if (waitExpressionAst.WaitTime is CustomLogicMethodCallExpressionAst methodCallExpressionAst)
                    {
                        if (isCutscene)
                            value = null;

                        if (value is Coroutine coroutine)
                            yield return value;
                        else
                            yield return null;
                    }
                    else
                    {
                        if (isCutscene)
                        {
                            float time = value.UnboxToFloat();
                            while (time > 0f && !CustomLogicManager.SkipCutscene)
                            {
                                yield return new WaitForSeconds(0.1f);
                                time -= 0.1f;
                            }
                        }
                        else
                            yield return new WaitForSeconds(value.UnboxToFloat());
                        yield return null;
                    }
                }
                else if (statement is CustomLogicConditionalBlockAst)
                {
                    CustomLogicConditionalBlockAst conditional = (CustomLogicConditionalBlockAst)statement;
                    if ((int)conditional.Token.Value == (int)CustomLogicSymbol.If)
                    {
                        if ((bool)EvaluateExpression(classInstance, localVariables, conditional.Condition))
                        {
                            var cwd = new CoroutineWithData(CustomLogicManager._instance, EvaluateBlockCoroutine(classInstance, localVariables, conditional.Statements));
                            yield return cwd.Coroutine;
                            yield return cwd.Result;
                            if (cwd.Result is CustomLogicReturnExpressionAst || cwd.Result is CustomLogicBreakExpressionAst || cwd.Result is CustomLogicContinueExpressionAst)
                                yield break;
                            conditionalState = ConditionalEvalState.PassedIf;
                        }
                        else
                            conditionalState = ConditionalEvalState.FailedIf;
                    }
                    else if ((int)conditional.Token.Value == (int)CustomLogicSymbol.While)
                    {
                        while ((bool)EvaluateExpression(classInstance, localVariables, conditional.Condition))
                        {
                            var cwd = new CoroutineWithData(CustomLogicManager._instance, EvaluateBlockCoroutine(classInstance, localVariables, conditional.Statements));
                            yield return cwd.Coroutine;
                            yield return cwd.Result;
                            if (cwd.Result is CustomLogicBreakExpressionAst || cwd.Result is CustomLogicContinueExpressionAst)
                                yield return null;
                            if (cwd.Result is CustomLogicReturnExpressionAst || cwd.Result is CustomLogicBreakExpressionAst)
                                yield break;
                        }
                        conditionalState = ConditionalEvalState.None;
                    }
                    else if ((int)conditional.Token.Value == (int)CustomLogicSymbol.Else)
                    {
                        if (conditionalState == ConditionalEvalState.FailedIf || conditionalState == ConditionalEvalState.FailedElseIf)
                        {
                            var cwd = new CoroutineWithData(CustomLogicManager._instance, EvaluateBlockCoroutine(classInstance, localVariables, conditional.Statements));
                            yield return cwd.Coroutine;
                            yield return cwd.Result;
                            if (cwd.Result is CustomLogicReturnExpressionAst || cwd.Result is CustomLogicBreakExpressionAst || cwd.Result is CustomLogicContinueExpressionAst)
                                yield break;
                        }
                        conditionalState = ConditionalEvalState.None;
                    }
                    else if ((int)conditional.Token.Value == (int)CustomLogicSymbol.ElseIf)
                    {
                        if (conditionalState == ConditionalEvalState.PassedIf || conditionalState == ConditionalEvalState.PassedElseIf)
                        {
                        }
                        else if ((conditionalState == ConditionalEvalState.FailedIf || conditionalState == ConditionalEvalState.FailedElseIf) &&
                            (bool)EvaluateExpression(classInstance, localVariables, conditional.Condition))
                        {
                            var cwd = new CoroutineWithData(CustomLogicManager._instance, EvaluateBlockCoroutine(classInstance, localVariables, conditional.Statements));
                            yield return cwd.Coroutine;
                            yield return cwd.Result;
                            if (cwd.Result is CustomLogicReturnExpressionAst || cwd.Result is CustomLogicBreakExpressionAst || cwd.Result is CustomLogicContinueExpressionAst)
                                yield break;
                            conditionalState = ConditionalEvalState.PassedElseIf;
                        }
                        else
                            conditionalState = ConditionalEvalState.FailedElseIf;
                    }
                }
                else if (statement is CustomLogicForBlockAst)
                {
                    CustomLogicForBlockAst forBlock = (CustomLogicForBlockAst)statement;
                    foreach (object variable in ((CustomLogicListBuiltin)EvaluateExpression(classInstance, localVariables, forBlock.Iterable)).List)
                    {
                        string variableName = forBlock.Variable.Name;
                        if (localVariables.ContainsKey(variableName))
                            localVariables[variableName] = variable;
                        else
                            localVariables.Add(variableName, variable);
                        var cwd = new CoroutineWithData(CustomLogicManager._instance, EvaluateBlockCoroutine(classInstance, localVariables, forBlock.Statements));
                        yield return cwd.Coroutine;
                        yield return cwd.Result;
                        if (cwd.Result is CustomLogicBreakExpressionAst || cwd.Result is CustomLogicContinueExpressionAst)
                            yield return null;
                        if (cwd.Result is CustomLogicReturnExpressionAst || cwd.Result is CustomLogicBreakExpressionAst)
                            yield break;
                    }
                }
                else if (statement is CustomLogicBaseExpressionAst)
                    EvaluateExpression(classInstance, localVariables, ((CustomLogicBaseExpressionAst)statement));
                if (!(statement is CustomLogicConditionalBlockAst))
                    conditionalState = ConditionalEvalState.None;
            }
        }

        private bool EvaluateBlock(CustomLogicClassInstance classInstance, Dictionary<string, object> localVariables, List<CustomLogicBaseAst> statements, out object result)
        {
            ConditionalEvalState conditionalState = ConditionalEvalState.None;
            bool iter = false;
            result = null;
            foreach (CustomLogicBaseAst statement in statements)
            {
                if (statement is CustomLogicAssignmentExpressionAst assignment)
                {
                    EvaluateAssignmentExpression(classInstance, localVariables, assignment);
                }
                else if (statement is CustomLogicReturnExpressionAst)
                {
                    iter = true;
                    result = EvaluateExpression(classInstance, localVariables, ((CustomLogicReturnExpressionAst)statement).ReturnValue);
                    return iter;
                }
                else if (statement is CustomLogicBreakExpressionAst || statement is CustomLogicContinueExpressionAst)
                {
                    iter = true;
                    result = statement;
                    return iter;
                }
                else if (statement is CustomLogicConditionalBlockAst)
                {
                    CustomLogicConditionalBlockAst conditional = (CustomLogicConditionalBlockAst)statement;
                    if ((int)conditional.Token.Value == (int)CustomLogicSymbol.If)
                    {
                        if ((bool)EvaluateExpression(classInstance, localVariables, conditional.Condition))
                        {
                            bool nextIter = EvaluateBlock(classInstance, localVariables, conditional.Statements, out object nextResult);
                            if (nextIter)
                            {
                                if (nextResult is not CustomLogicContinueExpressionAst && nextResult is not CustomLogicBreakExpressionAst)
                                {
                                    result = nextResult;
                                    return nextIter;
                                }
                            }
                            conditionalState = ConditionalEvalState.PassedIf;
                        }
                        else
                            conditionalState = ConditionalEvalState.FailedIf;
                    }
                    else if ((int)conditional.Token.Value == (int)CustomLogicSymbol.While)
                    {
                        while ((bool)EvaluateExpression(classInstance, localVariables, conditional.Condition))
                        {
                            bool nextIter = EvaluateBlock(classInstance, localVariables, conditional.Statements, out object nextResult);
                            if (nextIter)
                            {
                                if (nextResult is CustomLogicBreakExpressionAst)
                                    break;
                                else if (nextResult is not CustomLogicContinueExpressionAst)
                                {
                                    result = nextResult;
                                    return nextIter;
                                }
                            }
                        }
                        conditionalState = ConditionalEvalState.None;
                    }
                    else if ((int)conditional.Token.Value == (int)CustomLogicSymbol.Else)
                    {
                        if (conditionalState == ConditionalEvalState.FailedIf || conditionalState == ConditionalEvalState.FailedElseIf)
                        {
                            bool nextIter = EvaluateBlock(classInstance, localVariables, conditional.Statements, out object nextResult);
                            if (nextIter)
                            {
                                if (nextResult is not CustomLogicContinueExpressionAst && nextResult is not CustomLogicBreakExpressionAst)
                                {
                                    result = nextResult;
                                    return nextIter;
                                }
                            }
                        }
                        conditionalState = ConditionalEvalState.None;
                    }
                    else if ((int)conditional.Token.Value == (int)CustomLogicSymbol.ElseIf)
                    {
                        if (conditionalState == ConditionalEvalState.PassedIf || conditionalState == ConditionalEvalState.PassedElseIf)
                        {
                        }
                        else if ((conditionalState == ConditionalEvalState.FailedIf || conditionalState == ConditionalEvalState.FailedElseIf) &&
                            (bool)EvaluateExpression(classInstance, localVariables, conditional.Condition))
                        {
                            bool nextIter = EvaluateBlock(classInstance, localVariables, conditional.Statements, out object nextResult);
                            if (nextIter)
                            {
                                if (nextResult is not CustomLogicContinueExpressionAst && nextResult is not CustomLogicBreakExpressionAst)
                                {
                                    result = nextResult;
                                    return nextIter;
                                }
                            }
                            conditionalState = ConditionalEvalState.PassedElseIf;
                        }
                        else
                            conditionalState = ConditionalEvalState.FailedElseIf;
                    }
                }
                else if (statement is CustomLogicForBlockAst)
                {
                    CustomLogicForBlockAst forBlock = (CustomLogicForBlockAst)statement;
                    foreach (object variable in ((CustomLogicListBuiltin)EvaluateExpression(classInstance, localVariables, forBlock.Iterable)).List)
                    {
                        string variableName = forBlock.Variable.Name;
                        if (localVariables.ContainsKey(variableName))
                            localVariables[variableName] = variable;
                        else
                            localVariables.Add(variableName, variable);
                        bool nextIter = EvaluateBlock(classInstance, localVariables, forBlock.Statements, out object nextResult);
                        if (nextIter)
                        {
                            if (nextResult is CustomLogicBreakExpressionAst)
                                break;
                            else if (nextResult is not CustomLogicContinueExpressionAst)
                            {
                                result = nextResult;
                                return nextIter;
                            }
                        }
                    }
                }
                else if (statement is CustomLogicBaseExpressionAst)
                    EvaluateExpression(classInstance, localVariables, ((CustomLogicBaseExpressionAst)statement));
                if (!(statement is CustomLogicConditionalBlockAst))
                    conditionalState = ConditionalEvalState.None;
            }
            return iter;
        }

        private void EvaluateAssignmentExpression(CustomLogicClassInstance classInstance, Dictionary<string, object> localVariables,
            CustomLogicAssignmentExpressionAst assignment)
        {
            var op = (CustomLogicSymbol)assignment.Operator.Value;
            var isCompoundAssignment = op is CustomLogicSymbol.PlusEquals or CustomLogicSymbol.MinusEquals
                or CustomLogicSymbol.TimesEquals or CustomLogicSymbol.DivideEquals;

            var value = EvaluateExpression(classInstance, localVariables, assignment.Right);

            if (assignment.Right is not CustomLogicClassInstantiateExpressionAst)
            {
                if (value is CustomLogicClassInstance instance)
                {
                    if (instance.HasVariable(copy))
                        value = EvaluateMethod(instance, copy);
                }
            }

            if (assignment.Left is CustomLogicVariableExpressionAst variableAst)
            {
                var variableName = variableAst.Name;
                var newValue = value;

                if (isCompoundAssignment)
                {
                    var originalValue = localVariables[variableName];
                    newValue = op switch
                    {
                        CustomLogicSymbol.PlusEquals => AddValues(originalValue, value),
                        CustomLogicSymbol.MinusEquals => SubtractValues(originalValue, value),
                        CustomLogicSymbol.TimesEquals => MultiplyValues(originalValue, value),
                        CustomLogicSymbol.DivideEquals => DivideValues(originalValue, value),
                        _ => value,
                    };
                }

                localVariables[variableName] = newValue;
            }
            else if (assignment.Left is CustomLogicFieldExpressionAst fieldAst)
            {
                var fieldName = fieldAst.FieldName;
                var fieldInstance = (CustomLogicClassInstance)EvaluateExpression(classInstance, localVariables, fieldAst.Left);

                var newValue = value;

                if (isCompoundAssignment)
                {
                    var originalValue = fieldInstance.GetVariable(fieldName);
                    if (originalValue is CLPropertyBinding property)
                        originalValue = property.GetValue(fieldInstance);
                    newValue = op switch
                    {
                        CustomLogicSymbol.PlusEquals => AddValues(originalValue, value),
                        CustomLogicSymbol.MinusEquals => SubtractValues(originalValue, value),
                        CustomLogicSymbol.TimesEquals => MultiplyValues(originalValue, value),
                        CustomLogicSymbol.DivideEquals => DivideValues(originalValue, value),
                        _ => value,
                    };
                }

                if (fieldInstance.TryGetVariable(fieldName, out var field))
                {
                    if (field is CLPropertyBinding property)
                    {
                        if (property.IsReadOnly)
                            throw new Exception($"Cannot reassign read-only field '{fieldInstance.ClassName}.{fieldName}'");

                        property.SetValue(fieldInstance, newValue);
                    }
                    else if (field is CLMethodBinding)
                        throw new Exception($"Cannot reassign built-in method '{fieldInstance.ClassName}.{fieldName}'");
                    else
                        fieldInstance.Variables[fieldName] = newValue;
                }
                else
                    fieldInstance.Variables.Add(fieldName, newValue);
            }
        }

        public bool HasMethod(CustomLogicClassInstance classInstance, string methodName)
        {
            return _start.Classes[classInstance.ClassName].Methods.ContainsKey(methodName);
        }

        public object EvaluateMethod(CustomLogicClassInstance classInstance, string methodName, object[] parameterValues = null)
        {
            if (parameterValues == null)
                parameterValues = EmptyArgs;
            try
            {
                if (classInstance.TryGetVariable(methodName, out var variable) && variable is CLMethodBinding method)
                {
                    return method.Call(classInstance, parameterValues);
                }

                if (classInstance is BuiltinClassInstance)
                    throw new Exception($"Method {methodName} not found in class {classInstance.ClassName}");

                CustomLogicMethodDefinitionAst methodAst;
                if (classInstance.Variables.ContainsKey(methodName) &&
                    classInstance.Variables[methodName] is UserMethod userMethod)
                {
                    methodAst = userMethod.Ast;
                    classInstance = userMethod.Owner;
                }
                else if (_start.Classes[classInstance.ClassName].Methods.ContainsKey(methodName))
                    methodAst = _start.Classes[classInstance.ClassName].Methods[methodName];
                else
                    return null;

                if (methodAst.Coroutine)
                {
                    Dictionary<string, object> localVariables = new Dictionary<string, object>();
                    int maxValues = Math.Min(parameterValues.Length, methodAst.ParameterNames.Count);
                    for (int i = 0; i < maxValues; i++)
                        localVariables.Add(methodAst.ParameterNames[i], parameterValues[i]);
                    return CustomLogicManager._instance.StartCoroutine(EvaluateBlockCoroutine(classInstance,
                        localVariables, methodAst.Statements));
                }
                else
                {
                    Dictionary<string, object> localVariables = UnityEngine.Pool.DictionaryPool<string, object>.Get();
                    int maxValues = Math.Min(parameterValues.Length, methodAst.ParameterNames.Count);
                    for (int i = 0; i < maxValues; i++)
                        localVariables.Add(methodAst.ParameterNames[i], parameterValues[i]);
                    EvaluateBlock(classInstance, localVariables, methodAst.Statements, out object result);
                    UnityEngine.Pool.DictionaryPool<string, object>.Release(localVariables);
                    return result;
                }
            }
            catch (TargetInvocationException e)
            {
                LogCustomLogicError("Custom logic runtime error at method " + methodName + " in class " + classInstance.ClassName + ": " + e.InnerException?.Message, true);
                return null;
            }
            catch (Exception e)
            {
                LogCustomLogicError("Custom logic runtime error at method " + methodName + " in class " + classInstance.ClassName + ": " + e.Message, true);
                return null;
            }
        }

        public object EvaluateMethod(UserMethod userMethod, object[] parameterValues = null)
        {
            var ast = userMethod.Ast;
            var methodName = userMethod.Ast.Name;
            var classInstance = userMethod.Owner;

            if (parameterValues == null)
                parameterValues = EmptyArgs;

            try
            {
                if (ast.Coroutine)
                {
                    Dictionary<string, object> localVariables = new Dictionary<string, object>();
                    int maxValues = Math.Min(parameterValues.Length, ast.ParameterNames.Count);
                    for (int i = 0; i < maxValues; i++)
                        localVariables.Add(ast.ParameterNames[i], parameterValues[i]);

                    return CustomLogicManager._instance.StartCoroutine(EvaluateBlockCoroutine(classInstance,
                        localVariables, ast.Statements));
                }
                else
                {
                    Dictionary<string, object> localVariables = UnityEngine.Pool.DictionaryPool<string, object>.Get();
                    int maxValues = Math.Min(parameterValues.Length, ast.ParameterNames.Count);
                    for (int i = 0; i < maxValues; i++)
                        localVariables.Add(ast.ParameterNames[i], parameterValues[i]);
                    EvaluateBlock(classInstance, localVariables, ast.Statements, out object result);
                    UnityEngine.Pool.DictionaryPool<string, object>.Release(localVariables);
                    return result;
                }
            }
            catch (TargetInvocationException e)
            {
                LogCustomLogicError("Custom logic runtime error at method " + methodName + " in class " + classInstance.ClassName + ": " + e.InnerException?.Message, true);
                return null;
            }
            catch (Exception e)
            {
                LogCustomLogicError("Custom logic runtime error at line " + GetLineNumberString(userMethod.Ast.Line) + " at method " + methodName + " in class " + classInstance.ClassName + ": " + e.Message, true);
                return null;
            }
        }

        private object EvaluateExpression(CustomLogicClassInstance classInstance, Dictionary<string, object> localVariables, CustomLogicBaseExpressionAst expression)
        {
            try
            {
                if (expression.Type == CustomLogicAstType.PrimitiveExpression)
                {
                    return ((CustomLogicPrimitiveExpressionAst)expression).Value;
                }
                else if (expression.Type == CustomLogicAstType.VariableExpression)
                {
                    string name = ((CustomLogicVariableExpressionAst)expression).Name;
                    if (name == "self")
                        return classInstance;
                    else if (_staticClasses.ContainsKey(name))
                        return _staticClasses[name];
                    else
                    {
                        object value = localVariables[name];
                        return value;
                    }
                }
                else if (expression.Type == CustomLogicAstType.ClassInstantiateExpression)
                {
                    CustomLogicClassInstantiateExpressionAst instantiate = (CustomLogicClassInstantiateExpressionAst)expression;
                    var parameters = ArrayPool<object>.New(instantiate.Parameters.Count);
                    for (int i = 0; i < instantiate.Parameters.Count; i++)
                    {
                        CustomLogicBaseAst ast = instantiate.Parameters[i];
                        parameters[i] = EvaluateExpression(classInstance, localVariables, (CustomLogicBaseExpressionAst)ast);
                    }

                    if (CustomLogicBuiltinTypes.IsBuiltinType(instantiate.Name) || _start.Classes.ContainsKey(instantiate.Name))
                    {
                        var result = CreateClassInstance(instantiate.Name, parameters, true);
                        ArrayPool<object>.Free(parameters);
                        return result;
                    }

                    // If no class was found with that name, interpret the expression as local method call
                    if (localVariables.ContainsKey(instantiate.Name) && localVariables[instantiate.Name] is CLMethodBinding method)
                    {
                        var result = method.Call(classInstance, parameters);
                        ArrayPool<object>.Free(parameters);
                        return result;
                    }

                    var userMethod = (UserMethod)localVariables[instantiate.Name];
                    var evalResult = EvaluateMethod(userMethod, parameters);
                    ArrayPool<object>.Free(parameters);
                    return evalResult;
                }
                else if (expression.Type == CustomLogicAstType.FieldExpression)
                {
                    CustomLogicFieldExpressionAst fieldExpression = (CustomLogicFieldExpressionAst)expression;
                    CustomLogicClassInstance fieldInstance = (CustomLogicClassInstance)EvaluateExpression(classInstance, localVariables, fieldExpression.Left);
                    object value = fieldInstance.GetVariable(fieldExpression.FieldName);
                    if (value is CLPropertyBinding builtinField)
                        return builtinField.GetValue(fieldInstance);
                    return value;
                }
                else if (expression.Type == CustomLogicAstType.NotExpression)
                {
                    return !(bool)EvaluateExpression(classInstance, localVariables, ((CustomLogicNotExpressionAst)expression).Next);
                }
                else if (expression.Type == CustomLogicAstType.UnaryExpression)
                {
                    CustomLogicUnaryExpressionAst unaryExpression = (CustomLogicUnaryExpressionAst)expression;
                    CustomLogicSymbol symbol = (CustomLogicSymbol)unaryExpression.Token.Value;
                    object next = EvaluateExpression(classInstance, localVariables, ((CustomLogicUnaryExpressionAst)expression).Next);
                    return EvaluateUnaryExpression(symbol, next);
                }
                else if (expression.Type == CustomLogicAstType.MethodCallExpression)
                {
                    CustomLogicMethodCallExpressionAst methodCallExpression = (CustomLogicMethodCallExpressionAst)expression;
                    CustomLogicClassInstance methodCallInstance = (CustomLogicClassInstance)EvaluateExpression(classInstance, localVariables, methodCallExpression.Left);
                    var parameters = ArrayPool<object>.New(methodCallExpression.Parameters.Count); // new object[methodCallExpression.Parameters.Count];
                    for (int i = 0; i < methodCallExpression.Parameters.Count; i++)
                    {
                        CustomLogicBaseExpressionAst parameterExpression = (CustomLogicBaseExpressionAst)methodCallExpression.Parameters[i];
                        parameters[i] = EvaluateExpression(classInstance, localVariables, parameterExpression);
                    }
                    var result = EvaluateMethod(methodCallInstance, methodCallExpression.Name, parameters);
                    ArrayPool<object>.Free(parameters);
                    return result;
                }
                else if (expression.Type == CustomLogicAstType.BinopExpression)
                {
                    CustomLogicBinopExpressionAst binopExpression = (CustomLogicBinopExpressionAst)expression;
                    CustomLogicSymbol symbol = (CustomLogicSymbol)binopExpression.Token.Value;
                    if (symbol == CustomLogicSymbol.Or)
                    {
                        object left = EvaluateExpression(classInstance, localVariables, binopExpression.Left);
                        if ((bool)left)
                            return true;
                        return (bool)EvaluateExpression(classInstance, localVariables, binopExpression.Right);
                    }
                    else if (symbol == CustomLogicSymbol.And)
                    {
                        object left = EvaluateExpression(classInstance, localVariables, binopExpression.Left);
                        if (!(bool)left)
                            return false;
                        return (bool)EvaluateExpression(classInstance, localVariables, binopExpression.Right);
                    }
                    else
                    {
                        object left = EvaluateExpression(classInstance, localVariables, binopExpression.Left);
                        object right = EvaluateExpression(classInstance, localVariables, binopExpression.Right);
                        return EvaluateBinopExpression(symbol, left, right);
                    }
                }
            }
            catch (Exception e)
            {
                LogCustomLogicError("Custom logic runtime error at line " + GetLineNumberString(expression.Line) + ": " + e.Message, true);
            }
            return null;
        }

        private object EvaluateUnaryExpression(CustomLogicSymbol symbol, object next)
        {
            if (symbol == CustomLogicSymbol.Plus) return next;
            else if (symbol == CustomLogicSymbol.Minus)
            {
                if (next is int i) return -i;
                if (next is float f) return -f;
                if (next is double d) return -d;
            }
            return null;
        }

        private object EvaluateBinopExpression(CustomLogicSymbol symbol, object left, object right)
        {
            if (symbol == CustomLogicSymbol.Plus) return AddValues(left, right);
            else if (symbol == CustomLogicSymbol.Minus) return SubtractValues(left, right);
            else if (symbol == CustomLogicSymbol.Times) return MultiplyValues(left, right);
            else if (symbol == CustomLogicSymbol.Divide) return DivideValues(left, right);
            else if (symbol == CustomLogicSymbol.Modulo) return ModuloValues(left, right);
            else if (symbol == CustomLogicSymbol.Equals)
                return CheckEquals(left, right);
            else if (symbol == CustomLogicSymbol.NotEquals)
                return !CheckEquals(left, right);
            else if (symbol == CustomLogicSymbol.LessThan)
                return left.UnboxToFloat() < right.UnboxToFloat();
            else if (symbol == CustomLogicSymbol.GreaterThan)
                return left.UnboxToFloat() > right.UnboxToFloat();
            else if (symbol == CustomLogicSymbol.LessThanOrEquals)
                return left.UnboxToFloat() <= right.UnboxToFloat();
            else if (symbol == CustomLogicSymbol.GreaterThanOrEquals)
                return left.UnboxToFloat() >= right.UnboxToFloat();
            return null;
        }

        public object[] Parameters3 = new object[2];

        private object ClassMathOperation(object left, object right, string method)
        {
            CustomLogicClassInstance instance = left is CustomLogicClassInstance ? (CustomLogicClassInstance)left : (CustomLogicClassInstance)right;
            if (instance.HasVariable(method))
            {
                object[] parameters = ArrayPool<object>.New(2);
                parameters[0] = left;
                parameters[1] = right;
                var result = EvaluateMethod(instance, method, parameters);
                ArrayPool<object>.Free(parameters);
                return result;
            }
            else
                throw new Exception($"No {method} method found in class " + instance.ClassName);
        }

        string add = nameof(ICustomLogicMathOperators.__Add__);
        string sub = nameof(ICustomLogicMathOperators.__Sub__);
        string mul = nameof(ICustomLogicMathOperators.__Mul__);
        string div = nameof(ICustomLogicMathOperators.__Div__);
        string mod = nameof(ICustomLogicMathOperators.__Mod__);

        string eq = nameof(ICustomLogicEquals.__Eq__);
        string copy = nameof(ICustomLogicCopyable.__Copy__);


        private object AddValues(object left, object right)
        {
            if (left is int && right is int)
                return (int)left + (int)right;

            var leftStr = left is string;
            var rightStr = right is string;
            if (leftStr || rightStr)
            {
                if (leftStr)
                    return (string)left + right;

                return left + (string)right;
            }
            else if (left is CustomLogicClassInstance || right is CustomLogicClassInstance)
                return ClassMathOperation(left, right, add);
            else
                return left.UnboxToFloat() + right.UnboxToFloat();
        }

        private object SubtractValues(object left, object right)
        {
            if (left is int && right is int)
                return (int)left - (int)right;
            else if (left is CustomLogicClassInstance || right is CustomLogicClassInstance)
                return ClassMathOperation(left, right, sub);
            else
                return left.UnboxToFloat() - right.UnboxToFloat();
        }

        private object MultiplyValues(object left, object right)
        {
            if (left is int && right is int)
                return (int)((int)left * (int)right);
            else if (left is CustomLogicClassInstance || right is CustomLogicClassInstance)
                return ClassMathOperation(left, right, mul);
            else
                return left.UnboxToFloat() * right.UnboxToFloat();
        }

        private object DivideValues(object left, object right)
        {
            if (left is int && right is int)
                return (int)left / (int)right;
            else if (left is CustomLogicClassInstance || right is CustomLogicClassInstance)
                return ClassMathOperation(left, right, div);
            else
                return left.UnboxToFloat() / right.UnboxToFloat();
        }

        private object ModuloValues(object left, object right)
        {
            if (left is int && right is int)
                return (int)left % (int)right;
            else if (left is CustomLogicClassInstance || right is CustomLogicClassInstance)
                return ClassMathOperation(left, right, mod);
            else
                return left.UnboxToFloat() % right.UnboxToFloat();
        }

        public bool CheckEquals(object left, object right)
        {
            if (left == null && right == null)
                return true;
            if (left is CustomLogicClassInstance)
            {
                CustomLogicClassInstance leftInstance = (CustomLogicClassInstance)left;
                if (leftInstance.HasVariable(eq))
                {
                    object[] parameters = ArrayPool<object>.New(2);
                    parameters[0] = left;
                    parameters[1] = right;
                    var result = EvaluateMethod(leftInstance, eq, parameters);
                    ArrayPool<object>.Free(parameters);
                    return (bool)result;
                }
            }
            else if (right is CustomLogicClassInstance)
            {
                CustomLogicClassInstance rightInstance = (CustomLogicClassInstance)right;
                if (rightInstance.HasVariable(eq))
                {
                    object[] parameters = ArrayPool<object>.New(2);
                    parameters[0] = left;
                    parameters[1] = right;
                    var result = EvaluateMethod(rightInstance, eq, parameters);
                    ArrayPool<object>.Free(parameters);
                    return (bool)result;
                }
            }
            if (left != null)
                return left.Equals(right);
            return right.Equals(left);
        }

        public static T ConvertTo<T>(object obj)
        {
            var res = obj;
            if (typeof(T) == ReflectionExtensions.IntType)
                res = obj.UnboxToInt();
            else if (typeof(T) == ReflectionExtensions.FloatType)
                res = obj.UnboxToFloat();

            return (T)res;
        }
    }

    public enum ConditionalEvalState
    {
        None,
        PassedIf,
        FailedIf,
        PassedElseIf,
        FailedElseIf
    }

    #endregion
}