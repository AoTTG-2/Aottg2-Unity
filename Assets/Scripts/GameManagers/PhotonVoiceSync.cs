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

namespace GameManagers
{
    class PhotonVoiceSync : Photon.Pun.MonoBehaviourPun
    {
        public PhotonView PhotonView;
        public PhotonVoiceView VoiceView;
        public Recorder Recorder;
        public GameObject SpeakerObject;
        public AudioSource AudioSource;
        public Speaker Speaker;
        public Transform Transform;

        void Awake()
        {
            ((InGameManager)SceneLoader.CurrentGameManager).PhotonVoiceSyncs.Add(this);
            Transform = transform;
            PhotonView = GetComponent<PhotonView>();
            VoiceView = GetComponent<PhotonVoiceView>();
            Recorder = GetComponent<Recorder>();
            AudioSource = transform.Find("Speaker").GetComponent<AudioSource>();
            Speaker = transform.Find("Speaker").GetComponent<Speaker>();
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
            }
            if (SettingsManager.InGameCurrent.Misc.VoiceChat.Value == (int)VoiceChatMode.Proximity)
            {
                AudioSource.maxDistance = SettingsManager.InGameCurrent.Misc.ProximityMaxDistance.Value;
                AudioSource.minDistance = SettingsManager.InGameCurrent.Misc.ProximityMinDistance.Value;
                AudioSource.spatialBlend = VoiceChatManager.ProximitySpatialBlend;
            }
            else
                AudioSource.spatialBlend = 0.0f;
            if (PhotonView.IsMine)
                AudioSource.volume = VoiceChatManager.GetMyVoiceChatVolume();
            else
                AudioSource.volume = VoiceChatManager.GetOtherVoiceChatVolume(PhotonView);
        }

        private void Update()
        {
            if (!PhotonView.IsMine)
                return;
            var inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
            if (inGameManager.CurrentCharacter != null)
            {
                Transform.position = inGameManager.CurrentCharacter.GetCameraAnchor().position;
            }
            var setting = SettingsManager.InGameCurrent.Misc.VoiceChat.Value;
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
