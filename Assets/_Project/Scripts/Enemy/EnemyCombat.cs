using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private int damage = 10;
    private LayerMask playerLayer;

    public void Initialized(int damage, LayerMask playerLayer)
    {
        this.damage = damage;
        this.playerLayer = playerLayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        if (other.TryGetComponent<PlayerController>(out var player))
            player.PlayerHealth.TakeDamage(damage);
    }
}
