using UnityEngine;

/// <summary>
/// Helper script to track a focal point object for the TiltShift render feature.
/// Attach this script to the GameObject you want to use as the focal point (e.g., your character).
/// The render feature will automatically find this and use it.
/// </summary>
public class TiltShiftFocusHelper : MonoBehaviour
{
    public static TiltShiftFocusHelper Instance { get; private set; }
    
    private void OnEnable()
    {
        // Make this the active focal point when enabled
        Instance = this;
    }
    
    private void OnDisable()
    {
        // Clear the reference when disabled
        if (Instance == this)
            Instance = null;
    }
}
