using System.Collections.Generic;
using UnityEngine;
using Weather;
using UI;
using Utility;
using CustomSkins;
using ApplicationManagers;
using System.Diagnostics;
using Characters;
using Settings;
using CustomLogic;
using Effects;
using Map;
using System.Collections;
using GameProgress;
using Cameras;
using System;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using Debug = UnityEngine.Debug;

namespace GameManagers
{
    class VoiceChatManager2 : InGameManager
    {
        public override void OnPlayerSpawn()
        {
            Debug.Log("Test");
        }
    }
}