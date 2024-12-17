using UnityEngine;

namespace SampleSolution
{
    public class BirdPlacement : MonoBehaviour
    {
        public bool applyLocalTransformation;
        public bool applyRelativeTransformation;
        public bool applyMatrixTransformation;

        // Start is called before the first frame update
        void Start()
        {
            if (applyMatrixTransformation)
            {
                ApplyMatrixTransformation();
                return;
            }

            if (applyLocalTransformation)
                ApplyLocalTransformation();
            if (applyRelativeTransformation)
                ApplyRelativeTransformation();
        }

        void ApplyLocalTransformation()
        {
            transform.localPosition = new Vector3(-32f, -13.45f, -8.6f);
            transform.localRotation = Quaternion.Euler(0, 130, 0);
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        void ApplyRelativeTransformation()
        {
            transform.Translate(Vector3.back * 2.5f, Space.World);
            transform.Rotate(0, -90, 0);
            transform.localScale = transform.localScale * 1.5f;
        }

        void ApplyMatrixTransformation()
        {
            Matrix4x4 trsMatrix = Matrix4x4.TRS(
                new Vector3(-32f, -13.45f, -8.6f),
                Quaternion.Euler(0, 130, 0),
                new Vector3(1.5f, 1.5f, 1.5f)
            );

            // Invert the local transform matrix to convert from world to local space
            Matrix4x4 inverseTransform = trsMatrix.inverse;
            Vector3 localDirection = inverseTransform.MultiplyVector(Vector3.back);
            Matrix4x4 relativeTranslation = Matrix4x4.Translate(localDirection * 2.5f);

            // Rotation
            Matrix4x4 relativeRotation = Matrix4x4.Rotate(Quaternion.Euler(0, -90, 0));

            // Scale
            Matrix4x4 relativeScaling = Matrix4x4.Scale(new Vector3(1.5f, 1.5f, 1.5f));

            // Combine transformations
            Matrix4x4 combinedMatrix = trsMatrix * relativeTranslation * relativeRotation * relativeScaling;

            // Apply the transformation to the GameObject
            SetTransformByMatrix(combinedMatrix);

        }

        // Tim & Paulines method for applying matrix transformation
        void SetTransformByMatrix(Matrix4x4 mat)
        {
            transform.localPosition = mat.GetColumn(3);
            transform.localRotation = mat.rotation;
            transform.localScale = mat.lossyScale;
        }
    }
}
