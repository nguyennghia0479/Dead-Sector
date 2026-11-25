using UnityEngine;
using UnityEngine.UI;

public class PauseGameUI : MonoBehaviour
{
    [Header("Buttons Reference")]
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button gameInputsBtn;
    [SerializeField] private Button settingsBtn;
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
        resumeBtn.onClick.AddListener(OnResumeButtonClicked);
        mainMenuBtn.onClick.AddListener(OnMainMenuButtonClicked);
        gameInputsBtn.onClick.AddListener(OnGameInputsButtonClicked);
        settingsBtn.onClick.AddListener(OnSettingsButtonClicked);
        quitBtn.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnDisable()
    {
        resumeBtn.onClick.RemoveListener(OnResumeButtonClicked);
        mainMenuBtn.onClick.RemoveListener(OnMainMenuButtonClicked);
        gameInputsBtn.onClick.RemoveListener(OnGameInputsButtonClicked);
        settingsBtn.onClick.RemoveListener(OnSettingsButtonClicked);
        quitBtn.onClick.RemoveListener(OnQuitButtonClicked);
    }

    public void EnableMainGameUI(bool isEnable)
    {
        canvasGroup.alpha = isEnable ? 1f : 0f;
        canvasGroup.interactable = isEnable;
        canvasGroup.blocksRaycasts = isEnable;
    }

    private void OnResumeButtonClicked()
    {
        GameEvents.RaisePauseGameButtonPressed(false);
    }

    private void OnMainMenuButtonClicked()
    {
        uiManager.DoLoading(() => UIEvents.RaiseMainMenuButtonClicked());
    }

    private void OnGameInputsButtonClicked()
    {
        UIEvents.RaiseGameInputsButtonClicked();
    }

    private void OnSettingsButtonClicked()
    {
        UIEvents.RaiseSettingButtonClicked();
    }

    private void OnQuitButtonClicked()
    {
        uiManager.QuitGame();
    }
}
