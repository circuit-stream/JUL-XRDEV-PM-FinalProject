using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void Update()
    {
        // Rotate to face the camera
        transform.LookAt(Camera.main.transform.position, Camera.main.transform.up);
    }
}
