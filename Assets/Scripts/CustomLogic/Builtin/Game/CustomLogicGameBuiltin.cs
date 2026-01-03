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
    [CLType(Name = "Game", Abstract = true, Static = true, Description = "Game functions such as spawning titans and managing game state.")]
    partial class CustomLogicGameBuiltin : BuiltinClassInstance
    {
        private string _lastSetTopLabel = string.Empty;
        private Dictionary<string, CustomLogicListBuiltin> _cachedLists = new Dictionary<string, CustomLogicListBuiltin>();

        [CLConstructor]
        public CustomLogicGameBuiltin(){}

        private InGameManager _inGameManager => (InGameManager)SceneLoader.CurrentGameManager;

        // Convert the setfield/getfield to CLProperties
        [CLProperty(Static = true, Description = "Is the game ending?")]
        public bool IsEnding => _inGameManager.IsEnding;

        [CLProperty(Static = true, Description = "Time left until the game ends")]
        public float EndTimeLeft => _inGameManager.EndTimeLeft;

        [CLProperty(Static = true, Description = "List of all titans", TypeArguments = new[] { "Titan" })]
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

        [CLProperty(Static = true, Description = "List of all AI titans", TypeArguments = new[] { "Titan" })]
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

        [CLProperty(Static = true, Description = "List of all player titans", TypeArguments = new[] { "Titan" })]
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

        [CLProperty(Static = true, Description = "List of all shifters", TypeArguments = new[] { "Shifter" })]
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

        [CLProperty(Static = true, Description = "List of all AI shifters", TypeArguments = new[] { "Shifter" })]
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

        [CLProperty(Static = true, Description = "List of all player shifters", TypeArguments = new[] { "Shifter" })]
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

        [CLProperty(Static = true, Description = "List of all humans", TypeArguments = new[] { "Human" })]
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

        [CLProperty(Static = true, Description = "List of all AI humans", TypeArguments = new[] { "Human" })]
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

        [CLProperty(Static = true, Description = "List of all player humans", TypeArguments = new[] { "Human" })]
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

        [CLProperty(Static = true, Description = "List of all loadouts", TypeArguments = new[] { "string" })]
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

        [CLProperty(Static = true, Description = "Is the kill score shown by default?")]
        public bool DefaultShowKillScore
        {
            get => CustomLogicManager.Evaluator.DefaultShowKillScore;
            set => CustomLogicManager.Evaluator.DefaultShowKillScore = value;
        }

        [CLProperty(Static = true, Description = "Is the kill feed shown by default?")]
        public bool DefaultHideKillScore
        {
            get => CustomLogicManager.Evaluator.DefaultShowKillFeed;
            set => CustomLogicManager.Evaluator.DefaultShowKillFeed = value;
        }

        [CLProperty(Static = true, Description = "Is the kill score added by default?")]
        public bool DefaultAddKillScore
        {
            get => CustomLogicManager.Evaluator.DefaultAddKillScore;
            set => CustomLogicManager.Evaluator.DefaultAddKillScore = value;
        }

        [CLProperty(Static = true, Description = "Is the loadout shown in the scoreboard?")]
        public bool ShowScoreboardLoadout
        {
            get => CustomLogicManager.Evaluator.ShowScoreboardLoadout;
            set => CustomLogicManager.Evaluator.ShowScoreboardLoadout = value;
        }

        [CLProperty(Static = true, Description = "Is the status shown in the scoreboard?")]
        public bool ShowScoreboardStatus
        {
            get => CustomLogicManager.Evaluator.ShowScoreboardStatus;
            set => CustomLogicManager.Evaluator.ShowScoreboardStatus = value;
        }

        [CLProperty(Static = true, Description = "Forced character type.", Enum = typeof(CustomLogicCharacterTypeEnum))]
        public string ForcedCharacterType
        {
            get => CustomLogicManager.Evaluator.ForcedCharacterType;
            set => CustomLogicManager.Evaluator.ForcedCharacterType = value;
        }

        [CLProperty(Static = true, Description = "Forced loadout.", Enum = typeof(CustomLogicLoadoutEnum))]
        public string ForcedLoadout
        {
            get => CustomLogicManager.Evaluator.ForcedLoadout;
            set => CustomLogicManager.Evaluator.ForcedLoadout = value;
        }

        // Add CLMethods
        [CLMethod(Static = true, Description = "Print a debug statement to the console")]
        public void Debug(
            [CLParam("The message to print.")]
            object message)
        {
            if (message == null)
                message = "null";
            DebugConsole.LogCustomLogic(message.ToString(), SettingsManager.UISettings.ChatCLErrors.Value);
        }

        [CLMethod(Static = true, Description = "Print a message to the chat")]
        public void Print(
            [CLParam("The message to print.")]
            object message)
        {
            if (message == null)
                message = "null";
            ChatManager.AddLine(message.ToString(), ChatTextColor.System);
        }

        [CLMethod(Static = true, Description = "Print a message to all players")]
        public void PrintAll(
            [CLParam("The message to print.")]
            object message)
        {
            ChatManager.SendChatAll(message.ToString(), ChatTextColor.System);
        }

        [CLMethod(Static = true, Description = "Get a general setting")]
        public object GetGeneralSetting(
            [CLParam("The name of the setting to get.")]
            string settingName)
        {
            var setting = SettingsManager.InGameCurrent.General.TypedSettings[settingName];
            return setting.GetType().GetProperty("Value").GetValue(setting);
        }

        [CLMethod(Static = true, Description = "Get a titan setting")]
        public object GetTitanSetting(
            [CLParam("The name of the setting to get.")]
            string settingName)
        {
            var setting = SettingsManager.InGameCurrent.Titan.TypedSettings[settingName];
            return setting.GetType().GetProperty("Value").GetValue(setting);
        }

        [CLMethod(Static = true, Description = "Get a misc setting")]
        public object GetMiscSetting(
            [CLParam("The name of the setting to get.")]
            string settingName)
        {
            var setting = SettingsManager.InGameCurrent.Misc.TypedSettings[settingName];
            return setting.GetType().GetProperty("Value").GetValue(setting);
        }

        [CLMethod(Static = true, Description = "End the game")]
        public void End(
            [CLParam("The delay in seconds before ending the game.")]
            float delay)
        {
            if (PhotonNetwork.IsMasterClient)
                RPCManager.PhotonView.RPC(nameof(RPCManager.EndGameRPC), RpcTarget.All, new object[] { delay });
        }

        [CLMethod(Static = true, Description = "Find a character by view ID")]
        public CustomLogicCharacterBuiltin FindCharacterByViewID(
            [CLParam("The Photon view ID of the character.")]
            int viewID)
        {
            var character = Util.FindCharacterByViewId(viewID);
            if (character == null || character.Dead)
                return null;
            return CustomLogicEvaluator.GetCharacterBuiltin(character);
        }

        [CLMethod(Static = true, Description = "Spawn a titan")]
        public CustomLogicTitanBuiltin SpawnTitan(
            [CLParam("The type of titan to spawn.", Enum = typeof(CustomLogicTitanTypeEnum))]
            string type)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var titan = new CustomLogicTitanBuiltin(_inGameManager.SpawnAITitan(type));
                return titan;
            }
            return null;
        }

        [CLMethod(Static = true, Description = "Spawn a titan at a position")]
        public CustomLogicTitanBuiltin SpawnTitanAt(
            [CLParam("The type of titan to spawn.", Enum = typeof(CustomLogicTitanTypeEnum))]
            string type,
            [CLParam("The spawn position.")]
            CustomLogicVector3Builtin position,
            [CLParam("The Y rotation in degrees (default: 0).")]
            float rotationY = 0f)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var titan = new CustomLogicTitanBuiltin(_inGameManager.SpawnAITitanAt(type, position.Value, rotationY));
                return titan;
            }
            return null;
        }

        [CLMethod(Static = true, Description = "Spawn titans", ReturnTypeArguments = new[] { "Titan" })]
        public CustomLogicListBuiltin SpawnTitans(
            [CLParam("The type of titan to spawn.", Enum = typeof(CustomLogicTitanTypeEnum))]
            string type,
            [CLParam("The number of titans to spawn.")]
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

        [CLMethod(Static = true, Description = "Spawn titans asynchronously")]
        public void SpawnTitansAsync(
            [CLParam("The type of titan to spawn.", Enum = typeof(CustomLogicTitanTypeEnum))]
            string type,
            [CLParam("The number of titans to spawn.")]
            int count)
        {
            if (PhotonNetwork.IsMasterClient)
                _inGameManager.SpawnAITitansAsync(type, count);
        }

        [CLMethod(Static = true, Description = "Spawn titans at a position", ReturnTypeArguments = new[] { "Titan" })]
        public CustomLogicListBuiltin SpawnTitansAt(
            [CLParam("The type of titan to spawn.", Enum = typeof(CustomLogicTitanTypeEnum))]
            string type,
            [CLParam("The number of titans to spawn.")]
            int count,
            [CLParam("The spawn position.")]
            CustomLogicVector3Builtin position,
            [CLParam("The Y rotation in degrees (default: 0).")]
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

        [CLMethod(Static = true, Description = "Spawn titans at a position asynchronously")]
        public void SpawnTitansAtAsync(
            [CLParam("The type of titan to spawn.", Enum = typeof(CustomLogicTitanTypeEnum))]
            string type,
            [CLParam("The number of titans to spawn.")]
            int count,
            [CLParam("The spawn position.")]
            CustomLogicVector3Builtin position,
            [CLParam("The Y rotation in degrees (default: 0).")]
            float rotationY = 0f)
        {
            if (PhotonNetwork.IsMasterClient)
                _inGameManager.SpawnAITitansAtAsync(type, count, position.Value, rotationY);
        }

        [CLMethod(Static = true, Description = "Spawn a shifter")]
        public CustomLogicShifterBuiltin SpawnShifter(
            [CLParam("The type of shifter to spawn.", Enum = typeof(CustomLogicShifterTypeEnum))]
            string type)
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

        [CLMethod(Static = true, Description = "Spawn a shifter at a position")]
        public CustomLogicShifterBuiltin SpawnShifterAt(
            [CLParam("The type of shifter to spawn.", Enum = typeof(CustomLogicShifterTypeEnum))]
            string type,
            [CLParam("The spawn position.")]
            CustomLogicVector3Builtin position,
            [CLParam("The Y rotation in degrees (default: 0).")]
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

        [CLMethod(Static = true, Description = "Spawn a projectile. Note: `extraParam` and `extraParam2` are optional. They may or may not be used depending on the value of `projectileName`")]
        public void SpawnProjectile(
            [CLParam("Name of the projectile.", Enum = typeof(CustomLogicProjectileNameEnum))]
            string projectileName,
            [CLParam("Spawn position")]
            CustomLogicVector3Builtin position,
            [CLParam("Spawn rotation")]
            CustomLogicVector3Builtin rotation,
            [CLParam("Spawn velocity")]
            CustomLogicVector3Builtin velocity,
            [CLParam("Spawn gravity")]
            CustomLogicVector3Builtin gravity,
            [CLParam("Live time of the projectile")]
            float liveTime,
            [CLParam("The team that the projectile belongs to.", Enum = typeof(CustomLogicTeamEnum))]
            string team,
            [CLParam("Optional. Type depends on projectile: Thunderspear: float (explosion radius), Flare: Color (flare color), Rock1: float (rock size), Others: unused")]
            object extraParam = null,
            [CLParam("Optional. Type depends on projectile: Thunderspear: Color (projectile color), Others: unused")]
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

        [CLMethod(Static = true, Description = "Spawn a projectile with an owner. Note: `extraParam` and `extraParam2` are optional. They may or may not be used depending on the value of `projectileName`")]
        public void SpawnProjectileWithOwner(
            [CLParam("Name of the projectile.", Enum = typeof(CustomLogicProjectileNameEnum))]
            string projectileName,
            [CLParam("Spawn position")]
            CustomLogicVector3Builtin position,
            [CLParam("Spawn rotation")]
            CustomLogicVector3Builtin rotation,
            [CLParam("Spawn velocity")]
            CustomLogicVector3Builtin velocity,
            [CLParam("Spawn gravity")]
            CustomLogicVector3Builtin gravity,
            [CLParam("Live time of the projectile")]
            float liveTime,
            [CLParam("The character that the projectile belongs to")]
            CustomLogicCharacterBuiltin owner,
            [CLParam("Optional. Type depends on projectile: Thunderspear: float (explosion radius), Flare: Color (flare color), Rock1: float (rock size), Others: unused")]
            object extraParam = null,
            [CLParam("Optional. Type depends on projectile: Thunderspear: Color (projectile color), Others: unused")]
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

        [CLMethod(Static = true, Description = "Spawn an effect")]
        public void SpawnEffect(
            [CLParam("Name of the effect.", Enum = typeof(CustomLogicEffectNameEnum))]
            string effectName,
            [CLParam("Spawn position")]
            CustomLogicVector3Builtin position,
            [CLParam("Spawn rotation")]
            CustomLogicVector3Builtin rotation,
            [CLParam("Spawn scale")]
            float scale,
            [CLParam("Thunderspear explode color (Only valid when effectName is \"ThunderspearExplode\")")]
            CustomLogicColorBuiltin tsExplodeColor = null,
            [CLParam("Optional. Thunderspear explode sound (Only valid when effectName is \"ThunderspearExplode\").", Enum = typeof(CustomLogicTSKillSoundEnum))]
            string tsKillSound = null)
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

        [CLMethod(Static = true, Description = "Spawn an unscaled effect")]
        public void SpawnUnscaledEffect(
            [CLParam("Name of the effect.", Enum = typeof(CustomLogicEffectNameEnum))]
            string effectName,
            [CLParam("Spawn position")]
            CustomLogicVector3Builtin position,
            [CLParam("Spawn rotation")]
            CustomLogicVector3Builtin rotation,
            [CLParam("Thunderspear explode color (Only valid when effectName is \"ThunderspearExplode\")")]
            CustomLogicColorBuiltin tsExplodeColor = null,
            [CLParam("Optional. Thunderspear explode sound (Only valid when effectName is \"ThunderspearExplode\").", Enum = typeof(CustomLogicTSKillSoundEnum))]
            string tsKillSound = null)
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

        [CLMethod(Static = true, Description = "Spawn a player")]
        public void SpawnPlayer(
            [CLParam("The player to spawn.")]
            CustomLogicPlayerBuiltin player,
            [CLParam("If true, forces respawn even if the player is already alive.")]
            bool force)
        {
            if (player.Player == PhotonNetwork.LocalPlayer)
                _inGameManager.SpawnPlayer(force);
            else if (PhotonNetwork.IsMasterClient)
                RPCManager.PhotonView.RPC(nameof(RPCManager.SpawnPlayerRPC), player.Player, new object[] { force });
        }

        [CLMethod(Static = true, Description = "Spawn a player for all players")]
        public void SpawnPlayerAll(
            [CLParam("If true, forces respawn even if players are already alive.")]
            bool force)
        {
            if (PhotonNetwork.IsMasterClient)
                RPCManager.PhotonView.RPC(nameof(RPCManager.SpawnPlayerRPC), RpcTarget.All, new object[] { force });
        }

        [CLMethod(Static = true, Description = "Spawn a player at a position")]
        public void SpawnPlayerAt(
            [CLParam("The player to spawn.")]
            CustomLogicPlayerBuiltin player,
            [CLParam("If true, forces respawn even if the player is already alive.")]
            bool force,
            [CLParam("The spawn position.")]
            CustomLogicVector3Builtin position,
            [CLParam("The Y rotation in degrees (default: 0).")]
            float rotationY = 0f)
        {
            if (player.Player == PhotonNetwork.LocalPlayer)
                _inGameManager.SpawnPlayerAt(force, position.Value, rotationY);
            else if (PhotonNetwork.IsMasterClient)
                RPCManager.PhotonView.RPC(nameof(RPCManager.SpawnPlayerAtRPC), player.Player, new object[] { force, position.Value, rotationY });
        }

        [CLMethod(Static = true, Description = "Spawn a player at a position for all players")]
        public void SpawnPlayerAtAll(
            [CLParam("If true, forces respawn even if players are already alive.")]
            bool force,
            [CLParam("The spawn position.")]
            CustomLogicVector3Builtin position,
            [CLParam("The Y rotation in degrees (default: 0).")]
            float rotationY = 0f)
        {
            if (PhotonNetwork.IsMasterClient)
                RPCManager.PhotonView.RPC(nameof(RPCManager.SpawnPlayerAtRPC), RpcTarget.All, new object[] { force, position.Value, rotationY });
        }

        [CLMethod(Static = true, Description = "Set the music playlist")]
        public void SetPlaylist(
            [CLParam("The name of the playlist to set.")]
            string playlist)
        {
            MusicManager.SetPlaylist(playlist);
            CustomLogicManager.Evaluator.HasSetMusic = true;
        }

        [CLMethod(Static = true, Description = "Set the music song")]
        public void SetSong(
            [CLParam("The name of the song to set.")]
            string song)
        {
            MusicManager.SetSong(song);
            CustomLogicManager.Evaluator.HasSetMusic = true;
        }

        [CLMethod(Static = true, Description = "Draw a ray")]
        public void DrawRay(
            [CLParam("The start position of the ray.")]
            CustomLogicVector3Builtin start,
            [CLParam("The direction vector of the ray.")]
            CustomLogicVector3Builtin dir,
            [CLParam("The color of the ray.")]
            CustomLogicColorBuiltin color,
            [CLParam("The duration in seconds to display the ray.")]
            float duration)
        {
            UnityEngine.Debug.DrawRay(start.Value, dir.Value, color.Value.ToColor(), duration);
        }

        [CLMethod(Static = true, Description = "Show the kill score")]
        public void ShowKillScore(
            [CLParam("The damage value to display.")]
            int damage)
        {
            ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage, true);
        }

        [CLMethod(Static = true, Description = "Show the kill feed")]
        public void ShowKillFeed(
            [CLParam("The name of the killer.")]
            string killer,
            [CLParam("The name of the victim.")]
            string victim,
            [CLParam("The score value.")]
            int score,
            [CLParam("The weapon name.")]
            string weapon)
        {
            ((InGameMenu)UIManager.CurrentMenu).ShowKillFeed(killer, victim, score, weapon);
        }

        [CLMethod(Static = true, Description = "Show the kill feed for all players")]
        public void ShowKillFeedAll(
            [CLParam("The name of the killer.")]
            string killer,
            [CLParam("The name of the victim.")]
            string victim,
            [CLParam("The score value.")]
            int score,
            [CLParam("The weapon name.")]
            string weapon)
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
