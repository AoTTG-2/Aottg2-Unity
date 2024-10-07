using ApplicationManagers;
using GameManagers;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Characters
{
    class CharacterSpawner: MonoBehaviour
    {
        public static BaseCharacter Spawn(string name, Vector3 position, Quaternion rotation)
        {
            GameObject go = PhotonNetwork.Instantiate(ResourcePaths.Characters + "/" + name, position, rotation, 0);
            return go.GetComponent<BaseCharacter>();
        }
    }
}
