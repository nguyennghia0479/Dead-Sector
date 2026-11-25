using UnityEngine;

public class MainGameUI : MonoBehaviour
{
    [SerializeField] private Crosshair crosshair;
    [SerializeField] private GameObject sniperImage;
    [SerializeField] private InteractionUI interactionUI;
    [SerializeField] private HUD hud;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

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

    public void EnableMainGameUI(bool isEnable)
    {
        canvasGroup.alpha = isEnable ? 1f : 0f;
        canvasGroup.interactable = isEnable;
        canvasGroup.blocksRaycasts = isEnable;
    }

    public void UpdateCrosshair(float currentSpread, float minSpread, float maxSpread)
    {
        crosshair.UpdateCrosshair(currentSpread, minSpread, maxSpread);
    }

    public void SetCrosshairVisible(bool isVisible, bool hasRedDot, bool isSniper)
    {
        crosshair.SetVisible(isVisible, hasRedDot, isSniper);
    }

    public void InteractionInitialized(PlayerInteraction playerInteraction)
    {
        interactionUI.Initialized(playerInteraction);
    }

    public void SetSniperVisible(bool isVisible)
    {
        sniperImage.SetActive(isVisible);
    }

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
