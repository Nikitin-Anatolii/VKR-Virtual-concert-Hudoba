using UnityEngine;

public class FPS : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform cameraTransform;
    
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    
    [Header("Mouse Look Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookUpAngle = 80f;
    [SerializeField] private float maxLookDownAngle = -80f;
    
    [Header("Ground Check")]
    [SerializeField] private LayerMask groundMask = 1;
    [SerializeField] private float groundCheckDistance = 0.4f;
    
    // Private variables
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;
    private float currentSpeed;
    
    // Input variables
    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;
    private bool runInput;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Get components if not assigned
        if (characterController == null)
            characterController = GetComponent<CharacterController>();
            
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
            
        if (cameraTransform == null && playerCamera != null)
            cameraTransform = playerCamera.transform;
            
        // Set initial speed
        currentSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        HandleMovement();
        HandleMouseLook();
        HandleJump();
        ApplyGravity();
    }
    
    void GetInput()
    {
        // Movement input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        // Jump input
        jumpInput = Input.GetButtonDown("Jump");
        
        // Run input
        runInput = Input.GetKey(KeyCode.LeftShift);
        
        // Toggle cursor lock with ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursor();
        }
        
        // Update speed based on run input
        currentSpeed = runInput ? runSpeed : walkSpeed;
    }
    
    void HandleMovement()
    {
        // Calculate movement direction
        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;
        
        // Apply movement
        characterController.Move(move * currentSpeed * Time.deltaTime);
    }
    
    void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // Rotate camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, maxLookDownAngle, maxLookUpAngle);
        
        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        
        // Rotate player left/right
        transform.Rotate(Vector3.up * mouseX);
    }
    
    void HandleJump()
    {
        // Check if grounded
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        
        // Jump
        if (jumpInput && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    
    void ApplyGravity()
    {
        // Apply gravity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep grounded
        }
        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    
    // Public methods for external control
    public void SetMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
    }
    
    public void SetWalkSpeed(float speed)
    {
        walkSpeed = speed;
        if (!runInput)
            currentSpeed = walkSpeed;
    }
    
    public void SetRunSpeed(float speed)
    {
        runSpeed = speed;
        if (runInput)
            currentSpeed = runSpeed;
    }
    
    public void ToggleCursor()
    {
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? 
            CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = !Cursor.visible;
    }
    
    // Gizmos for ground check visualization
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, groundCheckDistance);
    }
}
