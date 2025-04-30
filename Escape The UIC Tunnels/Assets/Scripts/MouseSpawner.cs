using UnityEngine;

public class MouseSpawner : MonoBehaviour
{
    public GameObject mousePrefab;
    public Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Force the capsule to be upright and face forward (Z+)
            Quaternion uprightRotation = Quaternion.Euler(-90f, spawnPoint.eulerAngles.y, 0f);

            GameObject mouse = Instantiate(mousePrefab, spawnPoint.position, uprightRotation);

            Destroy(gameObject); // remove the trigger
        }
    }
}
