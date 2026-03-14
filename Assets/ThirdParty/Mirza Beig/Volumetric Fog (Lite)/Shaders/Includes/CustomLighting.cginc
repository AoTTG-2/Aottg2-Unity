
#ifndef CUSTOM_LIGHTING_CGINC
#define CUSTOM_LIGHTING_CGINC

#if !SHADERGRAPH_PREVIEW

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

#endif

void GetMainLightColour_half(out half3 output)
{
    
#ifdef SHADERGRAPH_PREVIEW
    
    discard;
    
#else
    
    Light light = GetMainLight();
    output = light.color;

#endif
    
}

void GetAdditionalLightsColour_half(float3 position, float3 normal, out half3 output)
{
    
#ifdef SHADERGRAPH_PREVIEW
    
    discard;
    
#else
    
    output = 0.0;
    
#define _ADDITIONAL_LIGHTS
#define _ADDITIONAL_LIGHT_SHADOWS
    
#ifdef _ADDITIONAL_LIGHTS
    
    int additionalLightCount = GetAdditionalLightsCount();
         
    for (int i = 0; i < additionalLightCount; i++)
    {
        Light light = GetAdditionalLight(i, position, 1.0h);
  
        //half NdotL = saturate(dot(normal, light.direction));
        
        //float distanceToLight = length(light.position - position);
        //float inverseSquareFalloff = 1.0 / (distanceToLight * distanceToLight);
        
        half lightAttenuation = light.distanceAttenuation * light.shadowAttenuation;

        half3 lightColour = light.color * lightAttenuation;
        
        output += lightColour;
    }
    
#endif
    
#endif
    
}

void GetMainLightLambert_half(float3 normal, out half3 output)
{
    
#ifdef SHADERGRAPH_PREVIEW
    
    discard;
    
#else
    
    Light light = GetMainLight();
    output = LightingLambert(light.color, light.direction, normal);

#endif
    
}

float ignoise(float2 position, int frame)
{
    position += (float(frame) * 5.588238f);
    return frac(52.9829189f * frac(0.06711056f * float(position.x) + 0.00583715f * float(position.y)));
}

void GetMainLightShadow_float(float3 position, out float output)
{
    
#ifdef SHADERGRAPH_PREVIEW
    
    discard;
    
#else
    
    float4 shadowCoord = TransformWorldToShadowCoord(position);
    output = MainLightShadow(shadowCoord, position, 1.0h, _MainLightOcclusionProbes);
    
#endif
    
}

// Get animated random 3D noise (not smooth) in the range [0.0, 1.0].

float3 GetAnimatedRandomNoiseXYZ(float2 position)
{
    float2 positionX = position + 0.00;
    float2 positionY = position + 10.0;
    float2 positionZ = position - 10.0;
    
    float randomX = frac((sin(dot(positionX, float2(12.9898, 78.233))) * 43758.55) + _Time.y);
    float randomY = frac((sin(dot(positionY, float2(12.9898, 78.233))) * 43758.55) + _Time.y);
    float randomZ = frac((sin(dot(positionZ, float2(12.9898, 78.233))) * 43758.55) + _Time.y);
    
    float interleavedGradientNoiseX = ignoise(positionX, randomX * 9999.0);
    float interleavedGradientNoiseY = ignoise(positionY, randomY * 9999.0);
    float interleavedGradientNoiseZ = ignoise(positionZ, randomZ * 9999.0);
        
    return float3(interleavedGradientNoiseX, interleavedGradientNoiseY, interleavedGradientNoiseZ);
}

void GetMainLightShadow_float(float3 position, float noise, float noiseLightDirectionAttenuation, float noiseLightDirectionAttenuationBias, out float output)
{
    
#ifdef SHADERGRAPH_PREVIEW
    
    discard;
  
#else  
        
    Light light = GetMainLight();
    
    // Random noise.
    
    if (noise > 0.0)
    {
        float3 noiseXYZ = GetAnimatedRandomNoiseXYZ(position.xy);
        
        // [0.0, 1.0] -> [-1.0, 1.0]
        
        noiseXYZ *= 2.0;
        noiseXYZ -= 1.0;
        
        noiseXYZ *= noise;
    
        // Filter by dot product to extract noise along light direction.
    
        float3 randomDirection = normalize(noiseXYZ);
        float lightDirectionAlignment = abs(dot(randomDirection, light.direction));
        
        lightDirectionAlignment = smoothstep(noiseLightDirectionAttenuationBias, 1.0, lightDirectionAlignment);
        
        noiseXYZ *= lerp(1.0, lightDirectionAlignment, noiseLightDirectionAttenuation);
    
        position += noiseXYZ;
    }
    
    GetMainLightShadow_float(position, output);

#endif
    
}

void SampleCustomShadowmap_float(float3 position, Texture2D shadowmap, out float output)
{
    
#ifdef SHADERGRAPH_PREVIEW
    
    discard;
        
#else
    
    float4 shadowCoord = TransformWorldToShadowCoord(position);
    
    ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
    half4 shadowParams = GetMainLightShadowParams();
    
    output = SampleShadowmap(TEXTURE2D_ARGS(shadowmap, sampler_LinearClampCompare), shadowCoord, shadowSamplingData, shadowParams, false);
    
#endif

}

void SampleCustomShadowmap_float(float3 position, Texture2D shadowmap, float noise, float noiseLightDirectionAttenuation, float noiseLightDirectionAttenuationBias, out float output)
{
    
#ifdef SHADERGRAPH_PREVIEW
    
    discard;
    
#else
    
    Light light = GetMainLight();
    
    // Random noise.
    
    if (noise > 0.0)
    {
        float3 noiseXYZ = GetAnimatedRandomNoiseXYZ(position.xy);
        
        // [0.0, 1.0] -> [-1.0, 1.0]
        
        noiseXYZ *= 2.0;
        noiseXYZ -= 1.0;
        
        noiseXYZ *= noise;
    
        // Filter by dot product to extract noise along light direction.
    
        float3 randomDirection = normalize(noiseXYZ);
        float lightDirectionAlignment = abs(dot(randomDirection, light.direction));
        
        lightDirectionAlignment = smoothstep(noiseLightDirectionAttenuationBias, 1.0, lightDirectionAlignment);
        
        noiseXYZ *= lerp(1.0, lightDirectionAlignment, noiseLightDirectionAttenuation);
    
        position += noiseXYZ;
    }
    
    SampleCustomShadowmap_float(position, shadowmap, output);
    
    output = 1.0 - output;
    
#endif
    
}

void CustomDepthTest_float(UnityTexture2D depthTexture, float4 screenPosition, out float output)
{
    float currentDepth = screenPosition.z / screenPosition.w;
    float sceneDepth = tex2D(depthTexture, screenPosition.xy / screenPosition.w).r;
        
    if (sceneDepth > currentDepth)
    {
        discard;
    }
    
    output = 1.0;
}

void CustomDepthFade_float(UnityTexture2D depthTexture, float4 screenPosition, float distance, out float output)
{
    float currentDepth = screenPosition.z / screenPosition.w;
    float sceneDepth = tex2D(depthTexture, screenPosition.xy / screenPosition.w).r;
    
    float depthDifference = currentDepth - sceneDepth ;
    
    output = saturate(depthDifference / distance);
    
    //output = 1.0;

}

#endif