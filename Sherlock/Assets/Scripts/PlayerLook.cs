using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Camera cam;  // Assign in Inspector
    [SerializeField] private Transform cameraPos;  // Assign Camera Position Object

    private float xRotation = 0f;
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    void Update()
    {
        // Make Camera follow CameraPos object
        if (cameraPos != null)
        {
            cam.transform.position = cameraPos.position;
        }
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        // Vertical Look (Up/Down)
        xRotation -= mouseY * ySensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal Look (Left/Right)
        transform.Rotate(Vector3.up * (mouseX * xSensitivity * Time.deltaTime));
    }
}
