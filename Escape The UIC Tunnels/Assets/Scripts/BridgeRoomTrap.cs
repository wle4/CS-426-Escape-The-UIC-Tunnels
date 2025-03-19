using UnityEngine;
using System.Collections;

public class BridgeRoomTrap : MonoBehaviour
{
    public GameObject fallingBridgePart; // Assign the middle bridge part
    public float fallDelay = 1f;

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

        Rigidbody rb = fallingBridgePart.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Enable gravity so it falls
            rb.useGravity = true;
        }
    }
}
