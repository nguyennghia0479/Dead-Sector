using UnityEngine;

public interface IDamageable
{
    public void TakeGrenadeHit(int explosionDamage, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upwardsModifier);
    public void TakeMolotovHit(int explosionDamage, int burnDamage);
    public void ApplyBurnHit(float burnDuration, float burnDamageInterval);
    public void HandleBurnHit();
}
