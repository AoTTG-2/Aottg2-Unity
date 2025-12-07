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

using UnityEngine;

namespace CustomLogic
{
    internal partial class CustomLogicEvaluator
    {
        #region Callbacks

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
                LogCustomLogicError("Custom logic runtime error: " + e.Message, true);
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
                    RPCManager.PhotonView.RPC(nameof(RPCManager.SyncCurrentTimeRPC), RpcTarget.Others, new object[] { CurrentTime });
                    foreach (int key in IdToNetworkView.Keys)
                        IdToNetworkView[key].OnSecond();
                }
            }
        }

        private void Init()
        {
            // First, create C# builtin static classes
            // These should ALWAYS be available, regardless of user definitions
            foreach (var staticType in CustomLogicBuiltinTypes.StaticTypeNames)
            {
                var instance = CustomLogicBuiltinTypes.CreateClassInstance(staticType, EmptyArgs);
                _staticClasses[staticType] = instance;
            }
            
            // Then create user-defined static classes (Main and extensions)
            // User code may override builtins, but both versions should exist
            foreach (string className in _start.Classes.Keys)
            {
                if (className == "Main")
                {
                    // Always create Main from user code if it exists
                    CreateStaticClass(className);
                }
                else if ((int)_start.Classes[className].Token.Value == (int)CustomLogicSymbol.Extension)
                {
                    // Create user extensions
                    CreateStaticClass(className);
                }
                else
                {
                    // For non-extension user-defined classes that conflict with builtins,
                    // the user version is already registered in _start.Classes and will be
                    // resolved through CreateClassInstance's namespace-aware logic
                    // We don't add them to _staticClasses here
                }
            }

            // Run assignments for all class instances
            foreach (CustomLogicClassInstance instance in _staticClasses.Values)
            {
                if (instance is not BuiltinClassInstance)
                    RunAssignmentsClassInstance(instance);
            }
            
            // Load map objects
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

        #endregion

    }
}
