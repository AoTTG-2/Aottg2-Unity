using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmPlayerListManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerButtton PlayerButtonPrefab;
    [SerializeField]
    private Transform ScrollViewContent;

    private List<PlayerButtton> PlayerListings = new List<PlayerButtton>();

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerButtton listing = Instantiate(PlayerButtonPrefab, ScrollViewContent);
        if (listing != null)
            listing.SetPlayerInfo(newPlayer);
        PlayerListings.Add(listing);

        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = PlayerListings.FindIndex(x => x.PhotonPlayer == otherPlayer);
        if (index != -1)
        {
            Destroy(PlayerListings[index].gameObject);
            PlayerListings.RemoveAt(index);
        }

        base.OnPlayerLeftRoom(otherPlayer);
    }

    public override void OnLeftRoom()
    {
        if (PlayerListings.Count > 0)
        {
            foreach (var item in PlayerListings)
            {
                Destroy(item.gameObject);
            }
            PlayerListings.Clear();
        }
        base.OnLeftRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (PlayerListings.Count > 0)
        {
            foreach (var item in PlayerListings)
            {
                Destroy(item.gameObject);
            }
            PlayerListings.Clear();
        }
        base.OnDisconnected(cause);
    }
}
