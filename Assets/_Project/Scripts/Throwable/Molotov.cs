using UnityEngine;

public class Molotov : Throwable
{
    [Header("Molotov Settings")]
    [SerializeField] private float burnDamagePercentage;
    private int burnDamage;

    protected override void Awake()
    {
        base.Awake();
        burnDamagePercentage = throwableSO.BurnDamagePercentage;
        burnDamage = Mathf.CeilToInt(explosionDamage * burnDamagePercentage);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;

        hasCollided = true;
        AudioSource.PlayClipAtPoint(bounceClip, transform.position);
        Explode();
    }

    protected override void Explode()
    {
        int hitAmount = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, hitColliders, hitLayer);
        for (int i = 0; i < hitAmount; i++)
        {
            if (hitColliders[i].TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeMolotovHit(explosionDamage, burnDamage);
        }

        explosionEffect.gameObject.SetActive(true);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
