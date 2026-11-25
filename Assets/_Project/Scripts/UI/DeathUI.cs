using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathUI : MonoBehaviour
{
    [Header("Black Screen Settings")]
    [SerializeField] private Image blackScreenImg;
    [SerializeField] private float blackScreenDuration = 3.5f;

    [Header("Text Settings")]
    [SerializeField] private TMP_Text deathText;
    [SerializeField] private float deathTextDuration = 5f;

    [Header("Buttons Reference")]
    [SerializeField] private Button retryBtn;
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private GameObject buttonsHolder;

    [Header("Result Reference")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text wavesResultText;
    [SerializeField] private TMP_Text zombiesKilledText;
    [SerializeField] private TMP_Text itemPickedText;
    [SerializeField] private TMP_Text playedTimeText;

    private UIManager uiManager;
    private GameManager gameManager;

    private void Awake()
    {
        uiManager = GetComponentInParent<UIManager>();
        HideDeathUI();
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerDeath += HandlePlayerDeath;

        retryBtn.onClick.AddListener(OnRetryButtonClicked);
        mainMenuBtn.onClick.AddListener(OnMainMenuButtonClicked);
        quitBtn.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDeath -= HandlePlayerDeath;

        retryBtn.onClick.RemoveListener(OnRetryButtonClicked);
        mainMenuBtn.onClick.RemoveListener(OnMainMenuButtonClicked);
        quitBtn.onClick.RemoveListener(OnQuitButtonClicked);
    }

    private void HandlePlayerDeath()
    {
        if (gameManager == null)
            gameManager = GameManager.Instance;

        StartCoroutine(BlackScreenRoutine());
        StartCoroutine(DeathTextRoutine());
    }

    private IEnumerator BlackScreenRoutine()
    {
        float time = 0f;
        blackScreenImg.color = UpdateColorAlpha(blackScreenImg.color, 0f);

        while (time < blackScreenDuration)
        {
            float newAlpha = Mathf.Lerp(0f, 1f, time / blackScreenDuration);
            blackScreenImg.color = UpdateColorAlpha(blackScreenImg.color, newAlpha);
            time += Time.deltaTime;
            yield return null;
        }

        blackScreenImg.color = UpdateColorAlpha(blackScreenImg.color, 1f);
        buttonsHolder.SetActive(true);
        GameResult();
    }

    private IEnumerator DeathTextRoutine()
    {
        float time = 0f;
        deathText.color = UpdateColorAlpha(deathText.color, 0f);

        while (time < deathTextDuration)
        {
            float newAlpha = Mathf.Lerp(0f, 1f, time / deathTextDuration);
            deathText.color = UpdateColorAlpha(deathText.color, newAlpha);
            time += Time.deltaTime;
            yield return null;
        }

        deathText.color = UpdateColorAlpha(deathText.color, 1f);
    }

    private void GameResult()
    {
        resultPanel.SetActive(true);
        if (gameManager != null)
        {
            wavesResultText.text = gameManager.WavesCompleted.ToString();
            zombiesKilledText.text = gameManager.ZombiesKilled.ToString();
            itemPickedText.text = gameManager.ItemPicked.ToString();

            TimeSpan timeSpan = TimeSpan.FromSeconds(gameManager.PlayingTime);
            string formatTime = timeSpan.ToString(@"hh\:mm\:ss");
            playedTimeText.text = formatTime;
        }
    }

    private Color UpdateColorAlpha(Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }

    private void HideDeathUI()
    {
        blackScreenImg.color = UpdateColorAlpha(blackScreenImg.color, 0f);
        deathText.color = UpdateColorAlpha(deathText.color, 0f);
        buttonsHolder.SetActive(false);
        resultPanel.SetActive(false);
    }

    private void OnRetryButtonClicked()
    {
        HideDeathUI();
        uiManager.DoLoading(() => UIEvents.RaiseNewGameButtonClicked());
    }

    private void OnMainMenuButtonClicked()
    {
        HideDeathUI();
        uiManager.DoLoading(() => UIEvents.RaiseMainMenuButtonClicked());
    }

    private void OnQuitButtonClicked()
    {
        uiManager.QuitGame();
    }
}
