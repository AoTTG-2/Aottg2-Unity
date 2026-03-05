using UnityEngine;

/// <summary>
/// Controller for the Dynamic Snow Parallax shader.
/// Manages animation parameters and allows runtime control of snow effects.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class DynamicSnowController : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    private MaterialPropertyBlock propertyBlock;
    
    // Animation parameters
    [Header("Animation")]
    [SerializeField, Range(0f, 5f)] private float scrollSpeed = 1f;
    [SerializeField, Range(0f, 1f)] private float waveAmplitude = 0.1f;
    [SerializeField, Range(0f, 10f)] private float waveFrequency = 2f;
    
    // Snow parameters
    [Header("Snow")]
    [SerializeField, Range(0f, 1f)] private float snowAmount = 0.5f;
    [SerializeField] private Color snowColor = new Color(0.95f, 0.95f, 1f, 1f);
    
    // Parallax parameters
    [Header("Parallax")]
    [SerializeField, Range(0.0f, 1f)] private float heightScale = 0.1f;
    [SerializeField, Range(4, 32)] private int parallaxLayers = 8;
    
    // Layer animation parameters
    [Header("Layer Animation (End Portal Style)")]
    [SerializeField, Range(2, 8)] private int layerCount = 4;
    [SerializeField, Range(0f, 2f)] private float layerDepth = 0.5f;
    [SerializeField, Range(0f, 1f)] private float layerBrightness = 0.3f;
    
    // Rim lighting
    [Header("Rim Lighting")]
    [SerializeField, Range(0f, 10f)] private float rimPower = 2f;
    [SerializeField] private Color rimColor = new Color(0.5f, 0.7f, 1f, 0.5f);
    
    // Surface properties
    [Header("Surface")]
    [SerializeField, Range(0f, 1f)] private float smoothness = 0.5f;
    [SerializeField, Range(0f, 1f)] private float metallic = 0f;
    
    private void OnEnable()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();
        
        propertyBlock = new MaterialPropertyBlock();
        UpdateShaderProperties();
    }
    
    private void OnDisable()
    {
        if (propertyBlock != null)
        {
            targetRenderer.SetPropertyBlock(null);
        }
    }
    
    private void Update()
    {
        UpdateShaderProperties();
    }
    
    /// <summary>
    /// Updates all shader properties from the current values.
    /// Called every frame to ensure animation parameters are synchronized.
    /// </summary>
    private void UpdateShaderProperties()
    {
        if (targetRenderer == null)
            return;
        
        targetRenderer.GetPropertyBlock(propertyBlock);
        
        // Animation parameters
        propertyBlock.SetFloat("_ScrollSpeed", scrollSpeed);
        propertyBlock.SetFloat("_WaveAmplitude", waveAmplitude);
        propertyBlock.SetFloat("_WaveFrequency", waveFrequency);
        
        // Snow parameters
        propertyBlock.SetFloat("_SnowAmount", snowAmount);
        propertyBlock.SetColor("_SnowColor", snowColor);
        
        // Parallax parameters
        propertyBlock.SetFloat("_HeightScale", heightScale);
        propertyBlock.SetFloat("_ParallaxLayers", parallaxLayers);
        
        // Layer animation
        propertyBlock.SetFloat("_LayerCount", layerCount);
        propertyBlock.SetFloat("_LayerDepth", layerDepth);
        propertyBlock.SetFloat("_LayerBrightness", layerBrightness);
        
        // Rim lighting
        propertyBlock.SetFloat("_RimPower", rimPower);
        propertyBlock.SetColor("_RimColor", rimColor);
        
        // Surface properties
        propertyBlock.SetFloat("_Smoothness", smoothness);
        propertyBlock.SetFloat("_Metallic", metallic);
        
        targetRenderer.SetPropertyBlock(propertyBlock);
    }
    
    // Public methods for runtime control
    
    public void SetSnowAmount(float amount)
    {
        snowAmount = Mathf.Clamp01(amount);
    }
    
    public void SetScrollSpeed(float speed)
    {
        scrollSpeed = Mathf.Clamp(speed, 0f, 5f);
    }
    
    public void SetWaveAmplitude(float amplitude)
    {
        waveAmplitude = Mathf.Clamp01(amplitude);
    }
    
    public void SetWaveFrequency(float frequency)
    {
        waveFrequency = Mathf.Clamp(frequency, 0f, 10f);
    }
    
    public void SetLayerCount(int count)
    {
        layerCount = Mathf.Clamp(count, 2, 8);
    }
    
    public void SetSnowColor(Color color)
    {
        snowColor = color;
    }
    
    public void SetRimColor(Color color)
    {
        rimColor = color;
    }
    
    public void SetRimPower(float power)
    {
        rimPower = Mathf.Clamp(power, 0f, 10f);
    }
    
    /// <summary>
    /// Animate snow intensity over time.
    /// </summary>
    public void AnimateSnowIntensity(float targetAmount, float duration)
    {
        StartCoroutine(AnimateSnowIntensityCoroutine(targetAmount, duration));
    }
    
    private System.Collections.IEnumerator AnimateSnowIntensityCoroutine(float targetAmount, float duration)
    {
        float startAmount = snowAmount;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            snowAmount = Mathf.Lerp(startAmount, targetAmount, elapsed / duration);
            yield return null;
        }
        
        snowAmount = targetAmount;
    }
    
    /// <summary>
    /// Create a pulsing animation effect.
    /// </summary>
    public void PulseAnimation(float intensity, float duration, int pulses)
    {
        StartCoroutine(PulseAnimationCoroutine(intensity, duration, pulses));
    }
    
    private System.Collections.IEnumerator PulseAnimationCoroutine(float intensity, float duration, int pulses)
    {
        float pulseDuration = duration / pulses;
        
        for (int i = 0; i < pulses; i++)
        {
            float elapsed = 0f;
            
            // Pulse up
            while (elapsed < pulseDuration / 2f)
            {
                elapsed += Time.deltaTime;
                scrollSpeed = Mathf.Lerp(1f, 1f + intensity, elapsed / (pulseDuration / 2f));
                yield return null;
            }
            
            elapsed = 0f;
            
            // Pulse down
            while (elapsed < pulseDuration / 2f)
            {
                elapsed += Time.deltaTime;
                scrollSpeed = Mathf.Lerp(1f + intensity, 1f, elapsed / (pulseDuration / 2f));
                yield return null;
            }
        }
    }
}
