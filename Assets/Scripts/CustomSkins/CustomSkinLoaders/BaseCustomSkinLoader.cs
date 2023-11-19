using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace CustomSkins
{
    abstract class BaseCustomSkinLoader: MonoBehaviour
    {
        public static readonly string TransparentURL = "transparent";
        protected abstract string RendererIdPrefix { get; }
        protected GameObject _owner;
        protected const int BytesPerKb = 1000;
        protected const int MaxSizeLarge = BytesPerKb * 2000;
        protected const int MaxSizeMedium = BytesPerKb * 1000;
        protected const int MaxSizeSmall = BytesPerKb * 500;

        protected void Awake()
        {
            _owner = gameObject;
        }

        protected virtual BaseCustomSkinPart GetCustomSkinPart(int partId)
        {
            throw new NotImplementedException();
        }

        public abstract IEnumerator LoadSkinsFromRPC(object[] data);

        protected string GetRendererId(int partId)
        {
            return RendererIdPrefix + partId.ToString();
        }

        protected void AddRendererIfExists(List<Renderer> renderers, GameObject obj)
        {
            if (obj != null)
                renderers.Add(obj.GetComponent<Renderer>());
        }

        protected void AddAllRenderers(List<Renderer> renderers, GameObject obj)
        {
            foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>())
                renderers.Add(renderer);
        }

        protected void AddRenderersContainingName(List<Renderer> renderers, GameObject obj, string name)
        {
            foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>())
            {
                if (renderer.name.Contains(name))
                    renderers.Add(renderer);
            }
        }

        protected void AddRenderersMatchingName(List<Renderer> renderers, GameObject obj, string name)
        {
            foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>())
            {
                if (renderer.name == name)
                    renderers.Add(renderer);
            }
        }

        protected List<int> GetCustomSkinPartIds(Type t)
        {
            return Enum.GetValues(t).Cast<int>().ToList();
        }

        private void OnDestroy()
        {
            TextureDownloader.ResetConcurrentDownloads();
        }
    }
}
