using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this to use TextMesh Pro
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Reference to the UI Image whose transparency you want to modify
    public Image uiImage;

    // Reference to the TextMeshProUGUI object whose text you want to modify
    public TextMeshProUGUI gameOverText;  // TextMesh Pro reference

    // Reference to the GameObject you want to enable
    public List<GameObject> objectToEnable;

    // Duration for the transition (in seconds)
    public float transitionDuration = 1f;

    public float targetAlpha;

    private bool isGameover = false;

    public EnemySpawner enemySpawner;

    // Function to start the transparency change, enable the GameObject, and change text
    public void GameOverScreen()
    {
        if (!isGameover)
        {
            if (uiImage != null)
            {
                // Start the Coroutine to smoothly transition the transparency
                StartCoroutine(SmoothTransparencyTransition(targetAlpha));
            }
            else
            {
                Debug.LogWarning("UI Image is not assigned!");
            }


            objectToEnable[0].SetActive(false);
            objectToEnable[1].SetActive(true);
            objectToEnable[2].SetActive(false);
            gameOverText.text = $"Total souls: {enemySpawner.deathCount}";
            isGameover = true;
        }
    }

    // Coroutine to smoothly change the transparency (alpha) over time
    private IEnumerator SmoothTransparencyTransition(float targetAlpha)
    {
        // Get the current color of the UI Image
        Color currentColor = uiImage.color;
        float startAlpha = currentColor.a;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            // Gradually transition the alpha value
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / transitionDuration);

            // Apply the new alpha value to the UI Image
            currentColor.a = newAlpha;
            uiImage.color = currentColor;

            // Increase elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the target alpha is exactly set after the transition completes
        currentColor.a = targetAlpha;
        uiImage.color = currentColor;
    }

    public void restartGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneName);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}