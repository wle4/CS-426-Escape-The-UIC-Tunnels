using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement movement;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        movement = GetComponent<PlayerMovement>();
        if (movement == null)
            movement = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        float speed = movement != null ? movement.CurrentSpeed : 0f;
        //Debug.Log("Speed: " + speed);
        animator.SetFloat("Speed", speed);
    }
}
