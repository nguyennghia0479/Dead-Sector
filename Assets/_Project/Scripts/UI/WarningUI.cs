using TMPro;
using UnityEngine;

public class WarningUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private float countdown = 5f;
    private float countdownTimer;
    private bool isTimeout;
    private PlayerController player;

    private const int BIG_DAMAGE = 9999;

    private void Awake()
    {
        panel.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.OnSafeZoneEntered += HandleSafeZoneEntered;
        GameEvents.OnSafeZoneExited += HandleSafeZoneExited;
    }

    private void OnDisable()
    {
        GameEvents.OnSafeZoneEntered -= HandleSafeZoneEntered;
        GameEvents.OnSafeZoneExited -= HandleSafeZoneExited;
    }

    private void Update()
    {
        if (!panel.activeInHierarchy || isTimeout) return;

        countdownTimer -= Time.deltaTime;
        countdownText.text = Mathf.CeilToInt(countdownTimer).ToString();
        if (countdownTimer <= 0)
        {
            countdownText.text = "0";
            isTimeout = true;
            panel.SetActive(false);
            if (player != null)
                player.PlayerHealth.TakeDamage(BIG_DAMAGE);
        }
    }

    private void HandleSafeZoneEntered()
    {
        if (!panel.activeInHierarchy) return;

        player = null;
        panel.SetActive(false);
    }

    private void HandleSafeZoneExited(PlayerController player)
    {
        if (panel.activeInHierarchy) return;

        isTimeout = false;
        countdownTimer = countdown;
        this.player = player;
        panel.SetActive(true);
    }
}
