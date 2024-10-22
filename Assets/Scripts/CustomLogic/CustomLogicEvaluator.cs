using ApplicationManagers;
using GameManagers;
using Map;
using Photon.Pun;
using Settings;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace CustomLogic
{
    partial class CustomLogicEvaluator
    {
        public float CurrentTime;
        public bool HasSetMusic;
        public readonly Dictionary<int, CustomLogicNetworkViewBuiltin> IdToNetworkView = new();
        private readonly CustomLogicStartAst _start;
        private readonly Dictionary<string, CustomLogicClassInstance> _staticClasses = new();
        private readonly Dictionary<string, List<CustomLogicClassInstance>> _callbacks = new();
        public readonly Dictionary<int, Dictionary<string, float>> PlayerIdToLastPropertyChanges = new();
        public string ScoreboardHeader = "Kills / Deaths / Max / Total";
        public string ScoreboardProperty = "";
        
        private static readonly List<object> EmptyParameters = new();
        
        private static readonly Dictionary<string, CustomLogicBaseBuiltin> BuiltinStaticClasses = new()
        {
            { "Game", new CustomLogicGameBuiltin() },
            { "Convert", new CustomLogicConvertBuiltin() },
            { "Cutscene", new CustomLogicCutsceneBuiltin() },
            { "Time", new CustomLogicTimeBuiltin() },
            { "Network", new CustomLogicNetworkBuiltin() },
            { "UI", new CustomLogicUIBuiltin() },
            { "Input", new CustomLogicInputBuiltin() },
            { "Math", new CustomLogicMathBuiltin() },
            { "Vector3", new CustomLogicVector3Builtin(EmptyParameters) },
            { "Quaternion", new CustomLogicQuaternionBuiltin(EmptyParameters) },
            { "Color", new CustomLogicColorBuiltin(EmptyParameters) },
            { "Map", new CustomLogicMapBuiltin() },
            { "String", new CustomLogicStringBuiltin() },
            { "Random", new CustomLogicRandomBuiltin(EmptyParameters) },
            { "Camera", new CustomLogicCameraBuiltin() },
            { "RoomData", new CustomLogicRoomDataBuiltin() },
            { "PersistentData", new CustomLogicPersistentDataBuiltin() },
            { "Json", new CustomLogicJsonBuiltin() },
            { "Physics", new CustomLogicPhysicsBuiltin() },
            { "LineRenderer", new CustomLogicLineRendererBuiltin(null) },
        };

        public CustomLogicEvaluator(CustomLogicStartAst start)
        {
            _start = start;
        }

        public async UniTaskVoid Start(Dictionary<string, BaseSetting> modeSettings)
        {
            try
            {
                Init();
                CurrentTime = 0f;

                var main = _staticClasses["Main"];
                foreach (var variableName in modeSettings.Keys)
                {
                    var setting = modeSettings[variableName];
                    main.Variables[variableName] = setting switch
                    {
                        FloatSetting f => f.Value,
                        StringSetting s => s.Value,
                        IntSetting i => i.Value,
                        BoolSetting b => b.Value,
                        _ => throw new Exception("Unknown setting type")
                    };
                }

                foreach (var instance in _staticClasses.Values)
                    await EvaluateMethod(instance, "Init");

                EvaluateMethodForCallbacks("Init").Forget();
                AddCallbacks(_staticClasses["Main"]);
                EvaluateMethodForCallbacks("OnGameStart").Forget();
                OnSecondAsync(CustomLogicManager._instance.destroyCancellationToken).Forget();
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                DebugConsole.Log("Custom logic runtime error: " + e.Message, true);
            }
        }

        private void AddCallbacks(CustomLogicClassInstance instance)
        {
            foreach (var method in _start.Classes[instance.ClassName].Methods.Keys)
            {
                if (!_callbacks.ContainsKey(method))
                    _callbacks.Add(method, new List<CustomLogicClassInstance>());
                _callbacks[method].Add(instance);
            }
        }

        private void RemoveCallbacks(CustomLogicClassInstance instance)
        {
            foreach (var method in _start.Classes[instance.ClassName].Methods.Keys)
            {
                if (_callbacks.TryGetValue(method, out var instances))
                {
                    if (instances.Contains(instance))
                        instances.Remove(instance);
                }
            }
        }
        
        private async UniTaskVoid EvaluateMethodForCallbacks(string methodName, List<object> parameters = null)
        {
            if (_callbacks.TryGetValue(methodName, out var instance))
            {
                // for loop instead of foreach because the list might be modified during the loop
                for (var i = 0; i < instance.Count; i++)
                {
                    // Init function should run even if the class (component) is disabled
                    if (!instance[i].Enabled && methodName != "Init")
                        continue;

                    await EvaluateMethod(instance[i], methodName, parameters);
                }
            }
        }

        private async UniTaskVoid OnSecondAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                await UniTask.Delay(1000, cancellationToken: cancellationToken);
                EvaluateMethodForCallbacks("OnSecond").Forget();
                if (PhotonNetwork.IsMasterClient)
                {
                    RPCManager.PhotonView.RPC("SyncCurrentTimeRPC", RpcTarget.Others, CurrentTime);
                    foreach (var key in IdToNetworkView.Keys)
                        IdToNetworkView[key].OnSecond();
                }
            }
        }

        private void Init()
        {
            foreach (var name in BuiltinStaticClasses.Keys)
                CreateStaticClass(name);

            foreach (var className in new List<string>(_start.Classes.Keys))
            {
                if (className == "Main" || _start.Classes[className].IsExtension())
                    CreateStaticClass(className);
            }

            foreach (var instance in _staticClasses.Values)
            {
                if (instance is not CustomLogicBaseBuiltin)
                    RunAssignmentsClassInstance(instance);
            }

            foreach (var id in MapLoader.IdToMapObject.Keys)
            {
                var obj = MapLoader.IdToMapObject[id];
                LoadMapObjectComponents(obj).Forget();
            }
        }

        private void CreateStaticClass(string className)
        {
            if (!_staticClasses.ContainsKey(className))
            {
                var instance = BuiltinStaticClasses.TryGetValue(className, out var baseBuiltin)
                    ? baseBuiltin
                    : CreateClassInstance(className, new List<object>(), false);
                
                _staticClasses.Add(className, instance);
            }
        }

        public CustomLogicClassInstance CreateClassInstance(string className, List<object> parameterValues,
            bool init = true)
        {
            var classInstance = className switch
            {
                "Dict" => new CustomLogicDictBuiltin(),
                "List" => new CustomLogicListBuiltin(),
                "Vector3" => new CustomLogicVector3Builtin(parameterValues),
                "Color" => new CustomLogicColorBuiltin(parameterValues),
                "Quaternion" => new CustomLogicQuaternionBuiltin(parameterValues),
                "Range" => new CustomLogicRangeBuiltin(parameterValues),
                "Random" => new CustomLogicRandomBuiltin(parameterValues),
                _ => CreateCustomClassInstance()
            };

            return classInstance;

            CustomLogicClassInstance CreateCustomClassInstance()
            {
                var instance = new CustomLogicClassInstance(className);
                if (init)
                {
                    RunAssignmentsClassInstance(instance);
                    EvaluateMethod(instance, "Init", parameterValues).Forget();
                }

                return instance;
            }
        }

        private void RunAssignmentsClassInstance(CustomLogicClassInstance classInstance)
        {
            var classAst = _start.Classes[classInstance.ClassName];
            foreach (var assignment in classAst.Assignments)
            {
                var variableName = ((CustomLogicVariableExpressionAst)assignment.Left).Name;
                var value = EvaluateExpression(classInstance, new Dictionary<string, object>(), assignment.Right);
                classInstance.Variables[variableName] = value;
            }
        }
        
        private async UniTask<object[]> EvaluateBlock(CustomLogicClassInstance classInstance, bool isCoroutine,
            Dictionary<string, object> localVariables, List<CustomLogicBaseAst> statements, CancellationToken cancellationToken)
        {
            var conditionalState = ConditionalEvalState.None;
            var result = new object[] { false, null };
            foreach (var statement in statements)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                
                if (statement is CustomLogicAssignmentExpressionAst assignmentExpressionAst)
                {
                    EvaluateAssignmentExpression(classInstance, localVariables, assignmentExpressionAst);
                }
                else if (statement is CustomLogicCompoundAssignmentExpressionAst compoundAssignmentExpressionAst)
                {
                    EvaluateCompoundAssignmentExpression(classInstance, localVariables,
                        compoundAssignmentExpressionAst);
                }
                else if (statement is CustomLogicReturnExpressionAst returnAst)
                {
                    result[0] = true;
                    result[1] = isCoroutine ? null : EvaluateExpression(classInstance, localVariables, returnAst.ReturnValue);
                    return result;
                }
                else if (statement is CustomLogicBreakExpressionAst breakAst)
                {
                    result[0] = true;
                    result[1] = breakAst;
                    return result;
                }
                else if (statement is CustomLogicContinueExpressionAst continueAst)
                {
                    result[0] = true;
                    result[1] = continueAst;
                    return result;
                }
                else if (statement is CustomLogicWaitExpressionAst waitAst)
                {
                    if (isCoroutine)
                    {
                        var value = EvaluateExpression(classInstance, localVariables, waitAst.WaitTime);
                        var className = classInstance.ClassName;
                        if (_start.Classes[className].IsCutscene())
                        {
                            var time = value.UnboxToFloat();
                            while (time > 0f && !CustomLogicManager.SkipCutscene)
                            {
                                await Task.Delay(100, cancellationToken);
                                time -= 0.1f;
                            }
                        }
                        else
                            await Task.Delay((int)value.UnboxToFloat() * 1000, cancellationToken);
                    }
                    else
                    {
                        throw new Exception("Wait expression can only be used in a coroutine");
                    }
                }
                else if (statement is CustomLogicConditionalBlockAst conditionalAst)
                {
                    if ((int)conditionalAst.Token.Value == (int)CustomLogicSymbol.If)
                    {
                        if ((bool)EvaluateExpression(classInstance, localVariables, conditionalAst.Condition))
                        {
                            var nextResult = await EvaluateBlock(classInstance, isCoroutine, localVariables, conditionalAst.Statements, cancellationToken);
                            if ((bool)nextResult[0])
                                return nextResult;
                            conditionalState = ConditionalEvalState.PassedIf;
                        }
                        else
                            conditionalState = ConditionalEvalState.FailedIf;
                    }
                    else if ((int)conditionalAst.Token.Value == (int)CustomLogicSymbol.While)
                    {
                        while ((bool)EvaluateExpression(classInstance, localVariables, conditionalAst.Condition))
                        {
                            if (cancellationToken.IsCancellationRequested)
                                break;

                            var nextResult = await EvaluateBlock(classInstance, isCoroutine, localVariables, conditionalAst.Statements, cancellationToken);
                            var shouldBreak = (bool)nextResult[0];

                            if (shouldBreak && nextResult[1] is CustomLogicContinueExpressionAst)
                                continue;

                            if (shouldBreak && nextResult[1] is CustomLogicBreakExpressionAst)
                                break;

                            if (shouldBreak)
                                return nextResult;
                        }

                        conditionalState = ConditionalEvalState.None;
                    }
                    else if ((int)conditionalAst.Token.Value == (int)CustomLogicSymbol.Else)
                    {
                        if (conditionalState is ConditionalEvalState.FailedIf or ConditionalEvalState.FailedElseIf)
                        {
                            var nextResult = await EvaluateBlock(classInstance, isCoroutine, localVariables, conditionalAst.Statements, cancellationToken);
                            if ((bool)nextResult[0])
                                return nextResult;
                        }

                        conditionalState = ConditionalEvalState.None;
                    }
                    else if ((int)conditionalAst.Token.Value == (int)CustomLogicSymbol.ElseIf)
                    {
                        if (conditionalState is ConditionalEvalState.PassedIf or ConditionalEvalState.PassedElseIf)
                        {
                        }
                        else if (conditionalState is ConditionalEvalState.FailedIf or ConditionalEvalState.FailedElseIf &&
                                 (bool)EvaluateExpression(classInstance, localVariables, conditionalAst.Condition))
                        {
                            var nextResult =
                                await EvaluateBlock(classInstance, isCoroutine, localVariables, conditionalAst.Statements, cancellationToken);
                            if ((bool)nextResult[0])
                                return nextResult;
                            conditionalState = ConditionalEvalState.PassedElseIf;
                        }
                        else
                            conditionalState = ConditionalEvalState.FailedElseIf;
                    }
                }
                else if (statement is CustomLogicForBlockAst forBlock)
                {
                    var expression = EvaluateExpression(classInstance, localVariables, forBlock.Iterable);
                    var list = (CustomLogicListBuiltin)expression;
                    foreach (var variable in list.List)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;
                        
                        var variableName = forBlock.Variable.Name;
                        localVariables[variableName] = variable;
                        var nextResult = await EvaluateBlock(classInstance, isCoroutine, localVariables, forBlock.Statements, cancellationToken);
                        var shouldBreak = (bool)nextResult[0];

                        if (shouldBreak && nextResult[1] is CustomLogicContinueExpressionAst)
                            continue;

                        if (shouldBreak && nextResult[1] is CustomLogicBreakExpressionAst)
                            break;

                        if (shouldBreak)
                            return nextResult;
                    }
                }
                else if (statement is CustomLogicBaseExpressionAst baseExpAst)
                    EvaluateExpression(classInstance, localVariables, baseExpAst);

                if (statement is not CustomLogicConditionalBlockAst)
                    conditionalState = ConditionalEvalState.None;
            }

            return result;
        }

        private void EvaluateAssignmentExpression(CustomLogicClassInstance classInstance,
            Dictionary<string, object> localVariables, CustomLogicAssignmentExpressionAst assignment)
        {
            var value = EvaluateExpression(classInstance, localVariables, assignment.Right);
            if (value is CustomLogicStructBuiltin structBuiltin)
                value = structBuiltin.Copy();
            if (assignment.Left is CustomLogicVariableExpressionAst variableAst)
            {
                var variableName = variableAst.Name;
                localVariables[variableName] = value;
            }
            else if (assignment.Left is CustomLogicFieldExpressionAst fieldExpression)
            {
                var fieldInstance = (CustomLogicClassInstance)EvaluateExpression(classInstance, localVariables,
                        fieldExpression.Left);
                if (fieldInstance is CustomLogicBaseBuiltin baseBuiltin)
                    baseBuiltin.SetField(fieldExpression.FieldName, value);
                else
                {
                    fieldInstance.Variables[fieldExpression.FieldName] = value;
                }
            }
        }

        private void EvaluateCompoundAssignmentExpression(CustomLogicClassInstance classInstance,
            Dictionary<string, object> localVariables, CustomLogicCompoundAssignmentExpressionAst assignment)
        {
            var op = (CustomLogicSymbol)assignment.Operator.Value;
            var value = EvaluateExpression(classInstance, localVariables, assignment.Right);
            if (value is CustomLogicStructBuiltin structBuiltin)
                value = structBuiltin.Copy();

            if (assignment.Left is CustomLogicVariableExpressionAst variableAst)
            {
                var variableName = variableAst.Name;
                var originalValue = localVariables[variableName];
                var newValue = op switch
                {
                    CustomLogicSymbol.PlusEquals => AddValues(originalValue, value),
                    CustomLogicSymbol.MinusEquals => SubtractValues(originalValue, value),
                    CustomLogicSymbol.TimesEquals => MultiplyValues(originalValue, value),
                    CustomLogicSymbol.DivideEquals => DivideValues(originalValue, value),
                    _ => value,
                };
                localVariables[variableName] = newValue;
            }
            else if (assignment.Left is CustomLogicFieldExpressionAst fieldExpression)
            {
                var fieldInstance =
                    (CustomLogicClassInstance)EvaluateExpression(classInstance, localVariables,
                        fieldExpression.Left);
                var originalValue = fieldInstance.Variables[fieldExpression.FieldName];
                var newValue = op switch
                {
                    CustomLogicSymbol.PlusEquals => AddValues(originalValue, value),
                    CustomLogicSymbol.MinusEquals => SubtractValues(originalValue, value),
                    CustomLogicSymbol.TimesEquals => MultiplyValues(originalValue, value),
                    CustomLogicSymbol.DivideEquals => DivideValues(originalValue, value),
                    _ => value,
                };
                fieldInstance.Variables[fieldExpression.FieldName] = newValue;
            }
        }

        public bool HasMethod(CustomLogicClassInstance classInstance, string methodName)
        {
            return _start.Classes[classInstance.ClassName].Methods.ContainsKey(methodName);
        }

        private bool IsMethodCoroutine(CustomLogicClassInstance classInstance, string methodName)
        {
            if (classInstance is CustomLogicBaseBuiltin)
                return false;
            
            if (_start.Classes[classInstance.ClassName].Methods.TryGetValue(methodName, out var methodAst))
                return methodAst.Coroutine;
            
            return false;
        }

        public async UniTask<object> EvaluateMethod(CustomLogicClassInstance classInstance, string methodName,
            List<object> parameterValues = null, CancellationToken cancellationToken = default)
        {
            parameterValues ??= EmptyParameters;
            if (cancellationToken == default)
                cancellationToken = CustomLogicManager._instance.destroyCancellationToken;
            
            try
            {
                if (classInstance is CustomLogicBaseBuiltin builtin)
                {
                    return builtin.CallMethod(methodName, parameterValues);
                }

                if (!HasMethod(classInstance, methodName))
                    return null;
                
                var localVariables = new Dictionary<string, object>();
                var methodAst = _start.Classes[classInstance.ClassName].Methods[methodName];
                var maxValues = Math.Min(parameterValues.Count, methodAst.ParameterNames.Count);
                for (var i = 0; i < maxValues; i++)
                    localVariables.Add(methodAst.ParameterNames[i], parameterValues[i]);
                
                var result = await EvaluateBlock(classInstance, methodAst.Coroutine, localVariables, methodAst.Statements, cancellationToken);
                return result[1];
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                DebugConsole.Log($"Custom logic runtime error at method {methodName} in class {classInstance.ClassName}: {e.Message}", true);
                return null;
            }
        }

        private object EvaluateExpression(CustomLogicClassInstance classInstance,
            Dictionary<string, object> localVariables, CustomLogicBaseExpressionAst expression)
        {
            try
            {
                if (expression.Type == CustomLogicAstType.PrimitiveExpression)
                {
                    return ((CustomLogicPrimitiveExpressionAst)expression).Value;
                }

                if (expression.Type == CustomLogicAstType.VariableExpression)
                {
                    var name = ((CustomLogicVariableExpressionAst)expression).Name;
                    if (name == "self")
                        return classInstance;
                    
                    if (_staticClasses.TryGetValue(name, out var staticClassInstance))
                        return staticClassInstance;
                    
                    var value = localVariables[name];
                    return value;
                }
                if (expression.Type == CustomLogicAstType.ClassInstantiateExpression)
                {
                    var instantiate = (CustomLogicClassInstantiateExpressionAst)expression;
                    var parameters = new List<object>();
                    foreach (var ast in instantiate.Parameters)
                    {
                        var exp = (CustomLogicBaseExpressionAst)ast;
                        var value = EvaluateExpression(classInstance, localVariables, exp);
                        parameters.Add(value);
                    }

                    return CreateClassInstance(instantiate.Name, parameters);
                }
                if (expression.Type == CustomLogicAstType.FieldExpression)
                {
                    var fieldExpression = (CustomLogicFieldExpressionAst)expression;
                    var fieldInstance = (CustomLogicClassInstance)EvaluateExpression(classInstance, localVariables, fieldExpression.Left);
                    if (fieldInstance is CustomLogicBaseBuiltin builtin)
                        return builtin.GetField(fieldExpression.FieldName);
                    
                    var value = fieldInstance.Variables[fieldExpression.FieldName];
                    if (value is CustomLogicStructBuiltin structBuiltin)
                        value = structBuiltin.Copy();
                    
                    return value;
                }
                if (expression.Type == CustomLogicAstType.NotExpression)
                {
                    var exp = (CustomLogicNotExpressionAst)expression;
                    var value = (bool)EvaluateExpression(classInstance, localVariables, exp.Next);
                    return !value;
                }
                if (expression.Type == CustomLogicAstType.MethodCallExpression)
                {
                    var methodCallExpression = (CustomLogicMethodCallExpressionAst)expression;
                    var methodCallInstance = (CustomLogicClassInstance)EvaluateExpression(classInstance, localVariables, methodCallExpression.Left);
                    var parameters = new List<object>();
                    foreach (var customLogicBaseAst in methodCallExpression.Parameters)
                    {
                        var exp = (CustomLogicBaseExpressionAst)customLogicBaseAst;
                        var value = EvaluateExpression(classInstance, localVariables, exp);
                        parameters.Add(value);
                    }

                    var isCoroutine = IsMethodCoroutine(methodCallInstance, methodCallExpression.Name);
                    var result = EvaluateMethod(methodCallInstance, methodCallExpression.Name, parameters);

                    if (isCoroutine)
                    {
                        return result;
                    }
                    
                    var awaiter = result.GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        return awaiter.GetResult();
                    }

                    throw new Exception("EvaluateMethod is still running. This should not happen.");
                }
                if (expression.Type == CustomLogicAstType.BinopExpression)
                {
                    var exp = (CustomLogicBinopExpressionAst)expression;
                    var symbol = (CustomLogicSymbol)exp.Token.Value;
                    if (symbol == CustomLogicSymbol.Or)
                    {
                        var left = (bool)EvaluateExpression(classInstance, localVariables, exp.Left);
                        if (left) return true;
                        return (bool)EvaluateExpression(classInstance, localVariables, exp.Right);
                    }

                    if (symbol == CustomLogicSymbol.And)
                    {
                        var left = (bool)EvaluateExpression(classInstance, localVariables, exp.Left);
                        if (!left) return false;
                        return (bool)EvaluateExpression(classInstance, localVariables, exp.Right);
                    }
                    else
                    {
                        var left = EvaluateExpression(classInstance, localVariables, exp.Left);
                        var right = EvaluateExpression(classInstance, localVariables, exp.Right);
                        return EvaluateBinopExpression(symbol, left, right);
                    }
                }
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                DebugConsole.Log("Custom logic runtime error at line " + expression.Line + ": " + e.Message, true);
            }

            return null;
        }

        private static object EvaluateBinopExpression(CustomLogicSymbol symbol, object left, object right)
        {
            return symbol switch
            {
                CustomLogicSymbol.Plus => AddValues(left, right),
                CustomLogicSymbol.Minus => SubtractValues(left, right),
                CustomLogicSymbol.Times => MultiplyValues(left, right),
                CustomLogicSymbol.Divide => DivideValues(left, right),
                CustomLogicSymbol.Equals => CheckEquals(left, right),
                CustomLogicSymbol.NotEquals => !CheckEquals(left, right),
                CustomLogicSymbol.LessThan => left.UnboxToFloat() < right.UnboxToFloat(),
                CustomLogicSymbol.GreaterThan => left.UnboxToFloat() > right.UnboxToFloat(),
                CustomLogicSymbol.LessThanOrEquals => left.UnboxToFloat() <= right.UnboxToFloat(),
                CustomLogicSymbol.GreaterThanOrEquals => left.UnboxToFloat() >= right.UnboxToFloat(),
                _ => null
            };
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
}