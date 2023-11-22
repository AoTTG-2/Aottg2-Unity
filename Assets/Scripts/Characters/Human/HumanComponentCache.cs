using UnityEngine;
using Utility;
using ApplicationManagers;

namespace Characters
{
    class HumanComponentCache: BaseComponentCache
    {
        public Transform HandL;
        public Transform HandR;
        public Transform Head;
        public Transform Neck;
        public Transform ForearmL;
        public Transform ForearmR;
        public Transform UpperarmL;
        public Transform UpperarmR;
        public ParticleSystem Sparks;
        public ParticleSystem Smoke;
        public ParticleSystem Wind;
        public Transform WindTransform;
        public Transform HookLeftAnchorDefault;
        public Transform HookRightAnchorDefault;
        public Transform HookLeftAnchorGun;
        public Transform HookRightAnchorGun;
        public BaseHitbox BladeHitLeft;
        public BaseHitbox BladeHitRight;
        public BaseHitbox AHSSHit;
        public BaseHitbox APGHit;
        
        public HumanComponentCache(GameObject owner): base(owner)
        {
            var chest = Transform.Find("Armature/Core/Controller_Body/hip/spine/chest");
            Neck = chest.Find("neck");
            Head = Neck.Find("head");
            UpperarmL = chest.Find("shoulder_L/upper_arm_L");
            UpperarmR = chest.Find("shoulder_R/upper_arm_R");
            ForearmL = UpperarmL.Find("forearm_L");
            ForearmR = UpperarmR.Find("forearm_R");
            HandL = ForearmL.Find("hand_L");
            HandR = ForearmR.Find("hand_R");
            Sparks = Transform.Find("slideSparks").GetComponent<ParticleSystem>();
            Smoke = Transform.Find("3dmg_smoke").GetComponent<ParticleSystem>();
            var emission = Smoke.emission;
            emission.enabled = false;
            emission = Sparks.emission;
            emission.enabled = false;
            Wind = Transform.Find("speedFX").GetComponentInChildren<ParticleSystem>();
            emission = Wind.emission;
            emission.enabled = false;
            WindTransform = Transform.Find("speedFX");
            HookLeftAnchorDefault = chest.Find("hookRefL1");
            HookRightAnchorDefault = chest.Find("hookRefR1");
            HookLeftAnchorGun = HandL.Find("hookRef");
            HookRightAnchorGun = HandR.Find("hookRef");
            var human = owner.GetComponent<BaseCharacter>();
            if (human != null)
            {
                BladeHitLeft = BaseHitbox.Create(human, HandL.Find("checkBox").gameObject);
                BladeHitLeft.TwoFixedUpdates = true;
                BladeHitRight = BaseHitbox.Create(human, HandR.Find("checkBox").gameObject);
                BladeHitRight.TwoFixedUpdates = true;
                CreateAHSSHitbox(human);
                CreateAPGHitbox(human);
                LoadAudio("Human/Prefabs/HumanSounds", Transform);
            }
        }

        private void CreateAHSSHitbox(BaseCharacter human)
        {
            GameObject obj = new GameObject();
            obj.layer = PhysicsLayer.Hitbox;
            var capsule = obj.AddComponent<CapsuleCollider>();
            capsule.direction = 2;
            capsule.isTrigger = true;
            var ahssInfo = CharacterData.HumanWeaponInfo["AHSS"];
            capsule.radius = ahssInfo["Radius"].AsFloat;
            capsule.height = ahssInfo["Range"].AsFloat;
            capsule.center = new Vector3(0f, 0f, capsule.height * 0.5f + 0.5f);
            AHSSHit = BaseHitbox.Create(human, obj);
        }

        private void CreateAPGHitbox(BaseCharacter human)
        {
            GameObject obj = new GameObject();
            obj.layer = PhysicsLayer.Hitbox;
            var capsule = obj.AddComponent<CapsuleCollider>();
            capsule.direction = 2;
            capsule.isTrigger = true;
            var ahssInfo = CharacterData.HumanWeaponInfo["APG"];
            capsule.radius = ahssInfo["Radius"].AsFloat;
            capsule.height = 10f;
            capsule.center = new Vector3(0f, 0f, capsule.height * 0.5f + 0.5f);
            APGHit = BaseHitbox.Create(human, obj);
        }
    }
}
