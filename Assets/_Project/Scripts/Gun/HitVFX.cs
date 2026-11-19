using System.Collections;
using UnityEngine;

public class HitVFX : MonoBehaviour
{
    private PooledObject pooledObject;
  
    private void Awake()
    {
        pooledObject = GetComponent<PooledObject>();
    }

    public void Initialized(WaitForSeconds lifeTime)
    {
        StartCoroutine(SelfDestroyRoutine(lifeTime));
    }

    private IEnumerator SelfDestroyRoutine(WaitForSeconds waitTime)
    {
        yield return waitTime;

        if (pooledObject != null)
            pooledObject.ReleasePool(gameObject);
        else
            Destroy(gameObject);
    }
}
