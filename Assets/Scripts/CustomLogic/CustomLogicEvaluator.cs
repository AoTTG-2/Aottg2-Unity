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
    class CustomLogicEvaluator
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
        //public List<string> AllowedSpecials = new List<string>();
        //public List<string> DisallowedSpecials = new List<string>();
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

        /// <summary>
        /// More relevant line number for when using MapLogic -> need to expand to handle builtin errors as well since its so annoying.
        /// </summary>
        public string GetLineNumberString(int lineNumber)
        {
            return CustomLogicManager.GetLineNumberString(lineNumber, _baseLogicOffset);
        }

        public Dictionary<string, BaseSetting> GetModeSettings()
        {
            var instance = CreateClassInstance("Main", EmptyArgs, false);
            try
            {
                RunAssignmentsClassInstance(instance);
                Dictionary<string, BaseSetting> settings = new Dictionary<string, BaseSetting>();
                foreach (string variableName in instance.Variables.Keys)
                {
                    if (!variableName.StartsWith("_") && variableName != "Type")
                    {
                        object value = instance.Variables[variableName];
                        if (value is float)
                            settings.Add(variableName, new FloatSetting((float)value));
                        else if (value is string)
                            settings.Add(variableName, new StringSetting((string)value));
                        else if (value is int)
                            settings.Add(variableName, new IntSetting((int)value));
                        else if (value is bool)
                            settings.Add(variableName, new BoolSetting((bool)value));
                    }
                }
                return settings;
            }
            catch (Exception e)
            {
                DebugConsole.Log("Custom logic error getting main logic settings", true);
                return new Dictionary<string, BaseSetting>();
            }
        }

        public Dictionary<string, BaseSetting> GetComponentSettings(string component, List<string> parameters)
        {
            Dictionary<string, BaseSetting> settings = new Dictionary<string, BaseSetting>();
            Dictionary<string, string> parameterDict = new Dictionary<string, string>();
            try
            {
                var instance = CreateClassInstance(component, EmptyArgs, false);
                RunAssignmentsClassInstance(instance);
                foreach (string str in parameters)
                {
                    string[] strArr = str.Split(':');
                    parameterDict.Add(strArr[0], strArr[1]);
                }
                foreach (string variableName in instance.Variables.Keys)
                {
                    if (!variableName.StartsWith("_") && variableName != "Type")
                    {
                        object value = instance.Variables[variableName];
                        if (parameterDict.ContainsKey(variableName))
                            value = CustomLogicComponentInstance.DeserializeValue(value, parameterDict[variableName]);
                        if (value is float)
                            settings.Add(variableName, new FloatSetting((float)value));
                        else if (value is string)
                            settings.Add(variableName, new StringSetting((string)value));
                        else if (value is int)
                            settings.Add(variableName, new IntSetting((int)value));
                        else if (value is bool)
                            settings.Add(variableName, new BoolSetting((bool)value));
                        else if (value is CustomLogicColorBuiltin)
                            settings.Add(variableName, new ColorSetting(((CustomLogicColorBuiltin)value).Value));
                        else if (value is CustomLogicVector3Builtin)
                            settings.Add(variableName, new Vector3Setting(((CustomLogicVector3Builtin)value).Value));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Custom logic error getting component settings for " + component + ": " + e.Message + "\n" + e.InnerException);
            }
            return settings;
        }

        public List<string> GetComponentNames()
        {
            var componentNames = new List<string>();
            foreach (string className in _start.Classes.Keys)
            {
                if ((int)_start.Classes[className].Token.Value == (int)CustomLogicSymbol.Component)
                    componentNames.Add(className);
            }
            return componentNames;
        }

        public void Start(Dictionary<string, BaseSetting> modeSettings)
        {
            try
            {
                Init();
                CurrentTime = 0f;
                CustomLogicClassInstance main = _staticClasses["Main"];
                foreach (string variableName in modeSettings.Keys)
                {
                    BaseSetting setting = modeSettings[variableName];
                    if (setting is FloatSetting)
                        main.Variables[variableName] = ((FloatSetting)setting).Value;
                    else if (setting is StringSetting)
                        main.Variables[variableName] = ((StringSetting)setting).Value;
                    else if (setting is IntSetting)
                        main.Variables[variableName] = ((IntSetting)setting).Value;
                    else if (setting is BoolSetting)
                        main.Variables[variableName] = ((BoolSetting)setting).Value;
                }
                foreach (var instance in _staticClasses.Values)
                {
                    EvaluateMethod(instance, "Init");
                    instance.Inited = true;
                }
                EvaluateMethodForCallbacks("Init");
                AddCallbacks(_staticClasses["Main"]);
                EvaluateMethodForCallbacks("OnGameStart");
                CustomLogicManager._instance.StartCoroutine(OnSecond());
            }
            catch (Exception e)
            {
                DebugConsole.Log("Custom logic runtime error: " + e.Message, true);
            }
        }

        protected void AddCallbacks(CustomLogicClassInstance instance)
        {
            foreach (string method in _start.Classes[instance.ClassName].Methods.Keys)
            {
                if (!_callbacks.ContainsKey(method))
                    _callbacks.Add(method, new List<CustomLogicClassInstance>());
                _callbacks[method].Add(instance);
            }
        }

        protected void RemoveCallbacks(CustomLogicClassInstance instance)
        {
            foreach (string method in _start.Classes[instance.ClassName].Methods.Keys)
            {
                if (_callbacks.ContainsKey(method))
                {
                    var callback = _callbacks[method];
                    if (callback.Contains(instance))
                        callback.Remove(instance);
                }
            }
        }

        [CLCallbackAttribute]
        public void OnTick()
        {
            EvaluateMethodForCallbacks("OnTick");
            CurrentTime += Time.fixedDeltaTime;
        }

        [CLCallbackAttribute]
        public void OnFrame()
        {
            EvaluateMethodForCallbacks("OnFrame");
        }

        [CLCallbackAttribute]
        public void OnLateFrame()
        {
            EvaluateMethodForCallbacks("OnLateFrame");
        }

        [CLCallbackAttribute]
        public void OnButtonClick(string name)
        {
            var parameters = ArrayPool<object>.New(1);
            parameters[0] = name;
            EvaluateMethodForCallbacks("OnButtonClick", parameters);
            ArrayPool<object>.Free(parameters);
        }

        [CLCallbackAttribute]
        public void OnPlayerSpawn(Player player, BaseCharacter character)
        {
            var playerBuiltin = new CustomLogicPlayerBuiltin(player);
            var characterBuiltin = GetCharacterBuiltin(character);
            if (characterBuiltin == null)
                return;

            EvaluateMethodForCallbacks("OnPlayerSpawn", new object[] { playerBuiltin, characterBuiltin });
        }

        [CLCallbackAttribute]
        public void OnCharacterSpawn(BaseCharacter character)
        {
            var builtin = GetCharacterBuiltin(character);
            if (builtin == null)
                return;

            EvaluateMethodForCallbacks("OnCharacterSpawn", new object[] { builtin });
        }

        [CLCallbackAttribute]
        public void OnCharacterReloaded(BaseCharacter character)
        {
            var builtin = GetCharacterBuiltin(character);
            if (builtin == null)
                return;
            EvaluateMethodForCallbacks("OnCharacterReloaded", new object[] { builtin });
        }

        [CLCallbackAttribute]
        public void OnCharacterDie(BaseCharacter victim, BaseCharacter killer, string killerName)
        {
            var victimBuiltin = GetCharacterBuiltin(victim);
            var killerBuiltin = GetCharacterBuiltin(killer);

            EvaluateMethodForCallbacks("OnCharacterDie", new object[] { victimBuiltin, killerBuiltin, killerName });
        }

        [CLCallbackAttribute]
        public void OnCharacterDamaged(BaseCharacter victim, BaseCharacter killer, string killerName, int damage)
        {
            var victimBuiltin = GetCharacterBuiltin(victim);
            var killerBuiltin = GetCharacterBuiltin(killer);

            EvaluateMethodForCallbacks("OnCharacterDamaged", new object[] { victimBuiltin, killerBuiltin, killerName, damage });
        }

        [CLCallbackAttribute]
        public object OnChatInput(string message)
        {
            return EvaluateMethod(_staticClasses["Main"], "OnChatInput", new object[] { message });
        }

        [CLCallbackAttribute]
        public void OnPlayerJoin(Player player)
        {
            var playerBuiltin = new CustomLogicPlayerBuiltin(player);
            EvaluateMethodForCallbacks("OnPlayerJoin", new object[] { playerBuiltin });
            ((CustomLogicUIBuiltin)_staticClasses["UI"]).OnPlayerJoin(player);
        }

        [CLCallbackAttribute]
        public void OnPlayerLeave(Player player)
        {
            var playerBuiltin = new CustomLogicPlayerBuiltin(player);
            EvaluateMethodForCallbacks("OnPlayerLeave", new object[] { playerBuiltin });
        }

        /// caching this object[] since its potentially heavily used.
        private object[] _networkCallback = new object[3];
        [CLCallbackAttribute]
        public void OnNetworkMessage(Player sender, string message, double sentServerTimestamp)
        {
            var playerBuiltin = new CustomLogicPlayerBuiltin(sender);
            _networkCallback[0] = playerBuiltin;
            _networkCallback[1] = message;
            _networkCallback[2] = sentServerTimestamp;
            EvaluateMethod(_staticClasses["Main"], "OnNetworkMessage", _networkCallback);
        }

        public static CustomLogicCharacterBuiltin GetCharacterBuiltin(BaseCharacter character)
        {
            if (character is Human)
                return new CustomLogicHumanBuiltin((Human)character);
            else if (character is BasicTitan)
                return new CustomLogicTitanBuiltin((BasicTitan)character);
            else if (character is WallColossalShifter)
                return new CustomLogicWallColossalBuiltin((WallColossalShifter)character);
            else if (character is BaseShifter)
                return new CustomLogicShifterBuiltin((BaseShifter)character);
            return null;
        }

        [CLCallbackAttribute]
        private IEnumerator OnSecond()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                EvaluateMethodForCallbacks("OnSecond");
                if (PhotonNetwork.IsMasterClient)
                {
                    RPCManager.PhotonView.RPC("SyncCurrentTimeRPC", RpcTarget.Others, new object[] { CurrentTime });
                    foreach (int key in IdToNetworkView.Keys)
                        IdToNetworkView[key].OnSecond();
                }
            }
        }

        private void Init()
        {
            foreach (var staticType in CustomLogicBuiltinTypes.StaticTypeNames)
            {
                var instance = CustomLogicBuiltinTypes.CreateClassInstance(staticType, EmptyArgs);
                _staticClasses[staticType] = instance;
            }

            foreach (string className in _start.Classes.Keys)
            {
                if (className == "Main")
                    CreateStaticClass(className);
                else if ((int)_start.Classes[className].Token.Value == (int)CustomLogicSymbol.Extension)
                    CreateStaticClass(className);
            }
            foreach (CustomLogicClassInstance instance in _staticClasses.Values)
            {
                if (instance is not BuiltinClassInstance)
                    RunAssignmentsClassInstance(instance);
            }
            foreach (int id in MapLoader.IdToMapObject.Keys)
            {
                MapObject obj = MapLoader.IdToMapObject[id];
                LoadMapObjectComponents(obj);
            }
        }

        private List<object> emptyList = new List<object>();

        private void EvaluateMethodForCallbacks(string methodName, object[] parameters = null)
        {
            // for loop instead of foreach because the list might be modified during the loop
            if (_callbacks.ContainsKey(methodName))
            {
                var callback = _callbacks[methodName];
                for (int i = 0; i < callback.Count; i++)
                {
                    if (methodName == "Init")
                    {
                        if (callback[i].Inited)
                            continue;
                    }
                    else if (!callback[i].Enabled)
                        continue;
                    EvaluateMethod(callback[i], methodName, parameters);
                }
            }
        }

        public void LoadMapObjectComponents(MapObject obj, bool init = false)
        {
            if (obj.ScriptObject is MapScriptSceneObject)
            {
                var photonView = SetupNetworking(obj);
                var mapObjectBuiltin = SetupMapObject(obj);
                List<MapScriptComponent> components = ((MapScriptSceneObject)obj.ScriptObject).Components;
                bool rigidbody = false;
                foreach (var component in components)
                {
                    if (_start.Classes.ContainsKey(component.ComponentName))
                    {
                        CustomLogicComponentInstance instance = CreateComponentInstance(component.ComponentName, obj, component);
                        obj.RegisterComponentInstance(instance);
                        if (init)
                        {
                            EvaluateMethod(instance, "Init");
                            instance.Inited = true;
                        }
                        if (component.ComponentName == "Rigidbody")
                            rigidbody = true;
                    }
                }
                if (photonView != null)
                    photonView.Init(obj.ScriptObject.Id, rigidbody);
            }
        }

        public CustomLogicComponentInstance AddMapObjectComponent(MapObject obj, string componentName)
        {
            if (_start.Classes.ContainsKey(componentName))
            {
                var parameters = new List<string>();
                foreach (var assignment in _start.Classes[componentName].Assignments)
                {
                    if (assignment.Left is CustomLogicVariableExpressionAst left)
                    {
                        if (assignment.Right is CustomLogicPrimitiveExpressionAst right)
                        {
                            if (left.Name == "Description")
                                continue;
                            if (left.Name.EndsWith("Tooltip") && right.Value is string)
                                continue;
                            if (left.Name.EndsWith("Dropbox") && right.Value is string)
                                continue;
                        }

                        var str = left.Name + ":" + CustomLogicUtils.SerializeAst(assignment.Right);
                        parameters.Add(str);
                    }
                }

                MapScriptComponent component = new()
                {
                    ComponentName = componentName,
                    Parameters = parameters
                };

                var photonView = SetupNetworking(obj);
                var mapObjectBuiltin = SetupMapObject(obj);
                CustomLogicComponentInstance instance = CreateComponentInstance(componentName, obj, component);
                obj.RegisterComponentInstance(instance);
                EvaluateMethod(instance, "Init");
                instance.Inited = true;
                if (photonView != null)
                    photonView.Init(obj.ScriptObject.Id, componentName == "Rigidbody");
                else if (componentName == "Rigidbody" && IdToNetworkView.TryGetValue(instance.MapObject.Value.ScriptObject.Id, out var networkView))
                {
                    // re-init if a rigidbody is added
                    networkView.Sync.Init(obj.ScriptObject.Id, true);
                }
                return instance;
            }

            throw new Exception("No component named " + componentName + " found");
        }

        public void RemoveComponent(CustomLogicComponentInstance instance)
        {
            RemoveCallbacks(instance);
        }

        private void CreateStaticClass(string className)
        {
            if (!_staticClasses.ContainsKey(className))
            {
                var instance = CreateClassInstance(className, EmptyArgs, false);
                _staticClasses.Add(className, instance);
            }
        }

        public CustomLogicComponentInstance CreateComponentInstance(string className, MapObject obj, MapScriptComponent script)
        {

            CustomLogicNetworkViewBuiltin networkView = null;
            if (obj.ScriptObject.Networked)
                networkView = IdToNetworkView[obj.ScriptObject.Id];
            var classInstance = new CustomLogicComponentInstance(className, IdToMapObjectBuiltin[obj.ScriptObject.Id], script, networkView);
            if (networkView != null)
                networkView.RegisterComponentInstance(classInstance);
            RunAssignmentsClassInstance(classInstance);
            classInstance.LoadVariables();
            AddCallbacks(classInstance);
            if (classInstance.UsesCollider())
            {
                HashSet<GameObject> colliders = new HashSet<GameObject>();
                FindSubcolliders(obj.GameObject.transform, colliders);
                foreach (var go in colliders)
                {
                    var collisionHandler = go.GetComponent<CustomLogicCollisionHandler>();
                    if (collisionHandler == null)
                        collisionHandler = go.AddComponent<CustomLogicCollisionHandler>();
                    collisionHandler.RegisterInstance(classInstance);
                }
            }
            return classInstance;
        }

        private void FindSubcolliders(Transform t, HashSet<GameObject> set)
        {
            if (t.GetComponent<Collider>() != null)
                set.Add(t.gameObject);
            foreach (Transform child in t)
            {
                if (MapLoader.GoToMapObject.ContainsKey(child.gameObject))
                    continue;
                FindSubcolliders(child, set);
            }
        }

        public CustomLogicMapObjectBuiltin SetupMapObject(MapObject obj)
        {
            if (!IdToMapObjectBuiltin.ContainsKey(obj.ScriptObject.Id))
            {
                IdToMapObjectBuiltin.Add(obj.ScriptObject.Id, new CustomLogicMapObjectBuiltin(obj));
            }
            return IdToMapObjectBuiltin[obj.ScriptObject.Id];
        }

        public CustomLogicPhotonSync SetupNetworking(MapObject obj)
        {
            if (obj.ScriptObject.Networked)
            {
                if (!IdToNetworkView.ContainsKey(obj.ScriptObject.Id))
                {
                    IdToNetworkView.Add(obj.ScriptObject.Id, new CustomLogicNetworkViewBuiltin(obj));
                    if (PhotonNetwork.IsMasterClient)
                    {
                        var go = PhotonNetwork.Instantiate("Game/CustomLogicPhotonSyncPrefab", Vector3.zero, Quaternion.identity, 0);
                        var photonView = go.GetComponent<CustomLogicPhotonSync>();
                        return photonView;
                    }
                }
            }
            return null;
        }

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
                classInstance = new UserClassInstance(className);
                if (init)
                {
                    RunAssignmentsClassInstance(classInstance);
                    EvaluateMethod(classInstance, "Init", parameterValues);
                    classInstance.Inited = true;
                }
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
                DebugConsole.Log("Custom logic runtime error at method " + methodName + " in class " + classInstance.ClassName + ": " + e.InnerException?.Message, true);
                return null;
            }
            catch (Exception e)
            {
                DebugConsole.Log("Custom logic runtime error at method " + methodName + " in class " + classInstance.ClassName + ": " + e.Message, true);
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
                DebugConsole.Log("Custom logic runtime error at method " + methodName + " in class " + classInstance.ClassName + ": " + e.InnerException?.Message, true);
                return null;
            }
            catch (Exception e)
            {
                DebugConsole.Log("Custom logic runtime error at line " + GetLineNumberString(userMethod.Ast.Line) + " at method " + methodName + " in class " + classInstance.ClassName + ": " + e.Message, true);
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
                DebugConsole.Log("Custom logic runtime error at line " + GetLineNumberString(expression.Line) + ": " + e.Message, true);
            }
            return null;
        }

        private object EvaluateBinopExpression(CustomLogicSymbol symbol, object left, object right)
        {
            if (symbol == CustomLogicSymbol.Plus) return AddValues(left, right);
            else if (symbol == CustomLogicSymbol.Minus) return SubtractValues(left, right);
            else if (symbol == CustomLogicSymbol.Times) return MultiplyValues(left, right);
            else if (symbol == CustomLogicSymbol.Divide) return DivideValues(left, right);
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
}