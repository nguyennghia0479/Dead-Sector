using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float weaponRange = 100f;
    [SerializeField] private float fireRate = .2f;
    [SerializeField] private float bulletForce = 10f;
    [SerializeField] private int pelletPerShot = 1;
    [SerializeField] private float pelletSpreadMultiplier = 1f;
    [SerializeField] private bool hasScope;
    [SerializeField] private bool isSniper;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private MuzzleFlashEffect muzzleFlash;
    [SerializeField] private GunInteraction gunDrop;
    [SerializeField] private Sprite gunIcon;
    private float nextFireTimer;

    [Header("Ammo Settings")]
    [SerializeField] private int totalAmmos;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int currentAmmos;
    [SerializeField] private bool isIncremental;
    [SerializeField] private bool isReloading;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource gunSource;
    [SerializeField] private AudioClip drawClip;
    [SerializeField] private AudioClip hideClip;
    [SerializeField] private AudioClip noAmmoClip;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private AudioClip reloadLoopClip;
    [SerializeField] private AudioClip reloadnEndClip;

    [Header("Gun Attributes")]
    [SerializeField] private GunAnimations gunAnimations;
    [SerializeField] private CameraRecoil cameraRecoil;
    [SerializeField] private GunSway gunSway;
    [SerializeField] private GunRecoil gunRecoil;
    [SerializeField] private BulletTracer bulletTracer;
    [SerializeField] private SurfaceHitEffect surfaceHitEffect;
    [SerializeField] private GunAiming gunAiming;
    [SerializeField] private GunAccuracy gunAccuracy;

    private PlayerController player;
    private GameInput gameInput;
    private Camera mainCamera;
    private bool gunActivated;
    private bool isShooting;
    private bool isAiming;

    public bool GunActivated => gunActivated;
    public GunInteraction GunDropped => gunDrop;
    public GunAnimations GunAnimations => gunAnimations;
    public GunAiming GunAiming => gunAiming;
    public PlayerController Player => player;
    public bool IsSniper => isSniper;
    public Sprite GunIcon => gunIcon;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        gameInput = player.GameInput;
        mainCamera = Camera.main;
        currentAmmos = maxAmmo;
    }

    private void OnEnable()
    {
        UpdateAmmosUI();
    }

    private void Start()
    {
        gunAnimations.Initialized();
        gunSway.Initialized(gameInput, transform.localPosition);
        gunRecoil.Initialized(transform.localPosition, transform.localRotation);
        surfaceHitEffect.Initialized();
        gunAiming.Initialized(player.ADS.DefaultPosition, player.ADS.DefaultRotation, player.DefaultMainCamFOV);
        gunAccuracy.Initialized();
        if (hasScope)
            gunAiming.InitializedScope(mainCamera, player.ScopeCamera.Camera);
    }

    private void Update()
    {
        if (!gunActivated) return;

        HandleShoot();
        HandleUpdateCameraRecoil();
        HandleGunSway();
        HandleGunRecoil();
        HandleGunAiming();
        HandleGunAccuracy();
        HandleCrosshairReactive();
    }

    private void LateUpdate()
    {
        if (!gunActivated || !hasScope) return;

        HandleScopeAiming();
    }

    private void HandleShoot()
    {
        nextFireTimer += Time.deltaTime;
        if (!isShooting || nextFireTimer < fireRate)
            return;

        if (currentAmmos > 0)
        {
            nextFireTimer = 0f;
            Shoot();
        }
        else
        {
            HandleReload();
            if (totalAmmos <= 0)
            {
                nextFireTimer = 0f;
                PlayNoAmmoSFX();
            }
        }
    }

    private void Shoot()
    {
        if (isReloading)
        {
            if (isIncremental)
                gunAnimations.PlayReloadFinishedAnimation();
            else
                return;
        }

        PlayGunShootSFX();
        currentAmmos--;
        muzzleFlash.Play();
        cameraRecoil.ApplyRecoil(isAiming);
        gunRecoil.ApplyRecoil(isAiming);
        gunAccuracy.ApplyBloom(isAiming);
        UpdateAmmosUI();

        for (int i = 0; i < pelletPerShot; i++)
            ShootPellet();
    }

    private void ShootPellet()
    {
        Vector3 shotDirection = gunAccuracy.GetDirection(mainCamera.transform, pelletSpreadMultiplier);
        Ray ray = new(mainCamera.transform.position, shotDirection);
        if (Physics.Raycast(ray, out RaycastHit hit, weaponRange, ~ignoreLayer))
        {
            Debug.Log(hit.collider.gameObject.name);
            bulletTracer.ApplyTracer(muzzleFlash.transform.position, hit.point);
            surfaceHitEffect.ApplyHitEffect(hit);

            if (hit.collider.TryGetComponent<Rigidbody>(out var rb))
                rb.AddForce(-hit.normal * bulletForce);
        }
        else
        {
            bulletTracer.ApplyTracer(muzzleFlash.transform.position, ray.origin + ray.direction * weaponRange);
        }
    }

    public void HandleReload()
    {
        if (isReloading || totalAmmos <= 0 || currentAmmos == maxAmmo) return;

        player.CancelAimingIfNeeded();
        isReloading = true;
        gunAnimations.PlayReloadAnimation(isIncremental);
        PlayGunReloadSFX(reloadClip);
    }

    public void ReloadFinished()
    {
        if (!isIncremental)
        {
            int ammosNeeded = maxAmmo - currentAmmos;
            int ammosToReload = Mathf.Min(ammosNeeded, totalAmmos);

            currentAmmos += ammosToReload;
            totalAmmos -= ammosToReload;
            UpdateAmmosUI();
        }
        isReloading = false;
    }

    public void ReloadLoop()
    {
        currentAmmos++;
        totalAmmos--;
        int ammosToReload = maxAmmo - currentAmmos;
        if (ammosToReload > 0)
        {
            gunAnimations.PlayReloadLoopAnimation();
        }
        else
        {
            PlayGunReloadSFX(reloadnEndClip);
            gunAnimations.PlayReloadFinishedAnimation();
        }
        UpdateAmmosUI();
    }

    public void PlayReloadLoopSound()
    {
        PlayGunReloadSFX(reloadLoopClip);
    }

    private void HandleUpdateCameraRecoil()
    {
        player.UpdateCameraRecoilRotation(cameraRecoil.GetCurrentRecoil());
    }

    private void HandleGunSway()
    {
        gunSway.HandleSway(transform, player, isShooting, isAiming);
    }

    private void HandleGunRecoil()
    {
        gunRecoil.UpdateRecoil(transform);
    }

    private void HandleGunAiming()
    {
        UIManager.Instance.SetCrosshairVisible(isAiming, hasScope, isSniper);
        gunAiming.ApplyAiming(transform.parent, mainCamera, isAiming);
        if (isSniper)
            UIManager.Instance.SetSniperVisible(isAiming);
    }

    private void HandleScopeAiming()
    {
        gunAiming.HandleScopeAiming();
    }

    private void HandleGunAccuracy()
    {
        gunAccuracy.UpdateSpread(player.IsMoving, player.IsSprinting, player.IsCrouching, isAiming);
    }

    private void HandleCrosshairReactive()
    {
        if (!isAiming)
            UIManager.Instance.UpdateCrosshair(gunAccuracy.CurrentSpread, gunAccuracy.MinSpread, gunAccuracy.MaxSpread);
    }

    private void UpdateAmmosUI()
    {
        GameEvents.RaiseAmmosChanged(currentAmmos, totalAmmos);
    }

    public void PlayUnequipAnimation(bool isSwitchGun = true)
    {
        PlayHideSFX();
        gunAnimations.PlayUnequipAnimation(isSwitchGun);
        if (isAiming)
            isAiming = false;
    }

    public void PlayDrawSFX()
    {
        if (gunSource && drawClip)
            gunSource.PlayOneShot(drawClip);
    }

    public void PlayHideSFX()
    {
        if (gunSource && hideClip)
            gunSource.PlayOneShot(hideClip);
    }

    private void PlayGunShootSFX()
    {
        if (gunSource && shootClip)
            gunSource.PlayOneShot(shootClip);
    }

    private void PlayNoAmmoSFX()
    {
        if (gunSource && noAmmoClip)
            gunSource.PlayOneShot(noAmmoClip);
    }

    private void PlayGunReloadSFX(AudioClip clip)
    {
        if (gunSource && clip)
            gunSource.PlayOneShot(clip);
    }

    public void SetGunActivated(bool activeStatus) => gunActivated = activeStatus;
    public void SetGunShooting(bool shootStatus) => isShooting = shootStatus;
    public void SetGunAiming(bool aimStatus) => isAiming = aimStatus;
}
