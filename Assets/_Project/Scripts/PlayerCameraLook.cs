using UnityEngine;

[System.Serializable]
public class PlayerCameraLook
{
    [Header("Mouse Settings")]
    [SerializeField] private float mouseXSensitivity = 20f;
    [SerializeField] private float mouseYSensitivity = 20f;
    [SerializeField] private float angleClamp = 60f;
    [SerializeField] private Transform playerCam;

    private float xRotation;

    public void Initialized()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void HandleLook(Transform playerTransform, GameInput gameInput)
    {
        Vector2 lookInput = gameInput.GetLookInput();
        float mouseX = lookInput.x * mouseXSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseYSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -angleClamp, angleClamp);

        playerCam.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerTransform.Rotate(Vector3.up * mouseX);
    }
}
