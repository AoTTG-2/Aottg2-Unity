using System;
using UnityEngine;
using Settings;
using Utility;
using System.Collections.Generic;
using Photon.Realtime;

namespace Anticheat
{
    class BaseEventFilter
    {
        protected virtual RateLimit TotalRateLimit => new RateLimit(100, 1f);
        protected virtual bool AlwaysAllowMaster => true;
        protected Player _player;
        protected PhotonEventType _eventType;

        public BaseEventFilter(Player player, PhotonEventType eventType)
        {
            _player = player;
            _eventType = eventType;
        }

        public bool IsMasterOrLocal()
        {
            if (_player.IsLocal)
                return true;
            if (AlwaysAllowMaster && _player.IsMasterClient)
                return true;
            return false;
        }

        public virtual bool CheckEvent(object[] data)
        {
            if (!TotalRateLimit.Use(1))
            {
                AnticheatManager.KickPlayer(_player, reason: "sending too many " + _eventType.ToString() + " events");
                return false;
            }
            return true;
        }
    }
}
