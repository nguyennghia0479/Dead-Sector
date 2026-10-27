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

    [Header("Gun Attributes")]
    [SerializeField] private GunAnimations gunAnimations;
    [SerializeField] private CameraRecoil cameraRecoil;
    [SerializeField] private GunSway gunSway;

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
    }

    private void Update()
    {
        HandleShoot();
        HandleUpdateRecoil();
        HandleGunSway();
    }

    private void HandleShoot()
    {
        nextFireTimer += Time.deltaTime;
        if (!isShooting || isReloading || nextFireTimer < fireRate)
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
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * weaponRange, Color.red);

        currentAmmos--;
        muzzleFlash.Play();
        cameraRecoil.ApplyRecoil(isAiming);

        Ray ray = new(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, weaponRange, ~ignoreLayer))
        {
            if (hit.collider != null)
                Debug.Log(hit.collider.gameObject.name);
        }
    }

    public void HandleReload()
    {
        if (totalAmmos <= 0 || !gunActivated) return;

        isReloading = true;
        gunAnimations.PlayReloadAnimation();
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

    public void GunActivated(bool activeStatus) => gunActivated = activeStatus;
    public void GunShooting(bool shootStatus) => isShooting = shootStatus;
}
