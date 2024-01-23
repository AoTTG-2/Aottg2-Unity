using ApplicationManagers;
using Characters;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using Settings;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Utility;

public class VoiceChatManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public PhotonVoiceView PV;
    private BaseCharacter character;
    private InGameManager _inGameManager;
    public static VoiceChatManager Instance;
    private bool _keepTalking = false;
    // Start is called before the first frame update

    private void Start()
    {
        if (gameObject.GetComponentInParent<PhotonView>().IsMine) 
        {
            if (!Instance) 
            {
                Instance = this;
                _inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
                character = _inGameManager.CurrentCharacter;
                PV = GetComponentInParent<PhotonVoiceView>();
                CheckStatus();
            }
            else 
            {
                Destroy(Instance);
                Instance = this;
                _inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
                character = _inGameManager.CurrentCharacter;
                PV = GetComponentInParent<PhotonVoiceView>();
                CheckStatus();
            }
        }
        else 
        {
            this.enabled = false;
        }
    }
    public void ApplySoundSettings()
    {
        var microphones = GameObject.FindGameObjectsWithTag("Speaker");
        foreach (GameObject MicObj in microphones) 
        {
            AudioSource Mic = MicObj.GetComponent<AudioSource>();
            Apply(Mic);
        }
    }

    private void Update()
    {
        if (PV.IsRecording && !_keepTalking) 
        {
            RPCManager.PhotonView.RPC("EmoteVoiceRPC", RpcTarget.All, new object[] { character.Cache.PhotonView.ViewID, "Speaking"});
            _keepTalking = true;
        }
        else if(!PV.IsSpeaking && _keepTalking)
        {
            RPCManager.PhotonView.RPC("StopVoiceRPC", RpcTarget.All, new object[] {});
            _keepTalking = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player player) 
    {
        ApplySoundSettings();
    }
    public void Apply(AudioSource Mic) 
    {
        CheckStatus();
        Mic.volume = GetVoiceChatVolume();
        Mic.spatialBlend = GetTypeOfAudio();
    }

    private void CheckStatus() 
    {
        if (SettingsManager.SoundSettings.VoiceChat.Value) 
        {
            PV.GetComponentInParent<Recorder>().RecordingEnabled = true;
        }
        else 
        {
            PV.GetComponentInParent<Recorder>().RecordingEnabled = false;
        }
    }
    private float GetVoiceChatVolume()
    {
        if (SettingsManager.SoundSettings.VoiceChat.Value)
        {
            return SettingsManager.SoundSettings.VoiceChatVolume.Value * 2.5f;
        }
        else
        {
            return 0f;
        }
    }
    private float GetTypeOfAudio()
    {
        if (SettingsManager.SoundSettings.SpatialVoiceChat.Value)
        {
            return 1;
        }
        else 
        {
            return 0;
        }
    }
}