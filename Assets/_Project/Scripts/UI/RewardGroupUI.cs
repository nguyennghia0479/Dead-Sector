using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardGroupUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 6f;
    [SerializeField] private RewardUI[] rewardUIs;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        rewardUIs = GetComponentsInChildren<RewardUI>();
        foreach (var reward in rewardUIs)
            reward.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.OnItemsDropped += HandleItemsDropped;
    }

    private void OnDisable()
    {
        GameEvents.OnItemsDropped -= HandleItemsDropped;
    }

    private void HandleItemsDropped(List<Sprite> itemsDropped)
    {
        canvasGroup.alpha = 1f;
        foreach (var reward in rewardUIs)
            reward.gameObject.SetActive(false);

        for (int i = 0; i < itemsDropped.Count; i++)
        {
            RewardUI rewardUI = rewardUIs[i];
            rewardUI.SetupRewardUI(itemsDropped[i]);
            rewardUI.gameObject.SetActive(true);
        }

        StartCoroutine(DecreaseAlphaRoutine());
    }

    private IEnumerator DecreaseAlphaRoutine()
    {
        if (canvasGroup.alpha <= 0) yield break;

        float time = 0;
        while (time < duration)
        {
            float newAlpha = Mathf.Lerp(1, 0, time / duration);
            canvasGroup.alpha = newAlpha;
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
    }
}
