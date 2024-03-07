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
using Photon.Voice;

namespace GameManagers
{
    class VoiceChatManager : MonoBehaviour
    {
        private static bool _keepTalking;
        public static PhotonVoiceView PVV;
        public static Recorder Recorder;
        public static AudioSource AudioSource;
        public static BaseCharacter character;
        
        private static float _audioMultiplier = 2.5f;
        private static float _proximitySpatialBlend = 1.0f;

        // Create a prop that returns the unity microphone device name list
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

        public static string[] VoiceChatModes = new string[] { "Global", "Proximity", "Off" };
        public static string[] VoiceChatInputModes = new string[] { "PushToTalk", "AutoDetect", "Off" };
     

        public static void Init()
        {
            // Create a PhotonVoiceClient if it doesn't exist
            if (FindObjectOfType<PunVoiceClient>() == null)
            {
                var go = new GameObject("PunVoiceClient");
                go.AddComponent<PunVoiceClient>();
                DontDestroyOnLoad(go);
            }
        }

        public static void SetupMyCharacterVoiceChat(BaseCharacter character)
        {
            PVV = character.PVV;
            Recorder = character.Recorder;
            AudioSource = character.AudioSource;
            VoiceChatManager.character = character;

            if (Recorder != null)
            {
                Recorder.UseMicrophoneTypeFallback = true;
                Recorder.LoopAudioClip = true;
                Recorder.VoiceDetectionThreshold = 0.01f;
                Recorder.FrameDuration = Photon.Voice.OpusCodec.FrameDuration.Frame20ms;
                Recorder.SamplingRate = POpusCodec.Enums.SamplingRate.Sampling24000;
                Recorder.VoiceDetectionDelayMs = 500;
            }

            ApplySoundSettings(character);
        }

        public static void ApplySoundSettings(BaseCharacter character)
        {
            if (character == null)
            {
                return;
            }

            if (character.IsMine())
            {
                // First change local player's sound settings (whether or not its transmitting, volume, etc)
                if (Recorder != null)
                {
                    Recorder.TransmitEnabled = SettingsManager.SoundSettings.VoiceChat.Value != "Off";
                    Recorder.VoiceDetection = SettingsManager.SoundSettings.VoiceChat.Value == "AutoDetect";
                    Recorder.MicrophoneType = Recorder.MicType.Unity;
                    Recorder.MicrophoneDevice = new DeviceInfo(SettingsManager.SoundSettings.VoiceChatDevice.Value);
                }
            }

            if (character.AudioSource != null)
            {
                if (SettingsManager.InGameCurrent.Misc.VoiceChatMode.Value == "Proximity")
                {
                    character.AudioSource.maxDistance = SettingsManager.InGameCurrent.Misc.ProximityMaxDistance.Value;
                    character.AudioSource.minDistance = SettingsManager.InGameCurrent.Misc.ProximityMinDistance.Value;
                    character.AudioSource.spatialBlend = _proximitySpatialBlend;
                }
                else
                {
                    character.AudioSource.spatialBlend = 0.0f;
                }

                if (character.IsMine())
                {
                    character.AudioSource.volume = GetMyVoiceChatVolume();
                }
                else
                {
                    character.AudioSource.volume = GetVoiceChatVolume(character);
                }
            }

        }

        public static void ApplySoundSettingsAll() 
        {
            // Cannot apply the settings unless we are in a game.
            if (SceneLoader.CurrentGameManager == null || !(SceneLoader.CurrentGameManager is InGameManager))
            {
                return;
            }

            // Then change all players sound settings (volume, spatial blend, etc)
            var players = ((InGameManager)SceneLoader.CurrentGameManager).GetAllNonAICharacters();
            foreach (BaseCharacter player in players)
            {
                ApplySoundSettings(player);
            }
        }

        private void Update()
        {
            if (PVV == null || character == null || Recorder == null)
            {
                return;
            }

            // Handle Push-To-Talk Logic
            if (SettingsManager.SoundSettings.VoiceChat.Value == "PushToTalk")
            {
                if (SettingsManager.InputSettings.General.PushToTalk.GetKeyDown())
                {
                    Recorder.TransmitEnabled = true;
                }
                else
                {
                        
                    Recorder.TransmitEnabled = false;
                }
            }

            // Needs fixing, emote system flickers when viewing non-local players - switch to a 2d side panel ui-based voice chat list.
            if (PVV.IsRecording && !_keepTalking)
            {
                RPCManager.PhotonView.RPC("EmoteVoiceRPC", RpcTarget.All, new object[] { character.Cache.PhotonView.ViewID, "Speaking" });
                _keepTalking = true;
            }
            else if (!PVV.IsSpeaking && _keepTalking)
            {
                RPCManager.PhotonView.RPC("StopVoiceRPC", RpcTarget.All, new object[] { });
                _keepTalking = false;
            }
        }

        public static float GetMyVoiceChatVolume()
        {
            if (SettingsManager.SoundSettings.VoiceChat.Value == "Off")
            {
                return 0f;
            }

            return SettingsManager.SoundSettings.VoiceChatMicVolume.Value * _audioMultiplier;
        }

        public static float GetVoiceChatVolume(BaseCharacter player)
        {
            var pActorID = player.Cache.PhotonView.OwnerActorNr;
            if (SettingsManager.SoundSettings.VoiceChat.Value == "Off" || InGameManager.MuteVoiceChat.Contains(pActorID))
            {
                return 0f;
            }
            
            float multiplier = 0.5f;
            if (InGameManager.VoiceChatVolumeMultiplier.ContainsKey(pActorID))
            {
                multiplier *= InGameManager.VoiceChatVolumeMultiplier[pActorID];
            }

            return SettingsManager.SoundSettings.VoiceChatAudioVolume.Value * _audioMultiplier * multiplier;
        }
    }
}