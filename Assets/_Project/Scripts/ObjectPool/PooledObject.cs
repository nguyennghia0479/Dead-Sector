using UnityEngine;
using UnityEngine.Pool;

public class PooledObject : MonoBehaviour
{
    private IObjectPool<GameObject> pool;
    private Transform originalParent;
    private Vector3 originalScale;


    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
        originalParent = transform.parent;
        originalScale = transform.localScale;
    }

    public void ReleasePool(GameObject pooledObject)
    {
        if (pool == null) return;

        transform.SetParent(originalParent, false);
        transform.localScale = originalScale;
        pool.Release(pooledObject);
    }
}
