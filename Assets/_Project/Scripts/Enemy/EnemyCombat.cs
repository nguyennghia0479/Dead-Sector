using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private float damage = 10;
    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        if (other.TryGetComponent<PlayerController>(out var player))
            Debug.Log("Damage: " + damage + " on " + player.gameObject.name);
    }

}
