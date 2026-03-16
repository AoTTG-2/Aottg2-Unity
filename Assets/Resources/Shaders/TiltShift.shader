Shader "Custom/TiltShift"
{
    Properties
    {
        _MainTex ("Source Texture", 2D) = "white" {}
        _CameraDepthTexture ("Depth Texture", 2D) = "white" {}
        _BlurAmount ("Blur Amount", Float) = 10
        _BlurRadiusScale ("Blur Radius Scale", Float) = 1.0
        _BlurSamples ("Blur Samples (8-32, higher=quality)", Float) = 16
        _FocusArea ("Focus Area (Y Position)", Float) = 0.5
        _FocusHeight ("Focus Height (Tilt Shift Band)", Float) = 0.2
        _BlurFalloff ("Blur Falloff", Float) = 1.8
        _MinDepth ("Min Depth (Close to Camera)", Float) = 0.1
        _MaxDepth ("Max Depth (Far from Camera)", Float) = 0.5
        _FocusScreenPos ("Focus Screen Position", Vector) = (0.5, 0.5, 0, 0)
        _HasFocusObject ("Has Focus Object", Float) = 0.0
    }
    
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalRenderPipeline"
        }
        
        Pass
        {
            ZWrite Off
            ZTest Always
            Cull Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_CameraDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);
            
            float _BlurAmount;
            float _BlurRadiusScale;
            float _BlurSamples;
            float _FocusArea;
            float _FocusHeight;
            float _BlurFalloff;
            float _MinDepth;
            float _MaxDepth;
            float4 _FocusScreenPos;
            float _HasFocusObject;
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }
            
            // Bokeh blur using circular sampling pattern
            float4 BokehBlur(float2 uv, float blurRadius, int samples)
            {
                float4 blurred = float4(0.0, 0.0, 0.0, 0.0);
                
                // Sample count (8 to 32)
                int sampleCount = max(8, min(samples, 32));
                float invSamples = 1.0 / float(sampleCount);
                
                for (int i = 0; i < sampleCount; i++)
                {
                    float angle = 6.28318 * float(i) * invSamples;
                    float radius = blurRadius * sqrt(float(i) * invSamples);
                    
                    float2 offset = float2(cos(angle), sin(angle)) * radius;
                    float4 sampleColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + offset);
                    
                    blurred += sampleColor;
                }
                
                return blurred / float(sampleCount);
            }
            
            float4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;
                
                // Sample center
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                
                // Sample depth at this pixel
                float depth = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, uv).r;
                
                // Convert depth to linear range (0 = near, 1 = far)
                float depthFade = smoothstep(_MinDepth, _MaxDepth, depth);
                
                // Calculate tilt shift blur (horizontal band)
                float yPos = saturate(uv.y);
                float distFromFocusY = abs(yPos - _FocusArea);
                float blurTiltShift = smoothstep(0.0, _FocusHeight, distFromFocusY);
                blurTiltShift = pow(blurTiltShift, _BlurFalloff);
                
                float blur = blurTiltShift;
                
                // If object exists, blend it with tilt shift using multiply
                if (_HasFocusObject > 0.5)
                {
                    float2 focusPos = _FocusScreenPos.xy;
                    float2 distFromObj = uv - focusPos;
                    float distanceFromObj = length(distFromObj);
                    
                    float blurObjectFocus = smoothstep(0.0, _FocusHeight, distanceFromObj);
                    blurObjectFocus = pow(blurObjectFocus, _BlurFalloff);
                    
                    // Multiply both blur masks together
                    blur = blurTiltShift * blurObjectFocus;
                }
                
                // Apply depth fade
                blur *= depthFade;
                
                // Apply bokeh blur if needed - MUCH SMALLER radius for smooth results
                if (blur > 0.001)
                {
                    int sampleCount = int(clamp(_BlurSamples, 8.0, 32.0));
                    float blurRadius = blur * _BlurAmount * _BlurRadiusScale * 0.0005;
                    
                    float4 blurred = BokehBlur(uv, blurRadius, sampleCount);
                    col = lerp(col, blurred, blur);
                }
                
                return col;
            }
            ENDHLSL
        }
    }
}
