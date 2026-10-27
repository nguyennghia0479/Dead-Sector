using UnityEngine;

[System.Serializable]
public class GunRecoil
{
    [Header("Recoil Settings")]
    [SerializeField] private Vector2 recoilKickBack = new(-.05f, -.07f);
    [SerializeField] private Vector3 recoilRot = new(-5f, 2f, 0f);
    
    [Space]
    [SerializeField] private float returnSpeed = 10f;
    [SerializeField] private float recoverSpeed = 8f;
    [SerializeField] private float aimRecoilMultiplier = .5f;

    private Vector3 originalPos;
    private Quaternion originalRot;

    private Vector3 currentRecoilPos;
    private Vector3 currentRecoilRot;

    public void Initialized(Vector3 originalPos, Quaternion originalRot)
    {
        this.originalPos = originalPos;
        this.originalRot = originalRot;
    }

    public void ApplyRecoil(bool isAiming)
    {
        float recoilMultiplier = isAiming ? aimRecoilMultiplier : 1f;

        Vector3 targetRecoilPos = new Vector3(0, 0, Random.Range(recoilKickBack.x, recoilKickBack.y)) * recoilMultiplier;
        currentRecoilPos += targetRecoilPos;

        Vector3 targetRecoilRot = new Vector3(recoilRot.x, Random.Range(-recoilRot.y, recoilRot.y), recoilRot.z) * recoilMultiplier;
        currentRecoilRot += targetRecoilRot;
    }

    public void UpdateRecoil(Transform weapon)
    {
        currentRecoilPos = Vector3.Lerp(currentRecoilPos, Vector3.zero, recoverSpeed * Time.deltaTime);
        weapon.localPosition = Vector3.Lerp(weapon.localPosition, originalPos + currentRecoilPos, returnSpeed * Time.deltaTime);

        currentRecoilRot = Vector3.Lerp(currentRecoilRot, Vector3.zero, recoverSpeed * Time.deltaTime);
        weapon.localRotation = Quaternion.Slerp(weapon.localRotation, originalRot * Quaternion.Euler(currentRecoilRot), returnSpeed * Time.deltaTime);
    }
}
