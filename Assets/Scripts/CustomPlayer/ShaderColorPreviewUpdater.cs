using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShaderColorPreviewUpdater : MonoBehaviour
{
    [System.Serializable]
    public struct ColorPreview
    {
        public string fieldName;  // Name of the color field in the shader
        public Image previewImage; // Image component to display the color
    }

    public Material characterMaterial; // Reference to the shader material
    public List<ColorPreview> colorPreviews = new List<ColorPreview>(); // List of color fields and their corresponding preview images

    void OnEnable()
    {
        UpdateColorPreviews();
    }


    // Fetches colors from the material and updates each preview image
    public void UpdateColorPreviews()
    {
        foreach (ColorPreview colorPreview in colorPreviews)
        {
            if (characterMaterial.HasProperty(colorPreview.fieldName))
            {
                Color fieldColor = characterMaterial.GetColor(colorPreview.fieldName);
                colorPreview.previewImage.color = fieldColor;
            }
            else
            {
                Debug.LogWarning($"Field '{colorPreview.fieldName}' not found in the material.");
            }
        }
    }
}
