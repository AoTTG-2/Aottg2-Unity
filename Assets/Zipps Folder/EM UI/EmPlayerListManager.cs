using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmPlayerListManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerButton PlayerButtonPrefab;
    [SerializeField]
    private Transform ScrollViewContent;

    private List<PlayerButton> PlayerListings = new List<PlayerButton>();

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerButton listing = Instantiate(PlayerButtonPrefab, ScrollViewContent);
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

    public override void OnJoinedRoom()
    {
        PlayerButton listing = Instantiate(PlayerButtonPrefab, ScrollViewContent);
        if (listing != null)
            listing.SetPlayerInfo(PhotonNetwork.LocalPlayer);
        PlayerListings.Add(listing);

        base.OnJoinedRoom();
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
