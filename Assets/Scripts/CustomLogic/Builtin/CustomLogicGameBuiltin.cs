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
    [CLType(Abstract = true, Static = true, InheritBaseMembers = true)]
    class CustomLogicGameBuiltin: CustomLogicClassInstanceBuiltin
    {
        private string _lastSetTopLabel = string.Empty;
        private Dictionary<string, CustomLogicListBuiltin> _cachedLists = new Dictionary<string, CustomLogicListBuiltin>();

        public CustomLogicGameBuiltin(): base("Game")
        {
        }

        private InGameManager _inGameManager => (InGameManager)SceneLoader.CurrentGameManager;

        // Convert the setfield/getfield to CLProperties
        [CLProperty(description: "Is the game ending?")]
        public bool IsEnding => _inGameManager.IsEnding;

        [CLProperty(description: "Time left until the game ends")]
        public float EndTimeLeft => _inGameManager.EndTimeLeft;

        [CLProperty(description: "List of all titans")]
        public CustomLogicListBuiltin Titans
        {
            get
            {
                if (NeedRefreshList("Titans", _inGameManager.Titans, includeAI: true, includeNonAI: true, isShifter: false))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var titan in _inGameManager.Titans)
                    {
                        if (titan != null && !titan.Dead)
                            list.List.Add(new CustomLogicTitanBuiltin(titan));
                    }
                    _cachedLists["Titans"] = list;
                }
                return _cachedLists["Titans"];
            }
        }

        [CLProperty(description: "List of all AI titans")]
        public CustomLogicListBuiltin AITitans
        {
            get
            {
                if (NeedRefreshList("AITitans", _inGameManager.Titans, includeAI: true, includeNonAI: false, isShifter: false))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var titan in _inGameManager.Titans)
                    {
                        if (titan != null && !titan.Dead && titan.AI)
                            list.List.Add(new CustomLogicTitanBuiltin(titan));
                    }
                    _cachedLists["AITitans"] = list;
                }
                return _cachedLists["AITitans"];
            }
        }

        [CLProperty(description: "List of all player titans")]
        public CustomLogicListBuiltin PlayerTitans
        {
            get
            {
                if (NeedRefreshList("PlayerTitans", _inGameManager.Titans, includeAI: false, includeNonAI: true, isShifter: false))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var titan in _inGameManager.Titans)
                    {
                        if (titan != null && !titan.Dead && !titan.AI)
                            list.List.Add(new CustomLogicTitanBuiltin(titan));
                    }
                    _cachedLists["PlayerTitans"] = list;
                }
                return _cachedLists["PlayerTitans"];
            }
        }

        [CLProperty(description: "List of all shifters")]
        public CustomLogicListBuiltin Shifters
        {
            get
            {
                if (NeedRefreshList("Shifters", _inGameManager.Shifters, includeAI: true, includeNonAI: true, isShifter: true))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var shifter in _inGameManager.Shifters)
                    {
                        if (shifter != null && (!shifter.Dead || shifter.TransformingToHuman))
                            list.List.Add(new CustomLogicShifterBuiltin(shifter));
                    }
                    _cachedLists["Shifters"] = list;
                }
                return _cachedLists["Shifters"];
            }
        }

        [CLProperty(description: "List of all AI shifters")]
        public CustomLogicListBuiltin AIShifters
        {
            get
            {
                if (NeedRefreshList("AIShifters", _inGameManager.Shifters, includeAI: true, includeNonAI: false, isShifter: true))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var shifter in _inGameManager.Shifters)
                    {
                        if (shifter != null && shifter.AI && (!shifter.Dead || shifter.TransformingToHuman))
                            list.List.Add(new CustomLogicShifterBuiltin(shifter));
                    }
                    _cachedLists["AIShifters"] = list;
                }
                return _cachedLists["AIShifters"];
            }
        }

        [CLProperty(description: "List of all player shifters")]
        public CustomLogicListBuiltin PlayerShifters
        {
            get
            {
                if (NeedRefreshList("PlayerShifters", _inGameManager.Shifters, includeAI: false, includeNonAI: true, isShifter: true))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var shifter in _inGameManager.Shifters)
                    {
                        if (shifter != null && !shifter.AI && (!shifter.Dead || shifter.TransformingToHuman))
                            list.List.Add(new CustomLogicShifterBuiltin(shifter));
                    }
                    _cachedLists["PlayerShifters"] = list;
                }
                return _cachedLists["PlayerShifters"];
            }
        }

        [CLProperty(description: "List of all humans")]
        public CustomLogicListBuiltin Humans
        {
            get
            {
                if (NeedRefreshList("Humans", _inGameManager.Humans, includeAI: true, includeNonAI: true, isShifter: false))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var human in _inGameManager.Humans)
                    {
                        if (human != null && !human.Dead)
                            list.List.Add(new CustomLogicHumanBuiltin(human));
                    }
                    _cachedLists["Humans"] = list;
                }
                return _cachedLists["Humans"];
            }
        }

        [CLProperty(description: "List of all AI humans")]
        public CustomLogicListBuiltin AIHumans
        {
            get
            {
                if (NeedRefreshList("AIHumans", _inGameManager.Humans, includeAI: true, includeNonAI: false, isShifter: false))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var human in _inGameManager.Humans)
                    {
                        if (human != null && !human.Dead && human.AI)
                            list.List.Add(new CustomLogicHumanBuiltin(human));
                    }
                    _cachedLists["AIHumans"] = list;
                }
                return _cachedLists["AIHumans"];
            }
        }

        [CLProperty(description: "List of all player humans")]
        public CustomLogicListBuiltin PlayerHumans
        {
            get
            {
                if (NeedRefreshList("PlayerHumans", _inGameManager.Humans, includeAI: false, includeNonAI: true, isShifter: false))
                {
                    var list = new CustomLogicListBuiltin();
                    foreach (var human in _inGameManager.Humans)
                    {
                        if (human != null && !human.Dead && !human.AI)
                            list.List.Add(new CustomLogicHumanBuiltin(human));
                    }
                    _cachedLists["PlayerHumans"] = list;
                }
                return _cachedLists["PlayerHumans"];
            }
        }

        [CLProperty(description: "List of all loadouts")]
        public CustomLogicListBuiltin Loadouts
        {
            get
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
        }

        [CLProperty(description: "Is the kill score shown by default?")]
        public bool DefaultShowKillScore
        {
            get => CustomLogicManager.Evaluator.DefaultShowKillScore;
            set => CustomLogicManager.Evaluator.DefaultShowKillScore = value;
        }

        [CLProperty(description: "Is the kill feed shown by default?")]
        public bool DefaultHideKillScore
        {
            get => CustomLogicManager.Evaluator.DefaultShowKillFeed;
            set => CustomLogicManager.Evaluator.DefaultShowKillFeed = value;
        }

        [CLProperty(description: "Is the kill score added by default?")]
        public bool DefaultAddKillScore
        {
            get => CustomLogicManager.Evaluator.DefaultAddKillScore;
            set => CustomLogicManager.Evaluator.DefaultAddKillScore = value;
        }

        [CLProperty(description: "Is the loadout shown in the scoreboard?")]
        public bool ShowScoreboardLoadout
        {
            get => CustomLogicManager.Evaluator.ShowScoreboardLoadout;
            set => CustomLogicManager.Evaluator.ShowScoreboardLoadout = value;
        }

        [CLProperty(description: "Is the status shown in the scoreboard?")]
        public bool ShowScoreboardStatus
        {
            get => CustomLogicManager.Evaluator.ShowScoreboardStatus;
            set => CustomLogicManager.Evaluator.ShowScoreboardStatus = value;
        }

        [CLProperty(description: "Forced character type")]
        public string ForcedCharacterType
        {
            get => CustomLogicManager.Evaluator.ForcedCharacterType;
            set => CustomLogicManager.Evaluator.ForcedCharacterType = value;
        }

        [CLProperty(description: "Forced loadout")]
        public string ForcedLoadout
        {
            get => CustomLogicManager.Evaluator.ForcedLoadout;
            set => CustomLogicManager.Evaluator.ForcedLoadout = value;
        }

        // Add CLMethods
        [CLMethod(description: "Print a debug statement to the console")]
        public void Debug(string message)
        {
            DebugConsole.Log(message, true);
        }

        [CLMethod(description: "Print a message to the chat")]
        public void Print(string message)
        {
            ChatManager.AddLine(message, ChatTextColor.System);
        }

        [CLMethod(description: "Print a message to all players")]
        public void PrintAll(string message)
        {
            ChatManager.SendChatAll(message, ChatTextColor.System);
        }

        [CLMethod(description: "Get a general setting")]
        public object GetGeneralSetting(string settingName)
        {
            var setting = SettingsManager.InGameCurrent.General.TypedSettings[settingName];
            return setting.GetType().GetProperty("Value").GetValue(setting);
        }

        [CLMethod(description: "Get a titan setting")]
        public object GetTitanSetting(string settingName)
        {
            var setting = SettingsManager.InGameCurrent.Titan.TypedSettings[settingName];
            return setting.GetType().GetProperty("Value").GetValue(setting);
        }

        [CLMethod(description: "Get a misc setting")]
        public object GetMiscSetting(string settingName)
        {
            var setting = SettingsManager.InGameCurrent.Misc.TypedSettings[settingName];
            return setting.GetType().GetProperty("Value").GetValue(setting);
        }

        [CLMethod(description: "End the game")]
        public void End(float delay)
        {
            if (PhotonNetwork.IsMasterClient)
                RPCManager.PhotonView.RPC("EndGameRPC", RpcTarget.All, new object[] { delay });
        }

        [CLMethod(description: "Find a character by view ID")]
        public CustomLogicCharacterBuiltin FindCharacterByViewID(int viewID)
        {
            var character = Util.FindCharacterByViewId(viewID);
            if (character == null || character.Dead)
                return null;
            return CustomLogicEvaluator.GetCharacterBuiltin(character);
        }

        [CLMethod(description: "Spawn a titan")]
        public CustomLogicTitanBuiltin SpawnTitan(string type)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var titan = new CustomLogicTitanBuiltin(_inGameManager.SpawnAITitan(type));
                return titan;
            }
            return null;
        }

        [CLMethod(description: "Spawn a titan at a position")]
        public CustomLogicTitanBuiltin SpawnTitanAt(string type, Vector3 position, float rotationY = 0f)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var titan = new CustomLogicTitanBuiltin(_inGameManager.SpawnAITitanAt(type, position, rotationY));
                return titan;
            }
            return null;
        }

        [CLMethod(description: "Spawn titans")]
        public CustomLogicListBuiltin SpawnTitans(string type, int count)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var list = new CustomLogicListBuiltin();
                foreach (var titan in _inGameManager.SpawnAITitans(type, count))
                    list.List.Add(new CustomLogicTitanBuiltin(titan));
                return list;
            }
            return null;
        }

        [CLMethod(description: "Spawn titans asynchronously")]
        public void SpawnTitansAsync(string type, int count)
        {
            if (PhotonNetwork.IsMasterClient)
                _inGameManager.SpawnAITitansAsync(type, count);
        }

        [CLMethod(description: "Spawn titans at a position")]
        public CustomLogicListBuiltin SpawnTitansAt(string type, int count, Vector3 position, float rotationY = 0f)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var list = new CustomLogicListBuiltin();
                for (int i = 0; i < count; i++)
                {
                    var titan = new CustomLogicTitanBuiltin(_inGameManager.SpawnAITitanAt(type, position, rotationY));
                    list.List.Add(titan);
                }
                return list;
            }
            return null;
        }

        [CLMethod(description: "Spawn titans at a position asynchronously")]
        public void SpawnTitansAtAsync(string type, int count, Vector3 position, float rotationY = 0f)
        {
            if (PhotonNetwork.IsMasterClient)
                _inGameManager.SpawnAITitansAtAsync(type, count, position, rotationY);
        }

        [CLMethod(description: "Spawn a shifter")]
        public CustomLogicShifterBuiltin SpawnShifter(string type)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var shifter = new CustomLogicShifterBuiltin(_inGameManager.SpawnAIShifter(type));
                return shifter;
            }
            return null;
        }

        [CLMethod(description: "Spawn a shifter at a position")]
        public CustomLogicShifterBuiltin SpawnShifterAt(string type, Vector3 position, float rotationY = 0f)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var shifter = new CustomLogicShifterBuiltin(_inGameManager.SpawnAIShifterAt(type, position, rotationY));
                return shifter;
            }
            return null;
        }

        [CLMethod(description: "Spawn a projectile")]
        public void SpawnProjectile(string projectileName, Vector3 position, Vector3 rotation, Vector3 velocity, Vector3 gravity, float liveTime, string team, object[] settings = null)
        {
            // TODO: Modify this to work with Kwargs (TS and Flare require optional params)
            ProjectileSpawner.Spawn(projectileName, position, Quaternion.Euler(rotation), velocity, gravity, liveTime, -1, team, settings);
        }

        [CLMethod(description: "Spawn a projectile with an owner")]
        public void SpawnProjectileWithOwner(string projectileName, Vector3 position, Vector3 rotation, Vector3 velocity, Vector3 gravity, float liveTime, CustomLogicCharacterBuiltin character, object[] settings = null)
        {
            // TODO: Modify this to work with Kwargs (TS and Flare require optional params)
            ProjectileSpawner.Spawn(projectileName, position, Quaternion.Euler(rotation), velocity, gravity, liveTime, character.Character.photonView.ViewID, character.Character.Team, settings);
        }

        [CLMethod(description: "Spawn an effect")]
        public void SpawnEffect(string effectName, Vector3 position, Vector3 rotation, float scale, object[] settings = null)
        // TODO: Modify this to work with Kwargs (TS and Flare require optional params Color And Sound)
        {
            EffectSpawner.Spawn(effectName, position, Quaternion.Euler(rotation), scale, true, settings);
        }

        [CLMethod(description: "Spawn a player")]
        public void SpawnPlayer(CustomLogicPlayerBuiltin player, bool force)
        {
            if (player.Player == PhotonNetwork.LocalPlayer)
                _inGameManager.SpawnPlayer(force);
            else if (PhotonNetwork.IsMasterClient)
                RPCManager.PhotonView.RPC("SpawnPlayerRPC", player.Player, new object[] { force });
        }

        [CLMethod(description: "Spawn a player for all players")]
        public void SpawnPlayerAll(bool force)
        {
            if (PhotonNetwork.IsMasterClient)
                RPCManager.PhotonView.RPC("SpawnPlayerRPC", RpcTarget.All, new object[] { force });
        }

        [CLMethod(description: "Spawn a player at a position")]
        public void SpawnPlayerAt(CustomLogicPlayerBuiltin player, bool force, Vector3 position, float rotationY = 0f)
        {
            if (player.Player == PhotonNetwork.LocalPlayer)
                _inGameManager.SpawnPlayerAt(force, position, rotationY);
            else if (PhotonNetwork.IsMasterClient)
                RPCManager.PhotonView.RPC("SpawnPlayerAtRPC", player.Player, new object[] { force, position, rotationY });
        }

        [CLMethod(description: "Spawn a player at a position for all players")]
        public void SpawnPlayerAtAll(bool force, Vector3 position, float rotationY = 0f)
        {
            if (PhotonNetwork.IsMasterClient)
                RPCManager.PhotonView.RPC("SpawnPlayerAtRPC", RpcTarget.All, new object[] { force, position, rotationY });
        }

        [CLMethod(description: "Set the music playlist")]
        public void SetPlaylist(string playlist)
        {
            MusicManager.SetPlaylist(playlist);
            CustomLogicManager.Evaluator.HasSetMusic = true;
        }

        [CLMethod(description: "Set the music song")]
        public void SetSong(string song)
        {
            MusicManager.SetSong(song);
            CustomLogicManager.Evaluator.HasSetMusic = true;
        }

        [CLMethod(description: "Draw a ray")]
        public void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
        {
            UnityEngine.Debug.DrawRay(start, dir, color, duration);
        }

        [CLMethod(description: "Show the kill score")]
        public void ShowKillScore(int damage)
        {
            ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage, true);
        }

        [CLMethod(description: "Show the kill feed")]
        public void ShowKillFeed(string killer, string victim, int score, string weapon)
        {
            ((InGameMenu)UIManager.CurrentMenu).ShowKillFeed(killer, victim, score, weapon);
        }

        [CLMethod(description: "Show the kill feed for all players")]
        public void ShowKillFeedAll(string killer, string victim, int score, string weapon)
        {
            RPCManager.PhotonView.RPC("ShowKillFeedRPC", RpcTarget.All, new object[] { killer, victim, score, weapon });
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
    }
}
