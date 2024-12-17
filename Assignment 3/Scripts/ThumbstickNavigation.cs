using UnityEngine;
using UnityEngine.InputSystem;
using VRSYS.Core.Avatar;
using VRSYS.Core.Networking;

public class ThumbstickNavigation : MonoBehaviour
{
    public enum InputMapping
    {
        PositionControl,
        VelocityControl,
        AccelerationControl
    }

    [Header("General Configuration")]
    public InputMapping inputMapping = InputMapping.PositionControl;
    public Transform head;
    public InputActionReference steeringAction;
    public InputActionReference rotationAction;
    
    [Header("Navigation Configuration")]
    private Vector3 startingPosition;

    public float movementSpeed = 3f;

    private float accelerationFactor = 1f;
    public float accelerationFactorControl = 1f;

    private float accelerationStart;
    public float positionOffset = 10f; // max position offset during position control 
    [Range(0.1f, 30.0f)] public float steeringSpeed = 10f; // max steering speed for rate control
    [Range(0.1f, 100.0f)] public float maxAcceleration = 2f; // max acceleration during acceleration control
    [Range(0.1f, 100.0f)] public float maxVelocity = 5; // max velocity reached during acceleration control
    private Vector3 currentVelocity = Vector3.zero;
    
    [Header("Rotation Configuration")]
    [Range(1.0f, 180.0f)] // Draws a slider with range in the inspector 
    public float rotationSpeed = 3f; // In angle degrees per second
    public bool snapRotation = true;
    public float snapAngles = 30f; // In angle degrees per snap
    private bool hasMoved = false;
    private bool accelerationStarted = false;

    [Header("Groundfollowing Configuration")]
    public LayerMask groundLayerMask;
    private RaycastHit hit;

    private void Start()
    {
        if (head == null)
            head = GetComponent<AvatarHMDAnatomy>().head;
        
        // Reference point for computing position control
        startingPosition = transform.position;
    }

    void Update()
    {
        ApplyDisplacement();
        ApplyRotation();
        ApplyGroundfollowing();
    }

    private void ApplyDisplacement()
    {
        Vector2 input = steeringAction.action.ReadValue<Vector2>();
        switch (inputMapping)
        {
            case InputMapping.PositionControl:
                PositionControl(input);
                break;
            case InputMapping.VelocityControl:
                VelocityControl(input);
                break;
            case InputMapping.AccelerationControl:
                AccelerationControl(input);
                break;
        }
    }

    private void PositionControl(Vector2 input)
    {
        Vector2 positionInput = steeringAction.action.ReadValue<Vector2>();
        
        if(positionInput.sqrMagnitude > 0.01f){
            positionInput = positionInput.normalized * movementSpeed;

            Vector3 displacement = new Vector3(positionInput.x, 0f, positionInput.y) * Time.deltaTime;

            if((transform.position + displacement - startingPosition).sqrMagnitude < positionOffset){
                transform.Translate(displacement, Space.Self);
            }
        }
    }
    
    private void VelocityControl(Vector2 input)
    {
        Vector2 velocityInput = steeringAction.action.ReadValue<Vector2>();

        if(velocityInput.sqrMagnitude > 0.01f){
            velocityInput = velocityInput * movementSpeed;
            if(velocityInput.sqrMagnitude > steeringSpeed){
                velocityInput = velocityInput.normalized * steeringSpeed;
            }

            Vector3 displacement = new Vector3(velocityInput.x, 0f, velocityInput.y) * Time.deltaTime;
            transform.Translate(displacement, Space.Self);
        }
    }
    
    private void AccelerationControl(Vector2 input)
    {
        Vector2 accelerationInput = steeringAction.action.ReadValue<Vector2>();

        if(accelerationInput.sqrMagnitude > 0.01f){

            if (!accelerationStarted){
                accelerationStart = Time.time;
                accelerationStarted = true;
            }

            accelerationFactor = Time.time - accelerationStart;

            Debug.Log("Acceleration Factor: " + accelerationFactor);
            Debug.Log("Acceleration Start: " + accelerationStart);

            accelerationFactor = accelerationFactor * accelerationFactorControl;

            if(accelerationFactor > maxAcceleration){
                accelerationFactor = maxAcceleration;
            }

            accelerationInput = accelerationInput * movementSpeed;
            if(accelerationInput.sqrMagnitude > steeringSpeed){
                accelerationInput = accelerationInput.normalized * steeringSpeed;
            }

            accelerationInput = accelerationInput * accelerationFactor;
            if(accelerationInput.sqrMagnitude > maxVelocity){
                accelerationInput = accelerationInput.normalized * maxVelocity;
            }

            Vector3 displacement = new Vector3(accelerationInput.x, 0f, accelerationInput.y) * Time.deltaTime;
            transform.Translate(displacement, Space.Self);
        }
        else {
            accelerationFactor = 0f;
            accelerationStarted = false;
        }
    }
    
    private void ApplyRotation()
    {
        Vector2 rotationInput = rotationAction.action.ReadValue<Vector2>();

        float rotationAmount = 0;
        //Check for input
        if(rotationInput.sqrMagnitude > 0.01f){
            //Calculate target rotation
            if(snapRotation){
                if(!hasMoved){
                    Debug.Log("snaping.....");
                    rotationAmount = (Mathf.Sign(rotationInput.x) * snapAngles);
                    hasMoved = true;
                }
            }
            else{
                Debug.Log("rotating smoothly...........");
                rotationAmount = (rotationInput.x * rotationSpeed * Time.deltaTime);
            }
            transform.Rotate(0f, rotationAmount, 0f, Space.World);
        }
        else if(hasMoved){
            Debug.Log("hasMoved ist wieder auf false.....");
            hasMoved = false;
        }
    }
    
    private void ApplyGroundfollowing()
    {
        
    }
}