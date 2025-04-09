using UnityEngine;

public class NPCRoam : MonoBehaviour
{
    public Transform[] roamPoints; // Drag your 3 points here in Inspector
    public float speed = 2f;
    public float waitTimeAtPoint = 1f;

    private int currentPoint = 0;
    private bool isPaused = false;
    private bool isWaiting = false;
    private float waitTimer;

    void Update()
    {
        if (isPaused || isWaiting || roamPoints.Length == 0) return;

        Transform target = roamPoints[currentPoint];
        Vector3 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < 0.5f)
        {
            isWaiting = true;
            waitTimer = waitTimeAtPoint;
            currentPoint = (currentPoint + 1) % roamPoints.Length;
        }
    }

    void LateUpdate()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }
}
