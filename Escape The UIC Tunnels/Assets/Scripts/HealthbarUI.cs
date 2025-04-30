using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    public Slider healthSlider;
    public PlayerHealthbar playerHealth;

    void Update()
    {
        if (playerHealth != null && healthSlider != null)
        {
            healthSlider.value = playerHealth.currentHealth;

        }
    }
}
