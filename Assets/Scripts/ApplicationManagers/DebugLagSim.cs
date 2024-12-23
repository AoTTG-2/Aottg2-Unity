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
        public PhotonStatsGui StatsGUI;
        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            _instance.LagSimGui = _instance.gameObject.AddComponent<PhotonLagSimulationGui>();
            _instance.StatsGUI = _instance.gameObject.AddComponent<PhotonStatsGui>();
            _instance.LagSimGui.enabled = false;
            _instance.StatsGUI.enabled = false;
        }

        public static void Toggle()
        {
            Enabled = !Enabled;
            _instance.LagSimGui.enabled = Enabled;
            _instance.StatsGUI.enabled = Enabled;
        }
    }
}
