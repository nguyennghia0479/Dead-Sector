using UnityEngine;

[System.Serializable]
public class CameraRecoil
{
    [Header("Camera Recoil Settings")]
    [SerializeField] private Vector2 verticalRecoilRange = new(1.2f, 2f);
    [SerializeField] private float horizontalRecoil = .85f;
    [SerializeField] private float rollRecoil = .15f;

    [Space]
    [SerializeField] private float aimRecoilMultiplier = .5f;
    [SerializeField] private float recoilSnappiness = 18f;
    [SerializeField] private float recoilReturnSpeed = 10f;
    [SerializeField] private float maxVerticalRecoil = 8f;

    private Vector3 currentRecoil;
    private Vector3 targetRecoil;

    public void ApplyRecoil(bool isAiming)
    {
        float recoilMultiplier = isAiming ? aimRecoilMultiplier : 1f;
        float recoilX = Random.Range(verticalRecoilRange.x, verticalRecoilRange.y);
        float recoilY = Random.Range(-horizontalRecoil, horizontalRecoil);
        float recoilZ = Random.Range(-rollRecoil, rollRecoil);

        targetRecoil += new Vector3(-recoilX, recoilY, recoilZ) * recoilMultiplier;
        targetRecoil.x = Mathf.Clamp(targetRecoil.x, -maxVerticalRecoil, 0);
    }

    public Vector3 GetCurrentRecoil()
    {
        targetRecoil = Vector3.Lerp(targetRecoil, Vector3.zero, recoilReturnSpeed * Time.deltaTime);
        currentRecoil = Vector3.Lerp(currentRecoil, targetRecoil, recoilSnappiness * Time.deltaTime);
        return currentRecoil;
    }
}
