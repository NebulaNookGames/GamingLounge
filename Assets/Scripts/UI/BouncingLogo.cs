using UnityEngine;
using UnityEngine.UI;

public class BouncingLogo : MonoBehaviour
{
    public RectTransform logo;        // Assign the Image RectTransform
    public float speed = 200f;        // Adjust speed as needed

    private Vector2 direction;        // Movement direction
    private RectTransform canvasRect; // Reference to the Canvas

    void Start()
    {
        if (logo == null)
            logo = GetComponent<RectTransform>();

        canvasRect = logo.parent.GetComponent<RectTransform>();

        // Random initial direction
        direction = new Vector2(Random.value > 0.5f ? 1 : -1, Random.value > 0.5f ? 1 : -1).normalized;
    }

    void Update()
    {
        // Move the logo
        logo.anchoredPosition += direction * speed * Time.deltaTime;

        // Get boundaries
        Vector2 min = canvasRect.rect.min + logo.rect.size * 0.5f;
        Vector2 max = canvasRect.rect.max - logo.rect.size * 0.5f;

        // Check collisions with screen bounds
        if (logo.anchoredPosition.x <= min.x || logo.anchoredPosition.x >= max.x)
            direction.x *= -1;

        if (logo.anchoredPosition.y <= min.y || logo.anchoredPosition.y >= max.y)
            direction.y *= -1;

        // Prevent it from hitting corners perfectly
        AvoidPerfectCornerHits();
    }

    void AvoidPerfectCornerHits()
    {
        // Add a small nudge to the direction if both X and Y hit simultaneously
        if (Mathf.Abs(direction.x) == Mathf.Abs(direction.y))
        {
            direction += new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            direction.Normalize(); // Keep speed consistent
        }
    }
}