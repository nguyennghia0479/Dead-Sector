using UnityEngine;

public class Grenade : Throwable
{
    protected override void Explode()
    {
        base.Explode();
        GrenadeExplode();
        Destroy(gameObject);
    }

    private void GrenadeExplode()
    {
        AudioSource.PlayClipAtPoint(explosionClip, transform.position);
        int hitAmount = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, hitColliders, hitLayer);
        for (int i = 0; i < hitAmount; i++)
        {
            if (hitColliders[i].TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeGrenadeHit(explosionDamage, explosionForce, transform.position, explosionRadius, upwardsModifier);
                return;
            }

            if (hitColliders[i].TryGetComponent<Rigidbody>(out var rb))
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier);
        }
    }
}
