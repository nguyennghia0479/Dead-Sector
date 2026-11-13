using UnityEngine;

[CreateAssetMenu(fileName = "GunSO", menuName = "Scriptable Objects/GunSO")]
public class GunSO : ScriptableObject
{
    [Header("Gun Info")]
    [SerializeField] private string gunName;
    [SerializeField] private Sprite gunIcon;
    [SerializeField] private GunController gunPrefab;
    [SerializeField] private GunInteraction pickupPrefab;
    public string GunName => gunName;
    public Sprite GunIcon => gunIcon;
    public GunController GunPrefab => gunPrefab;
    public GunInteraction PickupPrefab => pickupPrefab;

    [Header("Gun Settings")]
    [SerializeField] private int gunDamage = 10;
    [SerializeField] private float gunRange = 100f;
    [SerializeField] private float fireRate = .2f;
    [SerializeField] private float bulletForce = 10f;
    [SerializeField] private int pelletPerShot = 1;
    [SerializeField] private float pelletSpreadMultiplier = 1f;
    [SerializeField] private bool hasScope;
    [SerializeField] private bool isSniper;
    public int GunDamage => gunDamage;
    public float GunRange => gunRange;
    public float FireRate => fireRate;
    public float BulletForce => bulletForce;
    public int PelletPerShot => pelletPerShot;
    public float PelletSpreadMultiplier => pelletSpreadMultiplier;
    public bool HasScope => hasScope;
    public bool IsSniper => isSniper;

    [Header("Ammo Settings")]
    [SerializeField] private int reserveAmmo;
    [SerializeField] private int magazineSize;
    [SerializeField] private bool isIncremental;
    public int ReserveAmmo => reserveAmmo;
    public int MagazineSize => magazineSize;
    public bool IsIncremental => isIncremental;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip drawClip;
    [SerializeField] private AudioClip hideClip;
    [SerializeField] private AudioClip noAmmoClip;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private AudioClip reloadLoopClip;
    [SerializeField] private AudioClip reloadnEndClip;
    public AudioClip DrawClip => drawClip;
    public AudioClip HideClip => hideClip;
    public AudioClip NoAmmoClip => noAmmoClip;
    public AudioClip ShootClip => shootClip;
    public AudioClip ReloadClip => reloadClip;
    public AudioClip ReloadLoopClip => reloadLoopClip;
    public AudioClip ReloadEndClip => reloadnEndClip;
}
