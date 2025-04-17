using UnityEngine;

public class PlayerMovement : MonoBehaviour
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
    public float interactRange = 3f;
    public float holdDistance = 6f;
    public float grabSmoothness = 10f;
    public float minHoldDistance = 2f;
    public float maxHoldDistance = 5f;
    public float scrollSensitivity = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Rotation Settings")]
    public float rotationSensitivity = 5f;

    [Header("Player Sound Settings")]
    private AudioSource footstepAudio;
    public AudioClip throwObjectSound;
    private float footstepTimer = 0f;

    [Header("Managers")]
    private HUDController hud;

    [Header("Footstep Settings")]
    public float walkStepInterval = 0.3f;
    public float sprintStepInterval = 0.2f;

    private Transform holdPoint;
    private IInteractable currentInteractable = null;
    private Rigidbody grabbedObject = null;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float lastJumpPress = -1f;

    private float targetPitch = 0.0f;
    private float targetYaw = 0.0f;

    private CapsuleCollider playerCollider;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCollider = GetComponent<CapsuleCollider>();
        footstepAudio = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        targetYaw = transform.eulerAngles.y;

        holdPoint = new GameObject("HoldPoint").transform;
        holdPoint.SetParent(playerCamera.transform);
        holdPoint.localPosition = new Vector3(0, 0, holdDistance);

        hud = Object.FindFirstObjectByType<HUDController>();
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
            targetYaw = transform.eulerAngles.y;
            targetPitch = playerCamera.transform.localEulerAngles.x;
            if (targetPitch > 180f) targetPitch -= 360f;
        }

        HandleMovement();
        HandleInteraction();
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        targetYaw += mouseX;
        targetPitch -= mouseY;
        targetPitch = Mathf.Clamp(targetPitch, -89.0f, 89.0f);

        transform.rotation = Quaternion.Euler(0.0f, targetYaw, 0.0f);
        playerCamera.transform.localRotation = Quaternion.Euler(targetPitch, 0.0f, 0.0f);
    }

    private void HandleMovement()
    {
        groundedPlayer = controller.isGrounded;

        if (Input.GetButtonDown("Jump"))
        {
            lastJumpPress = Time.time;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        moveDirection = transform.TransformDirection(moveDirection);

        if (moveDirection.magnitude > 1.0f)
        {
            moveDirection.Normalize();
        }

        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        CurrentSpeed = moveDirection.magnitude * currentSpeed;

        bool jumpBufferActive = (Time.time - lastJumpPress) <= jumpBufferTime;

        if (groundedPlayer)
        {
            playerVelocity.y = -groundedGravity;
            if (jumpBufferActive)
            {
                playerVelocity.y = Mathf.Sqrt(jumpHeight * 2.0f * gravity);
                lastJumpPress = -1f;
            }
        }
        else
        {
            playerVelocity.y -= gravity * Time.deltaTime;
        }

        controller.Move(playerVelocity * Time.deltaTime);

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        bool isMoving = input.magnitude > 0.1f;

        HandleFootsteps(isMoving);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        transform.rotation = Quaternion.Euler(0.0f, targetYaw, 0.0f);
    }

    private void HandleFootsteps(bool isMoving)
    {
        //|| !controller.isGrounded
        if (!isMoving  || footstepAudio == null)
        {
            footstepTimer = 0f;
            return;
        }

        footstepTimer += Time.deltaTime;

        float interval = isSprinting ? sprintStepInterval : walkStepInterval;

        if (footstepTimer >= interval)
        {
            footstepAudio.pitch = Random.Range(0.95f, 1.05f);
            footstepAudio.PlayOneShot(footstepAudio.clip, 1.0f);

            footstepTimer = 0f; // Reset the step timer
        }
    }

    private void HandleInteraction()
    {
        if (grabbedObject != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                holdDistance += scroll * scrollSensitivity;
                holdDistance = Mathf.Clamp(holdDistance, minHoldDistance, maxHoldDistance);
                holdPoint.localPosition = new Vector3(0, 0, holdDistance);
            }

            Vector3 desiredPosition = holdPoint.position;
            Vector3 currentPosition = grabbedObject.position;
            Vector3 moveDirection = (desiredPosition - currentPosition);
            grabbedObject.linearVelocity = moveDirection * grabSmoothness;

            if (Input.GetKeyDown(interactKey))
            {
                ReleaseObject();
            }
        }
        else
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    hud?.ShowInteraction(interactable.GetPrompt());
                    if (Input.GetKeyDown(interactKey))
                    {
                        interactable.OnInteract();
                        return;
                    }
                }
                else
                {
                    hud?.HideInteraction();
                }
            }
            else
            {
                hud?.HideInteraction();
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

            if (playerCollider != null)
                Physics.IgnoreCollision(grabbedObject.GetComponent<Collider>(), playerCollider, false);

            grabbedObject = null;
            hud?.HideInteraction();

            AudioSource audio = this.GetComponent<AudioSource>();
            audio.pitch = Random.Range(0.95f, 1.05f);
            audio.PlayOneShot(throwObjectSound, 0.5f);
        }
    }

    public void GrabObject(Rigidbody rb)
    {
        if (rb == null || rb.isKinematic) return;

        string tag = rb.gameObject.tag;

        if (tag == "GrabbableObject")
        {
            // Regular grabbable item (physics-based)
            grabbedObject = rb;
            grabbedObject.useGravity = false;
            grabbedObject.freezeRotation = true;
            grabbedObject.linearDamping = 10f;

            if (playerCollider != null)
                Physics.IgnoreCollision(grabbedObject.GetComponent<Collider>(), playerCollider, true);

            hud?.ShowInteraction("Drop [E]");
        }
        else if (tag == "InventoryItem")
        {
            // Inventory item — no physics hold, just add to inventory
            AddToInventory(rb.gameObject);
            hud?.ShowInteraction("Picked up!");
            Destroy(rb.gameObject); // optionally remove the item from scene
        }
        else
        {
            Debug.LogWarning($"Unhandled grabbable tag: {tag}");
        }
    }

    private void AddToInventory(GameObject item)
    {
        Debug.Log($"Added {item.name} to inventory.");
        // TODO: push to inventory system
    }
}
