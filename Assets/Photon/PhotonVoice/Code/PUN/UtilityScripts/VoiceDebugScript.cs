#if PUN_2_OR_NEWER
namespace Photon.Voice.PUN.UtilityScripts
{
    using Pun;
    using Unity;
    using Realtime;
    using UnityEngine;
    using ExitGames.Client.Photon;

    /// <summary> Utility script to be attached next to PhotonVoiceView & PhotonView on the player prefab to be network instantiated.
    /// Call voiceDebugScript.CantHearYou() on the networked object of the remote (or local) player if you can't hear the corresponding player. </summary>
    [RequireComponent(typeof(PhotonVoiceView))]
    public class VoiceDebugScript : MonoBehaviourPun
    {
        private PhotonVoiceView photonVoiceView;

        /// <summary> Make sure recorder.TransmitEnabled and recorder.RecordingEnabled are true. </summary>
        public bool ForceRecordingAndTransmission;

        /// <summary> Audio file to be broadcast when TestUsingAudioClip is enabled. </summary>
        public AudioClip TestAudioClip;

        /// <summary> Broadcast Audio file to make sure transmission over network works if microphone (audio input device/hardware) is not reliable. Requires setting AudioClip in TestAudioClip. </summary>
        public bool TestUsingAudioClip;

        /// <summary> Disable recorder.VoiceDetection for easier testing. </summary>
        public bool DisableVad;

        /// <summary> Set main voice component's log level to ALL (max). </summary>
        public bool IncreaseLogLevels;

        /// <summary> Debug DebugEcho mode (Can't Hear My Self?!). </summary>
        public bool LocalDebug;

        private void Awake()
        {
            this.photonVoiceView = this.GetComponent<PhotonVoiceView>();
        }

        private void Update()
        {
            this.MaxLogs();
            if (this.photonVoiceView.RecorderInUse != null)
            {
                if (this.TestUsingAudioClip)
                {
                    if (ReferenceEquals(null, this.TestAudioClip) || !this.TestAudioClip)
                    {
                        Debug.LogError("Set an AudioClip first");
                    }
                    else
                    {
                        this.photonVoiceView.RecorderInUse.SourceType = Recorder.InputSourceType.AudioClip;
                        this.photonVoiceView.RecorderInUse.AudioClip = this.TestAudioClip;
                        this.photonVoiceView.RecorderInUse.LoopAudioClip = true;
                        this.photonVoiceView.RecorderInUse.RestartRecording();
                    }
                }
                if (this.ForceRecordingAndTransmission)
                {
                    this.photonVoiceView.RecorderInUse.RecordingEnabled = true;
                    this.photonVoiceView.RecorderInUse.TransmitEnabled = true;
                }
                if (this.DisableVad)
                {
                    this.photonVoiceView.RecorderInUse.VoiceDetection = false;
                }
            }
        }

        [ContextMenu("CantHearYou")]
        public void CantHearYou()
        {
            if (!PunVoiceClient.Instance.Client.InRoom)
            {
                Debug.LogError("local voice client is not joined to a voice room");
            }
            else
            {
                if (!this.photonVoiceView.SpeakerInUse.IsLinked)
                {
                    Debug.LogError("locally speaker not linked, trying late linking & asking anyway");
                }
                this.photonView.RPC("CantHearYou", this.photonView.Owner, PunVoiceClient.Instance.Client.CurrentRoom.Name, PunVoiceClient.Instance.Client.LoadBalancingPeer.ServerIpAddress, PunVoiceClient.Instance.Client.AppVersion);
            }
        }

        [PunRPC]
        private void CantHearYou(string roomName, string serverIp, string appVersion, PhotonMessageInfo photonMessageInfo)
        {
            string why;
            if (!PunVoiceClient.Instance.Client.InRoom)
            {
                why = "voice client not in a room";
            }
            else if (!PunVoiceClient.Instance.Client.CurrentRoom.Name.Equals(roomName))
            {
                why = string.Format("voice client is on another room {0} != {1}",
                    PunVoiceClient.Instance.Client.CurrentRoom.Name, roomName);
            }
            else if (!PunVoiceClient.Instance.Client.LoadBalancingPeer.ServerIpAddress.Equals(serverIp))
            {
                why = string.Format("voice client is on another server {0} != {1}, maybe different Photon Cloud regions",
                    PunVoiceClient.Instance.Client.LoadBalancingPeer.ServerIpAddress, serverIp);
            }
            else if (!PunVoiceClient.Instance.Client.AppVersion.Equals(appVersion))
            {
                why = string.Format("voice client uses different AppVersion {0} != {1}",
                    PunVoiceClient.Instance.Client.AppVersion, appVersion);
            }
            else if (this.photonVoiceView.RecorderInUse == null)
            {
                why = "recorder not setup (yet?)";
                //this.photonVoiceView.Setup();
            }
            else if (!this.photonVoiceView.RecorderInUse.RecordingEnabled)
            {
                why = "recorder is not recording";
                this.photonVoiceView.RecorderInUse.RecordingEnabled = true;
            }
            else if (!this.photonVoiceView.RecorderInUse.TransmitEnabled)
            {
                why = "recorder is not transmitting";
                this.photonVoiceView.RecorderInUse.TransmitEnabled = true;
            }
            else if (this.photonVoiceView.RecorderInUse.InterestGroup != 0)
            {
                why = "recorder.InterestGroup is not zero? is this on purpose? switching it back to zero";
                this.photonVoiceView.RecorderInUse.InterestGroup = 0;
            }
            else if (!(this.photonVoiceView.RecorderInUse.UserData is int) || (int)this.photonVoiceView.RecorderInUse.UserData != this.photonView.ViewID)
            {
                why = string.Format("recorder.UserData ({0}) != photonView.ViewID ({1}), fixing it now", this.photonVoiceView.RecorderInUse.UserData, this.photonView.ViewID);
                this.photonVoiceView.RecorderInUse.UserData = this.photonView.ViewID;
                this.photonVoiceView.RecorderInUse.RestartRecording();
            }
            else if (this.photonVoiceView.RecorderInUse.VoiceDetection && this.DisableVad) // todo: check WebRtcAudioDsp.VAD
            {
                why = "recorder vad is enabled, disable it for testing";
                this.photonVoiceView.RecorderInUse.VoiceDetection = false;
            }
            else if (this.photonView.OwnerActorNr == photonMessageInfo.Sender.ActorNumber)
            {
                if (this.LocalDebug)
                {
                    if (this.photonVoiceView.SpeakerInUse != null)
                    {
                        why = "no idea why!, should be working (1)";
                        this.photonVoiceView.RecorderInUse.RestartRecording();
                    }
                    else if (!this.photonVoiceView.RecorderInUse.DebugEchoMode)
                    {
                        why = "recorder debug echo mode not enabled, enabling it now";
                        this.photonVoiceView.RecorderInUse.DebugEchoMode = true;
                    }
                    else
                    {
                        why = "locally not speaker (yet?) (2)";
                        //this.photonVoiceView.Setup();
                    }
                }
                else
                {
                    why = "local object, are you trying to hear yourself? (feedback DebugEcho), LocalDebug is disabled, enable it if you want to diagnose this";
                }
            }
            else
            {
                why = "no idea why!, should be working (2)";
                this.photonVoiceView.RecorderInUse.RestartRecording();
            }
            this.Reply(why, photonMessageInfo.Sender);
        }

        private void Reply(string why, Player player)
        {
            this.photonView.RPC("HeresWhy", player, why);
        }

        [PunRPC]
        private void HeresWhy(string why, PhotonMessageInfo photonMessageInfo)
        {
            Debug.LogErrorFormat("Player {0} replied to my CantHearYou message with {1}", photonMessageInfo.Sender, why);
        }

        private void MaxLogs()
        {
            if (this.IncreaseLogLevels)
            {
                foreach (var l in FindObjectsOfType<VoiceLogger>())
                {
                    l.LogLevel = DebugLevel.ALL;
                }
            }
        }
    }
}
#endif