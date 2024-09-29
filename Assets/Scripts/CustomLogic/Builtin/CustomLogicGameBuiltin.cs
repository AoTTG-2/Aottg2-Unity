using ApplicationManagers;
using Characters;
using Effects;
using GameManagers;
using Map;
using NUnit.Framework;
using Photon.Pun;
using Projectiles;
using Settings;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicGameBuiltin: CustomLogicBaseBuiltin
    {
        private string _lastSetTopLabel = string.Empty;
        private Dictionary<string, CustomLogicListBuiltin> _cachedLists = new Dictionary<string, CustomLogicListBuiltin>();

        public CustomLogicGameBuiltin(): base("Game")
        {
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            if (name == "Debug")
            {
                DebugConsole.Log((string)parameters[0], true);
                return null;
            }
            var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
            if (name == "Print")
            {
                string message;
                if (parameters[0] == null)
                    message = "null";
                else
                    message = parameters[0].ToString();
                ChatManager.AddLine(message, ChatTextColor.System);
                return null;
            }
            if (name == "PrintAll")
            {
                string message;
                if (parameters[0] == null)
                    message = "null";
                else
                    message = parameters[0].ToString();
                ChatManager.SendChatAll(message, ChatTextColor.System);
                return null;
            }
            if (name == "GetGeneralSetting")
            {
                string settingName = (string)parameters[0];
                var setting = SettingsManager.InGameCurrent.General.TypedSettings[settingName];
                return setting.GetType().GetProperty("Value").GetValue(setting);
            }
            if (name == "GetTitanSetting")
            {
                string settingName = (string)parameters[0];
                var setting = SettingsManager.InGameCurrent.Titan.TypedSettings[settingName];
                return setting.GetType().GetProperty("Value").GetValue(setting);
            }
            if (name == "GetMiscSetting")
            {
                string settingName = (string)parameters[0];
                var setting = SettingsManager.InGameCurrent.Misc.TypedSettings[settingName];
                return setting.GetType().GetProperty("Value").GetValue(setting);
            }
            if (name == "End")
            {
                if (PhotonNetwork.IsMasterClient)
                    RPCManager.PhotonView.RPC("EndGameRPC", RpcTarget.All, new object[] { parameters[0].UnboxToFloat() });
                return null;
            }
            if (name == "FindCharacterByViewID")
            {
                int viewID = (int)parameters[0];
                var character = Util.FindCharacterByViewId(viewID);
                if (character == null || character.Dead)
                    return null;
                return CustomLogicEvaluator.GetCharacterBuiltin(character);
            }
            if (name == "SpawnTitan")
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    string type = (string)parameters[0];
                    var titan = new CustomLogicTitanBuiltin(gameManager.SpawnAITitan(type));
                    return titan;
                }
                return null;
            }
            if (name == "SpawnTitanAt")
            {
                string type = (string)parameters[0];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[1]).Value;
                float rotationY = parameters.Count > 2 ? parameters[2].UnboxToFloat() : 0f;
                if (PhotonNetwork.IsMasterClient)
                {
                    var titan = new CustomLogicTitanBuiltin(gameManager.SpawnAITitanAt(type, position, rotationY));
                    return titan;
                }
                return null;
            }
            if (name == "SpawnTitans")
            {
                string type = (string)parameters[0];
                if (PhotonNetwork.IsMasterClient)
                {
                    CustomLogicListBuiltin list = new CustomLogicListBuiltin();
                    foreach (var titan in gameManager.SpawnAITitans(type, (int)parameters[1]))
                        list.List.Add(new CustomLogicTitanBuiltin(titan));
                    return list;
                }
                return null;
            }
            if (name == "SpawnTitansAsync")
            {
                string type = (string)parameters[0];
                if (PhotonNetwork.IsMasterClient)
                    gameManager.SpawnAITitansAsync(type, (int)parameters[1]);
                return null;
            }
            if (name == "SpawnTitansAt")
            {
                string type = (string)parameters[0];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[2]).Value;
                float rotationY = parameters.Count > 3 ? parameters[3].UnboxToFloat() : 0f;
                if (PhotonNetwork.IsMasterClient)
                {
                    CustomLogicListBuiltin list = new CustomLogicListBuiltin();
                    for (int i = 0; i < (int)parameters[1]; i++)
                    {
                        var titan = new CustomLogicTitanBuiltin(gameManager.SpawnAITitanAt(type, position, rotationY));
                        list.List.Add(titan);
                    }
                    return list;
                }
                return null;
            }
            if (name == "SpawnTitansAtAsync")
            {
                string type = (string)parameters[0];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[2]).Value;
                float rotationY = parameters.Count > 3 ? parameters[3].UnboxToFloat() : 0f;
                if (PhotonNetwork.IsMasterClient)
                    gameManager.SpawnAITitansAtAsync(type, (int)parameters[1], position, rotationY);
                return null;
            }
            if (name == "SpawnShifter")
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    string type = (string)parameters[0];
                    var shifter = new CustomLogicShifterBuiltin(gameManager.SpawnAIShifter(type));
                    return shifter;
                }
                return null;
            }
            if (name == "SpawnShifterAt")
            {
                string type = (string)parameters[0];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[1]).Value;
                float rotationY = parameters.Count > 2 ? parameters[2].UnboxToFloat() : 0f;
                if (PhotonNetwork.IsMasterClient)
                {
                    var shifter = new CustomLogicShifterBuiltin(gameManager.SpawnAIShifterAt(type, position, rotationY));
                    return shifter;
                }
                return null;
            }
            if (name == "SpawnProjectile")
            {
                string projectileName = (string)parameters[0];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[1]).Value;
                Vector3 rotation = ((CustomLogicVector3Builtin)parameters[2]).Value;
                Vector3 velocity = ((CustomLogicVector3Builtin)parameters[3]).Value;
                Vector3 gravity = ((CustomLogicVector3Builtin)parameters[4]).Value;
                float liveTime = (float)parameters[5];
                string team = (string)parameters[6];
                object[] settings = null;
                if (projectileName == ProjectilePrefabs.Thunderspear)
                {
                    float radius = (float)parameters[7];
                    Color color = ((CustomLogicColorBuiltin)parameters[8]).Value.ToColor();
                    settings = new object[] { radius, color };
                }
                else if (projectileName == ProjectilePrefabs.Flare)
                {
                    Color color = ((CustomLogicColorBuiltin)parameters[7]).Value.ToColor();
                    settings = new object[] { color };
                }
                ProjectileSpawner.Spawn(projectileName, position, Quaternion.Euler(rotation), velocity, gravity, liveTime, -1, team, settings);
                return null;
            }
            if (name == "SpawnProjectileWithOwner")
            {
                string projectileName = (string)parameters[0];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[1]).Value;
                Vector3 rotation = ((CustomLogicVector3Builtin)parameters[2]).Value;
                Vector3 velocity = ((CustomLogicVector3Builtin)parameters[3]).Value;
                Vector3 gravity = ((CustomLogicVector3Builtin)parameters[4]).Value;
                float liveTime = (float)parameters[5];
                BaseCharacter character = ((CustomLogicCharacterBuiltin)parameters[6]).Character;
                object[] settings = null;
                if (projectileName == ProjectilePrefabs.Thunderspear)
                {
                    float radius = (float)parameters[7];
                    Color color = ((CustomLogicColorBuiltin)parameters[8]).Value.ToColor();
                    settings = new object[] { radius, color };
                }
                else if (projectileName == ProjectilePrefabs.Flare)
                {
                    Color color = ((CustomLogicColorBuiltin)parameters[7]).Value.ToColor();
                    settings = new object[] { color };
                }
                ProjectileSpawner.Spawn(projectileName, position, Quaternion.Euler(rotation), velocity, gravity, liveTime, character.photonView.ViewID, 
                    character.Team, settings);
                return null;
            }
            if (name == "SpawnEffect")
            {
                string effectName = (string)parameters[0];
                var field = typeof(EffectPrefabs).GetField(effectName);
                if (field == null)
                    return null;
                effectName = (string)field.GetValue(null);
                Vector3 position = ((CustomLogicVector3Builtin)parameters[1]).Value;
                Vector3 rotation = ((CustomLogicVector3Builtin)parameters[2]).Value;
                float scale = (float)parameters[3];
                object[] settings = null;
                if (effectName == EffectPrefabs.ThunderspearExplode)
                {
                    Color color = ((CustomLogicColorBuiltin)parameters[4]).Value.ToColor();
                    TSKillType killSound = TSKillType.Kill;
                    if (parameters.Count > 5)
                    {
                        string killSoundName = (string)(parameters[5]);
                        killSound = killSoundName switch
                        {
                            "Air" => TSKillType.Air,
                            "Ground" => TSKillType.Ground,
                            "ArmorHit" => TSKillType.ArmorHit,
                            "CloseShot" => TSKillType.CloseShot,
                            "MaxRangeShot" => TSKillType.MaxRangeShot,
                            _ => TSKillType.Kill
                        };
                    }
                    settings = new object[] { color, killSound };
                }
                EffectSpawner.Spawn(effectName, position, Quaternion.Euler(rotation), scale, true, settings);
                return null;
            }
            if (name == "SpawnPlayer")
            {
                var player = ((CustomLogicPlayerBuiltin)parameters[0]).Player;
                bool force = (bool)parameters[1];
                if (player == PhotonNetwork.LocalPlayer)
                    gameManager.SpawnPlayer(force);
                else if (PhotonNetwork.IsMasterClient)
                    RPCManager.PhotonView.RPC("SpawnPlayerRPC", player, new object[] { force });
                return null;
            }
            if (name == "SpawnPlayerAll")
            {
                bool force = (bool)parameters[0];
                if (PhotonNetwork.IsMasterClient)
                    RPCManager.PhotonView.RPC("SpawnPlayerRPC", RpcTarget.All, new object[] { force });
                return null;
            }
            if (name == "SpawnPlayerAt")
            {
                var player = ((CustomLogicPlayerBuiltin)parameters[0]).Player;
                bool force = (bool)parameters[1];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[2]).Value;
                float rotationY = parameters.Count > 3 ? parameters[3].UnboxToFloat() : 0f;
                if (player == PhotonNetwork.LocalPlayer)
                    gameManager.SpawnPlayerAt(force, position, rotationY);
                else if (PhotonNetwork.IsMasterClient)
                    RPCManager.PhotonView.RPC("SpawnPlayerAtRPC", player, force, position, rotationY);
                return null;
            }
            if (name == "SpawnPlayerAtAll")
            {
                bool force = (bool)parameters[0];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[1]).Value;
                float rotationY = parameters.Count > 2 ? parameters[2].UnboxToFloat() : 0f;
                if (PhotonNetwork.IsMasterClient)
                    RPCManager.PhotonView.RPC("SpawnPlayerAtRPC", RpcTarget.All, force, position, rotationY);
                return null;
            }
            if (name == "SetPlaylist")
            {
                string playlist = (string)parameters[0];
                MusicManager.SetPlaylist(playlist);
                CustomLogicManager.Evaluator.HasSetMusic = true;
                return null;
            }
            if (name == "SetSong")
            {
                string song = (string)parameters[0];
                MusicManager.SetSong(song);
                CustomLogicManager.Evaluator.HasSetMusic = true;
                return null;
            }
            if (name == "DrawRay")
            {
                Vector3 start = ((CustomLogicVector3Builtin)parameters[0]).Value;
                Vector3 dir = ((CustomLogicVector3Builtin)parameters[1]).Value;
                Color color = ((CustomLogicColorBuiltin)parameters[2]).Value.ToColor();
                float duration = parameters[3].UnboxToFloat();
                Debug.DrawRay(start, dir, color, duration);
                return null;
            }
            return base.CallMethod(name, parameters);
        }

        public override object GetField(string name)
        {
            var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
            if (name == "IsEnding")
                return gameManager.IsEnding;
            if (name == "EndTimeLeft")
                return gameManager.EndTimeLeft;
            if (name == "Titans")
            {
                if (NeedRefreshList(name, gameManager.Titans, includeAI: true, includeNonAI: true, isShifter: false))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var titan in gameManager.Titans)
                    {
                        if (titan != null && !titan.Dead)
                            list.List.Add(new CustomLogicTitanBuiltin(titan));
                    }
                    _cachedLists[name] = list;
                }
                return _cachedLists[name];
            }
            if (name == "AITitans")
            {
                if (NeedRefreshList(name, gameManager.Titans, includeAI: true, includeNonAI: false, isShifter: false))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var titan in gameManager.Titans)
                    {
                        if (titan != null && !titan.Dead && titan.AI)
                            list.List.Add(new CustomLogicTitanBuiltin(titan));
                    }
                    _cachedLists[name] = list;
                }
                return _cachedLists[name];
            }
            if (name == "PlayerTitans")
            {
                if (NeedRefreshList(name, gameManager.Titans, includeAI: false, includeNonAI: true, isShifter: false))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var titan in gameManager.Titans)
                    {
                        if (titan != null && !titan.Dead && !titan.AI)
                            list.List.Add(new CustomLogicTitanBuiltin(titan));
                    }
                    _cachedLists[name] = list;
                }
                return _cachedLists[name];
            }
            if (name == "Shifters")
            {
                if (NeedRefreshList(name, gameManager.Shifters, includeAI: true, includeNonAI: true, isShifter: true))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var shifter in gameManager.Shifters)
                    {
                        if (shifter != null && (!shifter.Dead || shifter.TransformingToHuman))
                            list.List.Add(new CustomLogicShifterBuiltin(shifter));
                    }
                    _cachedLists[name] = list;
                }
                return _cachedLists[name];
            }
            if (name == "AIShifters")
            {
                if (NeedRefreshList(name, gameManager.Shifters, includeAI: true, includeNonAI: false, isShifter: true))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var shifter in gameManager.Shifters)
                    {
                        if (shifter != null && shifter.AI && (!shifter.Dead || shifter.TransformingToHuman))
                            list.List.Add(new CustomLogicShifterBuiltin(shifter));
                    }
                    _cachedLists[name] = list;
                }
                return _cachedLists[name];
            }
            if (name == "PlayerShifters")
            {
                if (NeedRefreshList(name, gameManager.Shifters, includeAI: false, includeNonAI: true, isShifter: true))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var shifter in gameManager.Shifters)
                    {
                        if (shifter != null && !shifter.AI && (!shifter.Dead || shifter.TransformingToHuman))
                            list.List.Add(new CustomLogicShifterBuiltin(shifter));
                    }
                    _cachedLists[name] = list;
                }
                return _cachedLists[name];
            }
            if (name == "Humans")
            {
                if (NeedRefreshList(name, gameManager.Humans, includeAI: true, includeNonAI: true, isShifter: false))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var human in gameManager.Humans)
                    {
                        if (human != null && !human.Dead)
                            list.List.Add(new CustomLogicHumanBuiltin(human));
                    }
                    _cachedLists[name] = list;
                }
                return _cachedLists[name];
            }
            if (name == "AIHumans")
            {
                if (NeedRefreshList(name, gameManager.Humans, includeAI: true, includeNonAI: false, isShifter: false))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var human in gameManager.Humans)
                    {
                        if (human != null && !human.Dead && human.AI)
                            list.List.Add(new CustomLogicHumanBuiltin(human));
                    }
                    _cachedLists[name] = list;
                }
                return _cachedLists[name];
            }
            if (name == "PlayerHumans")
            {
                if (NeedRefreshList(name, gameManager.Humans, includeAI: false, includeNonAI: true, isShifter: false))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var human in gameManager.Humans)
                    {
                        if (human != null && !human.Dead && !human.AI)
                            list.List.Add(new CustomLogicHumanBuiltin(human));
                    }
                    _cachedLists[name] = list;
                }
                return _cachedLists[name];
            }
            if (name == "Loadouts")
            {
                var miscSettings = SettingsManager.InGameCurrent.Misc;
                List<string> loadouts = new List<string>();
                if (miscSettings.AllowBlades.Value)
                    loadouts.Add(HumanLoadout.Blades);
                if (miscSettings.AllowAHSS.Value)
                    loadouts.Add(HumanLoadout.AHSS);
                if (miscSettings.AllowAPG.Value)
                    loadouts.Add(HumanLoadout.APG);
                if (miscSettings.AllowThunderspears.Value)
                    loadouts.Add(HumanLoadout.Thunderspears);
                if (loadouts.Count == 0)
                    loadouts.Add(HumanLoadout.Blades);

                var result = new CustomLogicListBuiltin();
                result.List = loadouts.ConvertAll(x => (object)x);
                return result;
            }
            return base.GetField(name);
        }

        private bool NeedRefreshList<T>(string cacheKey, HashSet<T> currentSet, bool includeAI, bool includeNonAI, bool isShifter) where T: BaseCharacter
        {
            if (!_cachedLists.ContainsKey(cacheKey))
                return true;
            var cachedList = _cachedLists[cacheKey];
            if (cachedList.List.Count > currentSet.Count)
                return true;
            int index = 0;
            foreach (var character in currentSet)
            {
                if (isShifter)
                {
                    var shifter = character as BaseShifter;
                    if (shifter.Dead && !shifter.TransformingToHuman)
                        continue;
                }
                if (character != null && !character.Dead && ((includeAI && character.AI) || (includeNonAI && !character.AI)))
                {
                    if (index >= cachedList.List.Count || ((CustomLogicCharacterBuiltin)cachedList.List[index]).Character != character)
                        return true;
                    index += 1;
                }
            }
            return index != cachedList.List.Count;
        }

        public override void SetField(string name, object value)
        {
            base.SetField(name, value);
        }
    }
}
