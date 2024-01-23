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
    private List<AudioSource> _audio = new List<AudioSource>();
    // Start is called before the first frame update

    private void Start()
    {
        if (!Instance) 
        {
            Instance = this;
            _inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
            character = _inGameManager.CurrentCharacter;
        }
        else 
        {
            Destroy(Instance);
            Instance = this;
            _inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
            character = _inGameManager.CurrentCharacter;
        }
    }
    public void ApplySoundSettings()
    {
        int i = 0;
        var microphones = GameObject.FindGameObjectsWithTag("Speaker");
        foreach (GameObject MicObj in microphones) 
        {
            AudioSource Mic = MicObj.GetComponent<AudioSource>();
            Apply(Mic);
        }
    }

    private void Update()
    {
        if(PV.IsSpeaking && !_keepTalking) 
        {
            Debug.Log("A");
            RPCManager.PhotonView.RPC("EmoteVoiceRPC", RpcTarget.All, new object[] { character.Cache.PhotonView.ViewID, "Speaking"});
            _keepTalking = true;
        }
        else if(!PV.IsSpeaking && _keepTalking)
        {
            Debug.Log("B");
            RPCManager.PhotonView.RPC("StopVoiceRPC", RpcTarget.All, new object[] {});
            _keepTalking = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player player) 
    {
        Debug.Log("test");
        ApplySoundSettings();

    }
    public void Apply(AudioSource Mic) 
    {
        Mic.volume = GetVoiceChatVolume();
        Mic.spatialBlend = GetTypeOfAudio();
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