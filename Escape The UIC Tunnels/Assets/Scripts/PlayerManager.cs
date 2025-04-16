using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("Core Stats")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public float flashlightCharge = 100f;

    [Header("References")]
    public HUDController hud;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        hud = UIManager.instance.GetHUDController();
        if (hud != null)
        {
            hud.UpdateHealth(currentHealth, maxHealth);
            hud.UpdateFlashlight(flashlightCharge, 100f);
        }

        instance = this;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        currentHealth--;
        // Example: drain flashlight
        if (Input.GetKey(KeyCode.F))
        {
            flashlightCharge = Mathf.Max(0, flashlightCharge - Time.deltaTime * 5f);
            hud.UpdateFlashlight(flashlightCharge, 100f);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        hud.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died");
        // handle death logic here (disable movement, trigger animation, etc.)
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        hud.UpdateHealth(currentHealth, maxHealth);
    }
}
