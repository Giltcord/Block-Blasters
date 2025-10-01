using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Time;

[RequireComponent(typeof(TrajectoryPredictor))]
public class ProjectileThrow : MonoBehaviour
{
    TrajectoryPredictor trajectoryPredictor;
    [SerializeField] Rigidbody objectToThrow;
    [SerializeField, Range(0.0f, 500.0f)] float force;
    [SerializeField] Transform StartPosition;
    public InputAction fire;
    bool isThrown = false;
    private float throwTime;
    private bool isEnabled = false;
    void Awake()
    {
        trajectoryPredictor = GetComponent<TrajectoryPredictor>();
        
        if (StartPosition == null)
            StartPosition = transform;
    }
    void OnEnable()
    {
        EnableInput();
    }
    void OnDisable()
    {
        DisableInput();
    }
    void OnDestroy()
    {
        DisableInput();
    }
    void Update()
    {
        Predict();
        if (isThrown)
        {
            float timeSinceThrow = time - throwTime;
            UpdateVisualsBasedOnTime(timeSinceThrow);
        }
    }
    void Predict()
    {
        if (trajectoryPredictor != null)
            trajectoryPredictor.PredictTrajectory(ProjectileData());
    }
    ProjectileProperties ProjectileData()
    {
        ProjectileProperties properties = new ProjectileProperties();
    
        if (objectToThrow != null)
        {
            Rigidbody r = objectToThrow.GetComponent<Rigidbody>();
            properties.direction = StartPosition.forward;
            properties.initialPosition = StartPosition.position;
            properties.initialSpeed = force;
            properties.mass = r.mass;
            properties.drag = r.linearDamping;
            
        }
    
        return properties;
    }
    private void ThrowObject(InputAction.CallbackContext ctx)
    {
        if (this == null || !isEnabled) return;
        
        isThrown = false;
        throwTime = time;
        
        if (objectToThrow != null && StartPosition != null)
        {
            Rigidbody thrownObject = Instantiate(objectToThrow, StartPosition.position, Quaternion.identity);
            thrownObject.AddForce(StartPosition.forward * force, ForceMode.Impulse);
            isThrown = true;
        }
    }
    private void UpdateVisualsBasedOnTime(float timeSinceThrow)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            float alpha = Mathf.Clamp01(1 - (timeSinceThrow / 5f));
            Color objectColor = renderer.material.color;
            objectColor.a = alpha;
            renderer.material.color = objectColor;
        }
    }
    public void EnableInput()
    {
        if (isEnabled) return;
        
        fire.Enable();
        fire.performed += ThrowObject;
        isEnabled = true;
    }

    public void DisableInput()
    {
        if (!isEnabled) return;
        
        fire.performed -= ThrowObject;
        fire.Disable();
        isEnabled = false;
    }
    public void SetThrowingEnabled(bool enabled)
    {
        if (enabled)
            EnableInput();
        else
            DisableInput();
    }
}