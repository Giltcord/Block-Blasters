using UnityEngine;

public class Mouse : MonoBehaviour

{
    [Header("Rotation Settings")] public float horizontalSpeed = 2.0f;
    public float verticalSpeed = 2.0f;
    public bool invertVertical = false;
    [Header("Rotation Limits")] public bool clampVerticalRotation = true;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;
    [Header("Smoothing")] public float rotationSmoothing = 5.0f;
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    void Start()
    {
        currentRotation = transform.eulerAngles;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        HandleMouseRotation();
        
        transform.rotation = Quaternion.Slerp(transform.rotation, 
            Quaternion.Euler(targetRotation), 
            rotationSmoothing * Time.deltaTime);
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
        void HandleMouseRotation()
        {
            float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed * (invertVertical ? -1 : 1);

            currentRotation.y += mouseX;
            currentRotation.x -= mouseY;

            if (clampVerticalRotation)
            {
                currentRotation.x = Mathf.Clamp(currentRotation.x, minVerticalAngle, maxVerticalAngle);
            }

            transform.rotation = Quaternion.Euler(currentRotation);
        }
    }
