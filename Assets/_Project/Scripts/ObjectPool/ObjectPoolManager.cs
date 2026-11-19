using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public struct ObjectPool
{
    public GameObject objectPrefab;
    public Transform holder;
    public int defaultSize;
    public int maxSize;
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [SerializeField] private ObjectPool[] objectPools;
    private Dictionary<GameObject, IObjectPool<GameObject>> poolDict = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        GenerateObjectPools();
    }

    private void GenerateObjectPools()
    {
        foreach (var objectPool in objectPools)
        {
            IObjectPool<GameObject> pool = null;
            pool = new ObjectPool<GameObject>(
                createFunc: () => OnCreatePool(objectPool.objectPrefab, objectPool.holder, pool),
                actionOnGet: OnGetFromPool,
                actionOnRelease: OnReleaseToPool,
                actionOnDestroy: OnDestroyPool,
                collectionCheck: true,
                defaultCapacity: objectPool.defaultSize,
                maxSize: objectPool.maxSize
                );

            PreCreate(pool, objectPool.defaultSize);
            poolDict.Add(objectPool.objectPrefab, pool);
        }
    }

    private void PreCreate(IObjectPool<GameObject> pool, int defaultPoolSize)
    {
        GameObject[] preCreateObjects = new GameObject[defaultPoolSize];
        for (int i = 0; i < preCreateObjects.Length; i++)
            preCreateObjects[i] = pool.Get();

        for (int i = 0; i < preCreateObjects.Length; i++)
            pool.Release(preCreateObjects[i]);
    }

    public GameObject GetPool(GameObject obj, Vector3 position, Quaternion rotation)
    {
        if (obj == null || !poolDict.ContainsKey(obj)) 
            return null;

        GameObject activeObject = poolDict[obj].Get();
        activeObject.transform.SetPositionAndRotation(position, rotation);
        return activeObject;
    }

    private GameObject OnCreatePool(GameObject obj, Transform holder, IObjectPool<GameObject> pool)
    {
        if (holder == null)
            holder = transform;

        GameObject newObject = Instantiate(obj, holder);
        newObject.GetComponent<PooledObject>().SetPool(pool);
        return newObject;
    }

    private void OnGetFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReleaseToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyPool(GameObject obj)
    {
        if (obj != null)
            Destroy(obj);
    }
}
