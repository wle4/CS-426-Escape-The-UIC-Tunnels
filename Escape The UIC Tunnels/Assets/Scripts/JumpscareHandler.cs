using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JumpscareHandler : MonoBehaviour
{
    public CanvasGroup jumpscareCanvasGroup;
    public float fadeDuration = 0.2f;
    public float holdDuration = 3f;
    public PlayerHealthbar playerHealth;

    public void TriggerJumpscare()
        {
            Debug.Log("TRIGGER JUMPSCARE");

            jumpscareCanvasGroup.alpha = 1f;
            jumpscareCanvasGroup.blocksRaycasts = true;
            jumpscareCanvasGroup.interactable = true;

            playerHealth.currentHealth = playerHealth.maxHealth;
            playerHealth.ResetJumpscareFlag();

            // Force UI update
            GameObject.FindObjectOfType<HealthbarUI>().healthSlider.value = playerHealth.currentHealth;
        }


    private IEnumerator JumpscareSequence()
        {
            jumpscareCanvasGroup.alpha = 1f;
            jumpscareCanvasGroup.blocksRaycasts = true;

            yield return new WaitForSeconds(holdDuration);

            playerHealth.currentHealth = playerHealth.maxHealth;

            // Reset trigger flag
            playerHealth.SendMessage("ResetJumpscareFlag");

            jumpscareCanvasGroup.alpha = 0f;
            jumpscareCanvasGroup.blocksRaycasts = false;
        }

}
