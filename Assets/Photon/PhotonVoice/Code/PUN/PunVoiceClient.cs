// ----------------------------------------------------------------------------
// <copyright file="PunVoiceClient.cs" company="Exit Games GmbH">
// Photon Voice - Copyright (C) 2018 Exit Games GmbH
// </copyright>
// <summary>
// This class can be used to automatically join/leave Voice rooms when
// Photon Unity Networking (PUN) joins or leaves its rooms. The Voice room
// will use the same name as PUN, but with a "_voice_" postfix.
// It also finds the Speaker component for a character's voice. For this to work,
// the voice's UserData must be set to the character's PhotonView ID.
// (see "PhotonVoiceView.cs")
// </summary>
// <author>developer@photonengine.com</author>
// ----------------------------------------------------------------------------

#if PUN_2_OR_NEWER

using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using System;
using ExitGames.Client.Photon.StructWrapping;

namespace Photon.Voice.PUN
{

    /// <summary>
    /// This class can be used to automatically sync client states between PUN and Voice.
    /// It also finds the Speaker component for a character's voice.
    /// For this to work attach a <see cref="PhotonVoiceView"/> next to the <see cref="PhotonView"/> of your player's prefab.
    /// </summary>
    [AddComponentMenu("Photon Voice/PUN/Pun Voice Client")]
    [HelpURL("https://doc.photonengine.com/en-us/voice/v2/getting-started/voice-for-pun")]
    public class PunVoiceClient : VoiceFollowClient
    {
        // abstract VoiceFollowClient implementation
        protected override bool LeaderInRoom => PhotonNetwork.InRoom;
        protected override bool LeaderOfflineMode => PhotonNetwork.OfflineMode;

        /// <summary> Suffix for voice room names appended to Leader room names. </summary>
        public const string VoiceRoomNameSuffix = "_ID(";

        private static PunVoiceClient instance;

        [SerializeField]
        private bool usePunAppSettings = true;

        [SerializeField]
        private bool usePunAuthValues = true;

        #region Properties

        /// <summary>
        /// Singleton instance for PunVoiceClient
        /// </summary>
        public static PunVoiceClient Instance
        {
            get
            {
                if (instance == null)
                {
                    PunVoiceClient[] objects = FindObjectsOfType<PunVoiceClient>();
                    if (objects == null || objects.Length < 1)
                    {
                        GameObject singleton = new GameObject();
                        singleton.name = "PunVoiceClient";
                        instance = singleton.AddComponent<PunVoiceClient>();
                        instance.Logger.LogError("PunVoiceClient component was not found in the scene. Creating PunVoiceClient object.");
                    }
                    else if (objects.Length >= 1)
                    {
                        instance = objects[0];
                        instance.Logger.LogInfo("An instance of PunVoiceClient is found in the scene.");
                        if (objects.Length > 1)
                        {
                            instance.Logger.LogError("{0} instances of PunVoiceClient found in the scene. Using a random instance.", objects.Length);
                        }
                    }
                    instance.Logger.LogInfo("PunVoiceClient singleton instance is now set.");
                }
                return instance;
            }
        }

        /// <summary>
        /// Whether or not to use the Voice AppId and all the other AppSettings from PUN's PhotonServerSettings ScriptableObject singleton in the Voice client/app.
        /// </summary>
        public bool UsePunAppSettings
        {
            get
            {
                return this.usePunAppSettings;
            }
            set
            {
                this.usePunAppSettings = value;
            }
        }

        /// <summary>
        /// Whether or not to use the same PhotonNetwork.AuthValues in PunVoiceClient.Instance.Client.AuthValues.
        /// This means that the same UserID will be used in both clients.
        /// If custom authentication is used and setup in PUN app, the same configuration should be done for the Voice app.
        /// </summary>
        public bool UsePunAuthValues
        {
            get
            {
                return this.usePunAuthValues;
            }
            set
            {
                this.usePunAuthValues = value;
            }
        }

        #endregion

        protected override void Start()
        {
            PhotonNetwork.NetworkingClient.StateChanged += OnPunStateChange;
            base.Start();
            if (Instance == this)
            {
                if (this.UsePrimaryRecorder)
                {
                    if (this.PrimaryRecorder != null)
                    {
                        AddRecorder(this.PrimaryRecorder);
                    }
                    else
                    {
                        this.Logger.LogError("Primary Recorder is not set.");
                    }
                }
            }
        }

        protected override void OnDestroy()
        {
            PhotonNetwork.NetworkingClient.StateChanged -= OnPunStateChange;
            base.OnDestroy();
            if (instance == this)
            {
                instance.Logger.LogInfo("PunVoiceClient singleton instance is being reset because destroyed.");
                instance = null;
            }
        }

        protected override Speaker InstantiateSpeakerForRemoteVoice(int playerId, byte voiceId, object userData)
        {
            if (userData == null) // Recorder w/o PhotonVoiceView: probably created due to this.UsePrimaryRecorder = true
            {
                this.Logger.LogInfo("Creating Speaker for remote voice p#{0} v#{1} PunVoiceClient Primary Recorder (userData == null).", playerId, voiceId);
                return this.InstantiateSpeakerPrefab(this.gameObject, true);
            }

            if (!(userData is int))
            {
                this.Logger.LogWarning("UserData ({0}) does not contain PhotonViewId. Remote voice p#{1} v#{2} not linked. Do you have a Recorder not used with a PhotonVoiceView? is this expected?", userData == null ? "null" : userData.ToString(), playerId, voiceId);
                return null;
            }

            int photonViewId = (int)userData;
            PhotonView photonView = PhotonView.Find(photonViewId);
            if (null == photonView || !photonView)
            {
                this.Logger.LogWarning("No PhotonView with ID {0} found. Remote voice p#{1} v#{2} not linked.", userData, playerId, voiceId);
                return null;
            }

            PhotonVoiceView photonVoiceView = photonView.GetComponent<PhotonVoiceView>();
            if (null == photonVoiceView || !photonVoiceView)
            {
                this.Logger.LogWarning("No PhotonVoiceView attached to the PhotonView with ID {0}. Remote voice p#{1} v#{2} not linked.", userData, playerId, voiceId);
                return null;
            }
            this.Logger.LogInfo("Using PhotonVoiceView {0} Speaker for remote voice p#{1} v#{2}.", userData, playerId, voiceId);
            return photonVoiceView.SpeakerInUse;
        }

        // abstract VoiceFollowClient implementation
        protected override string GetVoiceRoomName()
        {
            if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom != null && !string.IsNullOrEmpty(PhotonNetwork.CurrentRoom.Name))
            {
                return PhotonNetwork.CurrentRoom.Name + "vc";
            }
            return null;
        }

        private void OnPunStateChange(ClientState s1, ClientState s2)
        {
            LeaderStateChanged(s2);
        }

        // abstract VoiceFollowClient implementation
        protected override bool ConnectVoice()
        {
            AppSettings settings = null;

            if (this.usePunAppSettings)
            {
                settings = new AppSettings();
                settings = PhotonNetwork.PhotonServerSettings.AppSettings.CopyTo(settings); // creates an independent copy (cause we need to modify it slightly)
                if (!string.IsNullOrEmpty(PhotonNetwork.CloudRegion))
                {
                    settings.FixedRegion = PhotonNetwork.CloudRegion; // makes sure the voice connection follows into the same cloud region (as PUN uses now).
                }

                this.Client.SerializationProtocol = PhotonNetwork.NetworkingClient.SerializationProtocol;
            }

            // use the same user, authentication, auth-mode and encryption as PUN
            if (this.UsePunAuthValues)
            {
                if (PhotonNetwork.AuthValues != null)
                {
                    if (this.Client.AuthValues == null)
                    {
                        this.Client.AuthValues = new AuthenticationValues();
                    }
                    this.Client.AuthValues = PhotonNetwork.AuthValues.CopyTo(this.Client.AuthValues);
                }
                this.Client.AuthMode = PhotonNetwork.NetworkingClient.AuthMode;
                this.Client.EncryptionMode = PhotonNetwork.NetworkingClient.EncryptionMode;
            }

            return this.ConnectUsingSettings(settings);
        }
    }
}
#endif