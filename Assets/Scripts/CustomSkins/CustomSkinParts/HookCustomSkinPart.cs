using Characters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins
{
    class HookCustomSkinPart : BaseCustomSkinPart
    {
        private float _tiling;

        public HookCustomSkinPart(BaseCustomSkinLoader loader, List<Renderer> renderers, string rendererId, int maxSize, float tiling, Vector2? textureScale = null) : base(loader, renderers, rendererId, maxSize, textureScale, true)
        {
            _tiling = tiling;
        }

        protected override void SetMaterial(Material material)
        {
            foreach (Renderer renderer in _renderers)
            {
                renderer.material = material;
                renderer.GetComponent<Hook>().SetSkin(_tiling);
            }
        }

        protected override Material SetNewTexture(Texture2D texture)
        {
            Material material = new Material(Shader.Find("Transparent/Diffuse"));
            material.mainTexture = texture;
            SetMaterial(material);
            return material;
        }
    }
}
