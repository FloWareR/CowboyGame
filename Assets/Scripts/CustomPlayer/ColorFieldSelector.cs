using UnityEngine;
using UnityEngine.UI;

public class ColorFieldSelector : MonoBehaviour
{
    public CustomColorPicker colorPicker;
    public ShaderColorPreviewUpdater colorPreview;
    public Material characterMaterial; // Reference to the material
    private string selectedFieldName;
    private Color originalColor; // Store the original color before preview

    // This method sets the color picker to edit a specific color field
    public void SelectColorField(string fieldName)
    {
        selectedFieldName = fieldName;
        originalColor = characterMaterial.GetColor(selectedFieldName); // Store the original color
        colorPicker.SetSliders(originalColor); // Update sliders to match the current color
    }

    // Preview color change in the material
    public void PreviewColor(Color color)
    {
        if (!string.IsNullOrEmpty(selectedFieldName))
        {
            characterMaterial.SetColor(selectedFieldName, color);
        }
    }

    // Apply the color change
    public void ApplyColorChange()
    {
        colorPreview.UpdateColorPreviews();
        // No action needed as the previewed color is already applied.
        // This function can be kept for clarity or future custom save logic.
    }

    // Cancel the color change and revert to the original color
    public void CancelColorChange()
    {
        if (!string.IsNullOrEmpty(selectedFieldName))
        {
            characterMaterial.SetColor(selectedFieldName, originalColor); // Restore original color in material
            colorPicker.SetSliders(originalColor); // Restore slider values to the original color
        }
    }
}
