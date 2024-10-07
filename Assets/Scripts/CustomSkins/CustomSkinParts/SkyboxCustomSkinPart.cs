using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins
{
    class SkyboxCustomSkinPart : BaseCustomSkinPart
    {
        private Material _skyboxMaterial;
        private string _textureName;

        public SkyboxCustomSkinPart(BaseCustomSkinLoader loader, Material skyboxMaterial, string textureName, string rendererId, int maxSize, Vector2? textureScale = null) : base(loader, null, rendererId, maxSize, textureScale)
        {
            _skyboxMaterial = skyboxMaterial;
            _textureName = textureName;
        }

        protected override bool IsValidPart()
        {
            return _skyboxMaterial != null;
        }

        protected override void DisableRenderers()
        {
            return;
        }

        protected override void SetMaterial(Material material)
        {
            _skyboxMaterial.SetTexture(_textureName, material.GetTexture(_textureName));
        }

        protected override Material SetNewTexture(Texture2D texture)
        {
            texture.wrapMode = TextureWrapMode.Clamp;
            Material material = new Material(Shader.Find("RenderFX/Skybox"));
            material.CopyPropertiesFromMaterial(_skyboxMaterial);
            material.SetTexture(_textureName, texture);
            SetMaterial(material);
            return material;
        }
    }
}
