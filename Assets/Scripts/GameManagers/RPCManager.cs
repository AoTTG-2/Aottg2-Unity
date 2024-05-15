using System.Collections.Generic;
using UnityEngine;
using Weather;
using UI;
using Map;
using Effects;
using CustomLogic;
using ApplicationManagers;
using Characters;
using Photon.Pun;
using Spawnables;
using UnityEngine.SceneManagement;

namespace GameManagers
{
    class RPCManager : Photon.Pun.MonoBehaviourPun
    {
        public static PhotonView PhotonView;

        [PunRPC]
        public void TransferLogicRPC(byte[][] strArray, int msgNumber, int msgTotal, PhotonMessageInfo info)
        {
            CustomLogicTransfer.OnTransferLogicRPC(strArray, msgNumber, msgTotal, info);
        }

        [PunRPC]
        public void LoadBuiltinLogicRPC(string name, PhotonMessageInfo info)
        {
            CustomLogicManager.OnLoadBuiltinLogicRPC(name, info);
        }

        [PunRPC]
        public void LoadCachedLogicRPC(PhotonMessageInfo info)
        {
            CustomLogicManager.OnLoadCachedLogicRPC(info);
        }

        [PunRPC]
        public void TransferMapRPC(byte[][] strArray, int msgNumber, int msgTotal, PhotonMessageInfo info)
        {
            MapTransfer.OnTransferMapRPC(strArray, msgNumber, msgTotal, info);
        }

        [PunRPC]
        public void LoadBuiltinMapRPC(string category, string name, PhotonMessageInfo info)
        {
            MapManager.OnLoadBuiltinMapRPC(category, name, info);
        }

        [PunRPC]
        public void LoadCachedMapRPC(PhotonMessageInfo info)
        {
            MapManager.OnLoadCachedMapRPC(info);
        }

        [PunRPC]
        public void LoadSkyboxRPC(string urls, PhotonMessageInfo info)
        {
            var manager = (InGameManager)SceneLoader.CurrentGameManager;
            string[] urlArr = urls.Split(',');
            if (info.Sender == PhotonNetwork.MasterClient)
                manager.StartCoroutine(manager.OnLoadSkyboxRPC(urlArr));
        }

        [PunRPC]
        public void LoadLevelSkinRPC(string indices, string urls1, string urls2, PhotonMessageInfo info)
        {
            var manager = (InGameManager)SceneLoader.CurrentGameManager;
            if (info.Sender == PhotonNetwork.MasterClient)
                manager.StartCoroutine(manager.OnLoadLevelSkinRPC(indices, urls1, urls2));
        }

        [PunRPC]
        public void RestartGameRPC(PhotonMessageInfo info)
        {
            InGameManager.OnRestartGameRPC(info);
        }

        [PunRPC]
        public void PreRestartGameRPC(bool immediate, PhotonMessageInfo info)
        {
            InGameManager.OnPreRestartGameRPC(immediate, info);
        }

        [PunRPC]
        public void PauseGameRPC(PhotonMessageInfo info)
        {
            ((InGameManager)SceneLoader.CurrentGameManager).OnPauseGameRPC(info);
        }

        [PunRPC]
        public void StartUnpauseGameRPC(PhotonMessageInfo info)
        {
            ((InGameManager)SceneLoader.CurrentGameManager).OnStartUnpauseGameRPC(info);
        }

        [PunRPC]
        public void UnpauseGameRPC(PhotonMessageInfo info)
        {
            ((InGameManager)SceneLoader.CurrentGameManager).OnUnpauseGameRPC(info);
        }

        [PunRPC]
        public void PlayerInfoRPC(byte[] data, PhotonMessageInfo info)
        {
            InGameManager.OnPlayerInfoRPC(data, info);
        }

        [PunRPC]
        public void GameSettingsRPC(byte[] data, PhotonMessageInfo info)
        {
            InGameManager.OnGameSettingsRPC(data, info);
        }

        [PunRPC]
        public void SetWeatherRPC(byte[] currentWeatherJson, byte[] startWeatherJson, byte[] targetWeatherJson, Dictionary<int, float> targetWeatherStartTimes,
            Dictionary<int, float> targetWeatherEndTimes, float currentTime, PhotonMessageInfo info)
        {
            WeatherManager.OnSetWeatherRPC(currentWeatherJson, startWeatherJson, targetWeatherJson, targetWeatherStartTimes, targetWeatherEndTimes, currentTime, info);
        }

        [PunRPC]
        public void EmoteEmojiRPC(int viewId, string emoji, PhotonMessageInfo info)
        {
            EmoteHandler.OnEmoteEmojiRPC(viewId, emoji, info);
        }

        [PunRPC]
        public void EmoteTextRPC(int viewId, string text, PhotonMessageInfo info)
        {
            EmoteHandler.OnEmoteTextRPC(viewId, text, info);
        }

        [PunRPC]
        public void SpawnEffectRPC(string name, Vector3 position, Quaternion rotation, float scale, bool scaleSize, object[] settings, PhotonMessageInfo info)
        {
            EffectSpawner.OnSpawnEffectRPC(name, position, rotation, scale, scaleSize, settings, info);
        }

        [PunRPC]
        public void SpawnSpawnableRPC(string name, Vector3 position, Quaternion rotation, float scale, object[] settings, PhotonMessageInfo info)
        {
            SpawnableSpawner.OnSpawnSpawnableRPC(name, position, rotation, scale, settings, info);
        }

        [PunRPC]
        public void SetLabelRPC(string label, string message, float time, PhotonMessageInfo info)
        {
            InGameManager.OnSetLabelRPC(label, message, time, info);
        }

        [PunRPC]
        public void ShowKillFeedRPC(string killer, string victim, int score, string weapon, PhotonMessageInfo info)
        {
            ((InGameMenu)UIManager.CurrentMenu).ShowKillFeed(killer, victim, score, weapon);
        }

        [PunRPC]
        public void EndGameRPC(float time, PhotonMessageInfo info)
        {
            ((InGameManager)SceneLoader.CurrentGameManager).EndGame(time, info);
        }

        [PunRPC]
        public void NotifyPlayerJoinedRPC(PhotonMessageInfo info)
        {
            ((InGameManager)SceneLoader.CurrentGameManager).OnNotifyPlayerJoined(info.Sender);
        }

        [PunRPC]
        public void TransferNetworkViewRPC(int id, PhotonMessageInfo info)
        {
            var go = PhotonNetwork.Instantiate("Game/CustomLogicPhotonSyncPrefab", Vector3.zero, Quaternion.identity, 0);
            var photonView = go.GetComponent<CustomLogicPhotonSync>();
            photonView.Init(id);
        }

        [PunRPC]
        public void SendMessageRPC(string message, PhotonMessageInfo info)
        {
            CustomLogicManager.Evaluator.OnNetworkMessage(info.Sender, message);
        }

        [PunRPC]
        public void NotifyPlayerSpawnRPC(int viewId, PhotonMessageInfo info)
        {
            var view = PhotonView.Find(viewId);
            if (view != null && view.Owner == info.Sender && CustomLogicManager.Evaluator != null)
            {
                var character = view.GetComponent<BaseCharacter>();
                CustomLogicManager.Evaluator.OnPlayerSpawn(info.Sender, character);
            }
        }

        [PunRPC]
        public void SpawnPlayerRPC(bool force, PhotonMessageInfo info)
        {
            if (info.Sender.IsMasterClient)
            {
                ((InGameManager)SceneLoader.CurrentGameManager).SpawnPlayer(force);
            }
        }

        [PunRPC]
        public void SpawnPlayerAtRPC(bool force, Vector3 position, float rotationY, PhotonMessageInfo info)
        {
            if (info.Sender.IsMasterClient)
            {
                ((InGameManager)SceneLoader.CurrentGameManager).SpawnPlayerAt(force, position, rotationY);
            }
        }

        [PunRPC]
        public void SyncCurrentTimeRPC(float time, PhotonMessageInfo info)
        {
            if (info.Sender.IsMasterClient && CustomLogicManager.Evaluator != null)
            {
                CustomLogicManager.Evaluator.CurrentTime = time;
            }
        }

        [PunRPC]
        public void ChatRPC(string message, PhotonMessageInfo info)
        {
            ChatManager.OnChatRPC(message, info);
        }

        [PunRPC]
        public void TestRPC(Color c)
        {
            Debug.Log(c);
        }

        #region Expedition RPCs

        [PunRPC]
        public void LoadSceneRPC(string SceneName, PhotonMessageInfo info)
        {
            if (!info.Sender.IsMasterClient) return;
            if (!EmVariables.IsSceneInBuild(SceneName)) return;

            SceneLoader.CustomSceneLoad = true;
            SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);

            ChatManager.AddLine($"Scene {SceneName} Loaded!");
        }

        #endregion

        void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
        }
    }
}
