using UnityEngine;
using UnityEngine.UI;

public class SmoothScroll : MonoBehaviour
{
    public float smoothness = 25f;
    public float scrollMultiplier = 1f;

    private ScrollRect scrollRect;
    private Vector2 targetVelocity;
    private bool isMouseWheelScrolling = false;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    private void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f)
        {
            targetVelocity.y += scrollInput * scrollMultiplier;
            isMouseWheelScrolling = true;
        }

        if (isMouseWheelScrolling)
        {
            scrollRect.velocity = Vector2.Lerp(scrollRect.velocity, targetVelocity, Time.deltaTime * smoothness);

            // Gradually reduce the target velocity
            targetVelocity *= 0.95f;

            if (targetVelocity.magnitude < 0.01f)
            {
                isMouseWheelScrolling = false;
                targetVelocity = Vector2.zero;
            }
        }
    }

    public void OnScrollbarValueChanged()
    {
        // Reset mouse wheel scrolling when the user interacts with the scrollbar
        isMouseWheelScrolling = false;
        targetVelocity = Vector2.zero;
    }
}