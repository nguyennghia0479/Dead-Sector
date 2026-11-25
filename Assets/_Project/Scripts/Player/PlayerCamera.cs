using UnityEngine;

[System.Serializable]
public class PlayerCamera
{
    [Header("Mouse Settings")]
    [SerializeField] private float maxSensitivity = 50f;
    [SerializeField] private float minSensitivity = 5f;
    [SerializeField] private float defaultSensitivity = 20f;
    [SerializeField] private float mouseSensitivity = 20f;
    [SerializeField] private float angleClamp = 60f;
    [SerializeField] private float sniperAimSenMultiplier = .25f;

    private float originalMouseXSen;
    private float xRotation;

    public float MaxSensitivity => maxSensitivity;
    public float MinSensitivity => minSensitivity;
    public float DefaultSensitivity => defaultSensitivity;

    public void Initialized()
    {
        mouseSensitivity = defaultSensitivity;
        originalMouseXSen = mouseSensitivity;
    }

    public void HandleSniperAiming(bool isSniperAiming)
    {
        if (isSniperAiming)
        {
            mouseSensitivity *= sniperAimSenMultiplier;
            mouseSensitivity = Mathf.Clamp(mouseSensitivity, minSensitivity, maxSensitivity);
        }
        else
            mouseSensitivity = originalMouseXSen;
    }

    public void HandleRotation(Transform playerTransform, Transform mainCamera, GameInput gameInput, Vector3 cameraRecoilRotation)
    {
        Vector2 mouseInput = gameInput.GetMouseInput();
        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -angleClamp, angleClamp);

        mainCamera.localRotation = Quaternion.Euler(xRotation + cameraRecoilRotation.x, cameraRecoilRotation.y, cameraRecoilRotation.z);
        playerTransform.Rotate(Vector3.up * mouseX);
    }

    public void UpdateMouseSensitivity(float value)
    {
        mouseSensitivity = Mathf.Clamp(value, minSensitivity, maxSensitivity);
        originalMouseXSen = mouseSensitivity;
    }

    public void ResetRotation() => xRotation = 0f;
}
