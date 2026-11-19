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
        GameObject bulletPool = ObjectPoolManager.Instance.GetPool(bulletPrefab.gameObject, spawnPosition, Quaternion.identity);
        if (bulletPool != null && bulletPool.TryGetComponent<BulletMovement>(out var newBullet))
            newBullet.Initialized(targetPosition, moveSpeed, lifeTime);
    }
}
