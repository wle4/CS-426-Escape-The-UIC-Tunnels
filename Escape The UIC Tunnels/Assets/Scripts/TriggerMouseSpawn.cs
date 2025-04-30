using UnityEngine;

public class TriggerMouseSpawn : MonoBehaviour
{
    public GameObject mousePrefab;
    public Transform[] spawnPoints;

    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasSpawned && other.CompareTag("Player"))
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                Instantiate(mousePrefab, spawnPoint.position, spawnPoint.rotation);
            }

            hasSpawned = true; // prevents repeated spawning
        }
    }
}
