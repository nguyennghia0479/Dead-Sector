using System;
using System.Collections;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private float duration = 2f;

    private CanvasGroup canvasGroup;
    private Coroutine loadingRoutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        DoLoading(null);
    }

    public void DoLoading(Action action)
    {
        if (loadingRoutine != null)
            StopCoroutine(loadingRoutine);

        loadingRoutine = StartCoroutine(LoadingRoutine(action));
    }

    private IEnumerator LoadingRoutine(Action action)
    {
        canvasGroup.alpha = 1f;
        float time = 0;

        action?.Invoke();
        while (time < duration)
        {
            float newAlpha = Mathf.Lerp(1f, 0f, time / duration);
            canvasGroup.alpha = newAlpha;
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
