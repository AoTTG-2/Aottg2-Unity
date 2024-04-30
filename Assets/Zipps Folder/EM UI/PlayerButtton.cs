using GameManagers;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text Button_Text;

    public Player PhotonPlayer { get; private set; }

    public void SetPlayerInfo(Player player)
    {
        PhotonPlayer = player;

        NameRefresh();
    }

    public void OnClick_Button()
    {
        EmVariables.SelectedPlayer = PhotonPlayer;
    }

    private void Start()
    {
        Invoke("NameRefresh", 1f);
    }

    private void OnEnable()
    {
        Invoke("NameRefresh", 0.3f);
    }

    private void NameRefresh()
    {
        Button_Text.text = $"[{PhotonPlayer.ActorNumber}] {PhotonPlayer.GetStringProperty(PlayerProperty.Name)}";
    }
}
