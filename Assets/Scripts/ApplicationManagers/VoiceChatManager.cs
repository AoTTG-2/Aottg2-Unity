using UnityEngine;
using ApplicationManagers;
using Characters;
using Settings;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using Photon.Voice;
using Utility;
using Unity.VisualScripting;
using GameManagers;

namespace ApplicationManagers
{
    class VoiceChatManager : MonoBehaviour
    {
        private static VoiceChatManager _instance;
        public static PunVoiceClient Client;
        public static float AudioMultiplier = 2.5f;
        public static float ProximitySpatialBlend = 1.0f;

        public static string[] MicrophoneDevices
        {
            get
            {
                return Microphone.devices;
            }
        }

        public static string DefaultDevice
        {   get
            {
                return Microphone.devices.Length > 0 ? Microphone.devices[0] : string.Empty;
            }
        }

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            Client = _instance.AddComponent<PunVoiceClient>();
            Client.AutoConnectAndJoin = false;
        }
        
        public static void ApplySoundSettings() 
        {
            if (SceneLoader.CurrentGameManager == null || !(SceneLoader.CurrentGameManager is InGameManager))
            {
                return;
            }
            foreach (var sync in ((InGameManager)SceneLoader.CurrentGameManager).PhotonVoiceSyncs)
            {
                if (sync != null)
                    sync.Apply();
            }
        }

        public static float GetMyVoiceChatVolume()
        {
            if (SettingsManager.SoundSettings.VoiceChatInput.Value == (int)VoiceChatInputMode.Off)
            {
                return 0f;
            }
            return SettingsManager.SoundSettings.VoiceChatMicVolume.Value * AudioMultiplier;
        }

        public static float GetOtherVoiceChatVolume(PhotonView view)
        {
            var pActorID = view.OwnerActorNr;
            if (SettingsManager.SoundSettings.VoiceChatInput.Value == (int)VoiceChatInputMode.Off || InGameManager.MuteVoiceChat.Contains(pActorID))
            {
                return 0f;
            }
            float multiplier = 0.5f;
            if (InGameManager.VoiceChatVolumeMultiplier.ContainsKey(pActorID))
            {
                multiplier = InGameManager.VoiceChatVolumeMultiplier[pActorID];
            }
            multiplier *= 2f;
            return SettingsManager.SoundSettings.VoiceChatAudioVolume.Value * AudioMultiplier * multiplier;
        }
    }
}