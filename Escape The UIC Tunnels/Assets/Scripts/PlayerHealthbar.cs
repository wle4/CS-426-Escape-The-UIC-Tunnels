using UnityEngine;

public class PlayerHealthbar : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public JumpscareHandler jumpscare;

    private bool hasTriggeredJumpscare = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (hasTriggeredJumpscare) return;

        hasTriggeredJumpscare = true;
        Debug.Log("Player died!");

        if (jumpscare != null)
        {
            jumpscare.TriggerJumpscare();
        }
    }

    public void ResetJumpscareFlag()
    {
        hasTriggeredJumpscare = false;
    }
}
