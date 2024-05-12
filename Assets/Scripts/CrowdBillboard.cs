using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdBillboard : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
    Vector3 newRotation = mainCamera.transform.eulerAngles;

    newRotation.x =  0;
    newRotation.z = -90;

    transform.eulerAngles = newRotation;
    transform.Rotate(-90, 0, 0);
    }
}
