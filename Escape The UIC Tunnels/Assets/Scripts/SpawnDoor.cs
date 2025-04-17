using UnityEngine;

public class DoorStartSound : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play(); // or PlayOneShot if you prefer
        }
    }
}
