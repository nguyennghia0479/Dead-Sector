using UnityEngine;

public class GunInteraction : Interaction
{
    [SerializeField] private GunSO gunSO;
    [SerializeField] private int ammoInMag;
    [SerializeField] private int currentReserveAmmo;

    protected override void Awake()
    {
        base.Awake();
        ammoInMag = gunSO.MagazineSize;
        currentReserveAmmo = gunSO.ReserveAmmo;
    }

    public override void OnInteract(PlayerController player)
    {
        player.WeaponManager.PickupGun(gunSO.GunPrefab, transform, ammoInMag, currentReserveAmmo);
        Destroy(gameObject);
    }

    public void SetAmmos(int ammoInMag, int currentReserveAmmo)
    {
        this.ammoInMag = ammoInMag;
        this.currentReserveAmmo = currentReserveAmmo;
    }
}
