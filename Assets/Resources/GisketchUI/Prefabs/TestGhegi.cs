using UnityEngine;
using System.Collections;

public class InstantiateColumnStack : MonoBehaviour
{
    public GameObject imagePrefab;
    public int numberOfInstances = 5;
    public float spacing = 50f;
    public float moveDistance = 500f;
    public float moveDuration = 1f;
    public float staggerDelay = 0.2f;

    private void Start()
    {
        StartCoroutine(SpawnAndMoveObjects());
    }

    private IEnumerator SpawnAndMoveObjects()
    {
        for (int i = 0; i < numberOfInstances; i++)
        {
            // Instantiate the prefab
            GameObject instantiatedObject = Instantiate(imagePrefab);

            // Get the RectTransform component of the instantiated object
            RectTransform rectTransform = instantiatedObject.GetComponent<RectTransform>();

            // Set the anchor to top left (0, 1)
            rectTransform.anchorMin = new Vector2(0f, 1f);
            rectTransform.anchorMax = new Vector2(0f, 1f);

            // Set the parent of the instantiated object to the first child of the current object's transform
            rectTransform.SetParent(transform.GetChild(0), false);

            // Set the pivot to top left (0, 1)
            rectTransform.pivot = new Vector2(0f, 1f);

            // Calculate the position based on the index and spacing
            float yPosition = -i * spacing;

            // Set the initial position of the instantiated object outside the view on the left
            rectTransform.anchoredPosition = new Vector2(-moveDistance, yPosition);

            // Wait for the stagger delay before moving the next object
            yield return new WaitForSeconds(staggerDelay);

            // Move the instantiated object into the view using LeanTween
            LeanTween.moveX(instantiatedObject, 0f, moveDuration).setEase(LeanTweenType.easeOutQuad);
        }
    }
}