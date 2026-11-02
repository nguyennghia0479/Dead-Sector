using UnityEngine;

public class ThrowableInteraction : Interaction
{
    [SerializeField] private Throwable throwablePrefab;
    [SerializeField] private int amount = 4;

    public override void OnInteract(PlayerController player)
    {
        player.WeaponManager.PickupThrowable(throwablePrefab, amount);
        Destroy(gameObject);
    }
}
