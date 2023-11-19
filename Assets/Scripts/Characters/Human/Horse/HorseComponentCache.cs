using UnityEngine;
using Utility;

namespace Characters
{
    class HorseComponentCache: BaseComponentCache
    {
        public ParticleSystem Dust;
        
        public HorseComponentCache(GameObject owner): base(owner)
        {
            Dust = owner.transform.Find("Dust").GetComponent<ParticleSystem>();
            LoadAudio("Horse/Prefabs/HorseSounds", Transform);
        }
    }
}
