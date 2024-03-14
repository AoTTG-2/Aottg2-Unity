namespace Photon.Voice.Unity.Demos.DemoVoiceUI
{
    using System.Collections.Generic;
    using Realtime;
    using ExitGames.Client.Photon;
    using UtilityScripts;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(UnityVoiceClient), typeof(ConnectAndJoin))]
    public class DemoVoiceUI : MonoBehaviour, IInRoomCallbacks, IMatchmakingCallbacks
    {
#pragma warning disable 649
        [SerializeField]
        private Text connectionStatusText;

        [SerializeField]
        private Text serverStatusText;

        [SerializeField]
        private Text roomStatusText;

        [SerializeField]
        private Text inputWarningText;

        [SerializeField]
        private Text rttText;

        [SerializeField]
        private Text rttVariationText;

        [SerializeField]
        private Text packetLossWarningText;

        [SerializeField]
        private InputField localNicknameText;

        [SerializeField]
        private Toggle debugEchoToggle;

        [SerializeField]
        private Toggle reliableTransmissionToggle;

        [SerializeField]
        private Toggle encryptionToggle;

        [SerializeField]
        private GameObject webRtcDspGameObject;

        [SerializeField]
        private Toggle aecToggle;

        [SerializeField]
        private Toggle aecHighPassToggle;

        [SerializeField]
        private InputField reverseStreamDelayInputField;

        [SerializeField]
        private Toggle noiseSuppressionToggle;

        [SerializeField]
        private Toggle agcToggle;

        [SerializeField]
        private Slider agcCompressionGainSlider;

        [SerializeField]
        private Slider agcTargetLevelSlider;

        [SerializeField]
        private Toggle vadToggle;

        [SerializeField]
        private Toggle muteToggle;

        [SerializeField]
        private Toggle streamAudioClipToggle;

        [SerializeField]
        private Toggle audioToneToggle;

        [SerializeField]
        private Toggle dspToggle;

        [SerializeField]
        private Toggle highPassToggle;

        [SerializeField]
        private Toggle photonVadToggle;

        [SerializeField]
        private MicrophoneSelector microphoneSelector;

        [SerializeField]
        private GameObject androidMicSettingGameObject;
        [SerializeField]
        private Toggle androidAgcToggle;
        [SerializeField]
        private Toggle androidAecToggle;
        [SerializeField]
        private Toggle androidNsToggle;

        [SerializeField]
        private bool defaultTransmitEnabled = false;

        [SerializeField]
        private bool fullScreen;

        [SerializeField]
        private InputField roomNameInputField;

        [SerializeField]
        private int rttYellowThreshold = 100;

        [SerializeField]
        private int rttRedThreshold = 160;

        [SerializeField]
        private int rttVariationYellowThreshold = 25;

        [SerializeField]
        private int rttVariationRedThreshold = 50;


#pragma warning restore 649

        private GameObject compressionGainGameObject;
        private GameObject targetLevelGameObject;
        private Text compressionGainText;
        private Text targetLevelText;

        private GameObject aecOptionsGameObject;

        public Transform RemoteVoicesPanel;

        protected UnityVoiceClient voiceConnection;
        private WebRtcAudioDsp voiceAudioPreprocessor;
        private ConnectAndJoin connectAndJoin;

        private readonly Color warningColor = new Color(0.9f, 0.5f, 0f, 1f);
        private readonly Color okColor = new Color(0.0f, 0.6f, 0.2f, 1f);
        private readonly Color redColor = new Color(1.0f, 0.0f, 0.0f, 1f);
        private readonly Color defaultColor = new Color(0.0f, 0.0f, 0.0f, 1f);

        private System.Func<IAudioDesc> toneInputFactory = () => new AudioUtil.ToneAudioReader<float>(null, 440, 48000, 2); // WebGL supports only Reader

        private void Start()
        {
            this.connectAndJoin = this.GetComponent<ConnectAndJoin>();
            this.voiceConnection = this.GetComponent<UnityVoiceClient>();
            this.voiceAudioPreprocessor = this.voiceConnection.PrimaryRecorder.GetComponent<WebRtcAudioDsp>();
            this.compressionGainGameObject = this.agcCompressionGainSlider.transform.parent.gameObject;
            this.compressionGainText = this.compressionGainGameObject.GetComponentInChildren<Text>();
            this.targetLevelGameObject = this.agcTargetLevelSlider.transform.parent.gameObject;
            this.targetLevelText = this.targetLevelGameObject.GetComponentInChildren<Text>();
            this.aecOptionsGameObject = this.aecHighPassToggle.transform.parent.gameObject;
            this.SetDefaults();
            this.InitUiCallbacks();
            this.GetSavedNickname();
            // tone is the default for InputSourceType.Factory although other factory sources may exist in the demo
            this.voiceConnection.PrimaryRecorder.InputFactory = toneInputFactory;

            this.voiceConnection.SpeakerLinked += this.OnSpeakerCreated;
            this.voiceConnection.Client.AddCallbackTarget(this);
        }

        protected virtual void SetDefaults()
        {
            this.muteToggle.isOn = !this.defaultTransmitEnabled;
        }

        private void OnDestroy()
        {
            this.voiceConnection.SpeakerLinked -= this.OnSpeakerCreated;
            this.voiceConnection.Client.RemoveCallbackTarget(this);
        }

        private void GetSavedNickname()
        {
            string savedNick = PlayerPrefs.GetString("vNick");
            if (!string.IsNullOrEmpty(savedNick))
            {
                //Debug.LogFormat("Saved nick = {0}", savedNick);
                this.localNicknameText.text = savedNick;
                this.voiceConnection.Client.NickName = savedNick;
            }
        }

        protected virtual void OnSpeakerCreated(Speaker speaker)
        {
            speaker.gameObject.transform.SetParent(this.RemoteVoicesPanel, false);
            RemoteSpeakerUI remoteSpeakerUi = speaker.GetComponent<RemoteSpeakerUI>();
            remoteSpeakerUi.Init(this.voiceConnection);
            speaker.OnRemoteVoiceRemoveAction += this.OnRemoteVoiceRemove;
        }

        private void OnRemoteVoiceRemove(Speaker speaker)
        {
            if (speaker != null)
            {
                Destroy(speaker.gameObject);
            }
        }

        private void ToggleMute(bool isOn) // transmit is used as opposite of mute...
        {
            this.muteToggle.targetGraphic.enabled = !isOn;
            if (isOn)
            {
                this.voiceConnection.Client.LocalPlayer.Mute();
            }
            else
            {
                this.voiceConnection.Client.LocalPlayer.Unmute();
            }
        }

        protected virtual void ToggleIsRecording(bool isRecording)
        {
            this.voiceConnection.PrimaryRecorder.RecordingEnabled = isRecording;
        }

        private void ToggleDebugEcho(bool isOn)
        {
            this.voiceConnection.PrimaryRecorder.DebugEchoMode = isOn;
        }

        private void ToggleReliable(bool isOn)
        {
            this.voiceConnection.PrimaryRecorder.ReliableMode = isOn;
        }

        private void ToggleEncryption(bool isOn)
        {
            this.voiceConnection.PrimaryRecorder.Encrypt = isOn;
        }

        private void ToggleAEC(bool isOn)
        {
            this.voiceAudioPreprocessor.AEC = isOn;
            this.aecOptionsGameObject.SetActive(isOn);
            this.voiceConnection.Client.LocalPlayer.SetAEC(isOn);
        }

        private void ToggleNoiseSuppression(bool isOn)
        {
            this.voiceAudioPreprocessor.NoiseSuppression = isOn;
        }

        private void ToggleAGC(bool isOn)
        {
            this.voiceAudioPreprocessor.AGC = isOn;
            this.compressionGainGameObject.SetActive(isOn);
            this.targetLevelGameObject.SetActive(isOn);

            this.voiceConnection.Client.LocalPlayer.SetAGC(isOn, this.voiceAudioPreprocessor.AgcCompressionGain, this.voiceAudioPreprocessor.AgcTargetLevel);
        }

        private void ToggleVAD(bool isOn)
        {
            this.voiceAudioPreprocessor.VAD = isOn;
            this.voiceConnection.Client.LocalPlayer.SetWebRTCVAD(isOn);
        }

        private void ToggleHighPass(bool isOn)
        {
            this.voiceAudioPreprocessor.HighPass = isOn;
        }

        private void ToggleDsp(bool isOn)
        {
            this.voiceAudioPreprocessor.enabled = isOn;
            this.voiceConnection.PrimaryRecorder.RestartRecording(); // restart to make 'enabled' change effective
            // or bypass the processor w/o need to restart the voice (the processor will still be initialized and added but bypassed on the next voice recreation)
            // see InitUiValues()
            // this.voiceAudioPreprocessor.Bypass = !isOn;

            this.webRtcDspGameObject.SetActive(isOn); // gui update

            this.voiceConnection.Client.LocalPlayer.SetWebRTCVAD(this.voiceAudioPreprocessor.VAD);
            this.voiceConnection.Client.LocalPlayer.SetAEC(this.voiceAudioPreprocessor.AEC);
            this.voiceConnection.Client.LocalPlayer.SetAGC(this.voiceAudioPreprocessor.AGC, this.voiceAudioPreprocessor.AgcCompressionGain, this.voiceAudioPreprocessor.AgcTargetLevel);
        }

        private void ToggleAudioClipStreaming(bool isOn)
        {
            //this.microphoneSelector.gameObject.SetActive(!isOn && !this.audioToneToggle.isOn);
            if (isOn)
            {
                this.audioToneToggle.SetValue(false);
                this.voiceConnection.PrimaryRecorder.SourceType = Recorder.InputSourceType.AudioClip;
            }
            else if (!this.audioToneToggle.isOn)
            {
                microphoneSelector.SwitchToSelectedMic();
            }
        }

        private void ToggleAudioToneFactory(bool isOn)
        {
            //this.microphoneSelector.gameObject.SetActive(!isOn && !this.streamAudioClipToggle.isOn);
            if (isOn)
            {
                this.streamAudioClipToggle.SetValue(false);
                this.voiceConnection.PrimaryRecorder.SourceType = Recorder.InputSourceType.Factory;
                this.voiceConnection.PrimaryRecorder.InputFactory = toneInputFactory;
            }
            else if (!this.streamAudioClipToggle.isOn)
            {
                microphoneSelector.SwitchToSelectedMic();
            }
        }

        private void TogglePhotonVAD(bool isOn)
        {
            this.voiceConnection.PrimaryRecorder.VoiceDetection = isOn;
            this.voiceConnection.Client.LocalPlayer.SetPhotonVAD(isOn);
        }

        private void ToggleAecHighPass(bool isOn)
        {
            this.voiceAudioPreprocessor.AecHighPass = isOn;
            this.voiceConnection.Client.LocalPlayer.SetAEC(isOn);
        }

        private void OnAgcCompressionGainChanged(float agcCompressionGain)
        {
            this.voiceAudioPreprocessor.AgcCompressionGain = (int)agcCompressionGain;
            this.compressionGainText.text = string.Concat("Compression Gain: ", agcCompressionGain);
            this.voiceConnection.Client.LocalPlayer.SetAGC(this.voiceAudioPreprocessor.AGC, (int)agcCompressionGain, this.voiceAudioPreprocessor.AgcTargetLevel);
        }

        private void OnAgcTargetLevelChanged(float agcTargetLevel)
        {
            this.voiceAudioPreprocessor.AgcTargetLevel = (int)agcTargetLevel;
            this.targetLevelText.text = string.Concat("Target Level: ", agcTargetLevel);
            this.voiceConnection.Client.LocalPlayer.SetAGC(this.voiceAudioPreprocessor.AGC, this.voiceAudioPreprocessor.AgcCompressionGain, (int)agcTargetLevel);
        }

        private void OnReverseStreamDelayChanged(string newReverseStreamString)
        {
            int newReverseStreamValue;
            if (int.TryParse(newReverseStreamString, out newReverseStreamValue) && newReverseStreamValue > 0)
            {
                this.voiceAudioPreprocessor.ReverseStreamDelayMs = newReverseStreamValue;
            }
            else
            {
                this.reverseStreamDelayInputField.text = this.voiceAudioPreprocessor.ReverseStreamDelayMs.ToString();
            }
        }

        private void OnMicrophoneChanged(Recorder.MicType micType, DeviceInfo deviceInfo)
        {
            this.voiceConnection.Client.LocalPlayer.SetMic(micType);

            this.androidMicSettingGameObject.SetActive(micType == Recorder.MicType.Photon);
        }

        private void OnAndroidMicSettingsChanged(bool isOn)
        {
            this.voiceConnection.PrimaryRecorder.SetAndroidNativeMicrophoneSettings(this.androidAecToggle.isOn, this.androidAgcToggle.isOn, this.androidNsToggle.isOn);
        }

        private void UpdateSyncedNickname(string nickname)
        {
            nickname = nickname.Trim();
            this.voiceConnection.Client.LocalPlayer.NickName = nickname;
            PlayerPrefs.SetString("vNick", nickname);
        }

        private void JoinOrCreateRoom(string roomName)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                this.connectAndJoin.RoomName = string.Empty;
                this.connectAndJoin.RandomRoom = true;
            }
            else
            {
                this.connectAndJoin.RoomName = roomName.Trim();
                this.connectAndJoin.RandomRoom = false;
            }
            if (this.voiceConnection.Client.InRoom)
            {
                this.voiceConnection.Client.OpLeaveRoom(false);
            }
            else if (!this.voiceConnection.Client.IsConnected)
            {
                this.voiceConnection.ConnectUsingSettings();
            }
        }

        void PhotonVoiceCreated(PhotonVoiceCreatedParams p)
        {
            this.InitUiValues();
        }

        protected virtual void Update()
        {
#if UNITY_EDITOR
            this.InitUiValues(); // refresh UI in case changed from Unity Editor
#endif
            this.connectionStatusText.text = this.voiceConnection.Client.State.ToString();
            this.serverStatusText.text = string.Format("{0}/{1}", this.voiceConnection.Client.CloudRegion, this.voiceConnection.Client.CurrentServerAddress);
            if (this.voiceConnection.PrimaryRecorder.IsCurrentlyTransmitting)
            {
                var amplitude = this.voiceConnection.PrimaryRecorder.LevelMeter.CurrentAvgAmp;
                if (amplitude > 1)
                {
                    amplitude /= (short.MaxValue + 1);
                }
                if (amplitude > 0.1)
                {
                    this.inputWarningText.text = "Input too loud!";
                    this.inputWarningText.color = this.warningColor;
                }
                else
                {
                    this.inputWarningText.text = string.Empty;
                    this.ResetTextColor(this.inputWarningText);
                }
            }

            if (this.voiceConnection.FramesReceivedPerSecond > 0)
            {
                this.packetLossWarningText.text = string.Format("{0:0.##}% Packet Loss", this.voiceConnection.FramesLostPercent);
                this.packetLossWarningText.color = this.voiceConnection.FramesLostPercent > 1 ? this.warningColor : this.okColor;
            }
            else
            {
                this.packetLossWarningText.text = string.Empty;
                this.ResetTextColor(this.packetLossWarningText);
            }

            this.rttText.text = string.Concat("RTT:", this.voiceConnection.Client.LoadBalancingPeer.RoundTripTime);
            this.SetTextColor(this.voiceConnection.Client.LoadBalancingPeer.RoundTripTime, this.rttText, this.rttYellowThreshold, this.rttRedThreshold);
            this.rttVariationText.text = string.Concat("VAR:", this.voiceConnection.Client.LoadBalancingPeer.RoundTripTimeVariance);
            this.SetTextColor(this.voiceConnection.Client.LoadBalancingPeer.RoundTripTimeVariance, this.rttVariationText, this.rttVariationYellowThreshold, this.rttVariationRedThreshold);
        }

        private void SetTextColor(int textValue, Text text, int yellowThreshold, int redThreshold)
        {
            if (textValue > redThreshold)
            {
                text.color = this.redColor;
            }
            else if (textValue > yellowThreshold)
            {
                text.color = this.warningColor;
            }
            else
            {
                text.color = this.okColor;
            }
        }

        private void ResetTextColor(Text text)
        {
            text.color = this.defaultColor;
        }

        private void InitUiCallbacks()
        {
            this.muteToggle.SetSingleOnValueChangedCallback(this.ToggleMute);
            this.debugEchoToggle.SetSingleOnValueChangedCallback(this.ToggleDebugEcho);
            this.reliableTransmissionToggle.SetSingleOnValueChangedCallback(this.ToggleReliable);
            this.encryptionToggle.SetSingleOnValueChangedCallback(this.ToggleEncryption);
            this.streamAudioClipToggle.SetSingleOnValueChangedCallback(this.ToggleAudioClipStreaming);
            this.audioToneToggle.SetSingleOnValueChangedCallback(this.ToggleAudioToneFactory);
            this.photonVadToggle.SetSingleOnValueChangedCallback(this.TogglePhotonVAD);

            this.vadToggle.SetSingleOnValueChangedCallback(this.ToggleVAD);
            this.aecToggle.SetSingleOnValueChangedCallback(this.ToggleAEC);
            this.agcToggle.SetSingleOnValueChangedCallback(this.ToggleAGC);
            this.dspToggle.SetSingleOnValueChangedCallback(this.ToggleDsp);
            this.highPassToggle.SetSingleOnValueChangedCallback(this.ToggleHighPass);
            this.aecHighPassToggle.SetSingleOnValueChangedCallback(this.ToggleAecHighPass);
            this.noiseSuppressionToggle.SetSingleOnValueChangedCallback(this.ToggleNoiseSuppression);
            this.agcCompressionGainSlider.SetSingleOnValueChangedCallback(this.OnAgcCompressionGainChanged);
            this.agcTargetLevelSlider.SetSingleOnValueChangedCallback(this.OnAgcTargetLevelChanged);

            this.localNicknameText.SetSingleOnEndEditCallback(this.UpdateSyncedNickname);
            this.roomNameInputField.SetSingleOnEndEditCallback(this.JoinOrCreateRoom);

            this.reverseStreamDelayInputField.SetSingleOnEndEditCallback(this.OnReverseStreamDelayChanged);

            //this.microphoneSelector.SetSingleOnValueChangedCallback(this.OnMicrophoneChanged);

            this.androidAgcToggle.SetSingleOnValueChangedCallback(this.OnAndroidMicSettingsChanged);
            this.androidAecToggle.SetSingleOnValueChangedCallback(this.OnAndroidMicSettingsChanged);
            this.androidNsToggle.SetSingleOnValueChangedCallback(this.OnAndroidMicSettingsChanged);
        }

        private void InitUiValues()
        {
            this.muteToggle.SetValue(this.voiceConnection.Client.LocalPlayer.IsMuted());
            this.debugEchoToggle.SetValue(this.voiceConnection.PrimaryRecorder.DebugEchoMode);
            this.reliableTransmissionToggle.SetValue(this.voiceConnection.PrimaryRecorder.ReliableMode);
            this.encryptionToggle.SetValue(this.voiceConnection.PrimaryRecorder.Encrypt);
            this.streamAudioClipToggle.SetValue(this.voiceConnection.PrimaryRecorder.SourceType == Recorder.InputSourceType.AudioClip);
            this.audioToneToggle.SetValue(this.voiceConnection.PrimaryRecorder.SourceType == Recorder.InputSourceType.Factory && this.voiceConnection.PrimaryRecorder.InputFactory == this.toneInputFactory); // may be also another custom factory
            this.photonVadToggle.SetValue(this.voiceConnection.PrimaryRecorder.VoiceDetection);

            this.androidAgcToggle.SetValue(this.voiceConnection.PrimaryRecorder.AndroidMicrophoneAGC);
            this.androidAecToggle.SetValue(this.voiceConnection.PrimaryRecorder.AndroidMicrophoneAEC);
            this.androidNsToggle.SetValue(this.voiceConnection.PrimaryRecorder.AndroidMicrophoneNS);

            //this.microphoneSelector.gameObject.SetActive(!this.streamAudioClipToggle.isOn && !this.audioToneToggle.isOn);

            if (this.webRtcDspGameObject != null)
            {
                this.dspToggle.gameObject.SetActive(true);
                this.dspToggle.SetValue(this.voiceAudioPreprocessor.enabled);
                // or bypass the processor w/o need to restart the voice (the processor will still be initialized and added but bypassed on the next voice recreation)
                // see ToggleDsp()
                // this.dspToggle.SetValue(!this.voiceAudioPreprocessor.Bypass);
                this.webRtcDspGameObject.SetActive(this.dspToggle.isOn);
                this.aecToggle.SetValue(this.voiceAudioPreprocessor.AEC);
                this.aecHighPassToggle.SetValue(this.voiceAudioPreprocessor.AecHighPass);
                this.reverseStreamDelayInputField.text = this.voiceAudioPreprocessor.ReverseStreamDelayMs.ToString();
                this.aecOptionsGameObject.SetActive(this.voiceAudioPreprocessor.AEC);
                this.noiseSuppressionToggle.isOn = this.voiceAudioPreprocessor.NoiseSuppression;
                this.agcToggle.SetValue(this.voiceAudioPreprocessor.AGC);
                this.agcCompressionGainSlider.SetValue(this.voiceAudioPreprocessor.AgcCompressionGain);
                this.agcTargetLevelSlider.SetValue(this.voiceAudioPreprocessor.AgcTargetLevel);
                this.compressionGainGameObject.SetActive(this.voiceAudioPreprocessor.AGC);
                this.targetLevelGameObject.SetActive(this.voiceAudioPreprocessor.AGC);
                this.vadToggle.SetValue(this.voiceAudioPreprocessor.VAD);
                this.highPassToggle.SetValue(this.voiceAudioPreprocessor.HighPass);
            }
            else
            {
                this.dspToggle.gameObject.SetActive(false);
            }
        }

        private void SetRoomDebugText()
        {
            string playerDebugString = string.Empty;
            if (this.voiceConnection.Client.InRoom)
            {
                foreach (Player p in this.voiceConnection.Client.CurrentRoom.Players.Values)
                {
                    playerDebugString = string.Concat(playerDebugString, p.ToStringFull());
                }
                this.roomStatusText.text = string.Format("{0} {1}", this.voiceConnection.Client.CurrentRoom.Name, playerDebugString);
            }
            else
            {
                this.roomStatusText.text = string.Empty;
            }
            this.roomStatusText.text = this.voiceConnection.Client.CurrentRoom == null ? string.Empty : string.Format("{0} {1}", this.voiceConnection.Client.CurrentRoom.Name, playerDebugString);
        }

        protected virtual void OnActorPropertiesChanged(Player targetPlayer, Hashtable changedProps)
        {
            if (targetPlayer.IsLocal)
            {
                bool isMuted = targetPlayer.IsMuted();
                this.voiceConnection.PrimaryRecorder.TransmitEnabled = !isMuted;
                this.muteToggle.SetValue(isMuted);
            }
            this.SetRoomDebugText();
        }

        protected void OnApplicationQuit()
        {
            this.voiceConnection.Client.RemoveCallbackTarget(this);
        }

        #region IInRoomCallbacks

        void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer)
        {
            this.SetRoomDebugText();
        }

        void IInRoomCallbacks.OnPlayerLeftRoom(Player otherPlayer)
        {
            this.SetRoomDebugText();
        }

        void IInRoomCallbacks.OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
        }

        void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            this.OnActorPropertiesChanged(targetPlayer, changedProps);
        }

        void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
        {
        }

        #endregion

        #region IMatchmakingCallbacks

        void IMatchmakingCallbacks.OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        void IMatchmakingCallbacks.OnCreatedRoom()
        {
        }

        void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)
        {
        }

        void IMatchmakingCallbacks.OnJoinedRoom()
        {
            this.SetRoomDebugText();

            this.voiceConnection.Client.LocalPlayer.SetMic(this.voiceConnection.PrimaryRecorder.MicrophoneType);

        }

        void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
        {
        }

        void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message)
        {
        }

        void IMatchmakingCallbacks.OnLeftRoom()
        {
            this.SetRoomDebugText();
            this.SetDefaults();
        }

        #endregion
    }
}