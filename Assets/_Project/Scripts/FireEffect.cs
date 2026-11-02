using UnityEngine;

public class FireEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem burnEffect;

    private void OnTriggerEnter(Collider other)
    {
        int burnAmount = GetRandomBurnAmount(other);
        for (int j = 0; j < burnAmount; j++)
        {
            Vector3 burnPosition = GetRandomPosition(other);
            Quaternion burnRotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
            Instantiate(burnEffect, burnPosition, burnRotation, other.transform);
        }
    }

    public int GetRandomBurnAmount(Collider hit)
    {
        float volume = hit.bounds.size.x * hit.bounds.size.z;
        if (volume < 2f)
            return Random.Range(1, 3);
        else if (volume < 10f)
            return Random.Range(1, 4);
        else if (volume < 30f)
            return Random.Range(3, 6);

        return Random.Range(5, 9);
    }

    public Vector3 GetRandomPosition(Collider hit)
    {
        float randomX = Random.Range(hit.bounds.min.x, hit.bounds.max.x);
        float randomY = Random.Range(hit.bounds.min.y, hit.bounds.max.y);
        float randomZ = Random.Range(hit.bounds.min.z, hit.bounds.max.z);

        return new Vector3(randomX, randomY, randomZ);
    }
}
