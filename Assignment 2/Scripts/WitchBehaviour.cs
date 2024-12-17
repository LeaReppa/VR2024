using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WitchBehaviour : NetworkBehaviour
{
    [HideInInspector] public List<Transform> flightTargets;
    private int currentFlightTargetIdx = -1;

    public bool randomizeTargetOrder = false;
    public float minTargetDistance = 1.5f;
    public float movementSpeed = 3f;
    public float rotationSpeed = 180f;

    private void Start()
    {
        if (IsServer) // Perform calculations only on the server
        {
            //Set the first target
            UpdateTarget();
        }

        Debug.Log("Flight targets: " + flightTargets.Count);
    }

    private void Update()
    {
        if (!IsServer) return; // Perform calculations only on the server

        //No flight targets available:
        if (flightTargets == null || flightTargets.Count == 0) return;

        ApplyMovement();
    }

    private void UpdateTarget()
    {
        if (randomizeTargetOrder)
        {
            currentFlightTargetIdx = Random.Range(0, flightTargets.Count);
        }
        else
        {
            //Set the currentFlightTargetIdx to the next bird or reset to the first bird
            currentFlightTargetIdx = (currentFlightTargetIdx + 1) % flightTargets.Count;
        }
    }

    private void ApplyMovement()
    {
        //Invalid flight target index
        if (currentFlightTargetIdx < 0 || currentFlightTargetIdx >= flightTargets.Count) return;

        Transform target = flightTargets[currentFlightTargetIdx];
        // Direction vector to the target
        Vector3 direction = target.position - transform.position; 

        // Check if witch has reached current target
        if (direction.magnitude < minTargetDistance)
        {
            UpdateTarget(); //set the next target
            return;
        }

        // Normalize direction vector to calculate movement
        Vector3 moveDirection = direction.normalized;

        // Move witch towards target
        transform.position += moveDirection * movementSpeed * Time.deltaTime;

        // Smoothly rotate witch to face target
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
