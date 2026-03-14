using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System.Reflection;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

using UnityEngine.UI;

namespace MirzaBeig.VolumetricFogLite
{
    public class VolumetricFogControllerLite : MonoBehaviour
    {
        public Material material;

        [Space]

        public Slider slider_raymarchSteps;
        public Slider slider_downsampleLevel;

        public Slider slider_mainLightIntensity;

        [Space]

        const string keyword_MAIN_LIGHT_ENABLED = "_MAIN_LIGHT_ENABLED";

        public const string materialPropertyName_raymarchSteps = "_Raymarch_Steps";
        public const string materialPropertyName_mainLightIntensity = "_Main_Light_Intensity";

        public ScriptableRendererFeature rendererFeature { get; private set; }
        public IVolumetricFog volumetricFogCommonInterface { get; private set; }

        void Awake()
        {

        }
        void Start()
        {
            // Must be in Start else volumetricFogCommonInterface ends up null.

            var renderer = (GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset).GetRenderer(0);
            var property = typeof(ScriptableRenderer).GetProperty("rendererFeatures", BindingFlags.NonPublic | BindingFlags.Instance);

            List<ScriptableRendererFeature> rendererFeatures = property.GetValue(renderer) as List<ScriptableRendererFeature>;

            // Take first IVolumetricFog that is also active.

            rendererFeature = rendererFeatures.Where(x => x.isActive && (x as IVolumetricFog) != null).First();

            // I know this feature has IVolumetricFog because it must be as per the previous line.

            volumetricFogCommonInterface = rendererFeature as IVolumetricFog;

            // ...

            SetRaymarchSteps(slider_raymarchSteps.value);
            SetDownsampleLevel(slider_downsampleLevel.value);

            SetMainLightIntensity(slider_mainLightIntensity.value);
        }

        void Update()
        {

        }

        public void SetRaymarchSteps(float value)
        {
            material.SetInt(materialPropertyName_raymarchSteps, Mathf.RoundToInt(value));
        }

        public void SetDownsampleLevel(float value)
        {
            volumetricFogCommonInterface.SetDownsampleLevel(Mathf.RoundToInt(value));
        }

        public void SetMainLightIntensity(float value)
        {
            material.SetFloat(materialPropertyName_mainLightIntensity, value);

            // Main light.

            if (value > 0.0f)
            {
                material.EnableKeyword(keyword_MAIN_LIGHT_ENABLED);
            }
            else
            {
                material.DisableKeyword(keyword_MAIN_LIGHT_ENABLED);
            }
        }
    }
}
