using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JumpscareHandler : MonoBehaviour
{
    public CanvasGroup jumpscareCanvasGroup;
    public float fadeDuration = 0.2f;
    public float holdDuration = 3f;
    public PlayerHealthbar playerHealth;
    public AudioSource jumpscareAudio; // 🎵 Reference the AudioSource

    public void TriggerJumpscare()
    {
        Debug.Log("TRIGGER JUMPSCARE");

        // Play the scream
        if (jumpscareAudio != null && !jumpscareAudio.isPlaying)
        {
            jumpscareAudio.Play();
        }

        StartCoroutine(JumpscareSequence());

        playerHealth.currentHealth = playerHealth.maxHealth;
        playerHealth.ResetJumpscareFlag();

        var ui = GameObject.FindObjectOfType<HealthbarUI>();
        if (ui != null)
        {
            ui.healthSlider.value = playerHealth.maxHealth;
        }
    }

    private IEnumerator JumpscareSequence()
    {
        jumpscareCanvasGroup.alpha = 1f;
        jumpscareCanvasGroup.blocksRaycasts = true;

        yield return new WaitForSeconds(holdDuration);

        jumpscareCanvasGroup.alpha = 0f;
        jumpscareCanvasGroup.blocksRaycasts = false;
    }
}
