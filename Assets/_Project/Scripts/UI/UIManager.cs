using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private HUD hud;
    [SerializeField] private Crosshair crosshair;
    [SerializeField] private InteractionUI interactionUI;
    [SerializeField] private GameObject sniperImage;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        sniperImage.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDeath -= HandlePlayerDeath;
    }

    public void UpdateCrosshair(float currentSpread, float minSpread, float maxSpread)
    {
        crosshair.UpdateCrosshair(currentSpread, minSpread, maxSpread);
    }

    public void SetCrosshairVisible(bool isVisible, bool hasRedDot, bool isSniper) => crosshair.SetVisible(isVisible, hasRedDot, isSniper);

    public void InteractionInitialized(PlayerInteraction playerInteraction)
    {
        interactionUI.Initialized(playerInteraction);
    }

    public void SetSniperVisible(bool isVisible) => sniperImage.SetActive(isVisible);

    public void HUDInitialzied(PlayerController player)
    {
        hud.Initialized(player);
    }

    private void HandlePlayerDeath()
    {
        if (sniperImage.activeInHierarchy)
            sniperImage.SetActive(false);
    }
}
