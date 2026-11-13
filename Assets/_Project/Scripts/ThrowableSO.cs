using UnityEngine;

[CreateAssetMenu(fileName = "ThrowableSO", menuName = "Scriptable Objects/ThrowableSO")]
public class ThrowableSO : ScriptableObject
{
    [Header("Throwable Info")]
    [SerializeField] private string throwableName;
    [SerializeField] private Sprite throwableIcon;
    [SerializeField] private Throwable throwablePrefab;
    [SerializeField] private ThrowableInteraction pickupPrefab;
    public string ThrowableName => throwableName;
    public Sprite ThrowableIcon => throwableIcon;
    public Throwable ThrowablePrefab => throwablePrefab;
    public ThrowableInteraction PickupPrefab => pickupPrefab;

    [Header("Throwable Settings")]
    [SerializeField] protected int explosionDamage = 100;
    [SerializeField] protected float explosionRadius = 10f;
    [SerializeField] protected float explosionForce = 100f;
    [SerializeField] protected float upwardsModifier = 3f;
    [SerializeField] protected float delay = 3f;
    [SerializeField] protected int maxAmount = 4;
    public int ExplosionDamage => explosionDamage;
    public float ExplosionRadius => explosionRadius;
    public float ExplosionForce => explosionForce;
    public float UpwardsModifier => upwardsModifier;
    public float Delay => delay;
    public int MaxAmount => maxAmount;

    [Header("Audio Settings")]
    [SerializeField] protected AudioClip explosionClip;
    [SerializeField] protected AudioClip bounceClip;
    [SerializeField] protected float bounceCooldown = .15f;
    public AudioClip ExplosionClip => explosionClip;
    public AudioClip BounceClip => bounceClip;
    public float BounceCooldown => bounceCooldown;

    [Header("Molotov Settings")]
    [SerializeField] private float burnDamagePercentage = .2f;
    public float BurnDamagePercentage => burnDamagePercentage;
}
