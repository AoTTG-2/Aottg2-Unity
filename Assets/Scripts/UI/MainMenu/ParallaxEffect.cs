using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public float parallaxIntensity = 1f;
    public float smoothTime = 0.3f;
    public float scale = 1.1f;

    private Vector2 currentVelocity;
    private RectTransform rectTransform;
    private Vector2 lastValidMousePosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one * scale;
        lastValidMousePosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        // Check if mouse is within the game window
        bool isMouseInWindow = mousePosition.x >= 0 && mousePosition.x <= Screen.width &&
                               mousePosition.y >= 0 && mousePosition.y <= Screen.height;

        if (isMouseInWindow)
        {
            lastValidMousePosition = mousePosition;
        }
        else
        {
            // Use the last valid mouse position when the mouse is outside the window
            mousePosition = lastValidMousePosition;
        }

        Vector2 targetPosition = GetConstrainedTargetPosition(mousePosition);

        rectTransform.anchoredPosition = Vector2.SmoothDamp(
            rectTransform.anchoredPosition,
            targetPosition * parallaxIntensity,
            ref currentVelocity,
            smoothTime
        );

        // Clamp the position to prevent going out of bounds
        rectTransform.anchoredPosition = ClampPosition(rectTransform.anchoredPosition);
    }

    private Vector2 GetConstrainedTargetPosition(Vector2 mousePosition)
    {
        float maxMovementX = (scale - 1) * (Screen.width / 2);
        float maxMovementY = (scale - 1) * (Screen.height / 2);

        float backgroundX = MapRange(mousePosition.x, 0, Screen.width, -maxMovementX, maxMovementX);
        float backgroundY = MapRange(mousePosition.y, 0, Screen.height, -maxMovementY, maxMovementY);

        return new Vector2(backgroundX, backgroundY);
    }

    private Vector2 ClampPosition(Vector2 position)
    {
        float maxMovementX = (scale - 1) * (Screen.width / 2);
        float maxMovementY = (scale - 1) * (Screen.height / 2);

        return new Vector2(
            Mathf.Clamp(position.x, -maxMovementX, maxMovementX),
            Mathf.Clamp(position.y, -maxMovementY, maxMovementY)
        );
    }

    private float MapRange(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
}