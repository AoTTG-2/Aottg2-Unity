using System;

using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MirzaBeig.VolumetricFogLite
{
    public interface IVolumetricFog
    {
        public int GetDownsampleLevel();
        public void SetDownsampleLevel(int downsampleLevel);
    }

    public class VolumetricFogRendererFeatureLite : ScriptableRendererFeature, IVolumetricFog
    {
        [Serializable]
        public enum RenderTextureQuality
        {
            Low,
            Medium,
            High
        }

        [Serializable]
        public class Settings
        {
            [Range(1, 8)]
            public int fogDownsampleLevel = 4;

            [Space]

            public Material fogMaterial;
            public Material depthMaterial;

            [Space]

            public Material compositeMaterial;

            [Space]

            public string compositeMaterialColourTextureName = "_ColourTexture";

            [Space]

            public string compositeMaterialFogTextureName = "_FogTexture";
            public string compositeMaterialDepthTextureName = "_DepthTexture";

            [Space]

            public RenderTextureQuality renderTextureQuality = RenderTextureQuality.Medium;
        }

        public int GetDownsampleLevel()
        {
            return settings.fogDownsampleLevel;
        }

        public void SetDownsampleLevel(int downsampleLevel)
        {
            settings.fogDownsampleLevel = downsampleLevel;
        }

        public bool renderInSceneView = true;

        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingSkybox;

        [Space]

        public Settings settings;
        private CustomRenderPass customRenderPass;

        public override void Create()
        {
            customRenderPass = new CustomRenderPass(settings)
            {
                renderPassEvent = renderPassEvent
            };
        }

        // Here you can inject one or multiple render passes in the renderer.
        // This method is called when setting up the renderer once per-camera.

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            bool enqueuePass = renderingData.cameraData.cameraType == CameraType.Game;
            enqueuePass |= renderingData.cameraData.cameraType == CameraType.Reflection;

            if (renderInSceneView)
            {
                enqueuePass |= renderingData.cameraData.cameraType == CameraType.SceneView;
            }

            if (enqueuePass)
            {
                renderer.EnqueuePass(customRenderPass);
            }
        }

        protected override void Dispose(bool disposing)
        {
            customRenderPass.Dispose();
        }

        class CustomRenderPass : ScriptableRenderPass
        {
            private Settings settings;

            private RenderTextureDescriptor colourTextureDescriptor;

            private RenderTextureDescriptor fogTextureDescriptor;
            private RenderTextureDescriptor depthTextureDescriptor;

            private RTHandle colourTextureHandle;

            private RTHandle fogTextureHandle;
            private RTHandle depthTextureHandle;

            public CustomRenderPass(Settings settings)
            {
                if (settings == null)
                {
                    return;
                }

                this.settings = settings;
                RenderTextureFormat renderTextureFormat;

                switch (settings.renderTextureQuality)
                {
                    case RenderTextureQuality.Low:
                        {
                            // Expect banding.

                            renderTextureFormat = RenderTextureFormat.Default;

                            break;
                        }
                    case RenderTextureQuality.Medium:
                        {
                            // Smooth.

                            renderTextureFormat = RenderTextureFormat.ARGB64;

                            break;
                        }
                    case RenderTextureQuality.High:
                        {
                            // Smooth + HDR (works with bloom).

                            renderTextureFormat = RenderTextureFormat.ARGBFloat;

                            break;
                        }
                    default:
                        {
                            throw new Exception("Unknown enum.");
                        }
                }

                colourTextureDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height, renderTextureFormat, 0);

                fogTextureDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height, renderTextureFormat, 0);
                depthTextureDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.RFloat, 0);
            }

            // This method is called before executing the render pass.
            // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
            // When empty this render pass will render to the active camera render target.
            // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
            // The render pipeline will ensure target setup and clearing happens in a performant manner.

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {

            }

            // Called before Execute().

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                int fogDownsampleLevel = settings.fogDownsampleLevel;

                colourTextureDescriptor.width = cameraTextureDescriptor.width;
                colourTextureDescriptor.height = cameraTextureDescriptor.height;

                fogTextureDescriptor.width = colourTextureDescriptor.width / fogDownsampleLevel;
                fogTextureDescriptor.height = colourTextureDescriptor.height / fogDownsampleLevel;

                depthTextureDescriptor.width = fogTextureDescriptor.width;
                depthTextureDescriptor.height = fogTextureDescriptor.height;

                // Check if the descriptor has changed, and reallocate the RTHandle if necessary.

                RenderingUtils.ReAllocateIfNeeded(ref colourTextureHandle, colourTextureDescriptor);

                RenderingUtils.ReAllocateIfNeeded(ref fogTextureHandle, fogTextureDescriptor);
                RenderingUtils.ReAllocateIfNeeded(ref depthTextureHandle, depthTextureDescriptor);
            }

            // Here you can implement the rendering logic.
            // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
            // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
            // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                // Get a CommandBuffer from pool.

                CommandBuffer cmd = CommandBufferPool.Get();

                CameraData cameraData = renderingData.cameraData;
                RTHandle cameraTargetHandle = cameraData.renderer.cameraColorTargetHandle;

                // Save full-res colour screen and set texture in material (for in-shader compositing).

                Blit(cmd, cameraTargetHandle, colourTextureHandle);
                settings.compositeMaterial.SetTexture(settings.compositeMaterialColourTextureName, colourTextureHandle);

                // Save depth texture and assign to material.

                Blit(cmd, cameraTargetHandle, depthTextureHandle, settings.depthMaterial);
                settings.compositeMaterial.SetTexture(settings.compositeMaterialDepthTextureName, depthTextureHandle);

                // Render fog.

                Blit(cmd, cameraTargetHandle, fogTextureHandle, settings.fogMaterial);

                // Set the fog texture.

                // You can also use Shader Graph's URP Buffer node -> BlitSource (_BlitTexture),
                // which will be whatever is passed as the blit source on the Blit command.

                settings.compositeMaterial.SetTexture(settings.compositeMaterialFogTextureName, fogTextureHandle);

                // Composite.

                Blit(cmd, fogTextureHandle, cameraTargetHandle, settings.compositeMaterial);

                // Execute the command buffer, then release it back to the pool.

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

            // Cleanup any allocated resources that were created during the execution of this render pass.

            public override void OnCameraCleanup(CommandBuffer cmd)
            {

            }

            public void Dispose()
            {
                colourTextureHandle?.Release();

                fogTextureHandle?.Release();
                depthTextureHandle?.Release();
            }
        }
    }
}

