using System;
using UnityEngine;
using ApplicationManagers;
using GameManagers;
using UnityEngine.UI;
using Utility;
using System.Collections.Generic;
using Settings;
using System.Collections;
using CustomLogic;
using UI;
using Cameras;
using Photon.Pun;
using Photon.Realtime;
using GameProgress;

namespace Characters
{
    interface ITargetable
    {
        public string GetTeam();

        public Vector3 GetPosition();

        public bool ValidTarget()
        {
            return true;
        }
    }
}
