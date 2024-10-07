using System;
using UnityEngine;
using Settings;
using Utility;
using Photon.Realtime;
using Photon.Pun;

namespace Anticheat
{
    class InstantiateEventFilter: BaseEventFilter
    {
        public InstantiateEventFilter(Player player, PhotonEventType eventType): base(player, eventType)
        {
        }

        public override bool CheckEvent(object[] data)
        {
            if (IsMasterOrLocal())
                return true;
            if (!base.CheckEvent(data))
                return false;
            string name = (string)data[0];
            name = name.ToLower();
            int[] viewIds = (int[])data[1];
            if (!CheckViewIds(_player.ActorNumber, viewIds))
                return false;
            return CheckInstantiate(name);
        }

        private bool CheckRateLimit(string name, RateLimit limit, int count = 1)
        {
            if (limit.Use(count))
                return true;
            AnticheatManager.KickPlayer(_player, reason: "instantiate spamming: " + name);
            return false;
        }

        private bool CheckInstantiate(string name)
        {
            return true;
        }

        private bool CheckViewIds(int senderId, int[] viewIds)
        {
            int min = senderId * PhotonNetwork.MAX_VIEW_IDS;
            int max = min + PhotonNetwork.MAX_VIEW_IDS;
            foreach (int viewId in viewIds)
            {
                if ((viewId <= min) || (viewId >= max))
                    return false;
            }
            return true;
        }
    }
}
