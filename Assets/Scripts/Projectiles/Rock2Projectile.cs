using Settings;
using UnityEngine;
using Photon;
using Characters;
using System.Collections.Generic;
using System.Collections;
using Effects;
using ApplicationManagers;
using GameManagers;
using UI;
using CustomLogic;

namespace Projectiles
{
    class Rock2Projectile : Rock1Projectile
    {
        protected override bool DestroyOnImpact => false;
        protected override float MinImpactVelocity => 50f;

        protected override void RegisterObjects()
        {
            var model = transform.Find("Sphere1").gameObject;
            _hideObjects.Add(model);
        }
    }
}
