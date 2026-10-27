using UnityEngine;

[System.Serializable]
public class GunAiming
{
    [Header("Aim Settings")]
    [SerializeField] private Vector3 aimPosition;
    [SerializeField] private Vector3 aimRotation;
    [SerializeField] private float aimMainCamFOV = 45;
    [SerializeField] private float aimingSpeed = 10f;

    private Vector3 hipPosition;
    private Vector3 hipRotation;
    private float originalMainCamFOV;

    public void Initialized(Vector3 hipPosition, Vector3 hipRotation, float originalMainCamFOV)
    {
        this.hipPosition = hipPosition;
        this.hipRotation = hipRotation;
        this.originalMainCamFOV = originalMainCamFOV;
    }

    public void ApplyAiming(Transform weapon, Camera mainCamera, bool isAiming)
    {
        Vector3 targetPositon = isAiming ? aimPosition : hipPosition;
        Vector3 targetRotation = isAiming ? aimRotation : hipRotation;
        float targetMainCamFOV = isAiming ? aimMainCamFOV : originalMainCamFOV;

        weapon.localPosition = Vector3.Lerp(weapon.localPosition, targetPositon, aimingSpeed * Time.deltaTime);
        weapon.localRotation = Quaternion.Slerp(weapon.localRotation, Quaternion.Euler(targetRotation), aimingSpeed * Time.deltaTime);
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetMainCamFOV, aimingSpeed * Time.deltaTime);
    }
}
