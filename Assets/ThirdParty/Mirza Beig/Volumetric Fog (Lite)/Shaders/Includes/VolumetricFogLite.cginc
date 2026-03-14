#ifndef VOLUMETRIC_FOG_LITE_CGINC
#define VOLUMETRIC_FOG_LITE_CGINC

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

float HenyeyGreenstein(float cosTheta, float g2, float gPow2)
{
    float denominator = (1.0 + gPow2) - (g2 * cosTheta);
    return (1.0 - gPow2) / (4.0 * PI * pow(denominator, 1.5));
}

float CalculateMainLightScattering(float cosTheta, float costThetaPow2, float g2, float gPow2)
{
    return HenyeyGreenstein(cosTheta, g2, gPow2);
}

// https://www.shadertoy.com/view/WsfBDf
// Shadertoy author comment: "this noise, including the 5.58... scrolling constant are from Jorge Jimenez".

// NOTE: Random.hlsl also has InterleavedGradientNoise() but it has some banding with high noise strength.

float ignoise(float2 position, int frame)
{
    position += (float(frame) * 5.588238f);
    return frac(52.9829189f * frac(0.06711056f * float(position.x) + 0.00583715f * float(position.y)));
}

float ignoise_blurred(float2 position, int frame, float blurAmount)
{
    // Sample at 4 corners and interpolate smoothly
    float2 gridPos = floor(position / blurAmount) * blurAmount;
    float2 localPos = frac(position / blurAmount);
    
    // Smooth interpolation
    float2 u = smoothstep(0.0, 1.0, localPos);
    
    // Sample 4 corners
    float n00 = ignoise(gridPos, frame);
    float n10 = ignoise(gridPos + float2(blurAmount, 0.0), frame);
    float n01 = ignoise(gridPos + float2(0.0, blurAmount), frame);
    float n11 = ignoise(gridPos + float2(blurAmount, blurAmount), frame);
    
    // Bilinear interpolation
    float nx0 = lerp(n00, n10, u.x);
    float nx1 = lerp(n01, n11, u.x);
    return lerp(nx0, nx1, u.y);
}

#define MAX_RAYMARCH_STEPS 128

void VolumetricFog_float(

        float4 colour,

        float3 position,
        float2 screenPosition,

        uint raymarchSteps,
        float raymarchNoise,

        float raymarchDistance,
        float raymarchDistanceBias,

        float3 raymarchDirection,

        float raymarchMaxDepth,

        float mainLightStrength,

        float mainLightAnisotropy,
        float mainLightAnisotropyBlend,

        float mainLightShadowStrength,
        
        float godrayStrength,
        float shadowOpacity,
        float heightFadeStart,
        float heightFadeDistance,
        float shadowPresenceThreshold,

        out float4 output)
{
    
    //discard;
    
#ifdef SHADERGRAPH_PREVIEW
    
    discard;
    
#endif
    
    float random = frac((sin(dot(screenPosition, float2(12.9898, 78.233))) * 43758.55) + _TimeParameters.x);
    float interleavedGradientNoise = ignoise_blurred(screenPosition, random * 9999.0, 2.0);  
        
    raymarchDistance = min(raymarchDistance, raymarchMaxDepth + (raymarchDistance * raymarchDistanceBias));
    raymarchDistance = lerp(raymarchDistance, raymarchDistance * interleavedGradientNoise, raymarchNoise);    
        
    float rayStep = raymarchDistance / raymarchSteps;
    float3 rayDirectionStep = raymarchDirection * rayStep;
    
    float density = 0.0;    
    float depth = 0.0;
    
    float mainLightShading = 0.0;
    float averageShadow = 0.0;
        
    float raymarchStepsMinusOne = raymarchSteps - 1.0;
    
//#define _MAIN_LIGHT_ENABLED
#ifdef _MAIN_LIGHT_ENABLED

    Light mainLight = GetMainLight();
    
    // Scattering pre-calculations.
    
    float g = mainLightAnisotropy;
    
    float g2 = g * 2.0;
    float gSquared = g * g;
        
    float cosTheta = dot(mainLight.direction, raymarchDirection);
    float cosThetaSquared = cosTheta * cosTheta;

#endif
    
    float3 raymarchStartPosition = position;
    
    // Raymarch.
    
    bool breakLoop;
    
    for (int i = 0; i < MAX_RAYMARCH_STEPS; i++)
    {
        if (i >= raymarchSteps)
        {
            break;
        }
        
        float progress = i / raymarchStepsMinusOne;
        
        // If last iteration, go all the way to the back :)
        
        if (i == raymarchSteps - 1)
        {
            rayStep = raymarchMaxDepth - depth;
            rayDirectionStep = raymarchDirection * rayStep;
        }
                
        // March position forward by ray step.
        
        position += rayDirectionStep;
        depth += rayStep;
        
        // Main light shadows and scattering.
        
        float mainLightShadow = 1.0;        
        float mainLightScattering = 1.0;
        
#define _MAIN_LIGHT_SHADOWS_ENABLED
        
#ifdef _MAIN_LIGHT_ENABLED 
#if defined(_MAIN_LIGHT_SHADOWS_ENABLED)
    
        float4 shadowCoord = TransformWorldToShadowCoord(position);
        
        mainLightShadow = MainLightRealtimeShadow(shadowCoord);
        //mainLightShadow = MainLightShadow(shadowCoord, position, half4(1.0, 1.0, 1.0, 1.0), _MainLightOcclusionProbes);
        
        mainLightShadow = lerp(1.0, mainLightShadow, mainLightShadowStrength);
    
#endif
        
        // Scattering.
        
        mainLightScattering = CalculateMainLightScattering(cosTheta, cosThetaSquared, g2, gSquared);
        mainLightScattering = lerp(1.0, mainLightScattering, mainLightAnisotropyBlend * godrayStrength);

#endif
        
        mainLightShading += mainLightShadow * mainLightScattering;
        averageShadow += mainLightShadow;
        
        density++;
                
        // Depth test.
       
        if (depth > raymarchMaxDepth)
        {
            break;
        }       
    }
    
    density /= raymarchSteps;
    colour.a *= density;
    
    // Apply shadow opacity control - reduces transparency in shadow areas
    averageShadow /= raymarchSteps;
    colour.a *= lerp(shadowOpacity, 1.0, averageShadow);
    
    // Height-based fade: fade out fog above heightFadeStart
    float heightDiff = position.y - heightFadeStart;
    float heightFade = smoothstep(0.0, heightFadeDistance, heightDiff);
    colour.a *= (1.0 - heightFade);
    
    // Reduce opacity when far from shadows (low averageShadow = far from shadow objects)
    colour.a *= lerp(shadowPresenceThreshold, 1.0, 1.0 - averageShadow);
    
#ifdef _MAIN_LIGHT_ENABLED
    
    mainLightShading /= raymarchSteps;
    mainLightShading *= mainLightStrength;
    
    float3 mainLightColour = mainLight.color;    
    
    mainLightColour *= mainLightShading;    
    colour.rgb += mainLightColour;
    
#endif    
    
    output = colour;
}

#endif
