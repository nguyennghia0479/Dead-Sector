using UnityEngine;

public class GunInteraction : Interaction
{
    [SerializeField] private GunSO gunSO;
    [SerializeField] private int ammoInMag;
    [SerializeField] private int currentReserveAmmo;
    [SerializeField] private bool hasPicked;

    protected override void Awake()
    {
        base.Awake();
        ammoInMag = gunSO.MagazineSize;
        currentReserveAmmo = gunSO.ReserveAmmo;
    }

    public override void OnInteract(PlayerController player)
    {
        if (!hasPicked)
            GameEvents.RaiseItemPicked(0f);

        player.WeaponManager.PickupGun(gunSO.GunPrefab, transform, ammoInMag, currentReserveAmmo);
        Destroy(gameObject);
    }

    public void SetAmmos(int ammoInMag, int currentReserveAmmo)
    {
        hasPicked = true;
        this.ammoInMag = ammoInMag;
        this.currentReserveAmmo = currentReserveAmmo;
    }
}
