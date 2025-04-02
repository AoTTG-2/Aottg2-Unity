using Settings;
using UnityEngine;
using Photon;
using Characters;
using System.Collections.Generic;
using System.Collections;
using Effects;
using ApplicationManagers;
using GameManagers;

namespace Projectiles
{
    class FlareProjectile : BaseProjectile
    {
        protected override float DestroyDelay => 120f;

        protected override void SetupSettings(object[] settings)
        {
            GetComponent<ParticleSystem>().startColor = (Color)settings[0];
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (photonView.IsMine && !Disabled)
            {
                DestroySelf();
            }
        }
    }
}
