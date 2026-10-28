using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [Header("Crosshair Settings")]
    [SerializeField] private RectTransform crosshairRect;
    [SerializeField] private float compactSize = 70f;
    [SerializeField] private float expandedSize = 220f;
    [SerializeField] private float changeSizeSpeed = 10f;

    [Space]
    [SerializeField] private GameObject aimDot;

    private float currentSize;
    private float minSpread;
    private float maxSpread;
    private float currentSpread;

    private void Awake()
    {
        if (crosshairRect == null) return;

        currentSize = compactSize;
        crosshairRect.sizeDelta = new(currentSize, currentSize);

        if (aimDot == null)
            aimDot.SetActive(false);
    }

    private void LateUpdate()
    {
        float normalizedSpread = Mathf.InverseLerp(minSpread, maxSpread, currentSpread);
        float targetSize = Mathf.Lerp(compactSize, expandedSize, normalizedSpread);
        currentSize = Mathf.Lerp(currentSize, targetSize, changeSizeSpeed * Time.deltaTime);
        crosshairRect.sizeDelta = new(currentSize, currentSize);
    }

    public void UpdateCrosshair(float currentSpread, float minSpread, float maxSpread)
    {
        this.currentSpread = currentSpread;
        this.minSpread = minSpread;
        this.maxSpread = maxSpread;
    }

    public void SetVisible(bool isAiming, bool hasRedDot)
    {
        if (crosshairRect != null)
            crosshairRect.gameObject.SetActive(!isAiming);

        if (aimDot != null)
            aimDot.SetActive(isAiming && hasRedDot);
    }
}
