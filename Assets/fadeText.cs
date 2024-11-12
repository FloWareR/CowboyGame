using TMPro;
using UnityEngine;

public class FadeTextEffect : MonoBehaviour
{
    public TextMeshProUGUI tapToPlayText;
    public float fadeDuration = 1.5f; // Time for one fade in/out cycle
    public float delayBetweenFades = 0.25f;

    private bool fadingIn = true;
    private float fadeTimer;

    void Start()
    {
        if (tapToPlayText == null)
        {
            tapToPlayText = GetComponent<TextMeshProUGUI>();
        }
        fadeTimer = 0;
    }

    void Update()
    {
        if (tapToPlayText == null) return;

        // Calculate fade amount
        fadeTimer += Time.deltaTime;
        float alpha = fadeTimer / fadeDuration;

        // Toggle fading in/out based on the timer
        if (fadingIn)
        {
            tapToPlayText.alpha = Mathf.Lerp(0, 1, alpha);
        }
        else
        {
            tapToPlayText.alpha = Mathf.Lerp(1, 0, alpha);
        }

        // Reset fade timer and toggle fading direction when the cycle completes
        if (fadeTimer >= fadeDuration)
        {
            fadingIn = !fadingIn;
            fadeTimer = -delayBetweenFades; // Adds a delay before starting the next cycle
        }
    }
}