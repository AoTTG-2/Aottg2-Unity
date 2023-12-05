using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public float parallaxIntensity = 1f;
    public float smoothTime = 0.3f;
    public float scale = 1.1f;

    private Vector2 currentVelocity;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one * scale;
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 targetPosition = GetConstrainedTargetPosition(mousePosition);

        rectTransform.anchoredPosition = Vector2.SmoothDamp(
            rectTransform.anchoredPosition,
            targetPosition * parallaxIntensity,
            ref currentVelocity,
            smoothTime
        );
    }

    private Vector2 GetConstrainedTargetPosition(Vector2 mousePosition)
    {
        float maxMovementX = (scale - 1) * (Screen.width / 2);
        float maxMovementY = (scale - 1) * (Screen.height / 2);

        float backgroundX = MapRange(mousePosition.x, 0, Screen.width, -maxMovementX, maxMovementX);
        float backgroundY = MapRange(mousePosition.y, 0, Screen.height, -maxMovementY, maxMovementY);

        return new Vector2(backgroundX, backgroundY);
    }

    private float MapRange(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
}