using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Utility;
using Characters;

namespace CustomSkins
{
    class HumanHairCustomSkinPart: BaseCustomSkinPart
    {
        private string _hairTexture;

        public HumanHairCustomSkinPart(BaseCustomSkinLoader loader, List<Renderer> renderers, string rendererId, int maxSize, string hairTexture, Vector2? textureScale = null): 
            base(loader, renderers, rendererId, maxSize, textureScale)
        {
            _hairTexture = hairTexture;
        }

        protected override Material SetNewTexture(Texture2D texture)
        {
            if (_hairTexture != string.Empty)
                _renderers[0].material = HumanSetupMaterials.Materials[_hairTexture];
            return base.SetNewTexture(texture);
        }
    }
}
