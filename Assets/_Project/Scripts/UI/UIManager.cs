using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Crosshair crosshair;
    [SerializeField] private InteractionUI interactionUI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateCrosshair(float currentSpread, float minSpread, float maxSpread)
    {
        crosshair.UpdateCrosshair(currentSpread, minSpread, maxSpread);
    }

    public void SetCrosshairVisible(bool isVisible, bool hasRedDot) => crosshair.SetVisible(isVisible, hasRedDot);

    public void InteractionInitialized(PlayerInteraction playerInteraction)
    {
        interactionUI.Initialized(playerInteraction);
    }
}
