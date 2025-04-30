using UnityEngine;

public class MouseSpawner : MonoBehaviour
{
    public GameObject mousePrefab;
    public Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(mousePrefab, spawnPoint.position, spawnPoint.rotation);
            Destroy(gameObject); // optional: remove the trigger after use
        }
    }
}
