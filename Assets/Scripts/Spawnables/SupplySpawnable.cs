using Photon.Realtime;
using Settings;
using UnityEngine;
using Characters;

namespace Spawnables
{
    class SupplySpawnable: BaseSpawnable
    {
        protected void OnCollisionStay(Collision collision)
        {
            var go = collision.transform.root.gameObject;
            var human = go.GetComponent<Human>();
            if (human != null && human.IsMine() && human.NeedRefill(false))
                human.SupplySpawnableRefill();
        }
    }
}
