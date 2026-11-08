using UnityEngine;

public class GunInteraction : Interaction
{
    [SerializeField] private GunController gunPrefab;
    [SerializeField] private int currentAmmos;
    [SerializeField] private int totalAmmos;

    protected override void Awake()
    {
        base.Awake();
        currentAmmos = gunPrefab.MaxAmmos;
        totalAmmos = gunPrefab.TotalAmmos;
    }

    public override void OnInteract(PlayerController player)
    {
        player.WeaponManager.PickupGun(gunPrefab, transform, currentAmmos, totalAmmos);
        Destroy(gameObject);
    }

    public void SetAmmos(int currentAmmos, int totalAmmos)
    {
        this.currentAmmos = currentAmmos;
        this.totalAmmos = totalAmmos;
    }
}
