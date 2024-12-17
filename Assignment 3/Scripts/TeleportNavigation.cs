using UnityEngine;
using UnityEngine.InputSystem;
using VRSYS.Core.Avatar;

public class TeleportNavigation : MonoBehaviour
{
    public InputActionReference teleportAction;

    public Transform head;
    public Transform hand;

    public LayerMask groundLayerMask;

    public GameObject previewAvatar;
    public GameObject hitpoint;
    
    public float rayLength = 10.0f;
    private bool rayIsActive = false;

    public LineRenderer lineVisual;
    private float rayActivationThreshhold = 0.01f;
    private float teleportActivationThreshhold = 0.9f;
    
    private bool previewIsActive = false;
    private Vector3 currentHitPoint;
    private Vector3 targetPoint;

    // Start is called before the first frame update
    void Start()
    {
        if (head == null)
            head = GetComponent<AvatarHMDAnatomy>().head;

        if (hand == null)
            hand = GetComponent<AvatarHMDAnatomy>().rightHand;
        
        lineVisual.positionCount = 2; // line between two vertices
        lineVisual.enabled = false;
        hitpoint.SetActive(false);
        previewAvatar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // example of how to control line visual
        float teleportActionValue = teleportAction.action.ReadValue<float>();
        if (teleportActionValue > rayActivationThreshhold)
        {
            ExampleUpdateLineVisual(true, hand.position, hand.position + hand.forward, Color.green);
        }
    }

    private void ExampleUpdateLineVisual(bool rayActive, Vector3 startPosition, Vector3 endPosition, Color color)
    {
        lineVisual.enabled = rayActive;
        lineVisual.SetPosition(0, startPosition);
        lineVisual.SetPosition(1, endPosition);
        lineVisual.startColor = color;
        lineVisual.endColor = color;
    }
    
    
}