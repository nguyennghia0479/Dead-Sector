using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float weaponRange = 100f;
    [SerializeField] private float fireRate = .2f;
    [SerializeField] private float spreadMultiplier = 1f;
    [SerializeField] private bool hasRedDot;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private MuzzleFlashEffect muzzleFlash;
    [SerializeField] private GunInteraction gunDrop;
    private float nextFireTimer;

    [Header("Ammo Settings")]
    [SerializeField] private int totalAmmos;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int currentAmmos;
    [SerializeField] private bool isIncremental;
    [SerializeField] private bool isReloading;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource gunSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private AudioClip reloadLoop;
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

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        gameInput = player.GameInput;
        mainCamera = Camera.main;
        currentAmmos = maxAmmo;
        
        gunAnimations.Initialized();
        gunSway.Initialized(gameInput, transform.localPosition);
        gunRecoil.Initialized(transform.localPosition, transform.localRotation);
        surfaceHitEffect.Initialized();
        gunAiming.Initialized(transform.parent.localPosition, transform.parent.localEulerAngles, mainCamera.fieldOfView);
        gunAccuracy.Initialized();
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
            HandleReload();
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

        Vector3 shotDirection = gunAccuracy.GetDirection(mainCamera.transform, spreadMultiplier);
        Ray ray = new(mainCamera.transform.position, shotDirection);
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
        if (isReloading || totalAmmos <= 0 || currentAmmos == maxAmmo) return;

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
    }

    public void PlayReloadLoopSound()
    {
        PlayGunReloadSFX(reloadLoop);
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
        UIManager.Instance.SetCrosshairVisible(isAiming, hasRedDot);
        gunAiming.ApplyAiming(transform.parent, mainCamera, isAiming);
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

    public void PlayUnequipAnimation()
    {
        gunAnimations.PlayUnequipAnimation();
        if (isAiming)
            isAiming = false;
    }

    private void PlayGunShootSFX()
    {
        if (gunSource && shootClip != null)
            gunSource.PlayOneShot(shootClip);
    }

    private void PlayGunReloadSFX(AudioClip clip)
    {
        if (gunSource && clip != null)
            gunSource.PlayOneShot(clip);
    }

    public void SetGunActivated(bool activeStatus) => gunActivated = activeStatus;
    public void SetGunShooting(bool shootStatus) => isShooting = shootStatus;
    public void SetGunAiming(bool aimStatus) => isAiming = aimStatus;
}
