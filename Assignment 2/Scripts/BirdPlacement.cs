using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPlacement : MonoBehaviour
{

    public bool applyLocalTransformation;
    public bool applyRelativeTransformation;
    public bool applyMatrixTransformation;

    GameObject targetBird;
    GameObject targetBird3;

    void ApplyLocalTransformation(){//Implement in Exercise 2.2
        transform.localPosition = targetBird.transform.localPosition;
        transform.localRotation = targetBird.transform.localRotation;
        transform.localScale = targetBird.transform.localScale;

        //Alternatively:

        /*transform.localPosition = new Vector3(-32, -13.45, -8.6);
        transform.localRotation = Quaternion.Euler(0, 130, 0);;
        transform.localScale = new Vector3(1.5,1.5,1.5);*/
    }
    
    void ApplyRelativeTransformation() {//Implement in Exercise 2.3
        transform.Translate(new Vector3(0f,0f,-2.5f), Space.World);
        transform.Rotate(0f,-90f, 0f, Space.World);

        Vector3 scaleFactor = new Vector3(targetBird3.transform.localScale.x/transform.localScale.x, targetBird3.transform.localScale.y/transform.localScale.y, targetBird3.transform.localScale.z/transform.localScale.z);
        transform.localScale = Vector3.Scale(transform.localScale, scaleFactor);
    }

    void ApplyMatrixTransformation(){//Implement in Exercise 2.4
        Matrix4x4 localTransformationMatrix = Matrix4x4.TRS(targetBird.transform.localPosition, targetBird.transform.localRotation, targetBird.transform.localScale);

        //Relative transformation

        Vector3 worldSpaceRelativeTranslation = new Vector3(0,0,-2.5f);

        Debug.Log("worldSpaceRelativeTranslation: " + worldSpaceRelativeTranslation.ToString());

        //Inverse of the local transformation matrix:
        Matrix4x4 inverseLocalTransform = localTransformationMatrix.inverse;

        //Convert the world space translation vector into local space:
        Vector3 localSpaceRelativeTranslation = inverseLocalTransform.MultiplyVector(worldSpaceRelativeTranslation);

        Debug.Log("localSpaceRelativeTranslation: " + localSpaceRelativeTranslation.ToString());

        Matrix4x4 relativeTranslationMatrix = Matrix4x4.Translate(localSpaceRelativeTranslation);

        Matrix4x4 relativeScaleMatrix = Matrix4x4.Scale(new Vector3(targetBird3.transform.localScale.x/targetBird.transform.localScale.x, targetBird3.transform.localScale.y/targetBird.transform.localScale.y, targetBird3.transform.localScale.z/targetBird.transform.localScale.z));

        Quaternion relativeRotation = Quaternion.Euler(0f, -90f, 0f);
        Matrix4x4 relativeRotationMatrix = Matrix4x4.Rotate(relativeRotation);

        //Combine transformation matrix

        Matrix4x4 relativeTransformationMatrix = relativeTranslationMatrix * relativeRotationMatrix * relativeScaleMatrix;

        Matrix4x4 finalMatrix = localTransformationMatrix*relativeTransformationMatrix;

        SetTransformByMatrix(finalMatrix);
    }

     void SetTransformByMatrix(Matrix4x4 mat) {
        transform.localPosition = mat.GetColumn(3);
        transform.localRotation = mat.rotation;
        transform.localScale = mat.lossyScale;
    }


    // Start is called before the first frame update
    void Start()
    {
        //Find TargetBirds
        targetBird = GameObject.Find("BirdTarget - 2.2");
        targetBird3 = GameObject.Find("BirdTarget - 2.3");

        if(applyMatrixTransformation){
            ApplyMatrixTransformation();
        }
        else {
            if(applyLocalTransformation){
            ApplyLocalTransformation();
            }
            if(applyRelativeTransformation){
            ApplyRelativeTransformation();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
