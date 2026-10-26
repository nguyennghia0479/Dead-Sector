using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float weaponRange = 100f;
    [SerializeField] private float fireRate;
    [SerializeField] private LayerMask ignoreLayer;
    private float nextFireTimer;

    [Header("Ammo Settings")]
    [SerializeField] private int totalAmmos;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int currentAmmos;
    [SerializeField] private bool isReloading;

    [Header("Gun References")]
    [SerializeField] private GunAnimations gunAnimations;

    [Header("References")]
    [SerializeField] private Camera mainCamera;

    private bool gunActivated;
    private bool isShooting;

    private void Awake()
    {
        currentAmmos = maxAmmo;
    }

    private void Start()
    {
        gunAnimations.Initialized();
    }

    private void Update()
    {
        nextFireTimer += Time.deltaTime;
        if (isShooting && !isReloading && nextFireTimer >= fireRate)
        {
            if (currentAmmos > 0)
            {
                nextFireTimer = 0f;
                HandleShoot();
            }
            else
                HandleReload();
        }
    }

    private void HandleShoot()
    {
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * weaponRange, Color.red);

        currentAmmos--;
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
        int ammosToReload = maxAmmo - currentAmmos;
        currentAmmos = totalAmmos > ammosToReload ? currentAmmos + ammosToReload : currentAmmos + totalAmmos;
        totalAmmos -= ammosToReload;
        totalAmmos = Mathf.Clamp(totalAmmos, 0, totalAmmos);
        isReloading = false;
    }

    public void GunActivated(bool activeStatus) => gunActivated = activeStatus;
    public void GunShooting(bool shootStatus) => isShooting = shootStatus;
}
