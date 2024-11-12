using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset;
    public Image foregroundImage;
    public Image backgroundImage; 

    private Camera _camera;

    public bool playerHP = false;
    private void Start()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (!playerHP)
        {
            var direction = (target.position - _camera.transform.position).normalized;
            bool isHidden = Vector3.Dot(direction, _camera.transform.forward) <= 0.0f;
            foregroundImage.enabled = !isHidden;
            backgroundImage.enabled = !isHidden;
            transform.position = _camera.WorldToScreenPoint(target.position + offset);
        }
    
    }

    public void SetHealthBarPercentage(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);

        var foregroundRectTransform = foregroundImage.rectTransform;

        var parentWidth = GetComponent<RectTransform>().rect.width;
        var newWidth = parentWidth * percentage;

        foregroundRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }
}