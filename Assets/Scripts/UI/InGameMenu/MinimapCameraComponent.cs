using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Settings;
using Characters;
using GameManagers;
using ApplicationManagers;
using Utility;
using Cameras;
using Map;

namespace UI
{
    class MinimapCameraComponent : MonoBehaviour
    {
        private List<MapLight> _disabledLights = new List<MapLight>();

        private void OnPreCull()
        {
            _disabledLights.Clear();
            foreach (var light in MapLoader.MapLights)
            {
                if (light.MinimapDisableLight())
                    _disabledLights.Add(light);
            }
        }

        private void OnPostRender()
        {
            foreach (var light in _disabledLights)
            {
                if (light != null)
                    light.MinimapEnableLight();
            }
        }
    }
}
