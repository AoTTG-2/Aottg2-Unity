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
        public bool IsEnding;
        public float EndTimeLeft;
        public GameState State = GameState.Loading;
        public BaseCharacter CurrentCharacter;
        private bool _gameSettingsLoaded = false;
        public static Dictionary<int, PlayerInfo> AllPlayerInfo = new Dictionary<int, PlayerInfo>();
        public static HashSet<int> MuteEmote = new HashSet<int>();
        public static HashSet<int> MuteText = new HashSet<int>();
        public static PlayerInfo MyPlayerInfo = new PlayerInfo();
        private static bool _needSendPlayerInfo;
        public bool HasSpawned = false;
        public bool GlobalPause = false;
        public bool Restarting = false;
        public float PauseTimeLeft = -1f;

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

            InGameSet settingsui = SettingsManager.InGameUI;
            if (settingsui.General.SceneLoading.Value != "" && settingsui.General.MapCategory.Value == "Custom")
            {
                RPCManager.PhotonView.RPC("LoadSceneRPC", RpcTarget.AllBuffered, new object[] { settingsui.General.SceneLoading.Value });
            }
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
            ResetPersistentPlayerProperties();
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.DestroyAll();
            if (PhotonNetwork.IsConnected)
                PhotonNetwork.Disconnect();
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
                LeaveRoom();
                MainMenuGameManager.JustLeftRoom = true;
            }
        }

        public static void OnJoinRoom()
        {
            ResetPersistentPlayerProperties();
            ResetPlayerInfo();
            _needSendPlayerInfo = true;
            if (PhotonNetwork.OfflineMode)
                ChatManager.AddLine("Welcome to single player. \nType /help for a list of commands.", ChatTextColor.System);
            else
                ChatManager.AddLine("Welcome to " + PhotonNetwork.CurrentRoom.GetStringProperty(RoomProperty.Name).Trim() + ". \nType /help for a list of commands.", 
                    ChatTextColor.System);
            SceneLoader.LoadScene(SceneName.InGame);

            if (!PhotonNetwork.IsMasterClient) return;
            InGameSet settingsui = SettingsManager.InGameUI;
            if (settingsui.General.SceneLoading.Value != "" && settingsui.General.MapCategory.Value == "Custom")
            {
                RPCManager.PhotonView.RPC("LoadSceneRPC", RpcTarget.AllBuffered, new object[] { settingsui.General.SceneLoading.Value });
            }
        }

        public void RegisterMainCharacterDie()
        {
            UpdateRoundPlayerProperties();
            if (CurrentCharacter == null)
                return;
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
                GameProgressManager.RegisterHumanKill(CurrentCharacter.gameObject, (Human)victim, killWeapon);
            else if (victim is BasicTitan)
                GameProgressManager.RegisterTitanKill(CurrentCharacter.gameObject, (BasicTitan)victim, killWeapon);
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
            GameProgressManager.RegisterDamage(CurrentCharacter.gameObject, victim.gameObject, killWeapon, damage);
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
        }

        public void OnNotifyPlayerJoined(Player player)
        {
            CustomLogicManager.Evaluator.OnPlayerJoin(player);
            string line = player.GetCustomProperty(PlayerProperty.Name) + ChatManager.GetColorString(" has joined the room.", ChatTextColor.System);
            ChatManager.AddLine(line);
        }

        public override void OnPlayerLeftRoom(Player player)
        {
            string line = player.GetCustomProperty(PlayerProperty.Name) + ChatManager.GetColorString(" has left the room.", ChatTextColor.System);
            ChatManager.AddLine(line);
            if (CustomLogicManager.Evaluator != null)
                CustomLogicManager.Evaluator.OnPlayerLeave(player);
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
            AllPlayerInfo[info.Sender.ActorNumber].DeserializeFromJsonString(StringCompression.Decompress(data));
        }

        public static void OnGameSettingsRPC(byte[] data, PhotonMessageInfo info)
        {
            if (!info.Sender.IsMasterClient)
                return;
            string original = SettingsManager.InGameCurrent.Misc.Motd.Value;
            SettingsManager.InGameCurrent.DeserializeFromJsonString(StringCompression.Decompress(data));
            ((InGameManager)SceneLoader.CurrentGameManager)._gameSettingsLoaded = true;
            if (SettingsManager.InGameCurrent.Misc.EndlessRespawnEnabled.Value)
            {
                var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
                gameManager.StartCoroutine(gameManager.RespawnForever(SettingsManager.InGameCurrent.Misc.EndlessRespawnTime.Value));
            }
            PrintMOTD(original);
        }

        public void SpawnPlayer(bool force)
        {
            if (!IsFinishedLoading())
                return;
            
            var isHuman = SettingsManager.InGameCharacterSettings.CharacterType.Value == PlayerCharacter.Human;

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
            
            if (shifterName == "Annie")
            {
                var shifter = (AnnieShifter)CharacterSpawner.Spawn(CharacterPrefabs.AnnieShifter, position, rotation);
                shifter.Init(false, GetPlayerTeam(false), null, liveTime);
                CurrentCharacter = shifter;
            }
            else if (shifterName == "Eren")
            {
                var shifter = (ErenShifter)CharacterSpawner.Spawn(CharacterPrefabs.ErenShifter, position, rotation);
                shifter.Init(false, GetPlayerTeam(false), null, liveTime);
                CurrentCharacter = shifter;
            }
            else if (shifterName == "Armored")
            {
                var shifter = (ArmoredShifter)CharacterSpawner.Spawn(CharacterPrefabs.ArmoredShifter, position, rotation);
                shifter.Init(false, GetPlayerTeam(false), null, liveTime);
                CurrentCharacter = shifter;
            }
        }

        public void SpawnPlayerAt(bool force, Vector3 position, float rotationY)
        {
            var rotation = Quaternion.Euler(0f, rotationY, 0f);
            
            if (!IsFinishedLoading())
                return;
            var settings = SettingsManager.InGameCharacterSettings;
            var character = settings.CharacterType.Value;
            var miscSettings = SettingsManager.InGameCurrent.Misc;
            if (settings.ChooseStatus.Value != (int)ChooseCharacterStatus.Chosen)
                return;
            if (CurrentCharacter != null && !CurrentCharacter.Dead && !force)
                return;
            if (CurrentCharacter != null && !CurrentCharacter.Dead)
                CurrentCharacter.GetKilled("");
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
                    loadouts.Add(HumanLoadout.Blades);
                if (miscSettings.AllowAHSS.Value)
                    loadouts.Add(HumanLoadout.AHSS);
                if (miscSettings.AllowAPG.Value)
                    loadouts.Add(HumanLoadout.APG);
                if (miscSettings.AllowThunderspears.Value)
                    loadouts.Add(HumanLoadout.Thunderspears);
                if (loadouts.Count == 0)
                    loadouts.Add(HumanLoadout.Blades);
                if (!loadouts.Contains(settings.Loadout.Value))
                    settings.Loadout.Value = loadouts[0];
                List<string> specials = HumanSpecials.GetSpecialNames(settings.Loadout.Value, miscSettings.AllowShifterSpecials.Value);
                if (!specials.Contains(settings.Special.Value))
                    settings.Special.Value = specials[0];
                var human = (Human)CharacterSpawner.Spawn(CharacterPrefabs.Human, position, rotation);
                human.Init(false, GetPlayerTeam(false), SettingsManager.InGameCharacterSettings);
                CurrentCharacter = human;
            }
            else if (character == PlayerCharacter.Shifter)
                SpawnPlayerShifterAt(settings.Loadout.Value, 0f, position, rotationY);
            else if (character == PlayerCharacter.Titan)
            {
                int[] combo = BasicTitanSetup.GetRandomBodyHeadCombo();
                string prefab = CharacterPrefabs.BasicTitanPrefix + combo[0];
                var titan = (BasicTitan)CharacterSpawner.Spawn(prefab, position, rotation);
                titan.Init(false, GetPlayerTeam(true), null, combo[1]);
                SetupTitan(titan);
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
            RPCManager.PhotonView.RPC("NotifyPlayerSpawnRPC", RpcTarget.All, new object[] { CurrentCharacter.Cache.PhotonView.ViewID });
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
            var data = CharacterData.GetTitanAI((GameDifficulty)SettingsManager.InGameCurrent.General.Difficulty.Value, type);
            int[] combo = BasicTitanSetup.GetRandomBodyHeadCombo(data);
            string prefab = CharacterPrefabs.BasicTitanPrefix + combo[0];
            var titan = (BasicTitan)CharacterSpawner.Spawn(prefab, position, rotation);
            titan.Init(true, TeamInfo.Titan, data, combo[1]);
            SetupTitan(titan);
            return titan;
        }

        public void SetupTitan(BasicTitan titan)
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
                { PlayerProperty.Name, MyPlayerInfo.Profile.Name.Value.HexColor() },
                { PlayerProperty.Guild, MyPlayerInfo.Profile.Guild.Value.HexColor() },
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
            string name = MyPlayerInfo.Profile.Name.Value.HexColor();
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
                    { RoomProperty.Name, PhotonNetwork.CurrentRoom.GetStringProperty(RoomProperty.Name) },
                    { RoomProperty.Map, mapName },
                    { RoomProperty.GameMode, gameMode },
                    { RoomProperty.Password, PhotonNetwork.CurrentRoom.GetStringProperty(RoomProperty.Password) }
                };
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                LoadSkin();
            }
            base.Start();
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
        }

        protected override void OnFinishLoading()
        {
            base.OnFinishLoading();
            if (CustomLogicManager.Logic == BuiltinLevels.UseMapLogic)
                CustomLogicManager.Logic = MapManager.MapScript.Logic;
            else
                CustomLogicManager.Logic += MapManager.MapScript.Logic;
            if (_needSendPlayerInfo)
            {
                RPCManager.PhotonView.RPC("PlayerInfoRPC", RpcTarget.Others, new object[] { StringCompression.Compress(MyPlayerInfo.SerializeToJsonString()) });
                if (!PhotonNetwork.IsMasterClient)
                    RPCManager.PhotonView.RPC("NotifyPlayerJoinedRPC", RpcTarget.Others, new object[0]);
                _needSendPlayerInfo = false;
            }
            UIManager.LoadingMenu.UpdateLoading(1f, true);
            if (State == GameState.Loading)
                State = GameState.Playing;
            if (SettingsManager.InGameCharacterSettings.ChooseStatus.Value == (int)ChooseCharacterStatus.Choosing)
                _inGameMenu.SetCharacterMenu(true);
            CustomLogicManager.StartLogic(SettingsManager.InGameCurrent.Mode.Current);
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
            if (_generalInputSettings.Pause.GetKeyDown() && !InGameMenu.InMenu())
                _inGameMenu.SetPauseMenu(true);
            if (_generalInputSettings.ChangeCharacter.GetKeyDown() && !InGameMenu.InMenu() && !CustomLogicManager.Cutscene)
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
                    _inGameMenu.SetScoreboardMenu(true);
                else
                    _inGameMenu.SetScoreboardMenu(false);
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
        }

        protected void LoadSkin()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (SettingsManager.CustomSkinSettings.Skybox.SkinsEnabled.Value)
                {
                    var set = (SkyboxCustomSkinSet)SettingsManager.CustomSkinSettings.Skybox.GetSelectedSet();
                    string urls = string.Join(",", new string[] {set.Front.Value, set.Back.Value , set.Left.Value , set.Right.Value ,
                                              set.Up.Value, set.Down.Value});
                    RPCManager.PhotonView.RPC("LoadSkyboxRPC", RpcTarget.AllBuffered, new object[] { urls });
                }
                /*
                string indices = string.Empty;
                string urls1 = string.Empty;
                string urls2 = string.Empty;
                bool send = false;
                var settings = SettingsManager.InGameCurrent.General;
                if (settings.MapCategory.Value == "General" && settings.MapName.Value == "City" && SettingsManager.CustomSkinSettings.City.SkinsEnabled.Value)
                {
                    CityCustomSkinSet set = (CityCustomSkinSet)SettingsManager.CustomSkinSettings.City.GetSelectedSet();
                    List<string> houses = new List<string>();
                    foreach (StringSetting house in set.Houses.GetItems())
                        houses.Add(house.Value);
                    urls1 = string.Join(",", houses.ToArray());
                    for (int i = 0; i < 300; i++)
                        indices = indices + Convert.ToString((int)UnityEngine.Random.Range(0f, 8f));
                    urls2 = string.Join(",", new string[] { set.Ground.Value, set.Wall.Value, set.Gate.Value });
                    if (urls1.Replace(",", "").Trim() != "" || urls2.Replace(",", "").Trim() != "")
                        send = true;
                }
                else if (settings.MapCategory.Value == "General" && settings.MapName.Value == "Forest" && SettingsManager.CustomSkinSettings.Forest.SkinsEnabled.Value)
                {
                    ForestCustomSkinSet set = (ForestCustomSkinSet)SettingsManager.CustomSkinSettings.Forest.GetSelectedSet();
                    List<string> trees = new List<string>();
                    foreach (StringSetting tree in set.TreeTrunks.GetItems())
                        trees.Add(tree.Value);
                    urls1 = string.Join(",", trees.ToArray());
                    List<string> leafs = new List<string>();
                    foreach (StringSetting leaf in set.TreeLeafs.GetItems())
                        leafs.Add(leaf.Value);
                    leafs.Add(set.Ground.Value);
                    urls2 = string.Join(",", leafs.ToArray());
                    for (int i = 0; i < 150; i++)
                    {
                        string str = Convert.ToString((int)UnityEngine.Random.Range((float)0f, (float)8f));
                        indices = indices + str;
                        if (!set.RandomizedPairs.Value)
                            indices = indices + str;
                        else
                            indices = indices + Convert.ToString((int)UnityEngine.Random.Range((float)0f, (float)8f));
                    }
                    if (urls1.Replace(",", "").Trim() != "" || urls2.Replace(",", "").Trim() != "")
                        send = true;
                }
                if (send)
                    RPCManager.PhotonView.RPC("LoadLevelSkinRPC", RpcTarget.AllBuffered, new object[] { indices, urls1, urls2 });
                */
            }
        }

        private IEnumerator RespawnForever(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                SpawnPlayer(false);
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
