using Unity.VisualScripting;
using UnityEngine;

namespace Characters
{
    class WallColossalComponentCache: ShifterComponentCache
    {
        public ParticleSystem ColossalSteam1;
        public ParticleSystem ColossalSteam2;

        //WallColossal/Armature_VER2/Core/Controller_Body/hip/spine/chest/upperchest/shoulder_L/upper_arm_L/forearm_L/hand_L
        public Transform LeftHand;

        //WallColossal/Armature_VER2/Core/Controller_Body/hip/spine/chest/upperchest/shoulder_R/upper_arm_R/forearm_R/hand_R
        public Transform RightHand;

        // WallColossal/Armature_VER2/Core/Controller_Body/hip/spine/chest/upperchest/shoulder_L/upper_arm_L/forearm_L/ArmSmokeParticle
        public ParticleSystem LeftHandSteam;

        // WallColossal/Armature_VER2/Core/Controller_Body/hip/spine/chest/upperchest/shoulder_R/upper_arm_R/ArmSmokeParticle
        public ParticleSystem RightHandSteam;

        public WallColossalComponentCache(GameObject owner): base(owner)
        {
            Head = Transform.Find("Armature_VER2/Core/Controller_Body/hip/spine/chest/upperchest/neck/head");
            Neck = Transform.Find("Armature_VER2/Core/Controller_Body/hip/spine/chest/upperchest/neck");
            Core = Transform.Find("Armature_VER2/Core");
            Hip = Transform.Find("Armature_VER2/Core/Controller_Body/hip");
            ColossalSteam1 = Neck.Find("ColossalSteam1").GetComponent<ParticleSystem>();
            ColossalSteam2 = Neck.Find("ColossalSteam2").GetComponent<ParticleSystem>();

            LeftHand = Transform.Find("Armature_VER2/Core/Controller_Body/hip/spine/chest/upperchest/shoulder_L/upper_arm_L/forearm_L/hand_L");
            RightHand = Transform.Find("Armature_VER2/Core/Controller_Body/hip/spine/chest/upperchest/shoulder_R/upper_arm_R/forearm_R/hand_R");

            LeftHandSteam = Transform.Find("Armature_VER2/Core/Controller_Body/hip/spine/chest/upperchest/shoulder_L/upper_arm_L/forearm_L/ArmSmokeParticle").GetComponent<ParticleSystem>();
            RightHandSteam = Transform.Find("Armature_VER2/Core/Controller_Body/hip/spine/chest/upperchest/shoulder_R/upper_arm_R/forearm_R/ArmSmokeParticle").GetComponent<ParticleSystem>();

            LoadAudio("Shifters/Prefabs/ShifterSounds", Neck);
            LoadAudio("Titans/Prefabs/TitanSounds", Neck);
        }
    }
}
