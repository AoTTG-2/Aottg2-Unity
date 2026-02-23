using UnityEngine;
using System.Collections.Generic;
using Utility;
using Settings;

namespace Characters
{
    class TitanColliderToggler: MonoBehaviour
    {
        public BaseTitan Owner;
        public HashSet<BaseCharacter> NearbyCharacters = new HashSet<BaseCharacter>();
        public TitanProjectileDetection _projectile;
        protected bool _look = false;
        protected bool _enabled = false;

        public static TitanColliderToggler Create(BaseTitan owner)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(owner.transform);
            TitanColliderToggler toggler = go.AddComponent<TitanColliderToggler>();
            toggler.Owner = owner;
            toggler._projectile = TitanProjectileDetection.Create(owner);
            toggler.SetColliders(false);
            return toggler;
        }

        public void RegisterLook()
        {
            _look = true;
        }

        public void SetNearby(BaseCharacter character, bool nearby)
        {
            if (nearby)
                NearbyCharacters.Add(character);
            else if (NearbyCharacters.Contains(character))
                NearbyCharacters.Remove(character);
        }

        protected void FixedUpdate()
        {
            if (NearbyCharacters.Count > 0)
                Util.RemoveNullOrDead(NearbyCharacters);
            if (_enabled)
            {
                if (!_look && !_projectile.Detect && NearbyCharacters.Count == 0)
                    ToggleComponents(false);
            }
            else if (_look || _projectile.Detect || NearbyCharacters.Count > 0)
                ToggleComponents(true);
            _look = false;
        }

        protected void ToggleComponents(bool enable)
        {
            if (SettingsManager.GeneralSettings.AnimationCullingFix.Value)
            {
                ForceAnimationCulling(enable);
            }
            
            SetColliders(enable);
            if (SettingsManager.GeneralSettings.DebugDetection.Value)
            {
                if (enable)
                {
                    Owner.AddOutline();
                }
                else
                {
                    Owner.RemoveOutline();
                }
            }
            
        }

        protected void ForceAnimationCulling(bool forcedOn)
        {
            Owner.Animation.OverrideCulling(forcedOn);
        }

        protected void SetColliders(bool enable)
        {
            foreach (Collider collider in Owner.BaseTitanCache.ToggleColliders)
            {
                if (collider != null)
                {
                    collider.enabled = enable;
                }
            }
            _enabled = enable;
        }
    }
}
