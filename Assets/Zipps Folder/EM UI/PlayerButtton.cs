using GameManagers;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerButtton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text Button_Text;

    public Player PhotonPlayer { get; private set; }

    public void SetPlayerInfo(Player player)
    {
        PhotonPlayer = player;

        Button_Text.text = $"[{player.ActorNumber}] {player.GetStringProperty(PlayerProperty.Name)}";
    }

    public void OnClick_Button()
    {
        //Zippy: change selected player variable once made
    }
}
