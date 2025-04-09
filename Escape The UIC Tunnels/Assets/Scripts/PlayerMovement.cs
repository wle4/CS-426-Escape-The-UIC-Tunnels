using UnityEngine;

public class PlayerMovement: MonoBehaviour
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
    public float CurrentSpeed { get; private set; }

    [Header("Sprint Settings")]
    public float sprintSpeed = 10.0f;
    private bool isSprinting => Input.GetKey(KeyCode.LeftShift);

    [Header("Grab Settings")]
    public float grabRange = 10f;
    public float holdDistance = 6f;
    public float grabSmoothness = 10f;
    public float minHoldDistance = 2f;
    public float maxHoldDistance = 10f;
    public float scrollSensitivity = 3f;
    public KeyCode grabKey = KeyCode.E;

    [Header("Rotation Settings")]
    public float rotationSensitivity = 5f;
    

    private Rigidbody grabbedObject = null;
    private Transform holdPoint;

    // Private variables
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float lastJumpPress = -1f;

    // Prevent rotation jitter
    private float targetPitch = 0.0f;
    private float targetYaw = 0.0f;

    private CapsuleCollider playerCollider;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCollider = GetComponent<CapsuleCollider>(); // ← Make sure this is on the player GameObject

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        targetYaw = transform.eulerAngles.y;

        holdPoint = new GameObject("HoldPoint").transform;
        holdPoint.SetParent(playerCamera.transform);
        holdPoint.localPosition = new Vector3(0, 0, holdDistance);

    }

    private void Update()
    {
        bool rotatingObject = Input.GetMouseButton(1) && grabbedObject != null;

        if (!rotatingObject)
        {
            HandleRotation();
        }
        else
        {
            // Sync camera rotation to prevent jump when releasing RMB
            targetYaw = transform.eulerAngles.y;
            targetPitch = playerCamera.transform.localEulerAngles.x;

            // Handle 360 wraparound
            if (targetPitch > 180f) targetPitch -= 360f;
        }

        // Handle movement second
        HandleMovement();

        // Handle grabbing objects
        HandleGrab();
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

        // If sprinting, increase speed
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        // Set speed for animation
        CurrentSpeed = moveDirection.magnitude * currentSpeed;

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

    private void HandleGrab()
    {
        // Scroll wheel to move grabbed object closer/farther
        if (grabbedObject != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                holdDistance += scroll * scrollSensitivity;
                holdDistance = Mathf.Clamp(holdDistance, minHoldDistance, maxHoldDistance);
                holdPoint.localPosition = new Vector3(0, 0, holdDistance);
            }
        }

        if (Input.GetKeyDown(grabKey))
        {
            if (grabbedObject == null)
            {
                TryGrabObject();
            }
            else
            {
                ReleaseObject();
            }
        }

        // Move the object smoothly if grabbed
        if (grabbedObject != null)
        {
            Vector3 desiredPosition = holdPoint.position;
            Vector3 currentPosition = grabbedObject.position;
            Vector3 moveDirection = (desiredPosition - currentPosition);

            grabbedObject.linearVelocity = moveDirection * grabSmoothness;
        }

        // Rotate object if right mouse is held
        if (Input.GetMouseButton(1)) // Right mouse button
        {

            float rotateX = Input.GetAxis("Mouse X") * rotationSensitivity;
            float rotateY = -Input.GetAxis("Mouse Y") * rotationSensitivity;

            grabbedObject.transform.Rotate(playerCamera.transform.up, rotateX, Space.World);        // horizontal rotation
            grabbedObject.transform.Rotate(playerCamera.transform.right, rotateY, Space.World);     // vertical rotation
        }
    }

    private void TryGrabObject()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;
            if (rb != null && !rb.isKinematic)
            {
                grabbedObject = rb;
                grabbedObject.useGravity = false;
                grabbedObject.freezeRotation = true;
                grabbedObject.linearDamping = 10f;

                // Prevent physics from pushing player
                if (playerCollider != null)
                    Physics.IgnoreCollision(grabbedObject.GetComponent<Collider>(), playerCollider, true);
            }
        }
    }


    private void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.useGravity = true;
            grabbedObject.freezeRotation = false;
            grabbedObject.linearDamping = 0f;

            // Re-enable collision
            if (playerCollider != null)
                Physics.IgnoreCollision(grabbedObject.GetComponent<Collider>(), playerCollider, false);

            grabbedObject = null;
        }
    }


}