using UnityEngine;

public class GunInteraction : Interaction
{
    [SerializeField] private GunController gunPrefab;

    public override void OnInteract(PlayerController player)
    {
        player.WeaponManager.PickupGun(gunPrefab, transform);
        Destroy(gameObject);
    }
}
