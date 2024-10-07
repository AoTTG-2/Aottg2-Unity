using ApplicationManagers;
using GameManagers;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Projectiles
{
    class ProjectileSpawner: MonoBehaviour
    {
        public static BaseProjectile Spawn(string name, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 gravity, float liveTime,
            int charViewId, string team, object[] settings = null)
        {
            GameObject go = PhotonNetwork.Instantiate(ResourcePaths.Projectiles + "/" + name, position, rotation, 0);
            BaseProjectile projectile;
            projectile = go.GetComponent<BaseProjectile>();
            projectile.Setup(liveTime, velocity, gravity, charViewId, team, settings);
            return projectile;
        }
    }
}
