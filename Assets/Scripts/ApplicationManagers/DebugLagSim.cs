using Photon.Pun.UtilityScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

namespace Assets.Scripts.ApplicationManagers
{
    public class DebugLagSim : MonoBehaviour
    {
        static DebugLagSim _instance;
        public static bool Enabled = false;
        public PhotonLagSimulationGui LagSimGui;
        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            _instance.LagSimGui = _instance.gameObject.AddComponent<PhotonLagSimulationGui>();
            _instance.LagSimGui.enabled = false;

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F10))
                LagSimGui.enabled = !LagSimGui.enabled;
        }
    }
}
