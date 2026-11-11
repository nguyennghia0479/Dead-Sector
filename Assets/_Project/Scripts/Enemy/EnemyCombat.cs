using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        if (other.TryGetComponent<PlayerController>(out var player))
            player.PlayerHealth.TakeDamage(damage);
    }

}
