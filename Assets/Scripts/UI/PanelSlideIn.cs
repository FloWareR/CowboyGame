using UnityEngine;
using System.Collections;

public class PanelSlideIn : MonoBehaviour
{
    public RectTransform panel; // Reference to the panel RectTransform
    public float slideDuration = 1f;  // Duration of the slide-in effect
    public float overshootAmount = 100f; // How far it overshoots before settling

    private Vector2 startPosition;
    private Vector2 offScreenPosition;

    void Start()
    {
        // Initialize the starting positions
        startPosition = panel.anchoredPosition;
        offScreenPosition = new Vector2(Screen.width, panel.anchoredPosition.y); // Offscreen to the right

        // Set the panel off-screen initially
        panel.anchoredPosition = offScreenPosition;

        // Start the slide-in animation
        StartCoroutine(SlideInPanel());
    }

    // Coroutine to handle the slide-in effect
    IEnumerator SlideInPanel()
    {
        float elapsedTime = 0f;
        Vector2 targetPosition = startPosition;

        // Move the panel from right to left with overshoot effect
        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;

            // Use an animation curve to simulate fast start and overshoot (ease out)
            float t = Mathf.Clamp01(elapsedTime / slideDuration);
            float overshootT = Mathf.Pow(1 - t, 3); // Easing function for overshoot effect

            // Calculate the new position with overshoot and ease-out
            Vector2 newPosition = Vector2.Lerp(offScreenPosition, targetPosition, t) + new Vector2(overshootAmount * overshootT, 0);

            panel.anchoredPosition = newPosition;

            yield return null;
        }

        // Ensure the final position is exactly at the target
        panel.anchoredPosition = startPosition;
    }
}
