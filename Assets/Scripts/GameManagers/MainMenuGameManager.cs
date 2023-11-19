using System.Collections.Generic;
using UnityEngine;
using Weather;
using UI;
using Utility;
using CustomSkins;
using ApplicationManagers;
using Photon;
using Settings;
using Photon.Pun;
using Photon.Realtime;

namespace GameManagers
{
    class MainMenuGameManager : BaseGameManager
    {
        public static bool JustLeftRoom;
        public static Dictionary<string, RoomInfo> RoomList = new Dictionary<string, RoomInfo>();

        public override void OnJoinedLobby()
        {
            RoomList = new Dictionary<string, RoomInfo>();
            if (JustLeftRoom)
            {
                PhotonNetwork.Disconnect();
                JustLeftRoom = false;
            }
            else if (UIManager.CurrentMenu != null && UIManager.CurrentMenu.GetComponent<MainMenu>() != null)
                UIManager.CurrentMenu.GetComponent<MainMenu>().ShowMultiplayerRoomListPopup();
        }

        public override void OnConnectedToMaster()
        {
            if (!PhotonNetwork.OfflineMode)
                PhotonNetwork.JoinLobby();
        }

        private void Update()
        {
        }

        public override void OnJoinedRoom()
        {
            InGameManager.OnJoinRoom();
        }

        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                RoomInfo info = roomList[i];
                if (info.RemovedFromList)
                {
                    RoomList.Remove(info.Name);
                }
                else
                {
                    RoomList[info.Name] = info;
                }
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            UpdateCachedRoomList(roomList);
        }
    }
}
