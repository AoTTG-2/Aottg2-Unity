using Photon.Realtime;
using Settings;
using UnityEngine;
using Characters;

namespace Spawnables
{
    class SupplySpawnable: BaseSpawnable
    {
        protected void OnTriggerStay(Collider other)
        {
            var go = other.transform.root.gameObject;
            var human = go.GetComponent<Human>();
            if (human != null && human.IsMine() && human.NeedRefill(false))
                human.SupplySpawnableRefill();
        }
    }
}
