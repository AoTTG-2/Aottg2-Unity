using ApplicationManagers;
using Characters;
using Effects;
using GameManagers;
using Photon.Pun;
using Projectiles;
using Settings;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    /// <summary>
    /// Game functions such as spawning titans and managing game state.
    /// </summary>
    [CLType(Name = "Game", Abstract = true, Static = true)]
    partial class CustomLogicGameBuiltin : BuiltinClassInstance
    {
        private string _lastSetTopLabel = string.Empty;
        private Dictionary<string, CustomLogicListBuiltin> _cachedLists = new Dictionary<string, CustomLogicListBuiltin>();

        [CLConstructor]
        public CustomLogicGameBuiltin(){}

        private InGameManager _inGameManager => (InGameManager)SceneLoader.CurrentGameManager;

        // Convert the setfield/getfield to CLProperties
        /// <summary>
        /// Is the game ending?
        /// </summary>
        [CLProperty(Static = true)]
        public bool IsEnding => _inGameManager.IsEnding;

        /// <summary>
        /// Time left until the game ends.
        /// </summary>
        [CLProperty(Static = true)]
        public float EndTimeLeft => _inGameManager.EndTimeLeft;

        /// <summary>
        /// List of all titans.
        /// </summary>
        [CLProperty(Static = true, TypeArguments = new[] { "Titan" })]
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

        /// <summary>
        /// List of all AI titans.
        /// </summary>
        [CLProperty(Static = true, TypeArguments = new[] { "Titan" })]
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

        /// <summary>
        /// List of all player titans.
        /// </summary>
        [CLProperty(Static = true, TypeArguments = new[] { "Titan" })]
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

        /// <summary>
        /// List of all shifters.
        /// </summary>
        [CLProperty(Static = true, TypeArguments = new[] { "Shifter" })]
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
                        {
                            if (shifter is WallColossalShifter)
                                list.List.Add(new CustomLogicWallColossalBuiltin((WallColossalShifter)shifter));
                            else
                                list.List.Add(new CustomLogicShifterBuiltin(shifter));
                        }
                    }
                    _cachedLists["Shifters"] = list;
                }
                return _cachedLists["Shifters"];
            }
        }

        /// <summary>
        /// List of all AI shifters.
        /// </summary>
        [CLProperty(Static = true, TypeArguments = new[] { "Shifter" })]
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
                        {
                            if (shifter is WallColossalShifter)
                                list.List.Add(new CustomLogicWallColossalBuiltin((WallColossalShifter)shifter));
                            else
                                list.List.Add(new CustomLogicShifterBuiltin(shifter));
                        }
                    }
                    _cachedLists["AIShifters"] = list;
                }
                return _cachedLists["AIShifters"];
            }
        }

        /// <summary>
        /// List of all player shifters.
        /// </summary>
        [CLProperty(Static = true, TypeArguments = new[] { "Shifter" })]
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
                        {
                            if (shifter is WallColossalShifter)
                                list.List.Add(new CustomLogicWallColossalBuiltin((WallColossalShifter)shifter));
                            else
                                list.List.Add(new CustomLogicShifterBuiltin(shifter));
                        }
                    }
                    _cachedLists["PlayerShifters"] = list;
                }
                return _cachedLists["PlayerShifters"];
            }
        }

        /// <summary>
        /// List of all humans.
        /// </summary>
        [CLProperty(Static = true, TypeArguments = new[] { "Human" })]
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

        /// <summary>
        /// List of all AI humans.
        /// </summary>
        [CLProperty(Static = true, TypeArguments = new[] { "Human" })]
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

        /// <summary>
        /// List of all player humans.
        /// </summary>
        [CLProperty(Static = true, TypeArguments = new[] { "Human" })]
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

        /// <summary>
        /// List of all loadouts.
        /// </summary>
        [CLProperty(Static = true, TypeArguments = new[] { "string" })]
        public CustomLogicListBuiltin Loadouts
        {
            get
            {
                var miscSettings = SettingsManager.InGameCurrent.Misc;
                List<string> loadouts = new List<string>();
                if (miscSettings.AllowBlades.Value)
                    loadouts.Add(HumanLoadout.Blade);
                if (miscSettings.AllowAHSS.Value)
                    loadouts.Add(HumanLoadout.AHSS);
                if (miscSettings.AllowAPG.Value)
                    loadouts.Add(HumanLoadout.APG);
                if (miscSettings.AllowThunderspears.Value)
                    loadouts.Add(HumanLoadout.Thunderspear);
                if (loadouts.Count == 0)
                    loadouts.Add(HumanLoadout.Blade);

                var result = new CustomLogicListBuiltin();
                result.List = loadouts.ConvertAll(x => (object)x);
                return result;
            }
        }

        /// <summary>
        /// Is the kill score shown by default?
        /// </summary>
        [CLProperty(Static = true)]
        public bool DefaultShowKillScore
        {
            get => CustomLogicManager.Evaluator.DefaultShowKillScore;
            set => CustomLogicManager.Evaluator.DefaultShowKillScore = value;
        }

        /// <summary>
        /// Is the kill feed shown by default?
        /// </summary>
        [CLProperty(Static = true)]
        public bool DefaultHideKillScore
        {
            get => CustomLogicManager.Evaluator.DefaultShowKillFeed;
            set => CustomLogicManager.Evaluator.DefaultShowKillFeed = value;
        }

        /// <summary>
        /// Is the kill score added by default?
        /// </summary>
        [CLProperty(Static = true)]
        public bool DefaultAddKillScore
        {
            get => CustomLogicManager.Evaluator.DefaultAddKillScore;
            set => CustomLogicManager.Evaluator.DefaultAddKillScore = value;
        }

        /// <summary>
        /// Is the loadout shown in the scoreboard?
        /// </summary>
        [CLProperty(Static = true)]
        public bool ShowScoreboardLoadout
        {
            get => CustomLogicManager.Evaluator.ShowScoreboardLoadout;
            set => CustomLogicManager.Evaluator.ShowScoreboardLoadout = value;
        }

        /// <summary>
        /// Is the status shown in the scoreboard?
        /// </summary>
        [CLProperty(Static = true)]
        public bool ShowScoreboardStatus
        {
            get => CustomLogicManager.Evaluator.ShowScoreboardStatus;
            set => CustomLogicManager.Evaluator.ShowScoreboardStatus = value;
        }

        /// <summary>
        /// Forced character type.
        /// </summary>
        [CLProperty(Static = true, Enum = typeof(CustomLogicCharacterTypeEnum))]
        public string ForcedCharacterType
        {
            get => CustomLogicManager.Evaluator.ForcedCharacterType;
            set => CustomLogicManager.Evaluator.ForcedCharacterType = value;
        }

        /// <summary>
        /// Forced loadout.
        /// </summary>
        [CLProperty(Static = true, Enum = typeof(CustomLogicLoadoutEnum))]
        public string ForcedLoadout
        {
            get => CustomLogicManager.Evaluator.ForcedLoadout;
            set => CustomLogicManager.Evaluator.ForcedLoadout = value;
        }

        // Add CLMethods
        /// <summary>
        /// Print a debug statement to the console.
        /// </summary>
        /// <param name="message">The message to print.</param>
        [CLMethod(Static = true)]
        public void Debug(object message)
        {
            if (message == null)
                message = "null";
            DebugConsole.LogCustomLogic(message.ToString(), SettingsManager.UISettings.ChatCLErrors.Value);
        }

        /// <summary>
        /// Print a message to the chat.
        /// </summary>
        /// <param name="message">The message to print.</param>
        [CLMethod(Static = true)]
        public void Print(object message)
        {
            if (message == null)
                message = "null";
            ChatManager.AddLine(message.ToString(), ChatTextColor.System);
        }

        /// <summary>
        /// Print a message to all players.
        /// </summary>
        /// <param name="message">The message to print.</param>
        [CLMethod(Static = true)]
        public void PrintAll(object message)
        {
            ChatManager.SendChatAll(message.ToString(), ChatTextColor.System);
        }

        /// <summary>
        /// Get a general setting.
        /// </summary>
        /// <param name="settingName">The name of the setting to get.</param>
        /// <returns>The setting value.</returns>
        [CLMethod(Static = true)]
        public object GetGeneralSetting(string settingName)
        {
            var setting = SettingsManager.InGameCurrent.General.TypedSettings[settingName];
            return setting.GetType().GetProperty("Value").GetValue(setting);
        }

        /// <summary>
        /// Get a titan setting.
        /// </summary>
        /// <param name="settingName">The name of the setting to get.</param>
        /// <returns>The setting value.</returns>
        [CLMethod(Static = true)]
        public object GetTitanSetting(string settingName)
        {
            var setting = SettingsManager.InGameCurrent.Titan.TypedSettings[settingName];
            return setting.GetType().GetProperty("Value").GetValue(setting);
        }

        /// <summary>
        /// Get a misc setting.
        /// </summary>
        /// <param name="settingName">The name of the setting to get.</param>
        /// <returns>The setting value.</returns>
        [CLMethod(Static = true)]
        public object GetMiscSetting(string settingName)
        {
            var setting = SettingsManager.InGameCurrent.Misc.TypedSettings[settingName];
            return setting.GetType().GetProperty("Value").GetValue(setting);
        }

        /// <summary>
        /// End the game.
        /// </summary>
        /// <param name="delay">The delay in seconds before ending the game.</param>
        [CLMethod(Static = true)]
        public void End(float delay)
        {
            if (PhotonNetwork.IsMasterClient)
                RPCManager.PhotonView.RPC(nameof(RPCManager.EndGameRPC), RpcTarget.All, new object[] { delay });
        }

        /// <summary>
        /// Find a character by view ID.
        /// </summary>
        /// <param name="viewID">The Photon view ID of the character.</param>
        /// <returns>The character if found, null otherwise.</returns>
        [CLMethod(Static = true)]
        public CustomLogicCharacterBuiltin FindCharacterByViewID(int viewID)
        {
            var character = Util.FindCharacterByViewId(viewID);
            if (character == null || character.Dead)
                return null;
            return CustomLogicEvaluator.GetCharacterBuiltin(character);
        }

        /// <summary>
        /// Spawn a titan.
        /// </summary>
        /// <param name="type">The type of titan to spawn.</param>
        /// <returns>The spawned titan, or null if not master client.</returns>
        [CLMethod(Static = true)]
        public CustomLogicTitanBuiltin SpawnTitan([CLParam(Enum = typeof(CustomLogicTitanTypeEnum))] string type)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var titan = new CustomLogicTitanBuiltin(_inGameManager.SpawnAITitan(type));
                return titan;
            }
            return null;
        }

        /// <summary>
        /// Spawn a titan at a position.
        /// </summary>
        /// <param name="type">The type of titan to spawn.</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotationY">The Y rotation in degrees (default: 0).</param>
        /// <returns>The spawned titan, or null if not master client.</returns>
        [CLMethod(Static = true)]
        public CustomLogicTitanBuiltin SpawnTitanAt(
            [CLParam(Enum = typeof(CustomLogicTitanTypeEnum))] string type,
            CustomLogicVector3Builtin position,
            float rotationY = 0f)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var titan = new CustomLogicTitanBuiltin(_inGameManager.SpawnAITitanAt(type, position.Value, rotationY));
                return titan;
            }
            return null;
        }

        /// <summary>
        /// Spawn titans.
        /// </summary>
        /// <param name="type">The type of titan to spawn.</param>
        /// <param name="count">The number of titans to spawn.</param>
        /// <returns>A list of spawned titans, or null if not master client.</returns>
        [CLMethod(Static = true, ReturnTypeArguments = new[] { "Titan" })]
        public CustomLogicListBuiltin SpawnTitans(
            [CLParam(Enum = typeof(CustomLogicTitanTypeEnum))] string type,
            int count)
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

        /// <summary>
        /// Spawn titans asynchronously.
        /// </summary>
        /// <param name="type">The type of titan to spawn.</param>
        /// <param name="count">The number of titans to spawn.</param>
        [CLMethod(Static = true)]
        public void SpawnTitansAsync(
            [CLParam(Enum = typeof(CustomLogicTitanTypeEnum))] string type,
            int count)
        {
            if (PhotonNetwork.IsMasterClient)
                _inGameManager.SpawnAITitansAsync(type, count);
        }

        /// <summary>
        /// Spawn titans at a position.
        /// </summary>
        /// <param name="type">The type of titan to spawn.</param>
        /// <param name="count">The number of titans to spawn.</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotationY">The Y rotation in degrees (default: 0).</param>
        /// <returns>A list of spawned titans, or null if not master client.</returns>
        [CLMethod(Static = true, ReturnTypeArguments = new[] { "Titan" })]
        public CustomLogicListBuiltin SpawnTitansAt(
            [CLParam(Enum = typeof(CustomLogicTitanTypeEnum))] string type,
            int count,
            CustomLogicVector3Builtin position,
            float rotationY = 0f)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var list = new CustomLogicListBuiltin();
                for (int i = 0; i < count; i++)
                {
                    var titan = new CustomLogicTitanBuiltin(_inGameManager.SpawnAITitanAt(type, position.Value, rotationY));
                    list.List.Add(titan);
                }
                return list;
            }
            return null;
        }

        /// <summary>
        /// Spawn titans at a position asynchronously.
        /// </summary>
        /// <param name="type">The type of titan to spawn.</param>
        /// <param name="count">The number of titans to spawn.</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotationY">The Y rotation in degrees (default: 0).</param>
        [CLMethod(Static = true)]
        public void SpawnTitansAtAsync(
            [CLParam(Enum = typeof(CustomLogicTitanTypeEnum))] string type,
            int count,
            CustomLogicVector3Builtin position,
            float rotationY = 0f)
        {
            if (PhotonNetwork.IsMasterClient)
                _inGameManager.SpawnAITitansAtAsync(type, count, position.Value, rotationY);
        }

        /// <summary>
        /// Spawn a shifter.
        /// </summary>
        /// <param name="type">The type of shifter to spawn.</param>
        /// <returns>The spawned shifter, or null if not master client.</returns>
        [CLMethod(Static = true)]
        public CustomLogicShifterBuiltin SpawnShifter([CLParam(Enum = typeof(CustomLogicShifterTypeEnum))] string type)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var shifter = _inGameManager.SpawnAIShifter(type);
                if (type == "WallColossal")
                    return new CustomLogicWallColossalBuiltin((WallColossalShifter)shifter);
                return new CustomLogicShifterBuiltin(shifter);
            }
            return null;
        }

        /// <summary>
        /// Spawn a shifter at a position.
        /// </summary>
        /// <param name="type">The type of shifter to spawn.</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotationY">The Y rotation in degrees (default: 0).</param>
        /// <returns>The spawned shifter, or null if not master client.</returns>
        [CLMethod(Static = true)]
        public CustomLogicShifterBuiltin SpawnShifterAt(
            [CLParam(Enum = typeof(CustomLogicShifterTypeEnum))] string type,
            CustomLogicVector3Builtin position,
            float rotationY = 0f)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var shifter = _inGameManager.SpawnAIShifterAt(type, position.Value, rotationY);
                if (type == "WallColossal")
                    return new CustomLogicWallColossalBuiltin((WallColossalShifter)shifter);
                return new CustomLogicShifterBuiltin(shifter);
            }
            return null;
        }

        /// <summary>
        /// Spawn a projectile. Note: `extraParam` and `extraParam2` are optional.
        /// They may or may not be used depending on the value of `projectileName`.
        /// </summary>
        /// <param name="projectileName">Name of the projectile.</param>
        /// <param name="position">Spawn position.</param>
        /// <param name="rotation">Spawn rotation.</param>
        /// <param name="velocity">Spawn velocity.</param>
        /// <param name="gravity">Spawn gravity.</param>
        /// <param name="liveTime">Live time of the projectile.</param>
        /// <param name="team">The team that the projectile belongs to.</param>
        /// <param name="extraParam">Optional. Type depends on projectile: Thunderspear: float (explosion radius), Flare: Color (flare color), Rock1: float (rock size), Others: unused.</param>
        /// <param name="extraParam2">Optional. Type depends on projectile: Thunderspear: Color (projectile color), Others: unused.</param>
        [CLMethod(Static = true)]
        public void SpawnProjectile(
            [CLParam(Enum = typeof(CustomLogicProjectileNameEnum))] string projectileName,
            CustomLogicVector3Builtin position,
            CustomLogicVector3Builtin rotation,
            CustomLogicVector3Builtin velocity,
            CustomLogicVector3Builtin gravity,
            float liveTime,
            [CLParam(Enum = typeof(CustomLogicTeamEnum))] string team,
            object extraParam = null,
            object extraParam2 = null)
        {
            object[] settings = null;
            if (projectileName == ProjectilePrefabs.Thunderspear)
            {
                float radius = CustomLogicEvaluator.ConvertTo<float>(extraParam);
                Color color = CustomLogicEvaluator.ConvertTo<CustomLogicColorBuiltin>(extraParam2).Value.ToColor();
                settings = new object[] { radius, color, false };
            }
            else if (projectileName == ProjectilePrefabs.Flare)
            {
                Color color = CustomLogicEvaluator.ConvertTo<CustomLogicColorBuiltin>(extraParam).Value.ToColor();
                settings = new object[] { color };
            }
            else if (projectileName == ProjectilePrefabs.Rock1 || projectileName == ProjectilePrefabs.Rock2)
            {
                float size = CustomLogicEvaluator.ConvertTo<float>(extraParam);
                settings = new object[] { size };
            }
            ProjectileSpawner.Spawn(projectileName, position, Quaternion.Euler(rotation), velocity, gravity, liveTime, -1, team, settings);
        }

        /// <summary>
        /// Spawn a projectile with an owner. Note: `extraParam` and `extraParam2` are optional.
        /// They may or may not be used depending on the value of `projectileName`.
        /// </summary>
        /// <param name="projectileName">Name of the projectile.</param>
        /// <param name="position">Spawn position.</param>
        /// <param name="rotation">Spawn rotation.</param>
        /// <param name="velocity">Spawn velocity.</param>
        /// <param name="gravity">Spawn gravity.</param>
        /// <param name="liveTime">Live time of the projectile.</param>
        /// <param name="owner">The character that the projectile belongs to.</param>
        /// <param name="extraParam">Optional. Type depends on projectile: Thunderspear: float (explosion radius), Flare: Color (flare color), Rock1: float (rock size), Others: unused.</param>
        /// <param name="extraParam2">Optional. Type depends on projectile: Thunderspear: Color (projectile color), Others: unused.</param>
        [CLMethod(Static = true)]
        public void SpawnProjectileWithOwner(
            [CLParam(Enum = typeof(CustomLogicProjectileNameEnum))] string projectileName,
            CustomLogicVector3Builtin position,
            CustomLogicVector3Builtin rotation,
            CustomLogicVector3Builtin velocity,
            CustomLogicVector3Builtin gravity,
            float liveTime,
            CustomLogicCharacterBuiltin owner,
            object extraParam = null,
            object extraParam2 = null)
        {
            BaseCharacter character = owner.Character;
            object[] settings = null;
            if (projectileName == ProjectilePrefabs.Thunderspear)
            {
                float radius = CustomLogicEvaluator.ConvertTo<float>(extraParam);
                Color color = CustomLogicEvaluator.ConvertTo<CustomLogicColorBuiltin>(extraParam2).Value.ToColor();
                settings = new object[] { radius, color, false };
            }
            else if (projectileName == ProjectilePrefabs.Flare)
            {
                Color color = CustomLogicEvaluator.ConvertTo<CustomLogicColorBuiltin>(extraParam).Value.ToColor();
                settings = new object[] { color };
            }
            else if (projectileName == ProjectilePrefabs.Rock1 || projectileName == ProjectilePrefabs.Rock2)
            {
                float size = CustomLogicEvaluator.ConvertTo<float>(extraParam);
                settings = new object[] { size };
            }
            ProjectileSpawner.Spawn(projectileName, position, Quaternion.Euler(rotation), velocity, gravity, liveTime, character.photonView.ViewID,
                character.Team, settings);
        }

        /// <summary>
        /// Spawn an effect.
        /// </summary>
        /// <param name="effectName">Name of the effect.</param>
        /// <param name="position">Spawn position.</param>
        /// <param name="rotation">Spawn rotation.</param>
        /// <param name="scale">Spawn scale.</param>
        /// <param name="tsExplodeColor">Thunderspear explode color (Only valid when effectName is "ThunderspearExplode").</param>
        /// <param name="tsKillSound">Optional. Thunderspear explode sound (Only valid when effectName is "ThunderspearExplode").</param>
        [CLMethod(Static = true)]
        public void SpawnEffect(
            [CLParam(Enum = typeof(CustomLogicEffectNameEnum))] string effectName,
            CustomLogicVector3Builtin position,
            CustomLogicVector3Builtin rotation,
            float scale,
            CustomLogicColorBuiltin tsExplodeColor = null,
            [CLParam(Enum = typeof(CustomLogicTSKillSoundEnum))] string tsKillSound = null)
        {
            var field = typeof(EffectPrefabs).GetField(effectName);
            if (field != null)
                effectName = (string)field.GetValue(null);
            
            object[] settings = null;
            if (effectName == EffectPrefabs.ThunderspearExplode)
            {
                Color color = tsExplodeColor.Value.ToColor();
                TSKillType killSound = TSKillType.Kill;
                if (tsKillSound != null)
                {
                    killSound = tsKillSound switch
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
        }

        /// <summary>
        /// Spawn an unscaled effect.
        /// </summary>
        /// <param name="effectName">Name of the effect.</param>
        /// <param name="position">Spawn position.</param>
        /// <param name="rotation">Spawn rotation.</param>
        /// <param name="tsExplodeColor">Thunderspear explode color (Only valid when effectName is "ThunderspearExplode").</param>
        /// <param name="tsKillSound">Optional. Thunderspear explode sound (Only valid when effectName is "ThunderspearExplode").</param>
        [CLMethod(Static = true)]
        public void SpawnUnscaledEffect(
            [CLParam(Enum = typeof(CustomLogicEffectNameEnum))] string effectName,
            CustomLogicVector3Builtin position,
            CustomLogicVector3Builtin rotation,
            CustomLogicColorBuiltin tsExplodeColor = null,
            [CLParam(Enum = typeof(CustomLogicTSKillSoundEnum))] string tsKillSound = null)
        {
            var field = typeof(EffectPrefabs).GetField(effectName);
            if (field != null)
                effectName = (string)field.GetValue(null);
            
            object[] settings = null;
            if (effectName == EffectPrefabs.ThunderspearExplode)
            {
                Color color = tsExplodeColor.Value.ToColor();
                TSKillType killSound = TSKillType.Kill;
                if (tsKillSound != null)
                {
                    killSound = tsKillSound switch
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
            EffectSpawner.Spawn(effectName, position, Quaternion.Euler(rotation), 1, false, settings);
        }

        /// <summary>
        /// Spawn a player.
        /// </summary>
        /// <param name="player">The player to spawn.</param>
        /// <param name="force">If true, forces respawn even if the player is already alive.</param>
        [CLMethod(Static = true)]
        public void SpawnPlayer(CustomLogicPlayerBuiltin player, bool force)
        {
            if (player.Player == PhotonNetwork.LocalPlayer)
                _inGameManager.SpawnPlayer(force);
            else if (PhotonNetwork.IsMasterClient)
                RPCManager.PhotonView.RPC(nameof(RPCManager.SpawnPlayerRPC), player.Player, new object[] { force });
        }

        /// <summary>
        /// Spawn a player for all players.
        /// </summary>
        /// <param name="force">If true, forces respawn even if players are already alive.</param>
        [CLMethod(Static = true)]
        public void SpawnPlayerAll(bool force)
        {
            if (PhotonNetwork.IsMasterClient)
                RPCManager.PhotonView.RPC(nameof(RPCManager.SpawnPlayerRPC), RpcTarget.All, new object[] { force });
        }

        /// <summary>
        /// Spawn a player at a position.
        /// </summary>
        /// <param name="player">The player to spawn.</param>
        /// <param name="force">If true, forces respawn even if the player is already alive.</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotationY">The Y rotation in degrees (default: 0).</param>
        [CLMethod(Static = true)]
        public void SpawnPlayerAt(CustomLogicPlayerBuiltin player, bool force, CustomLogicVector3Builtin position, float rotationY = 0f)
        {
            if (player.Player == PhotonNetwork.LocalPlayer)
                _inGameManager.SpawnPlayerAt(force, position.Value, rotationY);
            else if (PhotonNetwork.IsMasterClient)
                RPCManager.PhotonView.RPC(nameof(RPCManager.SpawnPlayerAtRPC), player.Player, new object[] { force, position.Value, rotationY });
        }

        /// <summary>
        /// Spawn a player at a position for all players.
        /// </summary>
        /// <param name="force">If true, forces respawn even if players are already alive.</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotationY">The Y rotation in degrees (default: 0).</param>
        [CLMethod(Static = true)]
        public void SpawnPlayerAtAll(bool force, CustomLogicVector3Builtin position, float rotationY = 0f)
        {
            if (PhotonNetwork.IsMasterClient)
                RPCManager.PhotonView.RPC(nameof(RPCManager.SpawnPlayerAtRPC), RpcTarget.All, new object[] { force, position.Value, rotationY });
        }

        /// <summary>
        /// Set the music playlist.
        /// </summary>
        /// <param name="playlist">The name of the playlist to set.</param>
        [CLMethod(Static = true)]
        public void SetPlaylist(string playlist)
        {
            MusicManager.SetPlaylist(playlist);
            CustomLogicManager.Evaluator.HasSetMusic = true;
        }

        /// <summary>
        /// Set the music song.
        /// </summary>
        /// <param name="song">The name of the song to set.</param>
        [CLMethod(Static = true)]
        public void SetSong(string song)
        {
            MusicManager.SetSong(song);
            CustomLogicManager.Evaluator.HasSetMusic = true;
        }

        /// <summary>
        /// Draw a ray.
        /// </summary>
        /// <param name="start">The start position of the ray.</param>
        /// <param name="dir">The direction vector of the ray.</param>
        /// <param name="color">The color of the ray.</param>
        /// <param name="duration">The duration in seconds to display the ray.</param>
        [CLMethod(Static = true)]
        public void DrawRay(CustomLogicVector3Builtin start, CustomLogicVector3Builtin dir, CustomLogicColorBuiltin color, float duration)
        {
            UnityEngine.Debug.DrawRay(start.Value, dir.Value, color.Value.ToColor(), duration);
        }

        /// <summary>
        /// Show the kill score.
        /// </summary>
        /// <param name="damage">The damage value to display.</param>
        [CLMethod(Static = true)]
        public void ShowKillScore(int damage)
        {
            ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage, true);
        }

        /// <summary>
        /// Show the kill feed.
        /// </summary>
        /// <param name="killer">The name of the killer.</param>
        /// <param name="victim">The name of the victim.</param>
        /// <param name="score">The score value.</param>
        /// <param name="weapon">The weapon name.</param>
        [CLMethod(Static = true)]
        public void ShowKillFeed(string killer, string victim, int score, string weapon)
        {
            ((InGameMenu)UIManager.CurrentMenu).ShowKillFeed(killer, victim, score, weapon);
        }

        /// <summary>
        /// Show the kill feed for all players.
        /// </summary>
        /// <param name="killer">The name of the killer.</param>
        /// <param name="victim">The name of the victim.</param>
        /// <param name="score">The score value.</param>
        /// <param name="weapon">The weapon name.</param>
        [CLMethod(Static = true)]
        public void ShowKillFeedAll(string killer, string victim, int score, string weapon)
        {
            RPCManager.PhotonView.RPC(nameof(RPCManager.ShowKillFeedRPC), RpcTarget.All, new object[] { killer, victim, score, weapon });
        }
        private bool NeedRefreshList<T>(string cacheKey, HashSet<T> currentSet, bool includeAI, bool includeNonAI, bool isShifter) where T : BaseCharacter
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
