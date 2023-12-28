using ApplicationManagers;
using GameManagers;
using Photon.Pun;
using Settings;
using UnityEngine;
using Utility;

namespace Spawnables
{
    class SpawnableSpawner
    {
        public static void Spawn(string name, Vector3 position, Quaternion rotation, float scale = 1f, object[] settings = null)
        {
            RPCManager.PhotonView.RPC("SpawnSpawnableRPC", RpcTarget.All, new object[] { name, position, rotation, scale, settings });
        }

        public static void OnSpawnSpawnableRPC(string name, Vector3 position, Quaternion rotation, float scale, object[] settings, PhotonMessageInfo info)
        {
            GameObject go;
            go = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Spawnables, name, position, rotation);
            BaseSpawnable spawnable;
            if (name == SpawnablePrefabs.Supply)
            {
                spawnable = go.AddComponent<SupplySpawnable>();
                spawnable.Setup(info.Sender, 30f, settings);
            }
            else if (name == SpawnablePrefabs.Rock1)
            {
                spawnable = go.AddComponent<Rock1Spawnable>();
                spawnable.Setup(info.Sender, 10f, settings);
            }
            else
            {
                spawnable = go.AddComponent<BaseSpawnable>();
                spawnable.Setup(info.Sender, 0f, settings);
            }
            go.transform.localScale *= scale;
        }
    }
}
