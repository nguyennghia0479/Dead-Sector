using UnityEngine;

public enum ThrowableType
{
    Grenade, Molotov
}

public class Throwable : MonoBehaviour
{
    [Header("Throwable Settings")]
    [SerializeField] protected ThrowableType throwableType;
    [SerializeField] protected LayerMask hitLayer;
    [SerializeField] protected ParticleSystem explosionEffect;
    [SerializeField] protected float explosionRadius = 10f;
    [SerializeField] protected float delay = 3f;
    [SerializeField] protected float explosiveForce = 100f;
    [SerializeField] protected float upwardsModifier = 3f;
    [SerializeField] protected int maxAmount = 4;

    [Header("Audio Settings")]
    [SerializeField] protected AudioClip explosionClip;
    [SerializeField] protected AudioClip bounceClip;
    [SerializeField] protected float bounceCooldown = .15f;

    protected AudioSource throwableSource;
    protected Collider[] hitColliders = new Collider[50];
    protected float delayTimer;
    protected bool hasBeenThown;
    protected bool hasExploded;
    protected bool hasCollided;

    public ThrowableType ThrowableType => throwableType;
    public int MaxAmount => maxAmount;

    protected void Awake()
    {
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

    public void HandleBeThorwn() => hasBeenThown = true;

    protected virtual void Explode()
    {
        explosionEffect.transform.SetParent(null);
        explosionEffect.gameObject.SetActive(true);
        explosionEffect.Play();
    }
}
