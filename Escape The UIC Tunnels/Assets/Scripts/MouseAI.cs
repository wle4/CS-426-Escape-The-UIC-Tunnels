using UnityEngine;
using UnityEngine.AI;

public class MouseAI : MonoBehaviour
{
    public float attackDistance = 1.5f;
    public float attackCooldown = 2f;
    public int damage = 10;

    private Transform player;
    private NavMeshAgent agent;
    private float attackTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        // Move toward the player
        agent.SetDestination(player.position);

        // Rotate to face the player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Keep upright
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // Attack if within range
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackDistance)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                PlayerHealthbar ph = player.GetComponent<PlayerHealthbar>();
                if (ph != null)
                {
                    ph.TakeDamage(damage);
                }
                attackTimer = 0f;
            }
        }
    }
}
