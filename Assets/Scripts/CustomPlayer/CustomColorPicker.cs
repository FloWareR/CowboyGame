using UnityEngine;
using UnityEngine.UI;

public class CustomColorPicker : MonoBehaviour
{
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    public Image colorDisplay;

    public ColorFieldSelector colorFieldSelector; // Reference to the ColorFieldSelector to apply preview

    private Color currentColor;

    void Start()
    {
        redSlider.onValueChanged.AddListener(UpdateColor);
        greenSlider.onValueChanged.AddListener(UpdateColor);
        blueSlider.onValueChanged.AddListener(UpdateColor);

        UpdateColor(0f); // Initial color update
    }

    // Set sliders to the color of the selected field without triggering preview
    public void SetSliders(Color color)
    {
        redSlider.SetValueWithoutNotify(color.r);
        greenSlider.SetValueWithoutNotify(color.g);
        blueSlider.SetValueWithoutNotify(color.b);

        currentColor = color;
        UpdateColorDisplay(); // Update only the UI preview color, not the material
    }

    // Get the current color from sliders (with alpha set to 1)
    public Color GetCurrentColor()
    {
        return new Color(redSlider.value, greenSlider.value, blueSlider.value, 1f);
    }

    // This method updates the color display and applies a preview to the material
    void UpdateColor(float value)
    {
        currentColor = GetCurrentColor();

        // Update the color display (UI Image)
        UpdateColorDisplay();

        // Apply preview color to the selected field in the material
        colorFieldSelector?.PreviewColor(currentColor);
    }

    // Update the UI color display (without updating the material)
    private void UpdateColorDisplay()
    {
        if (colorDisplay != null)
        {
            colorDisplay.color = currentColor;
        }
    }
}
