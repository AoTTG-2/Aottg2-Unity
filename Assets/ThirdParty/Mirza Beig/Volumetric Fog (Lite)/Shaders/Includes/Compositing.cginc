
#ifndef COMPOSITING_CGINC
#define COMPOSITING_CGINC

//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

float GetHalfDepth(float2 uv, Texture2D depthTexture)
{
    float d = SAMPLE_TEXTURE2D_X(depthTexture, sampler_PointClamp, uv).r;
    return LinearEyeDepth(d, _ZBufferParams);
}

float GetLinearDepth(float2 uv)
{
    float d = SampleSceneDepth(uv);
    return LinearEyeDepth(d, _ZBufferParams);
}

float4 LinearEyeDepth(float4 d)
{
    float4 linearEyeDepth = 0;
    
    linearEyeDepth.x = LinearEyeDepth(d.x, _ZBufferParams);
    linearEyeDepth.y = LinearEyeDepth(d.y, _ZBufferParams);
    linearEyeDepth.z = LinearEyeDepth(d.z, _ZBufferParams);
    linearEyeDepth.w = LinearEyeDepth(d.w, _ZBufferParams);
    
    return linearEyeDepth;
}

void UpdateNearestSample(inout float minDistance, inout float2 nearestUV, float z, float2 uv, float zFull)
{
    float distance = abs(z - zFull);
    
    if (distance < minDistance)
    {
        minDistance = distance;
        nearestUV = uv;
    }
}

// This upscales without (well, hugely reduced!) edge artifacts.

// More info on technique:
// > https://computergraphics.stackexchange.com/questions/318/how-to-improve-on-bilateral-upsampling-in-real-time-scenarios

// Read page 5, describing Nearest-Depth Filter: 
// > https://developer.download.nvidia.com/assets/gamedev/files/sdk/11/OpacityMappingSDKWhitePaper.pdf

void NearestDepthFilter_float(float2 uv, float2 texelSize, Texture2D fogTexture, Texture2D depthTexture, out float4 output)
{
    float zFull = GetLinearDepth(uv);
    const float depthThreshold = 0.5;
    
    float minDistance = 1e9;
    
    float2 nearestUV = uv;
    bool withinThreshold = true;
        
    withinThreshold = true;

    // Loop through adjacent texels to find the nearest depth.
    
    [unroll]
    for (int y = 0; y <= 1; ++y)
    {
        for (int x = 0; x <= 1; ++x)
        {
            float2 currentUV = uv + (float2(x, y) * texelSize) - (0.5 * texelSize);
            
            float z = GetHalfDepth(currentUV, depthTexture);
            
            float distance = abs(z - zFull);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestUV = currentUV;
            }

            if (distance >= depthThreshold)
            {
                withinThreshold = false;
            }
        }
    }
    
    float4 fogSample = 0.0;

    if (withinThreshold)
    {
        // Bilinear.
        // Non-edge pixel.
        
        fogSample = SAMPLE_TEXTURE2D_X(fogTexture, sampler_LinearClamp, uv);
    }
    else
    {
        // Edge pixel.
        // Nearest-neighbor (point sampling).
        
        fogSample = SAMPLE_TEXTURE2D_X(fogTexture, sampler_PointClamp, nearestUV);
    }

    output = fogSample;
}


#endif