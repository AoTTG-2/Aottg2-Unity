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
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using Settings;
using Photon.Voice;
using Photon.Realtime;
using Utility;
using Photon.Voice.Unity.UtilityScripts;

namespace GameManagers
{
    class PhotonVoiceSync : Photon.Pun.MonoBehaviourPunCallbacks
    {
        public PhotonView PhotonView;
        public PhotonVoiceView VoiceView;
        public Recorder Recorder;
        public GameObject SpeakerObject;
        public AudioSource AudioSource;
        public Speaker Speaker;
        public Transform Transform;
        public MicAmplifier MicAmplifier;

        void Awake()
        {
            ((InGameManager)SceneLoader.CurrentGameManager).PhotonVoiceSyncs.Add(this);
            Transform = transform;
            PhotonView = GetComponent<PhotonView>();
            VoiceView = GetComponent<PhotonVoiceView>();
            Recorder = GetComponent<Recorder>();
            AudioSource = transform.Find("Speaker").GetComponent<AudioSource>();
            Speaker = transform.Find("Speaker").GetComponent<Speaker>();
            MicAmplifier = GetComponent<MicAmplifier>();
            if (PhotonView.IsMine)
            {
                Recorder.UseMicrophoneTypeFallback = true;
                Recorder.LoopAudioClip = true;
                Recorder.VoiceDetectionThreshold = 0.01f;
                Recorder.FrameDuration = Photon.Voice.OpusCodec.FrameDuration.Frame20ms;
                Recorder.SamplingRate = POpusCodec.Enums.SamplingRate.Sampling24000;
                Recorder.VoiceDetectionDelayMs = 500;
            }
            Apply();
        }

        public void Apply()
        {
            if (PhotonView.IsMine)
            {
                Recorder.TransmitEnabled = false;
                Recorder.VoiceDetection = SettingsManager.SoundSettings.VoiceChatInput.Value == (int)VoiceChatInputMode.AutoDetect;
                Recorder.MicrophoneType = Recorder.MicType.Unity;
                Recorder.MicrophoneDevice = new DeviceInfo(SettingsManager.SoundSettings.VoiceChatDevice.Value);
                MicAmplifier.AmplificationFactor = VoiceChatManager.GetInputVolume();
            }
            if (SettingsManager.InGameCurrent.Misc.VoiceChat.Value == (int)VoiceChatMode.Proximity)
            {
                AudioSource.maxDistance = SettingsManager.InGameCurrent.Misc.ProximityMaxDistance.Value;
                AudioSource.minDistance = SettingsManager.InGameCurrent.Misc.ProximityMinDistance.Value;
                AudioSource.spatialBlend = VoiceChatManager.ProximitySpatialBlend;
            }
            else
                AudioSource.spatialBlend = 0.0f;
        }

        private void Update()
        {
            var inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
            var setting = SettingsManager.InGameCurrent.Misc.VoiceChat.Value;
            if (PhotonView.IsMine)
            {
                if (inGameManager.CurrentCharacter != null)
                {
                    Transform.position = inGameManager.CurrentCharacter.GetCameraAnchor().position;
                }
                bool alive = inGameManager.CurrentCharacter != null && !inGameManager.CurrentCharacter.Dead;
                if (setting == (int)VoiceChatMode.Off || (setting == (int)VoiceChatMode.Proximity && !alive))
                {
                    Recorder.TransmitEnabled = false;
                }
                else if (SettingsManager.SoundSettings.VoiceChatInput.Value == (int)VoiceChatInputMode.PushToTalk)
                {
                    if (SettingsManager.InputSettings.General.PushToTalk.GetKey())
                        Recorder.TransmitEnabled = true;
                    else
                        Recorder.TransmitEnabled = false;
                }
                else if (SettingsManager.SoundSettings.VoiceChatInput.Value == (int)VoiceChatInputMode.AutoDetect)
                    Recorder.TransmitEnabled = true;
                AudioSource.volume = 1f;
            }
            else
            {
                if (setting == (int)VoiceChatMode.Off)
                    AudioSource.volume = 0f;
                else
                    AudioSource.volume = VoiceChatManager.GetOuputVolume(PhotonView);
            }
        }

        private void LateUpdate()
        {
            if (ChatManager.IsChatAvailable())
            {
                if (PhotonView.IsMine)
                {
                    ChatManager.IsTalking(this.photonView.Owner, VoiceView.IsRecording);
                }
                else
                {
                    bool isSpeaking = VoiceView.IsSpeaking;
                    if (SettingsManager.InGameCurrent.Misc.VoiceChat.Value == (int)VoiceChatMode.Proximity)
                    {
                        var mainCharacter = ((InGameManager)SceneLoader.CurrentGameManager).CurrentCharacter;
                        if (mainCharacter != null)
                        {
                            var distance = Vector3.Distance(mainCharacter.Cache.Transform.position, Transform.position);
                            var volume = this.AudioSource.volume;
                            isSpeaking = isSpeaking && volume > 0f && distance <= SettingsManager.InGameCurrent.Misc.ProximityMaxDistance.Value;
                        }
                    }
                    ChatManager.IsTalking(this.photonView.Owner, isSpeaking);
                }
            }
        }
    }
}
