using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TiltShiftRenderFeature : ScriptableRendererFeature
{
    [SerializeField]
    public float blurAmount = 10f;
    
    [SerializeField]
    public float blurRadiusScale = 1f;
    
    [SerializeField]
    public float blurSamples = 16f;
    
    [SerializeField]
    public float focusDistance = 0.2f;
    
    [SerializeField]
    public float focusHeight = 0.2f;
    
    [SerializeField]
    public float blurFalloff = 1.8f;
    
    [SerializeField]
    public float minDepth = 0.1f;
    
    [SerializeField]
    public float maxDepth = 0.5f;
    
    [SerializeField]
    public float focusObjectYOffset = 0f;
    
    [SerializeField]
    public bool useFocusObject = false;
    
    private TiltShiftPass tiltShiftPass;
    
    public override void Create()
    {
        tiltShiftPass = new TiltShiftPass();
        tiltShiftPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }
    
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        tiltShiftPass.Setup(this);
        renderer.EnqueuePass(tiltShiftPass);
    }
    
    private class TiltShiftPass : ScriptableRenderPass
    {
        private TiltShiftRenderFeature featureSettings;
        private Material material;
        private int tempColorId;
        
        public TiltShiftPass()
        {
            Shader shader = Shader.Find("Custom/TiltShift");
            if (shader != null)
            {
                material = new Material(shader);
            }
            else
            {
                Debug.LogError("[TiltShift] Shader not found!");
            }
            
            tempColorId = Shader.PropertyToID("_TempTiltShift");
        }
        
        public void Setup(TiltShiftRenderFeature settings)
        {
            this.featureSettings = settings;
        }
        
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0;
            cmd.GetTemporaryRT(tempColorId, desc, FilterMode.Bilinear);
        }
        
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null)
                return;
            
            CommandBuffer cmd = CommandBufferPool.Get("TiltShift");
            
            // Update material properties
            material.SetFloat("_BlurAmount", featureSettings.blurAmount);
            material.SetFloat("_BlurRadiusScale", featureSettings.blurRadiusScale);
            material.SetFloat("_BlurSamples", featureSettings.blurSamples);
            material.SetFloat("_BlurFalloff", featureSettings.blurFalloff);
            material.SetFloat("_MinDepth", featureSettings.minDepth);
            material.SetFloat("_MaxDepth", featureSettings.maxDepth);
            material.SetFloat("_FocusArea", featureSettings.focusDistance);
            material.SetFloat("_FocusHeight", featureSettings.focusHeight);
            
            // Handle focal point
            Transform focusObject = TiltShiftFocusHelper.Instance != null ? TiltShiftFocusHelper.Instance.transform : null;
            
            if (focusObject != null)
            {
                // Calculate object-based focal point
                Camera camera = renderingData.cameraData.camera;
                Vector3 focusWorldPos = focusObject.position + Vector3.up * featureSettings.focusObjectYOffset;
                Vector4 focusScreenPos = camera.WorldToViewportPoint(focusWorldPos);
                
                material.SetVector("_FocusScreenPos", focusScreenPos);
                material.SetFloat("_HasFocusObject", 1.0f);
            }
            else
            {
                material.SetFloat("_HasFocusObject", 0.0f);
            }
            
            RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTargetHandle;
            
            // Blit: source -> temp with material
            cmd.Blit(source, tempColorId, material, 0);
            
            // Blit: temp -> source
            cmd.Blit(tempColorId, source);
            
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
        
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempColorId);
        }
    }
}
