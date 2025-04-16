using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Health")]
    public Slider healthSlider;
    public Image healthFill;
    public Color fullHPColor;
    public Color emptyHPColor;

    [Header("Flashlight")]
    public Slider flashlightSlider;
    public Image flashFill;
    public Color fullFlashColor;
    public Color emptyFlashColor;

    [Header("Interact Icon/Text")]
    public RawImage handIcon;
    private bool isVisible = false;
    public float checkDistance = 6f;
    public float hideDelay = 0.2f;
    private float hideTimer = 0f;
    public TextMeshProUGUI interactionText;
    private bool isHolding = false;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (handIcon != null)
            handIcon.enabled = false;
        if (interactionText != null) 
            interactionText.text = "";
    }

    void Update()
    {
        
    }

    public void UpdateHealth(float current, float max)
    {
        if (healthSlider != null)
        {
            healthSlider.value = current / max;
            healthFill.color = Color.Lerp(emptyHPColor, fullHPColor, current / max);
        }
    }

    public void UpdateFlashlight(float current, float max)
    {
        if (flashlightSlider != null)
        {
            flashlightSlider.value = current / max;
            flashFill.color = Color.Lerp(emptyFlashColor, fullFlashColor, current / max);
        }
    }

    public void ShowInteraction(string text)
    {
        if (handIcon != null) handIcon.enabled = true;
        if (interactionText != null)
        {
            interactionText.text = text;
            interactionText.enabled = true;
        }
        isVisible = true;
    }

    public void HideInteraction()
    {
        if (handIcon != null) handIcon.enabled = false;
        if (interactionText != null)
        {
            interactionText.text = "";
            interactionText.enabled = false;
        }
        isVisible = false;
    }
}
