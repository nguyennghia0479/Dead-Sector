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

    private void OnEnable()
    {
        GameEvents.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        StartCoroutine(BlackScreenRoutine());
        StartCoroutine(DeathTextRoutine());
    }

    private IEnumerator BlackScreenRoutine()
    {
        float time = 0f;
        blackScreenImg.color = UpdateColorAlpha(blackScreenImg.color, 0f);

        while (time < blackScreenDuration)
        {
            float newAlpha = Mathf.Lerp(0f, 1f, time /  blackScreenDuration);
            blackScreenImg.color = UpdateColorAlpha(blackScreenImg.color, newAlpha);
            time += Time.deltaTime;
            yield return null;
        }

        blackScreenImg.color = UpdateColorAlpha(blackScreenImg.color, 1f);
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

    private Color UpdateColorAlpha(Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
}
