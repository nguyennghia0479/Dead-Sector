using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private int damage;
    private LayerMask playerLayer;
    private EnemyController enemyController;

    public void Initialized(int damage, LayerMask playerLayer, EnemyController enemyController)
    {
        this.damage = damage;
        this.playerLayer = playerLayer;
        this.enemyController = enemyController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemyController.EnemyHealth.IsDeath) return;

        if ((playerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        if (other.TryGetComponent<PlayerController>(out var player))
            player.PlayerHealth.TakeDamage(damage);
    }
}
