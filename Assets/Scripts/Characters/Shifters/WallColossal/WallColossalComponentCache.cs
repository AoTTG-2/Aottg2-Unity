using UnityEngine;

namespace Characters
{
    class WallColossalComponentCache: ShifterComponentCache
    {
        public WallColossalComponentCache(GameObject owner): base(owner)
        {
            Head = Transform.Find("Armature_FemT/Core/Controller_Body/hip/spine/chest/neck/head");
            Neck = Transform.Find("Armature_FemT/Core/Controller_Body/hip/spine/chest/neck");
            Core = Transform.Find("Armature_FemT/Core");
            Hip = Transform.Find("Armature_FemT/Core/Controller_Body/hip");
            LoadAudio("Shifters/Prefabs/ShifterSounds", Neck);
            LoadAudio("Titans/Prefabs/TitanSounds", Neck);
        }
    }
}
