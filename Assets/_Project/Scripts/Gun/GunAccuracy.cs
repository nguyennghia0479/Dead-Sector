using UnityEngine;

[System.Serializable]
public class GunAccuracy
{
    [Header("Spread Settings")]
    [SerializeField] private float idleSpread = .35f;
    [SerializeField] private float walkSpread = 1.5f;
    [SerializeField] private float sprintSpread = 3f;
    [SerializeField] private float aimSpread = .1f;

    [Space]
    [SerializeField] private float spreadChangeSpeed = 12f;
    [SerializeField] private float spreadReturnSpeed = 2.5f;

    [Header("Bloom Settings")]
    [SerializeField] private float shotPerSpread = .35f;
    [SerializeField] private float maxSpread = 5f;
    [SerializeField] private float aimBloomMultiplier = .5f;

    private float currentSpread;
    private float shootingBloom;

    public float CurrentSpread => currentSpread;
    public float MinSpread => aimSpread;
    public float MaxSpread => maxSpread;

    public void Initialized()
    {
        currentSpread = idleSpread;
        shootingBloom = 0;
    }

    public void UpdateSpread(bool isMoving, bool isSprinting, bool isCrouching, bool isAiming)
    {
        float baseSpread;
        if (isAiming)
            baseSpread = aimSpread;
        else if (isCrouching && isMoving)
            baseSpread = walkSpread / 2f;
        else if (isCrouching)
            baseSpread = idleSpread / 2f;
        else if (isMoving && isSprinting)
            baseSpread = sprintSpread;
        else if (isMoving)
            baseSpread = walkSpread;
        else
            baseSpread = idleSpread;

        shootingBloom = Mathf.Lerp(shootingBloom, 0, spreadReturnSpeed * Time.deltaTime);
        float targetSpread = Mathf.Clamp(shootingBloom + baseSpread, aimSpread, maxSpread);
        currentSpread = Mathf.Lerp(currentSpread, targetSpread, spreadChangeSpeed * Time.deltaTime);
    }

    public void ApplyBloom(bool isAiming)
    {
        float bloomMultiplier = isAiming ? aimBloomMultiplier : 1f;
        float addedSpread = shotPerSpread * bloomMultiplier;
        shootingBloom = Mathf.Clamp(shootingBloom + addedSpread, 0, maxSpread);
        currentSpread = Mathf.Clamp(currentSpread + addedSpread, aimSpread, maxSpread);
    }

    public Vector3 GetDirection(Transform mainCamera, float spreadMultiplier)
    {
        Vector2 randomSpread = currentSpread * spreadMultiplier * Random.insideUnitCircle;
        Quaternion horizontalRot = Quaternion.AngleAxis(randomSpread.x, mainCamera.up);
        Quaternion verticalRot = Quaternion.AngleAxis(-randomSpread.y, mainCamera.right);

        return (horizontalRot * verticalRot * mainCamera.forward).normalized;
    }
}
