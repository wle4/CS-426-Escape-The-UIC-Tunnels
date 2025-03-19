using UnityEngine;

public class FixedFirstPersonController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera playerCamera;
    public float mouseSensitivity = 2.0f;

    [Header("Movement Settings")]
    public float walkSpeed = 5.0f;
    public float jumpHeight = 2.0f;
    public float gravity = 9.81f;
    public float groundedGravity = 0.05f;
    public float jumpBufferTime = 0.2f;

    // Private variables
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float lastJumpPress = -1f;

    // Prevent rotation jitter
    private float targetPitch = 0.0f;
    private float targetYaw = 0.0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        targetYaw = transform.eulerAngles.y;

        // Make sure camera is a direct child of the player
        if (playerCamera.transform.parent != transform)
        {
            Debug.LogWarning("Camera should be a direct child of the player GameObject!");
        }
    }

    private void Update()
    {
        // Handle rotation first
        HandleRotation();

        // Handle movement second
        HandleMovement();
    }

    private void HandleRotation()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Update target rotation values
        targetYaw += mouseX;
        targetPitch -= mouseY;
        targetPitch = Mathf.Clamp(targetPitch, -89.0f, 89.0f);

        // Apply clean rotations directly
        transform.rotation = Quaternion.Euler(0.0f, targetYaw, 0.0f);
        playerCamera.transform.localRotation = Quaternion.Euler(targetPitch, 0.0f, 0.0f);
    }

    private void HandleMovement()
    {
        // Check for grounded status with a more reliable approach
        groundedPlayer = controller.isGrounded;

        // Update jump buffer timing
        if (Input.GetButtonDown("Jump"))
        {
            lastJumpPress = Time.time;
        }

        // Handle horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        moveDirection = transform.TransformDirection(moveDirection);

        if (moveDirection.magnitude > 1.0f)
        {
            moveDirection.Normalize();
        }

        controller.Move(moveDirection * walkSpeed * Time.deltaTime);

        // Handle jumping with buffer time
        bool jumpBufferActive = (Time.time - lastJumpPress) <= jumpBufferTime;

        if (groundedPlayer)
        {
            // Apply consistent small gravity when grounded
            playerVelocity.y = -groundedGravity;

            // Process jump if within buffer time
            if (jumpBufferActive)
            {
                playerVelocity.y = Mathf.Sqrt(jumpHeight * 2.0f * gravity);
                lastJumpPress = -1f; // Reset jump buffer after successful jump
            }
        }
        else
        {
            // Apply normal gravity when in air
            playerVelocity.y -= gravity * Time.deltaTime;
        }

        // Apply vertical movement
        controller.Move(playerVelocity * Time.deltaTime);
    }

    // Keep rotation consistent even during collisions
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Force the correct rotation after collision
        transform.rotation = Quaternion.Euler(0.0f, targetYaw, 0.0f);
    }
}