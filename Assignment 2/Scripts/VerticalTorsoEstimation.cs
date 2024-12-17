using UnityEngine;
using System;

public class VerticalTorsoEstimation : MonoBehaviour
{
    [Tooltip("The head transform which is used to align the body to. If nothing is specified, transform of parent GameObject will be used.")]
    public Transform headTransform;

    private float backwardDisplacementFactor = 0.005f;

    private void Awake()
    {
        if (headTransform == null)
            headTransform = transform.parent;
    }

    void Update()
    {
        ApplyTorsoUpdate(headTransform, transform);
            

    }

    private void ApplyTorsoUpdate(Transform headTransform, Transform torsoTransform)
    {
        //transform.position = new Vector3(STATIC_X, transform.position.y, transform.position.x);

        torsoTransform.position = new Vector3(0,0,0);

        //Rotating the torso
        Vector3 headsetRotation = new Vector3(0, headTransform.eulerAngles.y, 0);
        torsoTransform.rotation = Quaternion.Euler(headsetRotation);

        //Displacing the torso
        float verticalTilt = Mathf.DeltaAngle(0,headTransform.eulerAngles.x); //Shortest difference between two angles
        
        verticalTilt = Mathf.Clamp(verticalTilt,-90,90); //Restricts value to be between 90 and -90
        float displacement = Mathf.Max(0, verticalTilt) * backwardDisplacementFactor;
        Vector3 targetPosition = transform.parent.transform.position - torsoTransform.forward * displacement;

        torsoTransform.position = targetPosition;

        transform.position = torsoTransform.position;
        transform.rotation = torsoTransform.rotation;
    }
}