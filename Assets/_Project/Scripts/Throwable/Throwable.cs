using UnityEngine;

public enum ThrowableType
{
    Grenade, Molotov
}

public class Throwable : MonoBehaviour
{
    [Header("Throwable Info")]
    [SerializeField] protected ThrowableSO throwableSO;
    [SerializeField] protected ThrowableType throwableType;
    [SerializeField] protected LayerMask hitLayer;
    [SerializeField] protected ParticleSystem explosionEffect;

    [Header("Throwable Settings")]
    [SerializeField] protected int explosionDamage;
    [SerializeField] protected float explosionRadius;
    [SerializeField] protected float explosionForce;
    [SerializeField] protected float delay;
    [SerializeField] protected float upwardsModifier;
    [SerializeField] protected int maxAmount;

    [Header("Audio Settings")]
    [SerializeField] protected float bounceCooldown;
    protected AudioSource throwableSource;
    protected AudioClip explosionClip;
    protected AudioClip bounceClip;

    protected Collider[] hitColliders = new Collider[50];
    protected float delayTimer;
    protected bool hasBeenThown;
    protected bool hasExploded;
    protected bool hasCollided;

    public ThrowableType ThrowableType => throwableType;
    public int MaxAmount => maxAmount;

    protected virtual void Awake()
    {
        Initialized();
        throwableSource = GetComponent<AudioSource>();
        explosionEffect.gameObject.SetActive(false);
        delayTimer = delay;
    }

    protected void Update()
    {
        if (!hasBeenThown) return;

        delayTimer -= Time.deltaTime;
        if (delayTimer <= 0 && !hasExploded)
        {
            hasExploded = true;
            Explode();
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;

        hasCollided = true;
        float impactForce = collision.relativeVelocity.magnitude;
        if (impactForce > 1.5f)
            throwableSource.PlayOneShot(bounceClip, Mathf.Clamp01(impactForce / 10f));
    }

    private void Initialized()
    {
        explosionDamage = throwableSO.ExplosionDamage;
        explosionRadius = throwableSO.ExplosionRadius;
        explosionForce = throwableSO.ExplosionForce;
        delay = throwableSO.Delay;
        upwardsModifier = throwableSO.UpwardsModifier;
        maxAmount = throwableSO.MaxAmount;

        explosionClip = throwableSO.ExplosionClip;
        bounceClip = throwableSO.BounceClip;
        bounceCooldown = throwableSO.BounceCooldown;
    }

    public void HandleBeThorwn() => hasBeenThown = true;

    protected virtual void Explode()
    {
        explosionEffect.transform.SetParent(null);
        explosionEffect.gameObject.SetActive(true);
        explosionEffect.Play();
    }
}
