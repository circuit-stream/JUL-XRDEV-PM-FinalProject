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

        // These vectors should be projected onto the normal of the ground beneath the player using a raycast
        if (!Physics.Raycast(transform.position + transform.up * 1f, -transform.up, out var hit))
        {
            Debug.Log($"Somehow didn't hit the ground under the player's feet");
            return;
        }

        // Project the forward and strafe directions of the camera onto the surface the player is standing on
        var forwardDirection = Vector3.ProjectOnPlane(camera.transform.forward, hit.normal);
        var strafeDirection = Vector3.ProjectOnPlane(camera.transform.right, hit.normal);

        // Handle forward movement
        transform.position += forwardDirection
            * forward
            * movementSpeed
            * Time.deltaTime;

        // Handle strafe movement
        transform.position += strafeDirection
            * strafe
            * movementSpeed
            * Time.deltaTime;
    }
}
