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
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicEvaluator
    {
        public float CurrentTime;
        public bool HasSetMusic = false;
        public Dictionary<int, CustomLogicNetworkViewBuiltin> IdToNetworkView = new Dictionary<int, CustomLogicNetworkViewBuiltin>();
        protected CustomLogicStartAst _start;
        protected Dictionary<string, CustomLogicClassInstance> _staticClasses = new Dictionary<string, CustomLogicClassInstance>();
        protected List<CustomLogicClassInstance> _callback = new List<CustomLogicClassInstance>();

        public CustomLogicEvaluator(CustomLogicStartAst start)
        {
            _start = start;
        }

        public Dictionary<string, BaseSetting> GetModeSettings()
        {
            var instance = CreateClassInstance("Main", new List<object>(), false);
            try
            {
                RunAssignmentsClassInstance(instance);
                Dictionary<string, BaseSetting> settings = new Dictionary<string, BaseSetting>();
                foreach (string variableName in instance.Variables.Keys)
                {
                    if (!variableName.StartsWith("_"))
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
                var instance = CreateClassInstance(component, new List<object>(), false);
                RunAssignmentsClassInstance(instance);
                foreach (string str in parameters)
                {
                    string[] strArr = str.Split(':');
                    parameterDict.Add(strArr[0], strArr[1]);
                }
                foreach (string variableName in instance.Variables.Keys)
                {
                    if (!variableName.StartsWith("_"))
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
            catch
            {
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
                    EvaluateMethod(instance, "Init", new List<object>());
                foreach (var instance in _callback)
                    EvaluateMethod(instance, "Init", new List<object>());
                _callback.Add(_staticClasses["Main"]);
                foreach (var instance in _callback)
                    EvaluateMethod(instance, "OnGameStart", new List<object>());
                CustomLogicManager._instance.StartCoroutine(OnSecond());
                CustomLogicManager._instance.StartCoroutine(WaitAndSetDefaultPlaylist());
            }
            catch (Exception e)
            {
                DebugConsole.Log("Custom logic runtime error: " + e.Message, true);
            }
        }

        public void OnTick()
        {
            foreach (var instance in _callback)
                EvaluateMethod(instance, "OnTick", new List<object>());
            CurrentTime += Time.fixedDeltaTime;
        }

        public void OnFrame()
        {
            foreach (var instance in _callback)
                EvaluateMethod(instance, "OnFrame", new List<object>());
        }

        public void OnLateFrame()
        {
            foreach (var instance in _callback)
                EvaluateMethod(instance, "OnLateFrame", new List<object>());
        }

        public void OnButtonClick(string name)
        {
            foreach (var instance in _callback)
                EvaluateMethod(instance, "OnButtonClick", new List<object>() { name });
        }

        public void OnPlayerSpawn(Player player, BaseCharacter character)
        {
            var playerBuiltin = new CustomLogicPlayerBuiltin(player);
            var characterBuiltin = GetCharacterBuiltin(character);
            foreach (var instance in _callback)
                EvaluateMethod(instance, "OnPlayerSpawn", new List<object>() { playerBuiltin, characterBuiltin });
        }

        public void OnCharacterSpawn(BaseCharacter character)
        {
            var builtin = GetCharacterBuiltin(character);
            foreach (var instance in _callback)
                EvaluateMethod(instance, "OnCharacterSpawn", new List<object>() { builtin });
        }

        public void OnCharacterDie(BaseCharacter victim, BaseCharacter killer, string killerName)
        {
            var victimBuiltin = GetCharacterBuiltin(victim);
            var killerBuiltin = GetCharacterBuiltin(killer);
            foreach (var instance in _callback)
                EvaluateMethod(instance, "OnCharacterDie", new List<object>() { victimBuiltin, killerBuiltin, killerName });
        }

        public void OnCharacterDamaged(BaseCharacter victim, BaseCharacter killer, string killerName, int damage)
        {
            var victimBuiltin = GetCharacterBuiltin(victim);
            var killerBuiltin = GetCharacterBuiltin(killer);
            foreach (var instance in _callback)
                EvaluateMethod(instance, "OnCharacterDamaged", new List<object>() { victimBuiltin, killerBuiltin, killerName, damage });
        }

        public void OnChatInput(string message)
        {
            foreach (var instance in _callback)
                EvaluateMethod(instance, "OnChatInput", new List<object>() { message });
        }

        public void OnPlayerJoin(Player player)
        {
            var playerBuiltin = new CustomLogicPlayerBuiltin(player);
            foreach (var instance in _callback)
                EvaluateMethod(instance, "OnPlayerJoin", new List<object>() { playerBuiltin });
            ((CustomLogicUIBuiltin)_staticClasses["UI"]).OnPlayerJoin(player);
        }

        public void OnPlayerLeave(Player player)
        {
            var playerBuiltin = new CustomLogicPlayerBuiltin(player);
            foreach (var instance in _callback)
                EvaluateMethod(instance, "OnPlayerLeave", new List<object>() { playerBuiltin });
        }

        public void OnNetworkMessage(Player sender, string message)
        {
            var playerBuiltin = new CustomLogicPlayerBuiltin(sender);
            EvaluateMethod(_staticClasses["Main"], "OnNetworkMessage", new List<object>() { playerBuiltin, message });
        }

        public static CustomLogicCharacterBuiltin GetCharacterBuiltin(BaseCharacter character)
        {
            if (character is Human)
                return new CustomLogicHumanBuiltin((Human)character);
            else if (character is BasicTitan)
                return new CustomLogicTitanBuiltin((BasicTitan)character);
            else if (character is BaseShifter)
                return new CustomLogicShifterBuiltin((BaseShifter)character);
            return null;
        }

        private IEnumerator WaitAndSetDefaultPlaylist()
        {
            yield return new WaitForSeconds(3f);
            if (!HasSetMusic)
                MusicManager.SetPlaylist(MusicPlaylist.Default);
        }

        private IEnumerator OnSecond()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                foreach (var instance in new List<CustomLogicClassInstance>(_callback))
                    EvaluateMethod(instance, "OnSecond", new List<object>());
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
            foreach (string name in new string[] {"Game", "Vector3", "Color", "Quaternion", "Convert", "Cutscene", "Time", "Network", "UI", "Input", "Math", "Map",
            "Random", "String", "Camera", "RoomData", "PersistentData", "Json", "Physics"})
                CreateStaticClass(name);
            foreach (string className in new List<string>(_start.Classes.Keys))
            {
                if (className == "Main")
                    CreateStaticClass(className);
                else if ((int)_start.Classes[className].Token.Value == (int)CustomLogicSymbol.Extension)
                    CreateStaticClass(className);
            }
            foreach (CustomLogicClassInstance instance in _staticClasses.Values)
            {
                if (!(instance is CustomLogicBaseBuiltin))
                    RunAssignmentsClassInstance(instance);
            }
            foreach (int id in MapLoader.IdToMapObject.Keys)
            {
                MapObject obj = MapLoader.IdToMapObject[id];
                LoadMapObjectComponents(obj);
            }
        }

        public void LoadMapObjectComponents(MapObject obj, bool init = false)
        {
            if (obj.ScriptObject is MapScriptSceneObject)
            {
                var photonView = SetupNetworking(obj);
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
                            EvaluateMethod(instance, "Init", new List<object>());
                        }
                        if (component.ComponentName == "Rigidbody")
                            rigidbody = true;
                    }
                }
                if (photonView != null)
                    photonView.Init(obj.ScriptObject.Id, rigidbody);
            }
        }

        private void CreateStaticClass(string className)
        {
            if (!_staticClasses.ContainsKey(className))
            {
                CustomLogicClassInstance instance;
                if (className == "Game")
                    instance = new CustomLogicGameBuiltin();
                else if (className == "Convert")
                    instance = new CustomLogicConvertBuiltin();
                else if (className == "Cutscene")
                    instance = new CustomLogicCutsceneBuiltin();
                else if (className == "Time")
                    instance = new CustomLogicTimeBuiltin();
                else if (className == "Network")
                    instance = new CustomLogicNetworkBuiltin();
                else if (className == "UI")
                    instance = new CustomLogicUIBuiltin();
                else if (className == "Input")
                    instance = new CustomLogicInputBuiltin();
                else if (className == "Math")
                    instance = new CustomLogicMathBuiltin();
                else if (className == "Vector3")
                    instance = new CustomLogicVector3Builtin(new List<object>());
                else if (className == "Quaternion")
                    instance = new CustomLogicQuaternionBuiltin(new List<object>());
                else if (className == "Map")
                    instance = new CustomLogicMapBuiltin();
                else if (className == "String")
                    instance = new CustomLogicStringBuiltin();
                else if (className == "Random")
                    instance = new CustomLogicRandomBuiltin(new List<object>());
                else if (className == "Camera")
                    instance = new CustomLogicCameraBuiltin();
                else if (className == "RoomData")
                    instance = new CustomLogicRoomDataBuiltin();
                else if (className == "PersistentData")
                    instance = new CustomLogicPersistentDataBuiltin();
                else if (className == "Json")
                    instance = new CustomLogicJsonBuiltin();
                else if (className == "Physics")
                    instance = new CustomLogicPhysicsBuiltin();
                else
                    instance = CreateClassInstance(className, new List<object>(), false);
                _staticClasses.Add(className, instance);
            }
        }

        public CustomLogicComponentInstance CreateComponentInstance(string className, MapObject obj, MapScriptComponent script)
        {

            CustomLogicNetworkViewBuiltin networkView = null;
            if (obj.ScriptObject.Networked)
                networkView = IdToNetworkView[obj.ScriptObject.Id];
            var classInstance = new CustomLogicComponentInstance(className, obj, script, networkView);
            if (networkView != null)
                networkView.RegisterComponentInstance(classInstance);
            RunAssignmentsClassInstance(classInstance);
            classInstance.LoadVariables();
            _callback.Add(classInstance);
            if (classInstance.UsesCollider())
            {
                HashSet<GameObject> children = new HashSet<GameObject>();
                children.Add(obj.GameObject);
                foreach (var collider in obj.GameObject.GetComponentsInChildren<Collider>())
                {
                    if (!children.Contains(collider.gameObject))
                        children.Add(collider.gameObject);
                }
                foreach (var go in children)
                {
                    var collisionHandler = go.GetComponent<CustomLogicCollisionHandler>();
                    if (collisionHandler == null)
                        collisionHandler = go.AddComponent<CustomLogicCollisionHandler>();
                    collisionHandler.RegisterInstance(classInstance);
                }
            }
            return classInstance;
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

        public CustomLogicClassInstance CreateClassInstance(string className, List<object> parameterValues, bool init = true)
        {
            CustomLogicClassInstance classInstance;
            if (className == "Dict")
                classInstance = new CustomLogicDictBuiltin();
            else if (className == "List")
                classInstance = new CustomLogicListBuiltin();
            else if (className == "Vector3")
                classInstance = new CustomLogicVector3Builtin(parameterValues);
            else if (className == "Color")
                classInstance = new CustomLogicColorBuiltin(parameterValues);
            else if (className == "Quaternion")
                classInstance = new CustomLogicQuaternionBuiltin(parameterValues);
            else if (className == "Range")
                classInstance = new CustomLogicRangeBuiltin(parameterValues);
            else if (className == "Random")
                classInstance = new CustomLogicRandomBuiltin(parameterValues);
            else
            {
                classInstance = new CustomLogicClassInstance(className);
                if (init)
                {
                    RunAssignmentsClassInstance(classInstance);
                    EvaluateMethod(classInstance, "Init", parameterValues);
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
                if (classInstance.Variables.ContainsKey(variableName))
                    classInstance.Variables[variableName] = value;
                else
                    classInstance.Variables.Add(variableName, value);
            }
        }

        private IEnumerator EvaluateBlockCoroutine(CustomLogicClassInstance classInstance, Dictionary<string, object> localVariables,
            List<CustomLogicBaseAst> statements)
        {
            ConditionalEvalState conditionalState = ConditionalEvalState.None;
            foreach (CustomLogicBaseAst statement in statements)
            {
                if (statement is CustomLogicAssignmentExpressionAst)
                {
                    CustomLogicAssignmentExpressionAst assignment = (CustomLogicAssignmentExpressionAst)statement;
                    object value = EvaluateExpression(classInstance, localVariables, assignment.Right);
                    if (value != null && value is CustomLogicStructBuiltin)
                        value = ((CustomLogicStructBuiltin)value).Copy();
                    if (assignment.Left is CustomLogicVariableExpressionAst)
                    {
                        string variableName = ((CustomLogicVariableExpressionAst)assignment.Left).Name;
                        if (localVariables.ContainsKey(variableName))
                            localVariables[variableName] = value;
                        else
                            localVariables.Add(variableName, value);
                    }
                    else if (assignment.Left is CustomLogicFieldExpressionAst)
                    {
                        CustomLogicFieldExpressionAst fieldExpression = (CustomLogicFieldExpressionAst)assignment.Left;
                        CustomLogicClassInstance fieldInstance = (CustomLogicClassInstance)EvaluateExpression(classInstance, localVariables, fieldExpression.Left);
                        if (fieldInstance is CustomLogicBaseBuiltin)
                            ((CustomLogicBaseBuiltin)fieldInstance).SetField(fieldExpression.FieldName, value);
                        else
                        {
                            if (fieldInstance.Variables.ContainsKey(fieldExpression.FieldName))
                                fieldInstance.Variables[fieldExpression.FieldName] = value;
                            else
                                fieldInstance.Variables.Add(fieldExpression.FieldName, value);
                        }
                    }
                }
                else if (statement is CustomLogicCompoundAssignmentExpressionAst)
                {
                    CustomLogicCompoundAssignmentExpressionAst assignment = (CustomLogicCompoundAssignmentExpressionAst)statement;
                    CustomLogicSymbol op = (CustomLogicSymbol)assignment.Operator.Value;
                    object value = EvaluateExpression(classInstance, localVariables, assignment.Right);
                    if (value != null && value is CustomLogicStructBuiltin)
                        value = ((CustomLogicStructBuiltin)value).Copy();
                    if (assignment.Left is CustomLogicVariableExpressionAst)
                    {
                        string variableName = ((CustomLogicVariableExpressionAst)assignment.Left).Name;
                        object originalValue = localVariables[variableName];
                        object newValue = op switch
                        {
                            CustomLogicSymbol.PlusEquals => AddValues(originalValue, value),
                            CustomLogicSymbol.MinusEquals => SubtractValues(originalValue, value),
                            CustomLogicSymbol.TimesEquals => MultiplyValues(originalValue, value),
                            CustomLogicSymbol.DivideEquals => DivideValues(originalValue, value),
                            _ => value,
                        };
                        localVariables[variableName] = newValue;
                    }
                    else if (assignment.Left is CustomLogicFieldExpressionAst)
                    {
                        CustomLogicFieldExpressionAst fieldExpression = (CustomLogicFieldExpressionAst)assignment.Left;
                        CustomLogicClassInstance fieldInstance = (CustomLogicClassInstance)EvaluateExpression(classInstance, localVariables, fieldExpression.Left);
                        object originalValue = fieldInstance.Variables[fieldExpression.FieldName];
                        object newValue = op switch
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
                else if (statement is CustomLogicReturnExpressionAst)
                {
                    yield break;
                }
                else if (statement is CustomLogicWaitExpressionAst)
                {
                    object value = EvaluateExpression(classInstance, localVariables, ((CustomLogicWaitExpressionAst)statement).WaitTime);
                    string className = classInstance.ClassName;
                    if ((int)_start.Classes[className].Token.Value == (int)CustomLogicSymbol.Cutscene)
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
                }
                else if (statement is CustomLogicConditionalBlockAst)
                {
                    CustomLogicConditionalBlockAst conditional = (CustomLogicConditionalBlockAst)statement;
                    if ((int)conditional.Token.Value == (int)CustomLogicSymbol.If)
                    {
                        if ((bool)EvaluateExpression(classInstance, localVariables, conditional.Condition))
                        {
                            yield return CustomLogicManager._instance.StartCoroutine(EvaluateBlockCoroutine(classInstance, localVariables, conditional.Statements));
                            conditionalState = ConditionalEvalState.PassedIf;
                        }
                        else
                            conditionalState = ConditionalEvalState.FailedIf;
                    }
                    else if ((int)conditional.Token.Value == (int)CustomLogicSymbol.While)
                    {
                        while ((bool)EvaluateExpression(classInstance, localVariables, conditional.Condition))
                        {
                            yield return CustomLogicManager._instance.StartCoroutine(EvaluateBlockCoroutine(classInstance, localVariables, conditional.Statements));
                        }
                        conditionalState = ConditionalEvalState.None;
                    }
                    else if ((int)conditional.Token.Value == (int)CustomLogicSymbol.Else)
                    {
                        if ((conditionalState == ConditionalEvalState.FailedIf || conditionalState == ConditionalEvalState.FailedElseIf) &&
                            (bool)EvaluateExpression(classInstance, localVariables, conditional.Condition))
                        {
                            yield return CustomLogicManager._instance.StartCoroutine(EvaluateBlockCoroutine(classInstance, localVariables, conditional.Statements));
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
                            yield return CustomLogicManager._instance.StartCoroutine(EvaluateBlockCoroutine(classInstance, localVariables, conditional.Statements));
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
                        yield return CustomLogicManager._instance.StartCoroutine(EvaluateBlockCoroutine(classInstance, localVariables, forBlock.Statements));
                    }
                }
                else if (statement is CustomLogicBaseExpressionAst)
                    EvaluateExpression(classInstance, localVariables, ((CustomLogicBaseExpressionAst)statement));
                if (!(statement is CustomLogicConditionalBlockAst))
                    conditionalState = ConditionalEvalState.None;
            }
        }

        private object[] EvaluateBlock(CustomLogicClassInstance classInstance, Dictionary<string, object> localVariables, List<CustomLogicBaseAst> statements)
        {
            ConditionalEvalState conditionalState = ConditionalEvalState.None;
            object[] result = new object[2] { false, null };
            foreach (CustomLogicBaseAst statement in statements)
            {
                if (statement is CustomLogicAssignmentExpressionAst)
                {
                    CustomLogicAssignmentExpressionAst assignment = (CustomLogicAssignmentExpressionAst)statement;
                    object value = EvaluateExpression(classInstance, localVariables, assignment.Right);
                    if (value != null && value is CustomLogicStructBuiltin)
                        value = ((CustomLogicStructBuiltin)value).Copy();
                    if (assignment.Left is CustomLogicVariableExpressionAst)
                    {
                        string variableName = ((CustomLogicVariableExpressionAst)assignment.Left).Name;
                        if (localVariables.ContainsKey(variableName))
                            localVariables[variableName] = value;
                        else
                            localVariables.Add(variableName, value);
                    }
                    else if (assignment.Left is CustomLogicFieldExpressionAst)
                    {
                        CustomLogicFieldExpressionAst fieldExpression = (CustomLogicFieldExpressionAst)assignment.Left;
                        CustomLogicClassInstance fieldInstance = (CustomLogicClassInstance)EvaluateExpression(classInstance, localVariables, fieldExpression.Left);
                        if (fieldInstance is CustomLogicBaseBuiltin)
                            ((CustomLogicBaseBuiltin)fieldInstance).SetField(fieldExpression.FieldName, value);
                        else
                        {
                            if (fieldInstance.Variables.ContainsKey(fieldExpression.FieldName))
                                fieldInstance.Variables[fieldExpression.FieldName] = value;
                            else
                                fieldInstance.Variables.Add(fieldExpression.FieldName, value);
                        }
                    }
                }
                else if (statement is CustomLogicCompoundAssignmentExpressionAst)
                {
                    CustomLogicCompoundAssignmentExpressionAst assignment = (CustomLogicCompoundAssignmentExpressionAst)statement;
                    CustomLogicSymbol op = (CustomLogicSymbol)assignment.Operator.Value;
                    object value = EvaluateExpression(classInstance, localVariables, assignment.Right);
                    if (value != null && value is CustomLogicStructBuiltin)
                        value = ((CustomLogicStructBuiltin)value).Copy();

                    if (assignment.Left is CustomLogicVariableExpressionAst)
                    {
                        string variableName = ((CustomLogicVariableExpressionAst)assignment.Left).Name;
                        object originalValue = localVariables[variableName];
                        object newValue = op switch
                        {
                            CustomLogicSymbol.PlusEquals => AddValues(originalValue, value),
                            CustomLogicSymbol.MinusEquals => SubtractValues(originalValue, value),
                            CustomLogicSymbol.TimesEquals => MultiplyValues(originalValue, value),
                            CustomLogicSymbol.DivideEquals => DivideValues(originalValue, value),
                            _ => value,
                        };
                        localVariables[variableName] = newValue;
                    }
                    else if (assignment.Left is CustomLogicFieldExpressionAst)
                    {
                        CustomLogicFieldExpressionAst fieldExpression = (CustomLogicFieldExpressionAst)assignment.Left;
                        CustomLogicClassInstance fieldInstance = (CustomLogicClassInstance)EvaluateExpression(classInstance, localVariables, fieldExpression.Left);
                        object originalValue = fieldInstance.Variables[fieldExpression.FieldName];
                        object newValue = op switch
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
                else if (statement is CustomLogicReturnExpressionAst)
                {
                    result[0] = true;
                    result[1] = EvaluateExpression(classInstance, localVariables, ((CustomLogicReturnExpressionAst)statement).ReturnValue);
                    return result;
                }
                else if (statement is CustomLogicConditionalBlockAst)
                {
                    CustomLogicConditionalBlockAst conditional = (CustomLogicConditionalBlockAst)statement;
                    if ((int)conditional.Token.Value == (int)CustomLogicSymbol.If)
                    {
                        if ((bool)EvaluateExpression(classInstance, localVariables, conditional.Condition))
                        {
                            object[] nextResult = EvaluateBlock(classInstance, localVariables, conditional.Statements);
                            if ((bool)nextResult[0])
                                return nextResult;
                            conditionalState = ConditionalEvalState.PassedIf;
                        }
                        else
                            conditionalState = ConditionalEvalState.FailedIf;
                    }
                    else if ((int)conditional.Token.Value == (int)CustomLogicSymbol.While)
                    {
                        while ((bool)EvaluateExpression(classInstance, localVariables, conditional.Condition))
                        {
                            object[] nextResult = EvaluateBlock(classInstance, localVariables, conditional.Statements);
                            if ((bool)nextResult[0])
                                return nextResult;
                        }
                        conditionalState = ConditionalEvalState.None;
                    }
                    else if ((int)conditional.Token.Value == (int)CustomLogicSymbol.Else)
                    {
                        if (conditionalState == ConditionalEvalState.FailedIf || conditionalState == ConditionalEvalState.FailedElseIf)
                        {
                            object[] nextResult = EvaluateBlock(classInstance, localVariables, conditional.Statements);
                            if ((bool)nextResult[0])
                                return nextResult;
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
                            object[] nextResult = EvaluateBlock(classInstance, localVariables, conditional.Statements);
                            if ((bool)nextResult[0])
                                return nextResult;
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
                        object[] nextResult = EvaluateBlock(classInstance, localVariables, forBlock.Statements);
                        if ((bool)nextResult[0])
                            return nextResult;
                    }
                }
                else if (statement is CustomLogicBaseExpressionAst)
                    EvaluateExpression(classInstance, localVariables, ((CustomLogicBaseExpressionAst)statement));
                if (!(statement is CustomLogicConditionalBlockAst))
                    conditionalState = ConditionalEvalState.None;
            }
            return result;
        }

        public bool HasMethod(CustomLogicClassInstance classInstance, string methodName)
        {
            return _start.Classes[classInstance.ClassName].Methods.ContainsKey(methodName);
        }

        public object EvaluateMethod(CustomLogicClassInstance classInstance, string methodName, List<object> parameterValues)
        {
            try
            {
                if (classInstance is CustomLogicBaseBuiltin)
                {
                    return ((CustomLogicBaseBuiltin)classInstance).CallMethod(methodName, parameterValues);
                }
                Dictionary<string, object> localVariables = new Dictionary<string, object>();
                if (!_start.Classes[classInstance.ClassName].Methods.ContainsKey(methodName))
                    return null;
                CustomLogicMethodDefinitionAst methodAst = _start.Classes[classInstance.ClassName].Methods[methodName];
                for (int i = 0; i < parameterValues.Count; i++)
                    localVariables.Add(methodAst.ParameterNames[i], parameterValues[i]);
                if (methodAst.Coroutine)
                {
                    return CustomLogicManager._instance.StartCoroutine(EvaluateBlockCoroutine(classInstance, localVariables, methodAst.Statements));
                }
                else
                {
                    var result = EvaluateBlock(classInstance, localVariables, methodAst.Statements);
                    return result[1];
                }
            }
            catch (Exception e)
            {
                DebugConsole.Log("Custom logic runtime error at method " + methodName + " in class " + classInstance.ClassName + ": " + e.Message, true);
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
                    List<object> parameters = new List<object>();
                    foreach (CustomLogicBaseAst ast in instantiate.Parameters)
                    {
                        parameters.Add(EvaluateExpression(classInstance, localVariables, (CustomLogicBaseExpressionAst)ast));
                    }
                    return CreateClassInstance(instantiate.Name, parameters, true);
                }
                else if (expression.Type == CustomLogicAstType.FieldExpression)
                {
                    CustomLogicFieldExpressionAst fieldExpression = (CustomLogicFieldExpressionAst)expression;
                    CustomLogicClassInstance fieldInstance = (CustomLogicClassInstance)EvaluateExpression(classInstance, localVariables, fieldExpression.Left);
                    if (fieldInstance is CustomLogicBaseBuiltin)
                        return ((CustomLogicBaseBuiltin)fieldInstance).GetField(fieldExpression.FieldName);
                    object value = fieldInstance.Variables[fieldExpression.FieldName];
                    if (value != null && value is CustomLogicStructBuiltin)
                        value = ((CustomLogicStructBuiltin)value).Copy();
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
                    List<object> parameters = new List<object>();
                    foreach (CustomLogicBaseExpressionAst parameterExpression in methodCallExpression.Parameters)
                    {
                        parameters.Add(EvaluateExpression(classInstance, localVariables, parameterExpression));
                    }
                    return EvaluateMethod(methodCallInstance, methodCallExpression.Name, parameters);
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
                DebugConsole.Log("Custom logic runtime error at line " + expression.Line.ToString() + ": " + e.Message, true);
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
            else if (left is CustomLogicVector3Builtin && right is CustomLogicVector3Builtin)
                return new CustomLogicVector3Builtin(((CustomLogicVector3Builtin)left).Value + ((CustomLogicVector3Builtin)right).Value);
            else
                return left.UnboxToFloat() + right.UnboxToFloat();
        }

        private object SubtractValues(object left, object right)
        {
            if (left is int && right is int)
                return (int)left - (int)right;
            else if (left is CustomLogicVector3Builtin && right is CustomLogicVector3Builtin)
                return new CustomLogicVector3Builtin(((CustomLogicVector3Builtin)left).Value - ((CustomLogicVector3Builtin)right).Value);
            else
                return left.UnboxToFloat() - right.UnboxToFloat();
        }

        private object MultiplyValues(object left, object right)
        {
            if (left is int && right is int)
                return (int)((int)left * (int)right);
            else if (left is CustomLogicVector3Builtin)
                return new CustomLogicVector3Builtin(((CustomLogicVector3Builtin)left).Value * right.UnboxToFloat());
            else if (right is CustomLogicVector3Builtin)
                return new CustomLogicVector3Builtin(((CustomLogicVector3Builtin)right).Value * left.UnboxToFloat());
            else if (left is CustomLogicQuaternionBuiltin && right is CustomLogicQuaternionBuiltin)
                return new CustomLogicQuaternionBuiltin(((CustomLogicQuaternionBuiltin)left).Value * ((CustomLogicQuaternionBuiltin)right).Value);
            else
                return left.UnboxToFloat() * right.UnboxToFloat();
        }

        private object DivideValues(object left, object right)
        {
            if (left is int && right is int)
                return (int)left / (int)right;
            else if (left is CustomLogicVector3Builtin)
                return new CustomLogicVector3Builtin(((CustomLogicVector3Builtin)left).Value / right.UnboxToFloat());
            else
                return left.UnboxToFloat() / right.UnboxToFloat();
        }

        private bool CheckEquals(object left, object right)
        {
            if (left == null && right == null)
                return true;
            if (left != null)
                return left.Equals(right);
            return right.Equals(left);
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