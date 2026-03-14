Shader "Custom/URP/RayMarchedVolumetricAtmosphere"
{
    Properties
    {
        _FogColor("Fog Color", Color) = (0.8, 0.8, 0.9, 1.0)
        _FogDensity("Fog Density", Range(0.0, 5.0)) = 1.0
        _FogDistance("Max Fog Distance", Range(10.0, 1000.0)) = 200.0
        _HeightFalloff("Height Falloff", Range(0.0, 10.0)) = 2.0
        _AtmosphericScattering("Scattering", Range(0.0, 1.0)) = 0.5
        _RayMarchSteps("Ray March Steps", Range(2, 128)) = 32
        _SoftIntersectionRange("Soft Intersection Range", Float) = 5.0
        _SunIntensity("Sun Intensity", Range(0.0, 2.0)) = 1.0
        _BacklightColor("Backlight Color", Color) = (0.2, 0.2, 0.3, 1.0)
        _BacklightIntensity("Backlight Intensity", Range(0.0, 2.0)) = 0.5
        _OverallOpacity("Overall Opacity", Range(0.0, 1.0)) = 1.0
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Pass
        {
            Name "RayMarchedVolume"
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest LEqual
            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _FogColor;
                float _FogDensity;
                float _FogDistance;
                float _HeightFalloff;
                float _AtmosphericScattering;
                int _RayMarchSteps;
                float _SoftIntersectionRange;
                float _SunIntensity;
                float4 _BacklightColor;
                float _BacklightIntensity;
                float _OverallOpacity;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.screenPos = ComputeScreenPos(output.positionHCS);
                return output;
            }

            // Pseudo-random noise for variation
            float hash(float3 p)
            {
                p = frac(p * 0.3183099 + 0.1);
                p *= 17.0;
                return frac(p.x * p.y * p.z * (p.x + p.y + p.z));
            }

            // Smooth 3D noise
            float noise(float3 p)
            {
                float3 i = floor(p);
                float3 f = frac(p);
                f = f * f * (3.0 - 2.0 * f);

                float n = lerp(
                    lerp(lerp(hash(i), hash(i + float3(1, 0, 0)), f.x),
                        lerp(hash(i + float3(0, 1, 0)), hash(i + float3(1, 1, 0)), f.x), f.y),
                    lerp(lerp(hash(i + float3(0, 0, 1)), hash(i + float3(1, 0, 1)), f.x),
                        lerp(hash(i + float3(0, 1, 1)), hash(i + float3(1, 1, 1)), f.x), f.y), f.z);
                return n;
            }

            // Fractional Brownian Motion for cloud-like variation
            float fbm(float3 p, int octaves)
            {
                float value = 0.0;
                float amplitude = 0.5;
                float frequency = 1.0;

                for (int i = 0; i < octaves; i++)
                {
                    value += amplitude * noise(p * frequency);
                    amplitude *= 0.5;
                    frequency *= 2.0;
                }
                return value;
            }

            // Calculate fog density at a point
            float getDensity(float3 worldPos, float3 cameraPos)
            {
                // Distance from camera
                float dist = distance(worldPos, cameraPos);
                float distFalloff = max(0.0, 1.0 - (dist / _FogDistance));
                
                // Height-based falloff (fog denser at lower altitudes)
                float heightDiff = worldPos.y - cameraPos.y;
                float heightFalloff = exp(-max(0.0, heightDiff) * _HeightFalloff * 0.1);
                
                // Add noise variation for organic appearance
                float noiseFactor = fbm(worldPos * 0.01 + _Time.y * 0.05, 3);
                noiseFactor = lerp(0.5, 1.5, noiseFactor);
                
                // Combine all factors
                float density = _FogDensity * distFalloff * heightFalloff * noiseFactor;
                return saturate(density);
            }

            float4 frag(Varyings input) : SV_Target
            {
                float3 rayStart = _WorldSpaceCameraPos;
                float3 rayEnd = input.positionWS;
                float3 rayDir = normalize(rayEnd - rayStart);
                float rayDistance = distance(rayStart, rayEnd);
                
                // Sample scene depth for soft particle fade
                float2 screenUV = input.screenPos.xy / input.screenPos.w;
                float sceneDepth = SampleSceneDepth(screenUV);
                float sceneDistance = LinearEyeDepth(sceneDepth, _ZBufferParams);
                float particleDepth = LinearEyeDepth(input.positionHCS.z, _ZBufferParams);
                
                // Soft intersection fade - fade out when particle is close to scene geometry
                float depthDifference = sceneDistance - particleDepth;
                float softFade = saturate(depthDifference / _SoftIntersectionRange);
                
                // Ray march through the volume
                float stepSize = rayDistance / float(_RayMarchSteps);
                float3 currentPos = rayStart;
                
                float transmittance = 1.0;
                float3 accumulatedLight = float3(0.0, 0.0, 0.0);
                
                // March through the volume
                for (int i = 0; i < _RayMarchSteps; i++)
                {
                    // Sample density at current position
                    float density = getDensity(currentPos, rayStart);
                    
                    // Apply Beer's Law for transmittance
                    float sampleTransmittance = exp(-density * stepSize * 0.1);
                    
                    // Accumulate light
                    float3 scatteredColor = _FogColor.rgb * (1.0 + _AtmosphericScattering * 0.5);
                    accumulatedLight += transmittance * (1.0 - sampleTransmittance) * scatteredColor;
                    
                    // Update transmittance
                    transmittance *= sampleTransmittance;
                    
                    // Move along ray
                    currentPos += rayDir * stepSize;
                }
                
                // Get main directional light (sun)
                Light mainLight = GetMainLight();
                float3 sunColor = mainLight.color;
                float3 sunDirection = mainLight.direction;
                
                // Calculate sun contribution based on view direction
                float3 viewDir = normalize(rayEnd - rayStart);
                float sunDot = dot(viewDir, sunDirection);
                float sunContribution = saturate(sunDot * 0.5 + 0.5);
                
                // Calculate backlight contribution (opposite side)
                float backlightDot = dot(viewDir, -sunDirection);
                float backlightContribution = saturate(backlightDot * 0.5 + 0.5);
                
                // Blend in sun and backlight colors
                float3 finalColor = accumulatedLight;
                finalColor = lerp(finalColor, sunColor * accumulatedLight, sunContribution * _SunIntensity);
                finalColor = lerp(finalColor, _BacklightColor.rgb * accumulatedLight, backlightContribution * _BacklightIntensity);
                
                float finalAlpha = 1.0 - transmittance;
                
                // Apply soft particle fade to hide intersection edges
                finalAlpha *= softFade;
                
                // Apply overall opacity control
                finalAlpha *= _OverallOpacity;
                
                return float4(finalColor, saturate(finalAlpha));
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}