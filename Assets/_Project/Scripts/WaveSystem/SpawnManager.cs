using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float offset = 1f;
    [SerializeField] private List<Transform> spawnPositions;

    private void Awake()
    {
        Transform[] child = transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < child.Length; i++)
            spawnPositions.Add(child[i]);
    }

    public Vector3 GetRandomSpawnPosition()
    {
        Transform spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];
        float xOffset = Random.Range(-offset, offset);
        float zOffset = Random.Range(-offset, offset);

        return spawnPosition.position + new Vector3(xOffset, 1, zOffset);
    }
}
