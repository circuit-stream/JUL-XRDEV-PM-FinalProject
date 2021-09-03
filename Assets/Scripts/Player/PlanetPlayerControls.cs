using UnityEngine;

public class PlanetPlayerControls : MonoBehaviour
{
    public float sensitivity = 1f;
    public float turningSpeed = 1f;
    public float movementSpeed = 1f;

    private bool usingVR;
    private Camera camera;

    void Start()
    {
        // Get the player camera
        camera = GetComponentInChildren<Camera>();

        // Detect whether VR is present or not
        usingVR = Util.IsVRPresent();
    }

    void Update()
    {
        var horizontal = 0f;
        var vertical = 0f;
        var forward = 0f;
        var strafe = 0f;

        if (usingVR)
        {
            // Get VR turning controls
            horizontal = Input.GetAxis("XRI_Right_Primary2DAxis_Horizontal") * sensitivity;
            vertical = -Input.GetAxis("XRI_Right_Primary2DAxis_Vertical") * sensitivity;

            // Get VR movement controls
            forward = -Input.GetAxis("XRI_Left_Primary2DAxis_Vertical") * sensitivity;
            strafe = Input.GetAxis("XRI_Left_Primary2DAxis_Horizontal") * sensitivity;
        }
        else
        {
            // Get mouse turning controls
            horizontal = Input.GetAxis("Mouse X") * sensitivity;
            vertical = -Input.GetAxis("Mouse Y") * sensitivity;

            // Get keyboard movement controls
            if (Input.GetKey(KeyCode.W))
                forward = 1f;
            if (Input.GetKey(KeyCode.S))
                forward = -1f;
            if (Input.GetKey(KeyCode.D))
                strafe = 1;
            if (Input.GetKey(KeyCode.A))
                strafe = -1;
        }

        // Handle turning
        transform.Rotate(0, horizontal * turningSpeed * Time.deltaTime, 0);
        camera.transform.Rotate(vertical * turningSpeed * Time.deltaTime, 0, 0);

        // TODO: These vectors should be projected onto the normal of the ground beneath the player using a raycast

        // Handle forward movement
        transform.position += Vector3.ProjectOnPlane(camera.transform.forward, transform.up)
            * forward
            * movementSpeed
            * Time.deltaTime;

        // Handle strafe movement
        transform.position += Vector3.ProjectOnPlane(camera.transform.right, transform.up)
            * strafe
            * movementSpeed
            * Time.deltaTime;
    }
}
