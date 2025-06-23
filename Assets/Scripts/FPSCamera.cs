using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 1.6f, 0);
    
    [Header("Head Bob Settings")]
    [SerializeField] private bool enableHeadBob = true;
    [SerializeField] private float bobFrequency = 2f;
    [SerializeField] private float bobHorizontalAmplitude = 0.1f;
    [SerializeField] private float bobVerticalAmplitude = 0.1f;
    
    [Header("Tilt Settings")]
    [SerializeField] private bool enableTilt = true;
    [SerializeField] private float tiltAngle = 5f;
    [SerializeField] private float tiltSpeed = 5f;
    
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CharacterController characterController;
    
    // Private variables
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private float bobTimer = 0f;
    private float currentTilt = 0f;
    private bool isMoving = false;
    
    void Start()
    {
        // Get references if not assigned
        if (playerTransform == null)
            playerTransform = transform.parent;
            
        if (characterController == null)
            characterController = GetComponentInParent<CharacterController>();
            
        // Store original position
        originalPosition = transform.localPosition;
        targetPosition = originalPosition;
    }
    
    void Update()
    {
        UpdateCameraPosition();
        HandleHeadBob();
        HandleTilt();
    }
    
    void UpdateCameraPosition()
    {
        if (playerTransform != null)
        {
            // Calculate target position with offset
            Vector3 desiredPosition = playerTransform.position + cameraOffset;
            
            // Smoothly move camera to target position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
    
    void HandleHeadBob()
    {
        if (!enableHeadBob) return;
        
        // Check if player is moving
        isMoving = characterController != null && characterController.velocity.magnitude > 0.1f && characterController.isGrounded;
        
        if (isMoving)
        {
            // Calculate head bob
            bobTimer += Time.deltaTime * bobFrequency;
            
            float horizontalBob = Mathf.Sin(bobTimer) * bobHorizontalAmplitude;
            float verticalBob = Mathf.Sin(bobTimer * 2f) * bobVerticalAmplitude;
            
            targetPosition = originalPosition + new Vector3(horizontalBob, verticalBob, 0);
        }
        else
        {
            // Return to original position when not moving
            targetPosition = originalPosition;
            bobTimer = 0f;
        }
        
        // Apply head bob
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * 10f);
    }
    
    void HandleTilt()
    {
        if (!enableTilt) return;
        
        // Calculate tilt based on horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float targetTilt = -horizontalInput * tiltAngle;
        
        // Smoothly apply tilt
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
        
        // Apply tilt to camera
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, currentTilt);
    }
    
    // Public methods for external control
    public void SetHeadBob(bool enabled)
    {
        enableHeadBob = enabled;
    }
    
    public void SetTilt(bool enabled)
    {
        enableTilt = enabled;
    }
    
    public void SetBobIntensity(float intensity)
    {
        bobHorizontalAmplitude = intensity;
        bobVerticalAmplitude = intensity;
    }
    
    public void SetTiltAngle(float angle)
    {
        tiltAngle = angle;
    }
    
    public void ShakeCamera(float intensity, float duration)
    {
        StartCoroutine(CameraShake(intensity, duration));
    }
    
    private System.Collections.IEnumerator CameraShake(float intensity, float duration)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;
            
            transform.localPosition = originalPos + new Vector3(x, y, 0);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.localPosition = originalPos;
    }
} 