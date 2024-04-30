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

    private float timer = 0;
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= 0.5f)
        {
            NameRefresh();
            timer = 0f;
        }
    }

    public void OnClick_Button()
    {
        EmVariables.SelectedPlayer = PhotonPlayer;
    }

    private void NameRefresh()
    {
        Button_Text.text = 
            $"[{PhotonPlayer.ActorNumber}]" +
            $" {PhotonPlayer.GetStringProperty(PlayerProperty.Name)}";
    }
}
