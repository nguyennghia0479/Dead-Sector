using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Gun Info")]
    [SerializeField] private GunSO gunSO;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private MuzzleFlashEffect muzzleFlash;
    [SerializeField] private float hitDelay = .15f;
    private WaitForSeconds hitWaitTime;

    [Header("Gun Settings")]
    [SerializeField] private int gunDamage;
    [SerializeField] private float gunRange;
    [SerializeField] private float fireRate;
    private float nextFireTimer;
    [SerializeField] private float bulletForce;
    [SerializeField] private int pelletPerShot;
    [SerializeField] private float pelletSpreadMultiplier;
    [SerializeField] private bool hasScope;
    [SerializeField] private bool isSniper;

    [Header("Ammo Settings")]
    [SerializeField] private int currentReserveAmmo;
    [SerializeField] private int magazineSize;
    [SerializeField] private bool isIncremental;
    private int ammoInMag;
    private bool isReloading;

    [Header("Gun Attributes")]
    [SerializeField] private GunAudio gunAudio;
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

    public GunSO Gun => gunSO;
    public int AmmoInMagazine => ammoInMag;
    public int CurrentReserveAmmo => currentReserveAmmo;
    public bool IsReloading => isReloading;
    public bool GunActivated => gunActivated;
    public GunAudio GunAudio => gunAudio;
    public GunAnimations GunAnimations => gunAnimations;
    public GunAiming GunAiming => gunAiming;
    public PlayerController Player => player;
    public bool IsSniper => isSniper;

    private void Awake()
    {
        Initialized();
        player = GetComponentInParent<PlayerController>();
        gameInput = player.GameInput;
        mainCamera = Camera.main;
        hitWaitTime = new WaitForSeconds(hitDelay);
    }

    private void OnEnable()
    {
        UpdateAmmoUI();
    }

    private void Start()
    {
        gunAudio.Initialized(gunSO);
        gunAnimations.Initialized();
        gunSway.Initialized(gameInput, transform.localPosition);
        gunRecoil.Initialized(transform.localPosition, transform.localRotation);
        surfaceHitEffect.Initialized();
        gunAiming.Initialized(player.ADS.DefaultPosition, player.ADS.DefaultRotation);
        gunAccuracy.Initialized();
        if (hasScope)
            gunAiming.InitializedScope(mainCamera, player.ScopeCamera.Camera);
    }

    private void Update()
    {
        if (!gunActivated || player.PlayerHealth.IsDeath) return;

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

    private void Initialized()
    {
        if (gunSO == null) return;

        gunDamage = gunSO.GunDamage;
        gunRange = gunSO.GunRange;
        fireRate = gunSO.FireRate;
        bulletForce = gunSO.BulletForce;
        pelletPerShot = gunSO.PelletPerShot;
        pelletSpreadMultiplier = gunSO.PelletSpreadMultiplier;
        hasScope = gunSO.HasScope;
        isSniper = gunSO.IsSniper;

        currentReserveAmmo = gunSO.ReserveAmmo;
        magazineSize = gunSO.MagazineSize;
        isIncremental = gunSO.IsIncremental;
        ammoInMag = magazineSize;
    }

    private void HandleShoot()
    {
        nextFireTimer += Time.deltaTime;
        if (!isShooting || nextFireTimer < fireRate)
            return;

        if (ammoInMag > 0)
        {
            nextFireTimer = 0f;
            Shoot();
        }
        else
        {
            HandleReload();
            if (currentReserveAmmo <= 0)
            {
                nextFireTimer = 0f;
                gunAudio.PlayNoAmmoSFX();
            }
        }
    }

    private void Shoot()
    {
        if (isReloading)
        {
            if (isIncremental)
            {
                gunAnimations.PlayStopReloadAnimation();
                ReloadFinished();
            }
            else
                return;
        }

        gunAudio.PlayGunShootSFX();
        ammoInMag--;
        muzzleFlash.Play();
        cameraRecoil.ApplyRecoil(isAiming);
        gunRecoil.ApplyRecoil(isAiming);
        gunAccuracy.ApplyBloom(isAiming);
        UpdateAmmoUI();

        for (int i = 0; i < pelletPerShot; i++)
            ShootPellet();
    }

    private void ShootPellet()
    {
        Vector3 shotDirection = gunAccuracy.GetDirection(mainCamera.transform, pelletSpreadMultiplier);
        Ray ray = new(mainCamera.transform.position, shotDirection);
        if (Physics.Raycast(ray, out RaycastHit hit, gunRange, ~ignoreLayer))
        {
            bulletTracer.ApplyTracer(muzzleFlash.transform.position, hit.point);
            StartCoroutine(BulletHitRoutine(hit));
        }
        else
            bulletTracer.ApplyTracer(muzzleFlash.transform.position, ray.origin + ray.direction * gunRange);
    }

    private IEnumerator BulletHitRoutine(RaycastHit hit)
    {
        yield return hitWaitTime;

        if (hit.collider.CompareTag("Enemy"))
        {
            if (hit.collider.TryGetComponent<EnemyController>(out var enemy))
                enemy.TakeHit(-hit.normal, bulletForce, gunDamage);
        }

        if (hit.collider.TryGetComponent<Rigidbody>(out var rb))
            rb.AddForce(-hit.normal * bulletForce, ForceMode.Impulse);

        surfaceHitEffect.ApplyHitEffect(hit);
    }

    public void HandleReload()
    {
        if (isReloading || currentReserveAmmo <= 0 || ammoInMag == magazineSize) return;

        player.CancelAimingIfNeeded();
        isReloading = true;
        gunAnimations.PlayReloadAnimation(isIncremental);
        gunAudio.PlayGunReloadSFX();
    }

    public void ReloadFinished()
    {
        if (!isIncremental)
        {
            int ammosNeeded = magazineSize - ammoInMag;
            int ammosToReload = Mathf.Min(ammosNeeded, currentReserveAmmo);

            ammoInMag += ammosToReload;
            currentReserveAmmo -= ammosToReload;
            UpdateAmmoUI();
        }
        isReloading = false;
    }

    public void ReloadLoop()
    {
        ammoInMag++;
        currentReserveAmmo--;
        int ammosToReload = magazineSize - ammoInMag;
        if (ammosToReload > 0)
        {
            gunAnimations.PlayReloadLoopAnimation();
        }
        else
        {
            gunAudio.PlayGunReloadEndSFX();
            gunAnimations.PlayReloadFinishedAnimation();
        }
        UpdateAmmoUI();
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
        gunAiming.ApplyAiming(transform.parent, isAiming);
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

    private void UpdateAmmoUI()
    {
        GameEvents.RaiseAmmoChanged(ammoInMag, currentReserveAmmo, magazineSize);
    }

    public void PlayUnequipAnimation(bool isSwitchGun = true)
    {
        gunAudio.PlayHideSFX();
        gunAnimations.PlayUnequipAnimation(isSwitchGun);
        if (isAiming)
            isAiming = false;
    }

    public void SetAmmos(int ammoInMag, int currentReserveAmmo)
    {
        this.ammoInMag = ammoInMag;
        this.currentReserveAmmo = currentReserveAmmo;
        UpdateAmmoUI();
    }

    public void SetGunActivated(bool activeStatus) => gunActivated = activeStatus;
    public void SetGunShooting(bool shootStatus) => isShooting = shootStatus;
    public void SetGunAiming(bool aimStatus) => isAiming = aimStatus;
}
