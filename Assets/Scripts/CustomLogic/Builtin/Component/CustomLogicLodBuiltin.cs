// implement
using Settings;
using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// WIP feature to support LOD (Level of Detail) for map objects, currently only supports a single LOD level with a distance threshold.
    /// </summary>
    [CLType(Name = "LodBuiltin", Static = true, Abstract = true, IsComponent = true)]
    partial class CustomLogicLodBuiltin : BuiltinComponentInstance
    {
        public LODGroup Value;
        public CustomLogicMapObjectBuiltin OwnerMapObject;
        public GameObject Owner;

        [CLConstructor]
        public CustomLogicLodBuiltin() : base(null) { }
        public CustomLogicLodBuiltin(CustomLogicMapObjectBuiltin owner) : base(GetOrAddComponent<LODGroup>(owner.Value.GameObject))
        {
            OwnerMapObject = owner;
            Owner = owner.Value.GameObject;
            Value = (LODGroup)Component;
            SetupSingleLod(1.0f);
        }

        /// <summary>
        /// Configures the distance threshold.
        /// </summary>
        [CLProperty]
        public float DistanceThreshold
        {
            get => Value.GetLODs()[0].screenRelativeTransitionHeight;
            set
            {
                var lods = Value.GetLODs();
                if (lods.Length > 0)
                {
                    lods[0].screenRelativeTransitionHeight = value;
                    Value.SetLODs(lods);
                    Value.RecalculateBounds();
                }
            }
        }

        private float _detailPriority = 1;
        /// <summary>
        /// Configures the detail priority.
        /// </summary>
        [CLProperty]
        public float DetailPriority
        {
            get
            {
                return _detailPriority;
            }
            set
            {
                _detailPriority = Mathf.Clamp01(value);

                // disable renderers based on lod settings.

                if (_detailPriority < SettingsManager.GraphicsSettings.LightDistance.Value)
                {
                    // Disable the renderers if the detail priority is below the light distance setting
                    var renderers = Owner.GetComponentsInChildren<Renderer>();
                    foreach (var renderer in renderers)
                    {
                        renderer.enabled = false;
                    }
                }
                else
                {
                    // Enable the renderers if the detail priority is above the light distance setting
                    var renderers = Owner.GetComponentsInChildren<Renderer>();
                    foreach (var renderer in renderers)
                    {
                        renderer.enabled = true;
                    }
                }
            }
        }

        public void SetupSingleLod(float threshold)
        {
            if (Value == null)
                return;

            var renderers = Owner.GetComponentsInChildren<Renderer>();

            // Only one LOD level that shows the object, and turns it off when below the threshold
            LOD[] lods = new LOD[1];
            lods[0] = new LOD(threshold, renderers);

            Value.SetLODs(lods);
            Value.fadeMode = LODFadeMode.None;
            Value.animateCrossFading = false;
            Value.RecalculateBounds();
        }
    }
}
