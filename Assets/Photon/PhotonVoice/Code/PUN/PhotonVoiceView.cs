// ----------------------------------------------------------------------------
// <copyright file="PhotonVoiceView.cs" company="Exit Games GmbH">
// Photon Voice - Copyright (C) 2018 Exit Games GmbH
// </copyright>
// <summary>
// Component that should be attached to a networked PUN prefab that has
// PhotonView. It will bind remote Recorder with local Speaker of the same
// networked prefab. This component makes automatic voice stream routing easy
// for players' characters/avatars.
// </summary>
// <author>developer@photonengine.com</author>
// ----------------------------------------------------------------------------

#if PUN_2_OR_NEWER
namespace Photon.Voice.PUN
{
    using Pun;
    using UnityEngine;
    using Unity;

    /// <summary>
    /// Component that should be attached to a networked PUN prefab that has <see cref="PhotonView"/>.
    /// It will bind remote <see cref="Recorder"/> with local <see cref="Speaker"/> of the same networked prefab.
    /// This component makes automatic voice stream routing easy for players' characters/avatars.
    /// </summary>
    [AddComponentMenu("Photon Voice/PUN/Photon Voice View")]
    [RequireComponent(typeof(PhotonView))]
    [HelpURL("https://doc.photonengine.com/en-us/voice/v2/getting-started/voice-for-pun")]
    public class PhotonVoiceView : VoiceComponent
    {
        #region Private Fields

        private PhotonView photonView;
        // using reference to instance instead of PunVoiceClient.Instance to make sure that it always accesses the same object
        private PunVoiceClient punVoiceClient;

        #endregion

        #region Public Fields
        #endregion

        #region Properties

        /// <summary> The Recorder component currently used by this PhotonVoiceView </summary>
        public Recorder RecorderInUse { get; private set; }

        /// <summary> The Speaker component currently used by this PhotonVoiceView </summary>
        public Speaker SpeakerInUse { get; private set; }

        /// <summary> If true, this PhotonVoiceView has a Speaker that is currently playing received audio frames from remote audio source </summary>
        public bool IsSpeaking
        {
            get { return this.SpeakerInUse != null && this.SpeakerInUse.IsPlaying; }
        }

        /// <summary> If true, this PhotonVoiceView has a Recorder that is currently transmitting audio stream from local audio source </summary>
        public bool IsRecording
        {
            get { return this.RecorderInUse != null && this.RecorderInUse.IsCurrentlyTransmitting; }
        }

        #endregion

        #region Private Methods

        protected override void Awake()
        {
            base.Awake();
            this.punVoiceClient = PunVoiceClient.Instance;
            this.photonView = this.GetComponent<PhotonView>();
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                this.SetupRecorder();
                if (this.RecorderInUse == null)
                {
                    this.Logger.LogWarning("Recorder not setup for PhotonVoiceView: playback may not work properly.");
                }
                else
                {
                    if (!this.RecorderInUse.TransmitEnabled)
                    {
                        this.Logger.LogWarning("PhotonVoiceView.RecorderInUse.TransmitEnabled is false, don't forget to set it to true to enable transmission.");
                    }
                    if (!this.RecorderInUse.isActiveAndEnabled)
                    {
                        this.Logger.LogWarning("PhotonVoiceView.RecorderInUse may not work properly if recorder is disabled or attached to an inactive GameObject.");
                    }
                }
            }

            this.SetupSpeaker();
            if (this.SpeakerInUse == null)
            {
                this.Logger.LogWarning("Speaker not setup for PhotonVoiceView: voice chat will not work.");
            }
            else
            {
                punVoiceClient.AddSpeaker(this.SpeakerInUse, this.photonView.ViewID);
            }
        }

    
        private void SetupRecorder()
        {
            Recorder recorder = null;

            Recorder[] recorders = this.GetComponentsInChildren<Recorder>();
            if (recorders.Length > 0)
            {
                if (recorders.Length > 1)
                {
                    this.Logger.LogWarning("Multiple Recorder components found attached to the GameObject or its children.");
                }
                recorder = recorders[0];
            }

            if (null == recorder && null != punVoiceClient.PrimaryRecorder)
            {
                recorder = punVoiceClient.PrimaryRecorder;
            }

            if (null == recorder)
            {
                this.Logger.LogWarning("Cannot find Recorder. Assign a Recorder to PhotonVoiceView object or set up PunVoiceClient.PrimaryRecorder.");
            }
            else
            {
                recorder.UserData = this.photonView.ViewID;
                punVoiceClient.AddRecorder(recorder);
            }
            this.RecorderInUse = recorder;
        }

        private void OnDestroy()
        {
            punVoiceClient.RemoveRecorder(this.RecorderInUse);
        }


        private void SetupSpeaker()
        {
            Speaker speaker = null;

            Speaker[] speakers = this.GetComponentsInChildren<Speaker>(true);
            if (speakers.Length > 0)
            {
                speaker = speakers[0];
                if (speakers.Length > 1)
                {
                    this.Logger.LogWarning("Multiple Speaker components found attached to the GameObject or its children. Using the first one we found.");
                }
            }

            if (null == speaker && null != punVoiceClient.SpeakerPrefab)
            {
                speaker = punVoiceClient.InstantiateSpeakerPrefab(this.gameObject, false);
            }

            if (null == speaker)
            {
                this.Logger.LogError("No Speaker component or prefab found. Assign a Speaker to PhotonVoiceView object or set up PunVoiceClient.SpeakerPrefab.");
            }
            else
            {
                this.Logger.LogInfo("Speaker instantiated.");
            }
            this.SpeakerInUse = speaker;
        }

        #endregion
    }
}
#endif