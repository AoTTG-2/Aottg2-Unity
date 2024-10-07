using UnityEngine;

namespace Characters
{
    class ArmoredComponentCache : ShifterComponentCache
    {
        public ArmoredComponentCache(GameObject owner): base(owner)
        {
            Head = Transform.Find("Amarture_VER2/Core/Controller.Body/hip/spine/chest/neck/head");
            Neck = Transform.Find("Amarture_VER2/Core/Controller.Body/hip/spine/chest/neck");
            Core = Transform.Find("Amarture_VER2/Core");
            Hip = Transform.Find("Amarture_VER2/Core/Controller.Body/hip");
            LoadAudio("Shifters/Prefabs/ShifterSounds", Neck);
            LoadAudio("Titans/Prefabs/TitanSounds", Neck);
        }
    }
}
