using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject[] panels;

    [Space]
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private MainGameUI mainGameUI;
    [SerializeField] private PauseGameUI pauseGameUI;
    [SerializeField] private GameInputsUI gameInputsUI;
    [SerializeField] private SettingsUI settingsUI;
    [SerializeField] private CreditsUI creditsUI;
    [SerializeField] private LoadingUI loadingUI;

    private PlayerController player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        UIEvents.OnNewGameBtnClicked += HandleNewGameButtonClicked;
        UIEvents.OnMainMenuBtnClicked += HandleMainMenuButtonClicked;
        UIEvents.OnGameInputsBtnClicked += HandleGameInputsBtnClicked;
        UIEvents.OnSettingsBtnClicked += HandleSettingsBtnClicked;
        UIEvents.OnCreditsBtnClicked += HandleCreditsBtnClicked;

        GameEvents.OnPauseGameBtnPressed += HandlePauseGameButtonPressed;
    }

    private void OnDisable()
    {
        UIEvents.OnNewGameBtnClicked -= HandleNewGameButtonClicked;
        UIEvents.OnMainMenuBtnClicked -= HandleMainMenuButtonClicked;
        UIEvents.OnGameInputsBtnClicked -= HandleGameInputsBtnClicked;
        UIEvents.OnSettingsBtnClicked -= HandleSettingsBtnClicked;
        UIEvents.OnCreditsBtnClicked -= HandleCreditsBtnClicked;

        GameEvents.OnPauseGameBtnPressed -= HandlePauseGameButtonPressed;
    }

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>();

        mainMenuUI.EnableMainGameUI(true);
        mainGameUI.EnableMainGameUI(false);
        pauseGameUI.EnableMainGameUI(false);

        gameInputsUI.Initialized(player.GameInput);
        settingsUI.Initialized(player.PlayerCamera);
        ClosePanels();
    }

    private void HandleNewGameButtonClicked()
    {
        mainMenuUI.EnableMainGameUI(false);
        mainGameUI.EnableMainGameUI(true);
        pauseGameUI.EnableMainGameUI(false);
        ClosePanels();
    }

    private void HandleMainMenuButtonClicked()
    {
        mainMenuUI.EnableMainGameUI(true);
        mainGameUI.EnableMainGameUI(false);
        pauseGameUI.EnableMainGameUI(false);
        ClosePanels();
    }

    private void HandleGameInputsBtnClicked()
    {
        SwitchToPanelActive(gameInputsUI.gameObject);
    }

    private void HandleSettingsBtnClicked()
    {
        SwitchToPanelActive(settingsUI.gameObject);
    }

    private void HandleCreditsBtnClicked()
    {
        SwitchToPanelActive(creditsUI.gameObject);
    }

    private void HandlePauseGameButtonPressed(bool isPauseGame)
    {
        mainGameUI.EnableMainGameUI(!isPauseGame);
        pauseGameUI.EnableMainGameUI(isPauseGame);
        if (!isPauseGame)
            ClosePanels();
    }

    public void SwitchToPanelActive(GameObject panelToActive)
    {
        ClosePanels();

        if (panelToActive != null)
            panelToActive.SetActive(true);
    }

    public void ClosePanel(GameObject panelToClose)
    {
        if (panelToClose != null)
        {
            UIEvents.RaiseButtonClicked();
            panelToClose.SetActive(false);
        }
    }

    private void ClosePanels()
    {
        foreach (var panel in panels)
            panel.SetActive(false);
    }

    public void DoLoading(Action action)
    {
        loadingUI.DoLoading(action);
    }

    public void QuitGame()
    {
        UIEvents.RaiseButtonClicked();
        Application.Quit();
    }

    public void UpdateCrosshair(float currentSpread, float minSpread, float maxSpread)
    {
        mainGameUI.UpdateCrosshair(currentSpread, minSpread, maxSpread);
    }

    public void SetCrosshairVisible(bool isVisible, bool hasRedDot, bool isSniper)
    {
        mainGameUI.SetCrosshairVisible(isVisible, hasRedDot, isSniper);
    }

    public void InteractionInitialized(PlayerInteraction playerInteraction)
    {
        mainGameUI.InteractionInitialized(playerInteraction);
    }

    public void SetSniperVisible(bool isVisible)
    {
        mainGameUI.SetSniperVisible(isVisible);
    }

    public void HUDInitialzied(PlayerController player)
    {
        mainGameUI.HUDInitialzied(player);
    }
}
