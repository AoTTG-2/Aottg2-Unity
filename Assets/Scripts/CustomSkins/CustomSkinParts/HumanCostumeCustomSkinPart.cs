using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Utility;
using Characters;

namespace CustomSkins
{
    class HumanCostumeCustomSkinPart: BaseCustomSkinPart
    {
        public HumanCostumeCustomSkinPart(BaseCustomSkinLoader loader, List<Renderer> renderers, string rendererId, int maxSize, Vector2? textureScale = null, bool useTransparentMaterial = false) :
            base(loader, renderers, rendererId, maxSize, textureScale, useTransparentMaterial)
        {
        }

        protected override Material SetNewTexture(Texture2D texture)
        {
            _renderers[0].material = HumanSetupMaterials.GetCustomSkinMaterial();
            return base.SetNewTexture(texture);
        }
    }
}
