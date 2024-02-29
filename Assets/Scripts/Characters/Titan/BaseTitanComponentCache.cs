using UnityEngine;
using Utility;
using ApplicationManagers;
using System.Collections.Generic;

namespace Characters
{
    class BaseTitanComponentCache: BaseComponentCache
    {
        public Transform Head;
        public Transform Neck;
        public Transform Core;
        public Transform Hip;
        public Transform GrabLSocket;
        public Transform GrabRSocket;
        public Collider NapeHurtbox;
        public Collider Movebox;
        public Collider SitPushbox;
        public BaseHitbox MouthHitbox;
        public BaseHitbox HandLHitbox;
        public BaseHitbox HandRHitbox;
        public BaseHitbox FootLHitbox;
        public BaseHitbox FootRHitbox;
        public Collider EyesHurtbox;
        public Collider LegLHurtbox;
        public Collider LegRHurtbox;
        public List<Collider> ToggleColliders = new List<Collider>();
        public List<BaseHitbox> Hitboxes = new List<BaseHitbox>();
        public List<Collider> Hurtboxes = new List<Collider>();

        public BaseTitanComponentCache(GameObject owner): base(owner)
        {
            BaseCharacter character = owner.GetComponent<BaseCharacter>();
            Movebox = owner.GetComponent<CapsuleCollider>();
            foreach (Collider c in owner.GetComponentsInChildren<Collider>())
            {
                GameObject go = c.gameObject;
                string name = go.name;
                if (go.layer == PhysicsLayer.TitanPushbox || go.layer == PhysicsLayer.Hurtbox)
                {
                    if (name != "SitPushbox")
                        ToggleColliders.Add(c);
                }
                if (go.layer == PhysicsLayer.Hurtbox)
                {
                    c.isTrigger = true;
                    Hurtboxes.Add(c);
                }
                else if (go.layer == PhysicsLayer.Hitbox)
                {
                    c.enabled = false;
                    c.isTrigger = true;
                }
                if (name == "NapeHurtbox")
                    NapeHurtbox = c;
                else if (name == "EyesHurtbox")
                    EyesHurtbox = c;
                else if (name == "LegLHurtbox")
                    LegLHurtbox = c;
                else if (name == "LegRHurtbox")
                    LegRHurtbox = c;
                else if (name == "MouthHitbox")
                    MouthHitbox = BaseHitbox.Create(character, go, c);
                else if (name == "HandLHitbox")
                    HandLHitbox = BaseHitbox.Create(character, go, c);
                else if (name == "HandRHitbox")
                    HandRHitbox = BaseHitbox.Create(character, go, c);
                else if (name == "FootLHitbox")
                    FootLHitbox = BaseHitbox.Create(character, go, c);
                else if (name == "FootRHitbox")
                    FootRHitbox = BaseHitbox.Create(character, go, c);
                else if (name == "SitPushbox")
                    SitPushbox = c;
            }
            foreach (BaseHitbox hitbox in owner.GetComponentsInChildren<BaseHitbox>())
            {
                Hitboxes.Add(hitbox);
            }
        }
    }
}
