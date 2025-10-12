using UnityEngine;

namespace Characters
{
    class WallColossalComponentCache: ShifterComponentCache
    {
        public ParticleSystem ColossalSteam1;
        public ParticleSystem ColossalSteam2;

        public WallColossalComponentCache(GameObject owner): base(owner)
        {
            Head = Transform.Find("Armature_VER2/Core/Controller_Body/hip/spine/chest/upperchest/neck/head");
            Neck = Transform.Find("Armature_VER2/Core/Controller_Body/hip/spine/chest/upperchest/neck");
            Core = Transform.Find("Armature_VER2/Core");
            Hip = Transform.Find("Armature_VER2/Core/Controller_Body/hip");
            ColossalSteam1 = Neck.Find("ColossalSteam1").GetComponent<ParticleSystem>();
            ColossalSteam2 = Neck.Find("ColossalSteam2").GetComponent<ParticleSystem>();
            LoadAudio("Shifters/Prefabs/ShifterSounds", Neck);
            LoadAudio("Titans/Prefabs/TitanSounds", Neck);
        }
    }
}
