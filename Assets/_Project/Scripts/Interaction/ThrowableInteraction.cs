using UnityEngine;

public class ThrowableInteraction : Interaction
{
    [SerializeField] private ThrowableSO throwableSO;
    [SerializeField] private int amount = 4;

    public override void OnInteract(PlayerController player)
    {
        player.WeaponManager.PickupThrowable(throwableSO.ThrowablePrefab, amount);
        Destroy(gameObject);
    }
}
