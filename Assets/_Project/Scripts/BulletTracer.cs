using UnityEngine;

[System.Serializable]
public class BulletTracer
{
    [Header("Bullet Settings")]
    [SerializeField] private BulletMovement bulletPrefab;
    [SerializeField] private float moveSpeed = 300f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private Transform bulletHolder;

    public void ApplyTracer(Vector3 spawnPosition, Vector3 targetPosition)
    {
        BulletMovement newBullet = Object.Instantiate(bulletPrefab, spawnPosition, Quaternion.identity, bulletHolder);
        newBullet.SetupBullet(targetPosition, moveSpeed, lifeTime);
    }
}
