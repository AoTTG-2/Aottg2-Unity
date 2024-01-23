using System.Collections.Generic;
using UnityEngine;
using Weather;
using UI;
using Utility;
using CustomSkins;
using ApplicationManagers;
using System.Diagnostics;
using Characters;
using Settings;
using CustomLogic;
using Effects;
using Map;
using System.Collections;
using GameProgress;
using Cameras;
using System;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using Debug = UnityEngine.Debug;
using Events;
using SimpleJSONFixed;
using Photon.Voice.PUN;
using Photon.Voice.Unity;

namespace GameManagers
{
    class VoiceChatManager : MonoBehaviour
    {
        private static bool _keepTalking;
        public static PhotonVoiceView PV;
        public static BaseCharacter character;
        private static MusicManager _instance;
     

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
        }

        public static void ApplySoundSettings() 
        {
            var microphones = GameObject.FindGameObjectsWithTag("Speaker");
            foreach (GameObject MicObj in microphones)
            {
                if(MicObj != null) 
                {
                    AudioSource Mic = MicObj.GetComponent<AudioSource>();
                    ApplyAudioSettings(Mic);
                }
            }
        }

        private void Update()
        {
            if (PV != null && character != null)
            {
                if (PV.IsRecording && !_keepTalking)
                {
                    RPCManager.PhotonView.RPC("EmoteVoiceRPC", RpcTarget.All, new object[] { character.Cache.PhotonView.ViewID, "Speaking" });
                    _keepTalking = true;
                }
                else if (!PV.IsSpeaking && _keepTalking)
                {
                    RPCManager.PhotonView.RPC("StopVoiceRPC", RpcTarget.All, new object[] { });
                    _keepTalking = false;
                }
            }
        }

        private static void CheckStatus()
        {
            PV.GetComponentInParent<Recorder>().RecordingEnabled = SettingsManager.SoundSettings.VoiceChat.Value;
        }

        public static void ApplyAudioSettings(AudioSource Mic)
        {
            CheckStatus();
            Mic.volume = GetVoiceChatVolume();
            Mic.spatialBlend = GetTypeOfAudio();
        }

        public static float GetVoiceChatVolume()
        {
            return SettingsManager.SoundSettings.VoiceChat.Value ?
             SettingsManager.SoundSettings.VoiceChatVolume.Value * 2.5f :
             0f;
        }
        public static float GetTypeOfAudio()
        {
            return SettingsManager.SoundSettings.SpatialVoiceChat.Value ? 1 : 0;
        }

    }
}