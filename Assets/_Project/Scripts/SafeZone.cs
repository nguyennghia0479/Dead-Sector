using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer)) == 0) return;

        if (other.TryGetComponent<PlayerController>(out var _))
            GameEvents.RaiseSafeZoneEntered();
    }

    private void OnTriggerExit(Collider other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer)) == 0) return;

        if (other.TryGetComponent<PlayerController>(out var player))
            GameEvents.RaiseSafeZoneExited(player);
    }
}
