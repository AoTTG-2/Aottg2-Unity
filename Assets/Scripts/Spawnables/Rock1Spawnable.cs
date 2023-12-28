using Photon.Realtime;
using Settings;
using UnityEngine;
using Characters;
using Utility;

namespace Spawnables
{
    class Rock1Spawnable: BaseSpawnable
    {
        protected override void SetupSettings(object[] settings)
        {
            int viewId = (int)settings[0];
            var character = Util.FindCharacterByViewId(viewId);
            if (character != null && character is BasicTitan)
            {
                var titan = (BasicTitan)character;
                transform.SetParent(titan.BasicCache.HandRHitbox.transform);
                transform.localPosition = new Vector3(0f, 0.07f, -0.07f);
                transform.localRotation = Quaternion.identity;
            }
        }
    }
}
