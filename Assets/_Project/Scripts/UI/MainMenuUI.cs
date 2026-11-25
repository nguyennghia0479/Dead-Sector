using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Buttons Reference")]
    [SerializeField] private Button newGameBtn;
    [SerializeField] private Button gameInputsBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button creditsBtn;
    [SerializeField] private Button quitBtn;

    private CanvasGroup canvasGroup;
    private UIManager uiManager;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        uiManager = GetComponentInParent<UIManager>();
    }

    private void OnEnable()
    {
        newGameBtn.onClick.AddListener(OnNewGameButtonClicked);
        gameInputsBtn.onClick.AddListener(OnGameInputsButtonClicked);
        settingsBtn.onClick.AddListener(OnSettingsButtonClicked);
        creditsBtn.onClick.AddListener(OnCreditsButtonClicked);
        quitBtn.onClick.AddListener(OnQuitButtonClicked);

    }

    private void OnDisable()
    {
        newGameBtn.onClick.RemoveListener(OnNewGameButtonClicked);
        gameInputsBtn.onClick.RemoveListener(OnGameInputsButtonClicked);
        settingsBtn.onClick.RemoveListener(OnSettingsButtonClicked);
        creditsBtn.onClick.RemoveListener(OnCreditsButtonClicked);
        quitBtn.onClick.RemoveListener(OnQuitButtonClicked);

    }

    public void EnableMainGameUI(bool isEnable)
    {
        canvasGroup.alpha = isEnable ? 1f : 0f;
        canvasGroup.interactable = isEnable;
        canvasGroup.blocksRaycasts = isEnable;
    }

    private void OnNewGameButtonClicked()
    {
        uiManager.DoLoading(() => UIEvents.RaiseNewGameButtonClicked());
    }

    private void OnGameInputsButtonClicked()
    {
        UIEvents.RaiseGameInputsButtonClicked();
    }

    private void OnSettingsButtonClicked()
    {
        UIEvents.RaiseSettingButtonClicked();
    }

    private void OnCreditsButtonClicked()
    {
        UIEvents.RaiseCreditsButtonClicked();
    }

    private void OnQuitButtonClicked()
    {
        uiManager.QuitGame();
    }
}
