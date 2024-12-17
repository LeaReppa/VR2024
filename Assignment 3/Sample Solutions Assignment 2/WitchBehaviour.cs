using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SampleSolution
{
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
            if (IsServer)
                UpdateTarget();
        }

        private void Update()
        {
            if (IsServer)
                ApplyMovement();
        }

        private void UpdateTarget()
        {
            if (randomizeTargetOrder)
            {
                int newIdx = currentFlightTargetIdx;

                while (newIdx == currentFlightTargetIdx)
                    newIdx = Random.Range(0, flightTargets.Count);

                currentFlightTargetIdx = newIdx;
            }
            else
            {
                currentFlightTargetIdx++;
                currentFlightTargetIdx %= flightTargets.Count;
            }
        }

        private void ApplyMovement()
        {
            // check Distance to current Target
            float distance = Vector3.Distance(transform.position, flightTargets[currentFlightTargetIdx].position);

            if (distance > minTargetDistance)
            {
                Vector3 direction = flightTargets[currentFlightTargetIdx].position - transform.position;

                Vector3 movement = direction.normalized * (movementSpeed * Time.deltaTime);
                transform.position += movement;

                var lookDirection = Quaternion.LookRotation(direction);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, lookDirection, rotationSpeed * Time.deltaTime);
            }
            else
            {
                UpdateTarget();
            }
        }
    }
}
