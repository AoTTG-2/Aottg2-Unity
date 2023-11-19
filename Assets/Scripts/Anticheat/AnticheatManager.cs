using ApplicationManagers;
using Events;
using GameManagers;
using Photon.Realtime;
using Photon.Pun;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Anticheat
{
    class AnticheatManager : MonoBehaviour
    {
        private static AnticheatManager _instance;
        private Dictionary<int, Dictionary<PhotonEventType, BaseEventFilter>> _IdToEventFilters = new Dictionary<int, Dictionary<PhotonEventType, BaseEventFilter>>();

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            EventManager.OnLoadScene += OnLoadScene;
        }

        private static void OnLoadScene(SceneName sceneName)
        {
            _instance._IdToEventFilters.Clear();
        }

        public static bool CheckPhotonEvent(Player sender, PhotonEventType eventType, object[] data)
        {
            if (!_instance._IdToEventFilters.ContainsKey(sender.ActorNumber))
                _instance._IdToEventFilters.Add(sender.ActorNumber, new Dictionary<PhotonEventType, BaseEventFilter>());
            var filters = _instance._IdToEventFilters[sender.ActorNumber];
            if (!filters.ContainsKey(eventType))
            {
                if (eventType == PhotonEventType.Instantiate)
                    filters.Add(eventType, new InstantiateEventFilter(sender, eventType));
            }
            return filters[eventType].CheckEvent(data);
        }

        public static void KickPlayer(Player player, bool ban = false, string reason = "")
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            if (PhotonNetwork.IsMasterClient && player == PhotonNetwork.LocalPlayer && reason != string.Empty)
            {
                DebugConsole.Log("Attempting to ban myself for: " + reason + ", please report this to the devs.", true);
                return;
            }
            PhotonNetwork.DestroyPlayerObjects(player);
            PhotonNetwork.CloseConnection(player);
            if (reason != string.Empty)
            {
                DebugConsole.Log("Player " + player.ActorNumber.ToString() + " was autobanned. Reason:" + reason, true);
            }
        }
    }

    public enum PhotonEventType
    {
        Instantiate,
        RPC
    }
}