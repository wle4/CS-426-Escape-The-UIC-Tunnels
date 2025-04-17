using UnityEngine;
using System.Collections;

public class BridgeRoomTrap : MonoBehaviour
{
    public GameObject fallingBridgePart;
    public float fallDelay = 1f;

    [Header("Audio")]
    public AudioSource breakSound;

    [Header("Camera Shake")]
    public Camera mainCamera; // Assign your main camera here
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.2f;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(FallBridge());
        }
    }

    IEnumerator FallBridge()
    {
        yield return new WaitForSeconds(fallDelay);

        if (breakSound != null)
            breakSound.Play();

        Rigidbody rb = fallingBridgePart.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if (mainCamera != null)
        {
            CameraShake shaker = mainCamera.GetComponent<CameraShake>();
            if (shaker != null)
            {
                StartCoroutine(shaker.Shake(shakeDuration, shakeMagnitude));
            }
        }
    }
}
