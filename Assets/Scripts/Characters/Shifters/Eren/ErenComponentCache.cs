using UnityEngine;

namespace Characters
{
    class ErenComponentCache: ShifterComponentCache
    {
        public ErenComponentCache(GameObject owner): base(owner)
        {
            Head = Transform.Find("ErenRig_VER2/Core/Controller_Body/hip/spine/chest/neck/head");
            Neck = Transform.Find("ErenRig_VER2/Core/Controller_Body/hip/spine/chest/neck");
            Core = Transform.Find("ErenRig_VER2/Core");
            Hip = Transform.Find("ErenRig_VER2/Core/Controller_Body/hip");
            LoadAudio("Shifters/Prefabs/ShifterSounds", Neck);
            LoadAudio("Titans/Prefabs/TitanSounds", Neck);
        }
    }
}
