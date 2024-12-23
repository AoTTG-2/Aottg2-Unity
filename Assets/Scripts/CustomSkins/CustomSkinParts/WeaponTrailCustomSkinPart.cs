using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins
{
    class WeaponTrailCustomSkinPart : BaseCustomSkinPart
    {
         
        private List<MeleeWeaponTrail> _weaponTrails;

        public WeaponTrailCustomSkinPart(BaseCustomSkinLoader loader, List<MeleeWeaponTrail> weaponTrails, string rendererId, int maxSize, Vector2? textureScale = null) : base(loader, null, rendererId, maxSize, textureScale, true)
        {
            _weaponTrails = weaponTrails;
        }
        protected override bool IsValidPart()
        {
            return _weaponTrails.Count > 0 && _weaponTrails[0] != null;
        }

        protected override void DisableRenderers()
        {
            SetMaterial(MaterialCache.TransparentMaterial);
        }

        protected override void SetMaterial(Material material)
        {
            foreach (MeleeWeaponTrail trail in _weaponTrails)
                trail.SetMaterial(material);
        }

        protected override Material SetNewTexture(Texture2D texture)
        {
            var material = new Material(_weaponTrails[0]._material);
            material.mainTexture = texture;
            if (_textureScale != _defaultTextureScale)
            {
                Vector2 scale = material.mainTextureScale;
                material.mainTextureScale = new Vector2(scale.x * _textureScale.x, scale.y * _textureScale.y);
            }
            SetMaterial(material);
            return material;
        }
    }
}
