using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public RawImage fillImage;

    private float fullWidth;

    void Start()
    {
        if (fillImage != null)
            fullWidth = fillImage.rectTransform.sizeDelta.x;
    }

    void Update()
    {
        if (playerHealth == null || fillImage == null) return;

        float fillAmount = playerHealth.CurrentHealth / playerHealth.MaxHealth;
        Vector2 size = fillImage.rectTransform.sizeDelta;
        size.x = fullWidth * fillAmount;
        fillImage.rectTransform.sizeDelta = size;
    }
}