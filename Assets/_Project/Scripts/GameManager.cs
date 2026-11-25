using UnityEngine;

public enum GameState
{
    MainMenu, MainGame, PauseGame, GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int wavesCompleted;
    [SerializeField] private int zombiesKilled;
    [SerializeField] private int itemPicked;
    [SerializeField] private float startGameTime;
    [SerializeField] private float endGameTime;
    private GameState gameState;
    private GameInput gameInput;

    public int WavesCompleted => wavesCompleted;
    public int ZombiesKilled => zombiesKilled;
    public int ItemPicked => itemPicked;
    public float PlayingTime => endGameTime - startGameTime;

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

        GameEvents.OnPauseGameBtnPressed += HandlePauseGameButtonPressed;
        GameEvents.OnPlayerDeath += HandlePlayerDeath;
        GameEvents.OnEnemyDeath += HandleEnemyDeath;
        GameEvents.OnWaveCompleted += HandleWaveCompleted;
        GameEvents.OnItemPicked += HandleItemPicked;
    }

    private void OnDisable()
    {
        UIEvents.OnNewGameBtnClicked -= HandleNewGameButtonClicked;
        UIEvents.OnMainMenuBtnClicked -= HandleMainMenuButtonClicked;

        GameEvents.OnPauseGameBtnPressed -= HandlePauseGameButtonPressed;
        GameEvents.OnPlayerDeath -= HandlePlayerDeath;
        GameEvents.OnEnemyDeath -= HandleEnemyDeath;
        GameEvents.OnWaveCompleted -= HandleWaveCompleted;
        GameEvents.OnItemPicked -= HandleItemPicked;
    }

    private void Start()
    {
        gameInput = FindAnyObjectByType<GameInput>();
        HandleStateChanged(GameState.MainMenu);
    }

    public bool IsMainGameState() => gameState == GameState.MainGame;

    private void HandleNewGameButtonClicked()
    {
        wavesCompleted = 0;
        zombiesKilled = 0;
        itemPicked = 0;
        startGameTime = Time.time;
        HandleStateChanged(GameState.MainGame);
    }

    private void HandleMainMenuButtonClicked()
    {
        HandleStateChanged(GameState.MainMenu);
    }

    private void HandlePauseGameButtonPressed(bool isPauseGame)
    {
        if (isPauseGame)
            HandleStateChanged(GameState.PauseGame);
        else
            HandleStateChanged(GameState.MainGame);
    }

    private void HandlePlayerDeath()
    {
        endGameTime = Time.time;
        HandleStateChanged(GameState.GameOver);
    }

    private void HandleEnemyDeath(EnemyController enemy) => zombiesKilled++;
    private void HandleWaveCompleted(int currentWave) => wavesCompleted++;
    private void HandleItemPicked(float volume) => itemPicked++;

    private void HandleStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MainMenu:
                MainMenuState();
                break;
            case GameState.MainGame:
                MainGameState();
                break;
            case GameState.PauseGame:
                GamePausedState();
                break;
            case GameState.GameOver:
                GameOverState();
                break;
        }
    }

    private void MainMenuState()
    {
        gameState = GameState.MainMenu;
        gameInput.DisablePlayerInput();
        gameInput.DisableUIInput();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void MainGameState()
    {
        gameState = GameState.MainGame;
        gameInput.EnablePlayerInput();
        gameInput.DisableUIInput();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void GamePausedState()
    {
        gameState = GameState.PauseGame;
        gameInput.DisablePlayerInput();
        gameInput.EnableUIInput();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void GameOverState()
    {
        gameState = GameState.GameOver;
        gameInput.DisablePlayerInput();
        gameInput.DisableUIInput();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
