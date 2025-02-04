using ApplicationManagers;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Caches common Unity components for more performant lookup.
    /// </summary>
    class BaseComponentCache
    {
        public Transform Transform;
        public Rigidbody Rigidbody;
        public PhotonView PhotonView;
        public List<Collider> Colliders = new List<Collider>();
        public Dictionary<string, AudioSource> AudioSources = new Dictionary<string, AudioSource>();

        public BaseComponentCache(GameObject owner)
        {
            Transform = owner.transform;
            Rigidbody = owner.GetComponent<Rigidbody>();
            PhotonView = owner.GetComponent<PhotonView>();
            foreach (var collider in owner.GetComponentsInChildren<Collider>())
                Colliders.Add(collider);
        }

        public void LoadAudio(string prefab, Transform parent)
        {
            GameObject soundPrefab = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, prefab);
            soundPrefab.transform.SetParent(parent);
            soundPrefab.transform.localPosition = Vector3.zero;
            foreach (var audio in soundPrefab.GetComponentsInChildren<AudioSource>())
                AudioSources.Add(audio.gameObject.name, audio);
        }
    }
}
