using Unity.Netcode;
using UnityEngine;

namespace SampleSolution
{
    public class BirdBehaviour : NetworkBehaviour
    {
        public float rotationSpeed = 40f;

        [Header("Rotation Definition")]
        public bool xRotation = false;
        public bool yRotation = false;
        public bool zRotation = false;
        public bool useConvenience = false;

        private void Update()
        {
            if (IsServer)
                ApplyRotation();
        }

        private void ApplyRotation()
        {
            if (useConvenience)
            {
                if(xRotation)
                    transform.RotateAround(transform.root.position, transform.root.right, rotationSpeed * Time.deltaTime);
                if(yRotation)
                    transform.RotateAround(transform.root.position, transform.root.up, rotationSpeed * Time.deltaTime);
                if(zRotation)
                    transform.RotateAround(transform.root.position, transform.root.forward, rotationSpeed * Time.deltaTime);
            
                // keep bird horizontal
                transform.localRotation = Quaternion.Euler(0,0,0);
            }
            else
            {
                Vector3 direction = transform.localPosition;
                float angle = rotationSpeed * Time.deltaTime;
        
                // rotate by angle
                direction = Quaternion.Euler(xRotation ? angle : 0, yRotation ? angle : 0, zRotation ? angle : 0) * direction;
                transform.localPosition = direction;
            }
        }
    }
}

