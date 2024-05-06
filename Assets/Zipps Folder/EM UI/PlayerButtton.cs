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

        if (PhotonPlayer.CustomProperties.ContainsKey("Cannoneer")) { Button_Text.text += " [<color=#74B831>CAN</color>]"; }
        if (PhotonPlayer.CustomProperties.ContainsKey("Carpenter")) { Button_Text.text += " [<color=#2C84DC>CAR</color>]"; }
        if (PhotonPlayer.CustomProperties.ContainsKey("Veteran")) { Button_Text.text += " [<color=#7B31B8>VET</color>]"; }
        if (PhotonPlayer.CustomProperties.ContainsKey("Logistician")) { Button_Text.text += " [<color=#DC2C2C>LOG</color>]"; }
        if (PhotonPlayer.CustomProperties.ContainsKey("Wagon")) { Button_Text.text += " [<color=#DC2C2C>WAG</color>]"; }
    }
}
