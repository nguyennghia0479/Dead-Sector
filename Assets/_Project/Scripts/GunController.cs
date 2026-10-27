using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float weaponRange = 100f;
    [SerializeField] private float fireRate;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private MuzzleFlashEffect muzzleFlash;
    private float nextFireTimer;

    [Header("Ammo Settings")]
    [SerializeField] private int totalAmmos;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int currentAmmos;
    [SerializeField] private bool isReloading;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource gunSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;

    [Header("Gun Attributes")]
    [SerializeField] private GunAnimations gunAnimations;
    [SerializeField] private CameraRecoil cameraRecoil;
    [SerializeField] private GunSway gunSway;
    [SerializeField] private GunRecoil gunRecoil;
    [SerializeField] private BulletTracer bulletTracer;
    [SerializeField] private SurfaceHitEffect surfaceHitEffect;
    [SerializeField] private GunAiming gunAiming;

    private PlayerController playerController;
    private GameInput gameInput;
    private Camera mainCamera;
    private bool gunActivated;
    private bool isShooting;
    private bool isAiming;

    private void Awake()
    {
        currentAmmos = maxAmmo;
    }

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        gameInput = playerController.GameInput;
        mainCamera = Camera.main;

        gunAnimations.Initialized();
        gunSway.Initialized(gameInput, transform.localPosition);
        gunRecoil.Initialized(transform.localPosition, transform.localRotation);
        surfaceHitEffect.Initialized();
        gunAiming.Initialized(transform.parent.localPosition, transform.parent.localEulerAngles, mainCamera.fieldOfView);
    }

    private void Update()
    {
        HandleShoot();
        HandleUpdateRecoil();
        HandleGunSway();
        HandleGunRecoil();
        HandleGunAiming();
    }

    private void HandleShoot()
    {
        nextFireTimer += Time.deltaTime;
        if (!gunActivated || !isShooting || isReloading || nextFireTimer < fireRate)
            return;

        if (currentAmmos > 0)
        {
            nextFireTimer = 0f;
            Shoot();
        }
        else
            HandleReload();
    }

    private void Shoot()
    {
        currentAmmos--;
        muzzleFlash.Play();
        cameraRecoil.ApplyRecoil(isAiming);
        gunRecoil.ApplyRecoil(isAiming);
        PlayGunShootSFX();

        Ray ray = new(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, weaponRange, ~ignoreLayer))
        {
            Debug.Log(hit.collider.gameObject.name);
            bulletTracer.ApplyTracer(muzzleFlash.transform.position, hit.point);
            surfaceHitEffect.ApplyHitEffect(hit);
        }
        else
        {
            bulletTracer.ApplyTracer(muzzleFlash.transform.position, ray.origin + ray.direction * weaponRange);
        }
    }

    public void HandleReload()
    {
        if (!gunActivated || totalAmmos <= 0 || isReloading) return;

        isReloading = true;
        gunAnimations.PlayReloadAnimation();
        PlayGunReloadSFX();
    }

    public void ReloadFinished()
    {
        int ammosNeeded = maxAmmo - currentAmmos;
        int ammosToReload = Mathf.Min(ammosNeeded, totalAmmos);

        currentAmmos += ammosToReload;
        totalAmmos -= ammosToReload;
        isReloading = false;
    }

    private void HandleUpdateRecoil()
    {
        playerController.UpdateCameraRecoilRotation(cameraRecoil.GetCurrentRecoil());
    }

    private void HandleGunSway()
    {
        gunSway.HandleSway(transform, playerController, isShooting, isAiming);
    }

    private void HandleGunRecoil()
    {
        gunRecoil.UpdateRecoil(transform);
    }

    private void HandleGunAiming()
    {
        gunAiming.ApplyAiming(transform.parent, mainCamera, isAiming);
    }

    private void PlayGunShootSFX()
    {
        if (gunSource)
            gunSource.PlayOneShot(shootClip);
    }

    private void PlayGunReloadSFX()
    {
        if (gunSource)
            gunSource.PlayOneShot(reloadClip);
    }

    public void GunActivated(bool activeStatus) => gunActivated = activeStatus;
    public void GunShooting(bool shootStatus) => isShooting = shootStatus;
    public void GunAiming(bool aimStatus) => isAiming = aimStatus;
}
