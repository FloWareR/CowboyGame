using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Button button;
    private Image buttonImage;
    private Color originalColor;

    // The hover effects
    public Color hoverColor;
    public Color pressedColor; // The color when the button is pressed

    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = button.GetComponent<Image>();

        // Save the original button color
        originalColor = buttonImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change button color on hover
        if (!button.interactable) return;  // Don't change color if button is not interactable
        buttonImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset color when hover ends
        if (!button.interactable) return;  // Don't reset color if button is not interactable
        buttonImage.color = originalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Change color when the button is pressed down
        if (!button.interactable) return;  // Don't change color if button is not interactable
        buttonImage.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Return to the hover color if pointer is still on the button after release
        if (!button.interactable) return;  // Don't reset color if button is not interactable
        buttonImage.color = hoverColor;
    }
}
