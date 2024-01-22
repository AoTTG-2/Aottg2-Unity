namespace Photon.Voice.Unity.Demos.DemoVoiceUI
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Serialization;
    using UnityEngine.UI;

    public enum MicType
    {
        Unity,
        Photon,
        FMOD
    }

    public struct MicRef
    {

        public readonly MicType MicType;
        public readonly DeviceInfo Device;

        public MicRef(MicType micType, DeviceInfo device)
        {
            this.MicType = micType;
            this.Device = device;
        }

        public override string ToString()
        {
            return string.Format("Mic reference: {0}", this.Device.Name);
        }
    }

    public class MicrophoneSelector : VoiceComponent
    {
        public class MicrophoneSelectorEvent: UnityEvent<MicType, DeviceInfo>
        {}

        public MicrophoneSelectorEvent onValueChanged = new MicrophoneSelectorEvent();

        private List<MicRef> micOptions;

#pragma warning disable 649
        [SerializeField]
        private Dropdown micDropdown;
        [SerializeField]
        private Slider micLevelSlider;

        [SerializeField]
        private Recorder recorder;

        [SerializeField]
        [FormerlySerializedAs("RefreshButton")]
        private GameObject refreshButton;

        private Image fillArea;
        private Color defaultFillColor = Color.white;
        private Color speakingFillColor = Color.green;


#pragma warning restore 649

        private IDeviceEnumerator unityMicEnum;
        private IDeviceEnumerator photonMicEnum;
#if PHOTON_VOICE_FMOD_ENABLE
        private IDeviceEnumerator fmodMicEnum;
        // set along with Recorder.InputFactory when switching to FMOD mic and then used for checking if the Recorder.InputFactory is FMOD mic
        private System.Func<IAudioDesc> fmodInputFactory;
#endif

        protected override void Awake()
        {
            base.Awake();
            // All enumerators trigger refresh automatically in constructor.
            unityMicEnum = new Unity.AudioInEnumerator(this.Logger);
#if PHOTON_VOICE_FMOD_ENABLE
            // referenced in SetupMicDropdown() called in photonMicEnum initialization
            this.fmodMicEnum = new Photon.Voice.FMOD.AudioInEnumerator(FMODUnity.RuntimeManager.CoreSystem, this.Logger);
#endif
            photonMicEnum = Platform.CreateAudioInEnumerator(this.Logger);
            photonMicEnum.OnReady = () => // refreshes asynchronously on WebGL
            {
                this.SetupMicDropdown();
                this.SetCurrentValue();
            };
            this.refreshButton.GetComponentInChildren<Button>().onClick.AddListener(RefreshMicrophones);

            this.fillArea = this.micLevelSlider.fillRect.GetComponent<Image>();

            this.defaultFillColor = this.fillArea.color;
        }

        private void Update()
        {
            if (this.recorder != null)
            {
                this.micLevelSlider.value = this.recorder.LevelMeter.CurrentPeakAmp;
                this.fillArea.color = this.recorder.IsCurrentlyTransmitting ? this.speakingFillColor : this.defaultFillColor;
            }
        }

        private void OnEnable()
        {
            UtilityScripts.MicrophonePermission.MicrophonePermissionCallback += this.OnMicrophonePermissionCallback;
        }

        private void OnMicrophonePermissionCallback(bool granted)
        {
            this.RefreshMicrophones();
        }

        private void OnDisable()
        {
            UtilityScripts.MicrophonePermission.MicrophonePermissionCallback -= this.OnMicrophonePermissionCallback;
        }

        private void SetupMicDropdown()
        {
            this.micDropdown.ClearOptions();

            this.micOptions = new List<MicRef>();
            List<string> micOptionsStrings = new List<string>();

            // using non-breaking spaces in menu items to avoid loosing text after the break
            this.micOptions.Add(new MicRef(MicType.Unity, DeviceInfo.Default));
            micOptionsStrings.Add(string.Format("[Unity]\u00A0[Default]"));

            foreach (var d in this.unityMicEnum)
            {
                this.micOptions.Add(new MicRef(MicType.Unity, d));
                micOptionsStrings.Add(string.Format("[Unity]\u00A0{0}", d));
            }

            this.micOptions.Add(new MicRef(MicType.Photon, DeviceInfo.Default));
            micOptionsStrings.Add(string.Format("[Photon]\u00A0[Default]"));

            foreach (var d in this.photonMicEnum)
            {
                this.micOptions.Add(new MicRef(MicType.Photon, d));
                micOptionsStrings.Add(string.Format("[Photon]\u00A0{0}", d));
            }

#if PHOTON_VOICE_FMOD_ENABLE
            foreach (var d in this.fmodMicEnum)
            {
                this.micOptions.Add(new MicRef(MicType.FMOD, d));
                micOptionsStrings.Add(string.Format("[FMOD]\u00A0{0}", d));
            }
#endif

            this.micDropdown.AddOptions(micOptionsStrings);
            this.micDropdown.onValueChanged.RemoveAllListeners();
            this.micDropdown.onValueChanged.AddListener( (x) => this.SwitchToSelectedMic() );
        }

        public void SwitchToSelectedMic()
        {
            MicRef mic = this.micOptions[this.micDropdown.value];
            switch (mic.MicType)
            {
                case MicType.Unity:
                    this.recorder.SourceType = Recorder.InputSourceType.Microphone;
                    this.recorder.MicrophoneType = Recorder.MicType.Unity;
                    this.recorder.MicrophoneDevice = mic.Device;
                    break;
                case MicType.Photon:
                    this.recorder.SourceType = Recorder.InputSourceType.Microphone;
                    this.recorder.MicrophoneType = Recorder.MicType.Photon;
                    this.recorder.MicrophoneDevice = mic.Device;
                    break;
#if PHOTON_VOICE_FMOD_ENABLE
                case MicType.FMOD:
                    this.recorder.SourceType = Recorder.InputSourceType.Factory;
                    this.recorder.InputFactory = fmodInputFactory = () => new Photon.Voice.FMOD.AudioInReader<short>(FMODUnity.RuntimeManager.CoreSystem, mic.Device.IsDefault ? 0 : mic.Device.IDInt,
                        (int)recorder.SamplingRate, this.Logger);
                    break;
#endif
            }

            onValueChanged?.Invoke(mic.MicType, mic.Device);
        }

        private void SetCurrentValue()
        {
            if (this.micOptions == null)
            {
                Debug.LogWarning("micOptions list is null");
                return;
            }
            this.micDropdown.gameObject.SetActive(true);
            this.refreshButton.SetActive(true);
            for (int valueIndex = 0; valueIndex < this.micOptions.Count; valueIndex++)
            {
                MicRef mic = this.micOptions[valueIndex];
                if (mic.MicType == MicType.Unity && this.recorder.SourceType == Recorder.InputSourceType.Microphone && this.recorder.MicrophoneType == Recorder.MicType.Unity
                    || mic.MicType == MicType.Photon && this.recorder.SourceType == Recorder.InputSourceType.Microphone && this.recorder.MicrophoneType == Recorder.MicType.Photon
#if PHOTON_VOICE_FMOD_ENABLE
                    || mic.MicType == MicType.FMOD && this.recorder.SourceType ==  Recorder.InputSourceType.Factory && this.recorder.InputFactory == fmodInputFactory
#endif
                    )
                {
                    this.micDropdown.value = valueIndex;
                    return;
                }
            }
        }

        public void RefreshMicrophones()
        {
        	// the result is processed in photonMicEnum.OnReady
            this.unityMicEnum.Refresh();
            this.photonMicEnum.Refresh();
#if PHOTON_VOICE_FMOD_ENABLE
            this.fmodMicEnum.Refresh();
#endif
        }

        // sync. UI in case a change happens from the Unity Editor Inspector
        private void PhotonVoiceCreated()
        {
            this.RefreshMicrophones();
        }
    }
}