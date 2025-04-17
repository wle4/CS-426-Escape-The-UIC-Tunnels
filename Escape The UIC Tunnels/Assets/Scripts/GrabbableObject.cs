using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour, IInteractable
{
    public AudioClip grabSound;
    public string GetPrompt() => "Grab [E]";

    public void OnInteract()
    {
        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        if (player != null)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            player.GrabObject(rb);

            if (grabSound != null)
            {
                AudioSource audio = player.GetComponent<AudioSource>();
                if (audio != null)
                    audio.PlayOneShot(grabSound, 0.5f);
            }
        }
    }
}