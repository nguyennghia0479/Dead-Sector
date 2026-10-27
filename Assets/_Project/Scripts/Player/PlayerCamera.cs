using UnityEngine;

[System.Serializable]
public class PlayerCamera
{
    [Header("Mouse Settings")]
    [SerializeField] private float mouseXSensitivity = 20f;
    [SerializeField] private float mouseYSensitivity = 20f;
    [SerializeField] private float angleClamp = 60f;

    private float xRotation;

    public void Initialized()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void HandleRotation(Transform playerTransform, Transform playerCamera, GameInput gameInput, Vector3 cameraRecoilRotation)
    {
        Vector2 mouseInput = gameInput.GetMouseInput();
        float mouseX = mouseInput.x * mouseXSensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * mouseYSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -angleClamp, angleClamp);

        playerCamera.localRotation = Quaternion.Euler(xRotation + cameraRecoilRotation.x, cameraRecoilRotation.y, cameraRecoilRotation.z);
        playerTransform.Rotate(Vector3.up * mouseX);
    }
}
