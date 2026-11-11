using UnityEngine;

public class EntityController : MonoBehaviour, IDamageable
{
    [Header("Explode Affect Settings")]
    [SerializeField] protected float recoverDuration = 1f;
    protected WaitForSeconds recoverWaitTime;
    protected bool isHitByGrenade;
    private bool isBurning;
    private float burnDuration;
    private float burnTimer;
    private int burnDamage;
    private float burnDamageInterval;
    private float burnIntervalTimer;
    private IHealth health;

    public void Initialized(IHealth health)
    {
        this.health = health;
        recoverWaitTime = new WaitForSeconds(recoverDuration);
    }

    public virtual void TakeGrenadeHit(int explosionDamage, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upwardsModifier)
    {
        if (health.IsDeath) return;

        int damage = CalculateGrenadeDamage(explosionDamage, explosionPosition, explosionRadius);
        if (damage > 0)
            health.TakeDamage(damage);
    }

    public void TakeMolotovHit(int explosionDamage, int burnDamage)
    {
        if (health.IsDeath) return;
        health.TakeDamage(explosionDamage);
        this.burnDamage = burnDamage;
    }

    public void ApplyBurnHit(float burnDuration, float burnDamageInterval)
    {
        this.burnDuration = burnDuration;
        this.burnDamageInterval = burnDamageInterval;
        burnTimer = 0;
        burnIntervalTimer = 0;
        isBurning = true;
        int burnAmount = GetComponentsInChildren<BurnEffect>().Length;
        burnDamage *= burnAmount;
    }

    public void HandleBurnHit()
    {
        if (!isBurning || health.IsDeath) return;

        burnTimer += Time.deltaTime;
        burnIntervalTimer += Time.deltaTime;

        if (burnTimer > burnDuration)
            isBurning = false;

        if (burnIntervalTimer > burnDamageInterval)
        {
            burnIntervalTimer = 0f;
            health.TakeDamage(burnDamage);
        }
    }

    private int CalculateGrenadeDamage(int explosionDamage, Vector3 explosionPosition, float explosionRadius)
    {
        float fullDamageRadius = explosionRadius / 2f;
        float distance = Vector3.Distance(explosionPosition, transform.position);
        if (distance <= fullDamageRadius)
            return explosionDamage;

        if (distance > explosionRadius)
            return 0;

        float normalizedDistance = Mathf.InverseLerp(fullDamageRadius, explosionRadius, distance);
        float damgeByRadius = Mathf.Lerp(explosionDamage, 0f, normalizedDistance);
        return Mathf.CeilToInt(damgeByRadius);
    }
}
