using System.Collections.Generic;
using UnityEngine;
using UI;
using Utility;
using CustomSkins;
using ApplicationManagers;
using Characters;
using Settings;
using CustomLogic;
using Map;
using System.Collections;
using GameProgress;
using Cameras;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System.Linq;
using Controllers;
using Photon.Voice.PUN;
using Anticheat;
using System.Globalization;

namespace GameManagers
{
    class InGameManager : BaseGameManager
    {
        private static readonly List<string> BlueSpawnTags = new List<string> { MapTags.HumanSpawnPointBlue, MapTags.HumanSpawnPoint, MapTags.HumanSpawnPointRed };
        private static readonly List<string> RedSpawnTags = new List<string> { MapTags.HumanSpawnPointRed, MapTags.HumanSpawnPoint, MapTags.HumanSpawnPointBlue };
        private static readonly List<string> HumanSpawnTags = new List<string> { MapTags.HumanSpawnPoint, MapTags.HumanSpawnPointBlue, MapTags.HumanSpawnPointRed };
        
        private SkyboxCustomSkinLoader _skyboxCustomSkinLoader;
        //private ForestCustomSkinLoader _forestCustomSkinLoader;
        //private CityCustomSkinLoader _cityCustomSkinLoader;
        private GeneralInputSettings _generalInputSettings;
        private InGameMenu _inGameMenu;
        public HashSet<Human> Humans = new HashSet<Human>();
        public HashSet<BasicTitan> Titans = new HashSet<BasicTitan>();
        public HashSet<BaseShifter> Shifters = new HashSet<BaseShifter>();
        public List<PhotonVoiceSync> PhotonVoiceSyncs = new List<PhotonVoiceSync>();
        public bool IsEnding;
        public float EndTimeLeft;
        public GameState State = GameState.Loading;
        public BaseCharacter CurrentCharacter;
        private bool _gameSettingsLoaded = false;
        public static Dictionary<int, PlayerInfo> AllPlayerInfo = new Dictionary<int, PlayerInfo>();
        public static HashSet<int> MuteEmote = new HashSet<int>();
        public static HashSet<int> MuteText = new HashSet<int>();
        public static HashSet<int> MuteVoiceChat = new HashSet<int>();
        public static Dictionary<int, float> VoiceChatVolumeMultiplier = new Dictionary<int, float>();
        public static PlayerInfo MyPlayerInfo = new PlayerInfo();
        private static bool _needSendPlayerInfo;
        public bool HasSpawned = false;
        public bool GlobalPause = false;
        public bool Restarting = false;
        public float PauseTimeLeft = -1f;
        public float RespawnTimeLeft;
        public HashSet<BaseDetection> Detections = new HashSet<BaseDetection>();

        //ping related
        private float pingUpdateInterval = 10f;
        private float timeSinceLastPingUpdate = 0f;

        public void RegisterCharacter(BaseCharacter character)
        {
            if (character is Human)
                Humans.Add((Human)character);
            else if (character is BasicTitan)
                Titans.Add((BasicTitan)character);
            else if (character is BaseShifter)
                Shifters.Add((BaseShifter)character);
            foreach (var detection in Detections)
                detection.OnCharacterSpawned(character);
        }

        public HashSet<BaseCharacter> GetAllCharacters()
        {
            var characters = new HashSet<BaseCharacter>();
            foreach (var human in Humans)
            {
                if (human != null && !human.Dead)
                    characters.Add(human);
            }
            foreach (var titan in Titans)
            {
                if (titan != null && !titan.Dead)
                    characters.Add(titan);
            }
            foreach (var shifter in Shifters)
            {
                if (shifter != null && !shifter.Dead)
                    characters.Add(shifter);
            }
            return characters;
        }

        public IEnumerable<BaseCharacter> GetAllCharactersEnumerable()
        {
            foreach (var human in Humans)
            {
                if (human != null && !human.Dead)
                    yield return human;
            }
            foreach (var titan in Titans)
            {
                if (titan != null && !titan.Dead)
                    yield return titan;
            }
            foreach (var shifter in Shifters)
            {
                if (shifter != null && !shifter.Dead)
                    yield return shifter;
            }
        }

        public HashSet<BaseCharacter> GetAllNonAICharacters()
        {
            return GetAllCharacters().Where(x => !x.AI).ToHashSet();
        }

        public void PauseGame()
        {
            if (!PhotonNetwork.IsMasterClient || GlobalPause)
                return;
            RPCManager.PhotonView.RPC("PauseGameRPC", RpcTarget.All, new object[0]);
        }

        public void OnPauseGameRPC(PhotonMessageInfo info)
        {
            if (!info.Sender.IsMasterClient)
                return;
            GlobalPause = true;
            PauseTimeLeft = -1f;
            Time.timeScale = 0f;
        }

        public void UnpauseGame()
        {
            if (!PhotonNetwork.IsMasterClient || !GlobalPause)
                return;
            RPCManager.PhotonView.RPC("UnpauseGameRPC", RpcTarget.All, new object[0]);
        }

        public void OnUnpauseGameRPC(PhotonMessageInfo info)
        {
            if (!info.Sender.IsMasterClient)
                return;
            GlobalPause = false;
            PauseTimeLeft = -1f;
            Time.timeScale = 1f;
        }

        public void StartUnpauseGame()
        {
            if (!PhotonNetwork.IsMasterClient || !GlobalPause)
                return;
            RPCManager.PhotonView.RPC("StartUnpauseGameRPC", RpcTarget.All, new object[0]);
        }

        public void OnStartUnpauseGameRPC(PhotonMessageInfo info)
        {
            if (!info.Sender.IsMasterClient)
                return;
            GlobalPause = true;
            PauseTimeLeft = 5f;
            StartCoroutine(WaitAndUnpauseGame());
        }

        private IEnumerator WaitAndUnpauseGame()
        {
            float endTime = Time.realtimeSinceStartup + PauseTimeLeft;
            while (PauseTimeLeft > 0f)
            {
                PauseTimeLeft = Mathf.Max(endTime - Time.realtimeSinceStartup, 0f);
                yield return null;
            }
            if (PhotonNetwork.IsMasterClient)
                UnpauseGame();
        }

        public static void RestartGame()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            var manager = (InGameManager)SceneLoader.CurrentGameManager;
            RPCManager.PhotonView.RPC("PreRestartGameRPC", RpcTarget.All, new object[] { !SettingsManager.UISettings.FadeLoadscreen.Value });
            Time.timeScale = 1f;
            manager.StartCoroutine(manager.FinishRestartGame());
        }

        private IEnumerator FinishRestartGame()
        {
            if (SettingsManager.UISettings.FadeLoadscreen.Value)
                yield return new WaitForSeconds(0.2f);
            else
                yield return new WaitForEndOfFrame();
            PhotonNetwork.DestroyAll();
            RPCManager.PhotonView.RPC("RestartGameRPC", RpcTarget.All, new object[0]);
        }

        public static void OnRestartGameRPC(PhotonMessageInfo info)
        {
            if (!info.Sender.IsMasterClient)
                return;
            ResetRoundPlayerProperties();
            if (!PhotonNetwork.OfflineMode)
                ChatManager.AddLine("Master client has restarted the game.", ChatTextColor.System);
            SceneLoader.LoadScene(SceneName.InGame);
        }

        public static void OnPreRestartGameRPC(bool immediate, PhotonMessageInfo info)
        {
            if (!info.Sender.IsMasterClient)
                return;
            ((InGameManager)SceneLoader.CurrentGameManager).Restarting = true;
            UIManager.CurrentMenu.gameObject.SetActive(false);
            UIManager.LoadingMenu.Show(immediate);
        }

        public static void LeaveRoom()
        {
            ChatManager.ResetAllPMState();
            ResetPersistentPlayerProperties();
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.DestroyAll();
            if (PhotonNetwork.IsConnected)
                SettingsManager.MultiplayerSettings.Disconnect();
            SettingsManager.InGameCurrent.SetDefault();
            SettingsManager.InGameUI.SetDefault();
            SettingsManager.InGameCharacterSettings.SetDefault();
            SceneLoader.LoadScene(SceneName.MainMenu);
            MaterialCache.Clear();
        }

        public override void OnLeftRoom()
        {
            if (PhotonNetwork.IsConnected)
            {
                var chatPanel = UIManager.CurrentMenu?.GetComponent<InGameMenu>()?.ChatPanel;
                if (chatPanel != null)
                {
                    chatPanel.ResetPMState();
                }
                LeaveRoom();
                MainMenuGameManager.JustLeftRoom = true;
            }
        }

        public static void OnJoinRoom()
        {
            AnticheatManager.Reset();
            ResetPlayerInfo();
            ResetPersistentPlayerProperties();
            _needSendPlayerInfo = true;
            
            // Add this line to sync PM state
            ChatManager.SyncPMPartnersOnJoin();
            
            if (PhotonNetwork.OfflineMode)
                ChatManager.AddLine("Welcome to single player. \nType /help for a list of commands.", ChatTextColor.System);
            else
                ChatManager.AddLine("Welcome to " + PhotonNetwork.CurrentRoom.GetStringProperty(RoomProperty.Name).Trim().HexColor() + ". \nType /help for a list of commands.",
                    ChatTextColor.System);
            SceneLoader.LoadScene(SceneName.InGame);
        }

        public void RegisterMainCharacterDie()
        {
            UpdateRoundPlayerProperties();
            if (CurrentCharacter == null)
                return;
            if (CustomLogicManager.Evaluator != null && CustomLogicManager.Evaluator.DefaultAddKillScore)
                PhotonNetwork.LocalPlayer.SetCustomProperty(PlayerProperty.Deaths, PhotonNetwork.LocalPlayer.GetIntProperty(PlayerProperty.Deaths) + 1);
        }

        public void RegisterMainCharacterKill(BaseCharacter victim)
        {
            if (CurrentCharacter == null)
                return;
            var killWeapon = KillWeapon.Other;
            if (CurrentCharacter is Human)
            {
                var human = (Human)CurrentCharacter;
                if (human.Setup.Weapon == HumanWeapon.AHSS)
                    killWeapon = KillWeapon.AHSS;
                else if (human.Setup.Weapon == HumanWeapon.Blade)
                    killWeapon = KillWeapon.Blade;
                else if (human.Setup.Weapon == HumanWeapon.Thunderspear)
                    killWeapon = KillWeapon.Thunderspear;
                else if (human.Setup.Weapon == HumanWeapon.APG)
                    killWeapon = KillWeapon.APG;
            }
            else if (CurrentCharacter is BasicTitan)
                killWeapon = KillWeapon.Titan;
            else if (CurrentCharacter is BaseShifter)
                killWeapon = KillWeapon.Shifter;
            if (victim is Human)
                GameProgressManager.RegisterHumanKill((Human)victim, killWeapon);
            else if (victim is BasicTitan)
                GameProgressManager.RegisterTitanKill((BasicTitan)victim, killWeapon);
            var properties = new Dictionary<string, object>
            {
                { PlayerProperty.Kills, PhotonNetwork.LocalPlayer.GetIntProperty(PlayerProperty.Kills) + 1 }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }

        public void RegisterMainCharacterDamage(BaseCharacter victim, int damage)
        {
            if (CurrentCharacter == null)
                return;
            KillMethod killMethod = KillWeapon.Other;
            if (CurrentCharacter is Human)
            {
                var human = (Human)CurrentCharacter;
                if (human.Setup.Weapon == HumanWeapon.AHSS)
                    killMethod.Weapon = KillWeapon.AHSS;
                else if (human.Setup.Weapon == HumanWeapon.Blade)
                    killMethod.Weapon = KillWeapon.Blade;
                else if (human.Setup.Weapon == HumanWeapon.Thunderspear)
                    killMethod.Weapon = KillWeapon.Thunderspear;
                else if (human.Setup.Weapon == HumanWeapon.APG)
                    killMethod.Weapon = KillWeapon.APG;
                killMethod.Special = human.State == HumanState.SpecialAttack ? human.CurrentSpecial : "";
            }
            else if (CurrentCharacter is BasicTitan)
                killMethod = KillWeapon.Titan;
            else if (CurrentCharacter is BaseShifter)
                killMethod = KillWeapon.Shifter;
            GameProgressManager.RegisterDamage(victim.gameObject, killMethod, damage);
            var properties = new Dictionary<string, object>
            {
                { PlayerProperty.TotalDamage, PhotonNetwork.LocalPlayer.GetIntProperty(PlayerProperty.TotalDamage) + damage },
                { PlayerProperty.HighestDamage, Mathf.Max(PhotonNetwork.LocalPlayer.GetIntProperty(PlayerProperty.HighestDamage), damage) }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            if (!AllPlayerInfo.ContainsKey(player.ActorNumber))
                AllPlayerInfo.Add(player.ActorNumber, new PlayerInfo());
            RPCManager.PhotonView.RPC("PlayerInfoRPC", player, new object[] { StringCompression.Compress(MyPlayerInfo.SerializeToJsonString()) });
            if (PhotonNetwork.IsMasterClient)
            {
                RPCManager.PhotonView.RPC("GameSettingsRPC", player, new object[] { StringCompression.Compress(SettingsManager.InGameCurrent.SerializeToJsonString()) });
                if (GlobalPause)
                    RPCManager.PhotonView.RPC("PauseGameRPC", player, new object[0]);
            }
            if (ChatManager.HasActivePlayerSuggestions())
                ChatManager.RefreshPlayerSuggestions();
        }

        public void OnNotifyPlayerJoined(Player player)
        {
            CustomLogicManager.Evaluator.OnPlayerJoin(player);
            if (SettingsManager.UISettings.JoinNotifications.Value)
            {
                string line = player.GetCustomProperty(PlayerProperty.Name) + ChatManager.GetColorString(" has joined the room.", ChatTextColor.System);
                ChatManager.AddLine(line);
            }
            if (ChatManager.HasActivePlayerSuggestions())
                ChatManager.RefreshPlayerSuggestions();
        }

        public override void OnPlayerLeftRoom(Player player)
        {
            base.OnPlayerLeftRoom(player);
            var chatPanel = UIManager.CurrentMenu?.GetComponent<InGameMenu>()?.ChatPanel;
            if (chatPanel != null)
            {
                chatPanel.RemovePMPartner(player);
                chatPanel.Sync();
            }
            if (AllPlayerInfo.ContainsKey(player.ActorNumber))
            {
                AllPlayerInfo.Remove(player.ActorNumber);
            }
            if (SettingsManager.UISettings.JoinNotifications.Value)
            {
                string line = player.GetCustomProperty(PlayerProperty.Name) + ChatManager.GetColorString(" has left the room.", ChatTextColor.System);
                ChatManager.AddLine(line);
            }
            if (CustomLogicManager.Evaluator != null)
                CustomLogicManager.Evaluator.OnPlayerLeave(player);
            if (MuteEmote.Contains(player.ActorNumber))
                MuteEmote.Remove(player.ActorNumber);
            if (MuteText.Contains(player.ActorNumber))
                MuteText.Remove(player.ActorNumber);
            if (MuteVoiceChat.Contains(player.ActorNumber))
                MuteVoiceChat.Remove(player.ActorNumber);
            if (VoiceChatVolumeMultiplier.ContainsKey(player.ActorNumber))
                VoiceChatVolumeMultiplier.Remove(player.ActorNumber);

            AnticheatManager.ResetVoteKicks(player);
            if (ChatManager.HasActivePlayerSuggestions())
                ChatManager.RefreshPlayerSuggestions();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            ChatManager.AddLine("Master client has switched to " + newMasterClient.GetCustomProperty(PlayerProperty.Name) + ".", ChatTextColor.System);
            if (PhotonNetwork.IsMasterClient)
            {
                RestartGame();
            }
        }

        public static void OnPlayerInfoRPC(byte[] data, PhotonMessageInfo info)
        {
            if (!AllPlayerInfo.ContainsKey(info.Sender.ActorNumber))
                AllPlayerInfo.Add(info.Sender.ActorNumber, new PlayerInfo());
            if (data.Length > 1000)
                return;
            AllPlayerInfo[info.Sender.ActorNumber].DeserializeFromJsonString(StringCompression.Decompress(data));
        }

        public static void OnGameSettingsRPC(byte[] data, PhotonMessageInfo info)
        {
            if (!info.Sender.IsMasterClient)
                return;
            string original = SettingsManager.InGameCurrent.Misc.Motd.Value;
            SettingsManager.InGameCurrent.DeserializeFromJsonString(StringCompression.Decompress(data));
            ((InGameManager)SceneLoader.CurrentGameManager)._gameSettingsLoaded = true;
            PrintMOTD(original);
            VoiceChatManager.ApplySoundSettings();
            if (!SettingsManager.InGameCurrent.Misc.EndlessRespawnEnabled.Value)
                return;
            var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
            gameManager.StartCoroutine(gameManager.RespawnForever(SettingsManager.InGameCurrent.Misc.EndlessRespawnTime.Value));
        }

        public static void OnCharacterChosen()
        {
            ResetRespawnTimeLeft();
        }

        public static void OnLocalPlayerDied(Player player)
        {
            ResetRespawnTimeLeft();
        }

        private static void ResetRespawnTimeLeft()
        {
            var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
            gameManager.RespawnTimeLeft = SettingsManager.InGameCurrent.Misc.EndlessRespawnTime.Value;
        }

        private IEnumerator RespawnForever(float delay)
        {
            while (true)
            {
                RespawnTimeLeft -= 1;
                if (RespawnTimeLeft <= 0)
                {
            SpawnPlayer(false);
                    RespawnTimeLeft = delay;
                }
                yield return new WaitForSeconds(1);
            }
        }

        public void SpawnPlayer(bool force)
        {
            if (!IsFinishedLoading())
                return;
            if (CustomLogicManager.Evaluator.ForcedCharacterType != string.Empty)
                SettingsManager.InGameCharacterSettings.CharacterType.Value = CustomLogicManager.Evaluator.ForcedCharacterType;
            string characterType = SettingsManager.InGameCharacterSettings.CharacterType.Value;
            var isHuman = characterType == PlayerCharacter.Human;
            if (PhotonNetwork.LocalPlayer.HasSpawnPoint())
            {
                var position = PhotonNetwork.LocalPlayer.GetSpawnPoint();
                SpawnPlayerAt(force, position, 0f);
            }
            else if (isHuman)
            {
                var (position, rotation) = GetHumanSpawnPoint();
                SpawnPlayerAt(force, position, rotation.eulerAngles.y);
            }
            else
            {
                var (position, rotation) = GetTitanSpawnPoint();
                SpawnPlayerAt(force, position, rotation.eulerAngles.y);
            }
        }

        public void SpawnPlayerShifterAt(string shifterName, float liveTime, Vector3 position, float rotationY)
        {
            var rotation = Quaternion.Euler(0f, rotationY, 0f);
            BaseShifter shifter = null;
            if (shifterName == "Annie")
                shifter = (BaseShifter)CharacterSpawner.Spawn(CharacterPrefabs.AnnieShifter, position, rotation);
            else if (shifterName == "Eren")
                shifter = (BaseShifter)CharacterSpawner.Spawn(CharacterPrefabs.ErenShifter, position, rotation);
            else if (shifterName == "Armored")
                shifter = (BaseShifter)CharacterSpawner.Spawn(CharacterPrefabs.ArmoredShifter, position, rotation);
            if (shifter != null)
            {
                shifter.Init(false, GetPlayerTeam(false), null, liveTime);
                CurrentCharacter = shifter;
                if (SettingsManager.InGameCurrent.Misc.ShifterHealth.Value > 0)
                    shifter.SetHealth(SettingsManager.InGameCurrent.Misc.ShifterHealth.Value);
            }
        }

        public InGameCharacterSettings GetSetHumanSettings()
        {
            var settings = SettingsManager.InGameCharacterSettings;
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
            if (!loadouts.Contains(settings.Loadout.Value))
                settings.Loadout.Value = loadouts[0];
            var specials = HumanSpecials.GetSpecialNames(settings.Loadout.Value, miscSettings.AllowShifterSpecials.Value);
            if (!specials.Contains(settings.Special.Value))
                settings.Special.Value = HumanSpecials.DefaultSpecial;

            return SettingsManager.InGameCharacterSettings;
        }

        public void SpawnPlayerAt(bool force, Vector3 position, float rotationY)
        {
            if (!IsFinishedLoading())
                return;
            var rotation = Quaternion.Euler(0f, rotationY, 0f);
            var settings = SettingsManager.InGameCharacterSettings;
            if (CustomLogicManager.Evaluator.ForcedCharacterType != string.Empty)
                settings.CharacterType.Value = CustomLogicManager.Evaluator.ForcedCharacterType;
            string character = settings.CharacterType.Value;
            var miscSettings = SettingsManager.InGameCurrent.Misc;
            if (settings.ChooseStatus.Value != (int)ChooseCharacterStatus.Chosen)
                return;
            if (CurrentCharacter != null && !CurrentCharacter.Dead && !force)
                return;
            if (CurrentCharacter != null && !CurrentCharacter.Dead)
                CurrentCharacter.GetKilled("");
            string forced = string.Empty;
            UpdatePlayerName();
            List<string> characters = new List<string>();
            if (miscSettings.AllowAHSS.Value || miscSettings.AllowBlades.Value || miscSettings.AllowThunderspears.Value || miscSettings.AllowAPG.Value)
                characters.Add(PlayerCharacter.Human);
            if (miscSettings.AllowPlayerTitans.Value)
                characters.Add(PlayerCharacter.Titan);
            if (miscSettings.AllowShifters.Value)
                characters.Add(PlayerCharacter.Shifter);
            if (characters.Count == 0)
                characters.Add(PlayerCharacter.Human);
            if (!characters.Contains(character))
                character = characters[0];
            if (character == PlayerCharacter.Human)
            {
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
                if (!loadouts.Contains(settings.Loadout.Value))
                    settings.Loadout.Value = loadouts[0];
                if (CustomLogicManager.Evaluator.ForcedLoadout != string.Empty)
                    settings.Loadout.Value = CustomLogicManager.Evaluator.ForcedLoadout;
                var specials = HumanSpecials.GetSpecialNames(settings.Loadout.Value, miscSettings.AllowShifterSpecials.Value);
                if (!specials.Contains(settings.Special.Value))
                    settings.Special.Value = HumanSpecials.DefaultSpecial;
                var human = (Human)CharacterSpawner.Spawn(CharacterPrefabs.Human, position, rotation);
                human.Init(false, GetPlayerTeam(false), SettingsManager.InGameCharacterSettings);
                CurrentCharacter = human;
                if (SettingsManager.InGameCurrent.Misc.HumanHealth.Value > 1)
                    human.SetHealth(SettingsManager.InGameCurrent.Misc.HumanHealth.Value);
            }
            else if (character == PlayerCharacter.Shifter)
            {
                forced = CustomLogicManager.Evaluator.ForcedLoadout;
                if (forced != string.Empty)
                    settings.Loadout.Value = forced;
                SpawnPlayerShifterAt(settings.Loadout.Value, 0f, position, rotationY);
            }
            else if (character == PlayerCharacter.Titan)
            {
                int[] combo = BasicTitanSetup.GetRandomBodyHeadCombo();
                TitanCustomSet selectedSet = null;
                int selectedSetIndex = settings.CustomSet.Value;
                if (selectedSetIndex > 0)
                {
                    selectedSet = (TitanCustomSet)SettingsManager.TitanCustomSettings.TitanCustomSets.Sets.GetItemAt(selectedSetIndex - 1);
                    combo[0] = selectedSet.Body.Value;
                    combo[1] = selectedSet.Head.Value;
                }
                string prefab = CharacterPrefabs.BasicTitanPrefix + combo[0];
                var titan = (BasicTitan)CharacterSpawner.Spawn(prefab, position, rotation);
                TitanCustomSet currentSet;
                if (selectedSetIndex == 0)
                    currentSet = titan.Setup.CreateRandomSet(combo[1]);
                else
                    currentSet = selectedSet;
                titan.Init(false, GetPlayerTeam(true), null, currentSet);
                SetupTitan(titan, false);
                float smallSize = 1f;
                float mediumSize = 2f;
                float largeSize = 3f;
                if (SettingsManager.InGameCurrent.Titan.TitanSizeEnabled.Value)
                {
                    float minSize = SettingsManager.InGameCurrent.Titan.TitanSizeMin.Value;
                    float maxSize = SettingsManager.InGameCurrent.Titan.TitanSizeMax.Value;
                    minSize = Mathf.Min(minSize, maxSize);
                    smallSize = minSize;
                    mediumSize = 0.5f * (minSize + maxSize);
                    largeSize = maxSize;
                }
                forced = CustomLogicManager.Evaluator.ForcedLoadout;
                if (forced != string.Empty)
                    settings.Loadout.Value = forced;
                if (settings.Loadout.Value == "Small")
                    titan.SetSize(smallSize);
                else if (settings.Loadout.Value == "Medium")
                    titan.SetSize(mediumSize);
                else if (settings.Loadout.Value == "Large")
                    titan.SetSize(largeSize);
                CurrentCharacter = titan;
            }
            HasSpawned = true;
            PhotonNetwork.LocalPlayer.SetCustomProperty(PlayerProperty.CharacterViewId, CurrentCharacter.Cache.PhotonView.ViewID);
            UpdateRoundPlayerProperties();
        }
        
        private (Vector3, Quaternion) GetHumanSpawnPoint()
        {
            var tags = HumanSpawnTags;
            if (SettingsManager.InGameCurrent.Misc.PVP.Value == (int)PVPMode.Team)
                tags = SettingsManager.InGameCharacterSettings.Team.Value == TeamInfo.Blue ? BlueSpawnTags : RedSpawnTags;

            return MapManager.TryGetRandomTagsXform(tags, out var xform)
                ? (xform.position, xform.rotation)
                : (Vector3.zero, Quaternion.identity);
        }

        private (Vector3 position, Quaternion rotation) GetTitanSpawnPoint()
        {
            return MapManager.TryGetRandomTagXform(MapTags.TitanSpawnPoint, out var xform)
                ? (xform.position, xform.rotation)
                : (Vector3.zero, Quaternion.identity);
        }

        private string GetPlayerTeam(bool titan)
        {
            if (SettingsManager.InGameCurrent.Misc.PVP.Value == (int)PVPMode.Team)
                return SettingsManager.InGameCharacterSettings.Team.Value;
            else if (SettingsManager.InGameCurrent.Misc.PVP.Value == (int)PVPMode.FFA)
                return TeamInfo.None;
            else if (titan)
                return TeamInfo.Titan;
            else
                return TeamInfo.Human;
        }

        public BasicTitan SpawnAITitan(string type)
        {
            var spawn = GetTitanSpawnPoint();
            return SpawnAITitanAt(type, spawn.position, spawn.rotation.eulerAngles.y);
        }

        public IEnumerable<BasicTitan> SpawnAITitans(string type, int count)
        {
            return GetTitanSpawnPositions(count).Select(p => SpawnAITitanAt(type, p.position, p.rotation.eulerAngles.y));
        }

        public void SpawnAITitansAsync(string type, int count)
        {
            StartCoroutine(SpawnAITitansCoroutine(type, count));
        }

        private IEnumerator SpawnAITitansCoroutine(string type, int count)
        {
            var randomPositions = GetTitanSpawnPositions(count);
            foreach (var spawn in randomPositions)
            {
                SpawnAITitanAt(type, spawn.position, spawn.rotation.eulerAngles.y);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
            }
        }

        public void SpawnAITitansAtAsync(string type, int count, Vector3 position, float rotationY)
        {
            StartCoroutine(SpawnAITitansAtCoroutine(type, count, position, rotationY));
        }

        private IEnumerator SpawnAITitansAtCoroutine(string type, int count, Vector3 position, float rotationY)
        {
            for (int i = 0; i < count; i++)
            {
                SpawnAITitanAt(type, position, rotationY);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
            }
        }
        
        /// <returns><paramref name="count"/> number of positions, from the list of spawn points, or Vector3.zero if no spawn points were found.</returns>
        private IEnumerable<(Vector3 position, Quaternion rotation)> GetTitanSpawnPositions(int count)
        {
            bool avoidPlayer = CurrentCharacter != null && CurrentCharacter is Human && Humans.Count == 1;

            var avoidPosition = avoidPlayer ? CurrentCharacter.Cache.Transform.position : Vector3.zero;
            var avoidRadius = avoidPlayer ? 100f : 0f;

            return MapManager.TryGetRandomTagXforms(MapTags.TitanSpawnPoint, avoidPosition, avoidRadius, count, out List<Transform> xforms)
                ? xforms.Select(xform => (xform.position, xform.rotation))
                : Enumerable.Repeat((Vector3.zero,  Quaternion.identity), count);
        }

        public BasicTitan SpawnAITitanAt(string type, Vector3 position, float rotationY)
        {
            var rotation = Quaternion.Euler(0f, rotationY, 0f);
            
            if (type == "Default")
            {
                var settings = SettingsManager.InGameCurrent.Titan;
                if (settings.TitanSpawnEnabled.Value)
                {
                    float roll = UnityEngine.Random.Range(0f, 1f);
                    float normal = settings.TitanSpawnNormal.Value / 100f;
                    float abnormal = normal + settings.TitanSpawnAbnormal.Value / 100f;
                    float jumper = abnormal + settings.TitanSpawnJumper.Value / 100f;
                    float crawler = jumper + settings.TitanSpawnCrawler.Value / 100f;
                    float thrower = crawler + settings.TitanSpawnThrower.Value / 100f;
                    float punk = thrower + settings.TitanSpawnPunk.Value / 100f;
                    if (roll < normal)
                        type = "Normal";
                    else if (roll < abnormal)
                        type = "Abnormal";
                    else if (roll < jumper)
                        type = "Jumper";
                    else if (roll < crawler)
                        type = "Crawler";
                    else if (roll < thrower)
                        type = "Thrower";
                    else if (roll < punk)
                        type = "Punk";
                }
            }
            else if (type == "Random")
            {
                string[] types = new string[]{ "Normal", "Abnormal", "Jumper", "Crawler", "Thrower" };
                type = types[UnityEngine.Random.Range(0, types.Length)];
            }
            var data = CharacterData.GetTitanAI((GameDifficulty)SettingsManager.InGameCurrent.General.Difficulty.Value, type);
            int[] combo = BasicTitanSetup.GetRandomBodyHeadCombo(data);
            string prefab = CharacterPrefabs.BasicTitanPrefix + combo[0];
            var titan = (BasicTitan)CharacterSpawner.Spawn(prefab, position, rotation);
            titan.Init(true, TeamInfo.Titan, data, titan.Setup.CreateRandomSet(combo[1]));
            SetupTitan(titan);
            return titan;
        }

        public void SetupTitan(BasicTitan titan, bool ai=true)
        {
            var settings = SettingsManager.InGameCurrent.Titan;
            if (settings.TitanSizeEnabled.Value)
            {
                float size = UnityEngine.Random.Range(settings.TitanSizeMin.Value, settings.TitanSizeMax.Value);
                titan.SetSize(size);
            }
            else
            {
                float size = UnityEngine.Random.Range(1f, 3f);
                titan.SetSize(size);
            }
            if (settings.TitanHealthMode.Value > 0)
            {
                if (settings.TitanHealthMode.Value == 1)
                {
                    int health = UnityEngine.Random.Range(settings.TitanHealthMin.Value, settings.TitanHealthMax.Value);
                    titan.SetHealth(health);
                }
                else if (settings.TitanHealthMode.Value == 2)
                {
                    float minSize = 1f;
                    float maxSize = 3f;
                    if (settings.TitanSizeEnabled.Value)
                    {
                        minSize = settings.TitanSizeMin.Value;
                        maxSize = settings.TitanSizeMax.Value;
                        maxSize = Mathf.Max(minSize, maxSize);
                    }
                    float size = Mathf.Clamp(titan.Size, minSize, maxSize);
                    float range = maxSize - minSize;
                    float scale = 0f;
                    if (range > 0f)
                        scale = (size - minSize) / range;
                    scale = Mathf.Clamp(scale, 0f, 1f);
                    int health = (int)(scale * (settings.TitanHealthMax.Value - settings.TitanHealthMin.Value) + settings.TitanHealthMin.Value);
                    health = Mathf.Max(health, 1);
                    titan.SetHealth(health);
                }
            }
            else if (!ai)
            {
                titan.SetHealth(10);
            }
        }

        public BaseShifter SpawnAIShifter(string type)
        {
            var spawn = GetTitanSpawnPoint();
            return SpawnAIShifterAt(type, spawn.position, spawn.rotation.eulerAngles.y);
        }

        public BaseShifter SpawnAIShifterAt(string type, Vector3 position, float rotationY)
        {
            var rotation = Quaternion.Euler(0f, rotationY, 0f);
            
            string prefab = "";
            if (type == "Annie")
                prefab = CharacterPrefabs.AnnieShifter;
            else if (type == "Armored")
                prefab = CharacterPrefabs.ArmoredShifter;
            else if (type == "Eren")
                prefab = CharacterPrefabs.ErenShifter;
            else if (type == "WallColossal")
                prefab = CharacterPrefabs.WallColossal;
            if (prefab == "")
                return null;
            var shifter = (BaseShifter)CharacterSpawner.Spawn(prefab, position, rotation);
            var data = CharacterData.GetShifterAI((GameDifficulty)SettingsManager.InGameCurrent.General.Difficulty.Value, type);
            shifter.Init(true, TeamInfo.Titan, data, 0f);
            return shifter;
        }

        public static void OnSetLabelRPC(string label, string message, float time, PhotonMessageInfo info)
        {
            if (info.Sender != PhotonNetwork.MasterClient)
                return;
            SetLabel(label, message, time);
        }

        public static void SetLabel(string label, string message, float time = 0f)
        {
            var menu = (InGameMenu)UIManager.CurrentMenu;
            menu.SetLabel(label, message, time);
        }

        public void EndGame(float time, PhotonMessageInfo info)
        {
            if (info.Sender != PhotonNetwork.MasterClient)
                return;
            if (!IsEnding)
            {
                IsEnding = true;
                EndTimeLeft = time;
                if (PhotonNetwork.IsMasterClient)
                    StartCoroutine(WaitAndEndGame(time));
                if (SettingsManager.UISettings.GameFeed.Value)
                {
                    float timestamp = CustomLogicManager.Evaluator.CurrentTime;
                    string feed = ChatManager.GetColorString("(" + Util.FormatFloat(timestamp, 2) + ")", ChatTextColor.System) + " Round ended.";
                    ChatManager.AddFeed(feed);
                }
            }
        }

        private IEnumerator WaitAndEndGame(float time)
        {
            yield return new WaitForSeconds(time);
            RestartGame();
        }

        private static void ResetPersistentPlayerProperties()
        {
            PhotonNetwork.LocalPlayer.CustomProperties.Clear();
            var properties = new Dictionary<string, object>
            {
                { PlayerProperty.Name, MyPlayerInfo.Profile.Name.Value.FilterBadWords().HexColor() },
                { PlayerProperty.Guild, MyPlayerInfo.Profile.Guild.Value.FilterBadWords().HexColor() },
                { PlayerProperty.Team, null },
                { PlayerProperty.CharacterViewId, -1 },
                { PlayerProperty.CustomMapHash, null },
                { PlayerProperty.CustomLogicHash, null },
                { PlayerProperty.Status, null },
                { PlayerProperty.Character, null },
                { PlayerProperty.Loadout, null },
                { PlayerProperty.Kills, 0 },
                { PlayerProperty.Deaths, 0 },
                { PlayerProperty.HighestDamage, 0 },
                { PlayerProperty.TotalDamage, 0 },
                { PlayerProperty.SpawnPoint, "null" }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }

        public static void ResetPlayerKD(Player player)
        {
            var kdrProperties = new Dictionary<string, object>
            {
                { PlayerProperty.Kills, 0 },
                { PlayerProperty.Deaths, 0 },
                { PlayerProperty.HighestDamage, 0 },
                { PlayerProperty.TotalDamage, 0 }
            };
            player.SetCustomProperties(kdrProperties);
        }
        private static void ResetRoundPlayerProperties()
        {
            if (SettingsManager.InGameCurrent.Misc.ClearKDROnRestart.Value)
            {
                var kdrProperties = new Dictionary<string, object>
                {
                    { PlayerProperty.Kills, 0 },
                    { PlayerProperty.Deaths, 0 },
                    { PlayerProperty.HighestDamage, 0 },
                    { PlayerProperty.TotalDamage, 0 }
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(kdrProperties);
            }
            var properties = new Dictionary<string, object>
            {
                { PlayerProperty.Status, PlayerStatus.Spectating },
                { PlayerProperty.CharacterViewId, -1 },
                { PlayerProperty.SpawnPoint, "null" }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }

        public static void UpdatePlayerName()
        {
            string name = MyPlayerInfo.Profile.Name.Value.FilterBadWords().HexColor();
            if (SettingsManager.InGameCurrent.Misc.PVP.Value == (int)PVPMode.Team)
            {
                if (SettingsManager.InGameCharacterSettings.Team.Value == TeamInfo.Blue)
                    name = ChatManager.GetColorString(name, ChatTextColor.TeamBlue);
                else if (SettingsManager.InGameCharacterSettings.Team.Value == TeamInfo.Red)
                    name = ChatManager.GetColorString(name, ChatTextColor.TeamRed);
            }
            PhotonNetwork.LocalPlayer.SetCustomProperty(PlayerProperty.Name, name);
        }

        public static void UpdateRoundPlayerProperties()
        {
            var manager = (InGameManager)SceneLoader.CurrentGameManager;
            string status;
            if (SettingsManager.InGameCharacterSettings.ChooseStatus.Value != (int)ChooseCharacterStatus.Chosen)
                status = PlayerStatus.Spectating;
            else if (manager.CurrentCharacter != null && !manager.CurrentCharacter.Dead)
                status = PlayerStatus.Alive;
            else
                status = PlayerStatus.Dead;
           
            var properties = new Dictionary<string, object>
            {
                { PlayerProperty.Status, status },
                { PlayerProperty.Character, SettingsManager.InGameCharacterSettings.CharacterType.Value },
                { PlayerProperty.Loadout, SettingsManager.InGameCharacterSettings.Loadout.Value },
                { PlayerProperty.Team, SettingsManager.InGameCharacterSettings.Team.Value }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }

        private static void ResetPlayerInfo()
        {
            AllPlayerInfo.Clear();
            MuteEmote.Clear();
            MuteText.Clear();
            PlayerInfo myPlayerInfo = new PlayerInfo();
            myPlayerInfo.Profile.Copy(SettingsManager.ProfileSettings);
            AllPlayerInfo.Add(PhotonNetwork.LocalPlayer.ActorNumber, myPlayerInfo);
            MyPlayerInfo = myPlayerInfo;
        }

        private static void PrintMOTD(string original)
        {
            if (original != SettingsManager.InGameCurrent.Misc.Motd.Value)
            {
                ChatManager.AddLine("MOTD: " + SettingsManager.InGameCurrent.Misc.Motd.Value, ChatTextColor.System);
            }
        }

        protected override void Awake()
        {
            _skyboxCustomSkinLoader = gameObject.AddComponent<SkyboxCustomSkinLoader>();
            //_forestCustomSkinLoader = gameObject.AddComponent<ForestCustomSkinLoader>();
            //_cityCustomSkinLoader = gameObject.AddComponent<CityCustomSkinLoader>();
            _generalInputSettings = SettingsManager.InputSettings.General;
            ResetRoundPlayerProperties();
            if (PhotonNetwork.IsMasterClient)
            {
                string original = SettingsManager.InGameCurrent.Misc.Motd.Value;
                SettingsManager.InGameCurrent.Copy(SettingsManager.InGameUI);
                PrintMOTD(original);
            }
            base.Awake();
        }

        protected override void Start()
        {
            _inGameMenu = (InGameMenu)UIManager.CurrentMenu;
            if (PhotonNetwork.IsMasterClient)
            {
                RPCManager.PhotonView.RPC("GameSettingsRPC", RpcTarget.All, new object[] { StringCompression.Compress(SettingsManager.InGameCurrent.SerializeToJsonString()) });
                var settings = SettingsManager.InGameCurrent;
                string mapName = settings.General.MapName.Value;
                string gameMode = settings.General.GameMode.Value;
                var properties = new ExitGames.Client.Photon.Hashtable
                {
                    { RoomProperty.Map, mapName },
                    { RoomProperty.GameMode, gameMode }
                };
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                LoadSkin();
            }
            PhotonNetwork.Instantiate("Game/PhotonVoicePrefab", Vector3.zero, Quaternion.identity, 0);
            base.Start();
            int currentPing = PhotonNetwork.GetPing();
            PhotonNetwork.LocalPlayer.SetCustomProperty(PlayerProperty.Ping, currentPing);
            PhotonNetwork.LocalPlayer.SetCustomProperty(PlayerProperty.SpectateID, -1);
        }

        public override bool IsFinishedLoading()
        {
            return base.IsFinishedLoading() && _gameSettingsLoaded;
        }

        private void Update()
        {
            if (State != GameState.Loading)
                UpdateInput();
            UpdateCleanCharacters();
            EndTimeLeft -= Time.deltaTime;
            EndTimeLeft = Mathf.Max(EndTimeLeft, 0f);

            timeSinceLastPingUpdate += Time.deltaTime;
            if (timeSinceLastPingUpdate >= pingUpdateInterval)
            {
                int currentPing = PhotonNetwork.GetPing();
                PhotonNetwork.LocalPlayer.SetCustomProperty(PlayerProperty.Ping, currentPing);
                timeSinceLastPingUpdate = 0f;
            }
        }

        protected override void OnFinishLoading()
        {
            base.OnFinishLoading();
            if (CustomLogicManager.Logic == BuiltinLevels.UseMapLogic)
                CustomLogicManager.Logic = MapManager.MapScript.Logic;
            else
                CustomLogicManager.Logic += MapManager.MapScript.Logic;
            UIManager.LoadingMenu.UpdateLoading(1f, true);
            if (State == GameState.Loading)
                State = GameState.Playing;
            if (SettingsManager.InGameCharacterSettings.ChooseStatus.Value == (int)ChooseCharacterStatus.Choosing)
                _inGameMenu.SetCharacterMenu(true);
            CustomLogicManager.StartLogic(SettingsManager.InGameCurrent.Mode.Current);
            if (_needSendPlayerInfo)
            {
                RPCManager.PhotonView.RPC("PlayerInfoRPC", RpcTarget.Others, new object[] { StringCompression.Compress(MyPlayerInfo.SerializeToJsonString()) });
                if (!PhotonNetwork.IsMasterClient)
                    RPCManager.PhotonView.RPC("NotifyPlayerJoinedRPC", RpcTarget.Others, new object[0]);
                _needSendPlayerInfo = false;
            }
            SpawnPlayer(false);
            if (SettingsManager.UISettings.GameFeed.Value)
            {
                float time = CustomLogicManager.Evaluator.CurrentTime;
                string feed = ChatManager.GetColorString("(" + Util.FormatFloat(time, 2) + ")", ChatTextColor.System) + " Round started.";
                ChatManager.AddFeed(feed);
            }
        }

        private void UpdateInput()
        {
            if (ChatManager.IsChatActive())
                return;
            if (_generalInputSettings.Pause.GetKeyDown())
                _inGameMenu.SetPauseMenu(true);
            if (_generalInputSettings.ChangeCharacter.GetKeyDown() && !InGameMenu.InMenu() && !CustomLogicManager.Cutscene && !Restarting && IsFinishedLoading())
            {
                if (CurrentCharacter != null && !CurrentCharacter.Dead)
                    CurrentCharacter.GetKilled("");
                SettingsManager.InGameCharacterSettings.ChooseStatus.Value = (int)ChooseCharacterStatus.Choosing;
                _inGameMenu.SetCharacterMenu(true);
            }
            if (_generalInputSettings.RestartGame.GetKeyDown() && PhotonNetwork.IsMasterClient && !_inGameMenu.IsPauseMenuActive())
                RestartGame();
            if (_generalInputSettings.TapScoreboard.Value)
            {
                if (_generalInputSettings.ToggleScoreboard.GetKeyDown())
                    _inGameMenu.ToggleScoreboardMenu();
            }
            else
            {
                if (_generalInputSettings.ToggleScoreboard.GetKey())
                    _inGameMenu.SetScoreboardMenu(true, false);
                else
                    _inGameMenu.SetScoreboardMenu(false, false);
            }
            if (_generalInputSettings.TapMap.Value)
            {
                if (_generalInputSettings.ToggleMap.GetKeyDown())
                    _inGameMenu.ToggleMapMenu();
            }
            else
            {
                if (_generalInputSettings.ToggleMap.GetKey())
                    _inGameMenu.SetMapMenu(true, false);
                else
                    _inGameMenu.SetMapMenu(false, false);
            }
            if (SettingsManager.InputSettings.General.HideUI.GetKeyDown() && !InGameMenu.InMenu() && !CustomLogicManager.Cutscene)
            {
                _inGameMenu.ToggleUI(!_inGameMenu.IsActive());
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                // TakePreviewScreenshot();
            }
        }

        private void TakePreviewScreenshot()
        {
            Texture2D texture = new Texture2D((int)1024, (int)1024, TextureFormat.RGB24, false);
            try
            {
                texture.SetPixel(0, 0, Color.white);
                texture.ReadPixels(new Rect(448, 28, 1024, 1024), 0, 0);
            }
            catch
            {
                texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, Color.white);
            }
            texture.Apply();
            TextureScaler.ScaleBlocking(texture, 256, 256);
            Directory.CreateDirectory(FolderPaths.Documents + "/MapPreviews");
            File.WriteAllBytes(FolderPaths.Documents + "/MapPreviews" + "/" + SettingsManager.InGameCurrent.General.MapName.Value + "Preview.png", texture.EncodeToPNG());
        }

        private void UpdateCleanCharacters()
        {
            Humans = Util.RemoveNullOrDead(Humans);
            Titans = Util.RemoveNullOrDead(Titans);
            Shifters = Util.RemoveNullOrDeadShifters(Shifters);
            Detections = Util.RemoveNullOrDeadDetections(Detections);
        }

        protected void LoadSkin()
        {
            if (PhotonNetwork.IsMasterClient && SettingsManager.CustomSkinSettings.Skybox.SkinsEnabled.Value && !SettingsManager.CustomSkinSettings.Skybox.SkinsLocal.Value)
            {
                var set = (SkyboxCustomSkinSet)SettingsManager.CustomSkinSettings.Skybox.GetSelectedSet();
                string urls = string.Join(",", new string[] {set.Front.Value, set.Back.Value , set.Left.Value , set.Right.Value ,
                                              set.Up.Value, set.Down.Value});
                RPCManager.PhotonView.RPC("LoadSkyboxRPC", RpcTarget.AllBuffered, new object[] { urls });
            }
            else if (PhotonNetwork.IsMasterClient)
            {
                // send empty strings for the values
                string urls = string.Join(",", new string[] {string.Empty, string.Empty, string.Empty , string.Empty ,
                                              string.Empty, string.Empty});
                RPCManager.PhotonView.RPC("LoadSkyboxRPC", RpcTarget.AllBuffered, new object[] { urls });
            }
        }

        public IEnumerator OnLoadSkyboxRPC(string[] urls)
        {
            var settings = SettingsManager.CustomSkinSettings.Skybox;
            if (settings.SkinsEnabled.Value && (!settings.SkinsLocal.Value || PhotonNetwork.IsMasterClient) && IsValidSkybox(urls))
            {
                yield return StartCoroutine(_skyboxCustomSkinLoader.LoadSkinsFromRPC(urls));
                StartCoroutine(ReloadSkybox());
            }
            else if (settings.SkinsEnabled.Value && settings.SkinsLocal.Value)
            {
                var set = (SkyboxCustomSkinSet)SettingsManager.CustomSkinSettings.Skybox.GetSelectedSet();
                string[] localUrls = new string[] { set.Front.Value, set.Back.Value, set.Left.Value, set.Right.Value, set.Up.Value, set.Down.Value };
                if (IsValidSkybox(localUrls))
                {
                    yield return StartCoroutine(_skyboxCustomSkinLoader.LoadSkinsFromRPC(localUrls));
                    StartCoroutine(ReloadSkybox());
                }
            }
        }

        protected IEnumerator ReloadSkybox()
        {
            yield return new WaitForSeconds(0.5f);
            var camera = (InGameCamera)SceneLoader.CurrentCamera;
            Material skyMaterial = SkyboxCustomSkinLoader.SkyboxMaterial;
            if (skyMaterial != null && camera.Skybox.material != skyMaterial)
                camera.Skybox.material = skyMaterial;
        }

        public IEnumerator OnLoadLevelSkinRPC(string indices, string urls1, string urls2)
        {
            yield break;
            /*
            var mapSettings = SettingsManager.InGameCurrent.General;
            while (!IsFinishedLoading())
                yield return null;
            if (mapSettings.MapCategory.Value == "General" && mapSettings.MapName.Value == "Forest")
            {
                BaseCustomSkinSettings<ForestCustomSkinSet> settings = SettingsManager.CustomSkinSettings.Forest;
                if (settings.SkinsEnabled.Value && (!settings.SkinsLocal.Value || PhotonNetwork.IsMasterClient))
                    yield return StartCoroutine(_forestCustomSkinLoader.LoadSkinsFromRPC(new object[] { indices, urls1, urls2 }));

            }
            else if (mapSettings.MapCategory.Value == "General" && mapSettings.MapName.Value == "City")
            {
                BaseCustomSkinSettings<CityCustomSkinSet> settings = SettingsManager.CustomSkinSettings.City;
                if (settings.SkinsEnabled.Value && (!settings.SkinsLocal.Value || PhotonNetwork.IsMasterClient))
                    yield return StartCoroutine(_cityCustomSkinLoader.LoadSkinsFromRPC(new object[] { indices, urls1, urls2 }));
            }
            */
        }

        private bool IsValidSkybox(string[] urls)
        {
            if (urls.Length != 6)
                return false;
            foreach (string url in urls)
            {
                if (TextureDownloader.ValidTextureURL(url))
                    return true;
            }
            return false;
        }
    }

    public enum GameState
    {
        Loading,
        Playing
    }
}
