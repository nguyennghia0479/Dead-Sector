using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [Header("HUD UI References")]
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private Image primaryGunImg;
    [SerializeField] private Image secondaryGunImg;
    [SerializeField] private TMP_Text currentAmmosText;
    [SerializeField] private TMP_Text totalAmmosText;
    [Space]
    [SerializeField] private TMP_Text grenadeAmountText;
    [SerializeField] private TMP_Text molotovAmountText;
    private int currentHP;

    [Header("Reload UI Settings")]
    [SerializeField] private GameObject reloadUI;
    [SerializeField] private TMP_Text reloadKeyText;
    [SerializeField] private float ammoThresholdPercentage = .3f;
    [Space]
    [SerializeField] private GameObject lowAmmoUI;
    [SerializeField] private GameObject noAmmoUI;

    [Header("Blood Screen Settings")]
    [SerializeField] private Image bloodEffectImg;
    [SerializeField] private Image bloodBlurImg;
    [SerializeField] private float bloodScreenDuration = 2f;
    [SerializeField] private float hpThresholdPercentage = .3f;
    private int hpThreshold;
    private bool enableBloodBlur;
    private Coroutine bloodEffectRoutine;
    private Coroutine bloodBlurRoutine;

    [Header("Heal Screen Settings")]
    [SerializeField] private Image healScreenImg;
    [SerializeField] private float healScreenDuration = 2f;
    private Coroutine healScreenRoutine;

    private void OnEnable()
    {
        GameEvents.OnWeaponSwitched += HandleWeaponSwitched;
        GameEvents.OnAmmosChanged += HandleAmmosChanged;
        GameEvents.OnHealthChanged += HandleHealthChanged;
        GameEvents.OnThrowableAmountChanged += HandleThrowableAmountChanged;
        GameEvents.OnPlayerHit += HandlePlayerHit;
        GameEvents.OnPlayerHealed += HandlePlayerHealed;
    }

    private void OnDisable()
    {
        GameEvents.OnWeaponSwitched -= HandleWeaponSwitched;
        GameEvents.OnAmmosChanged -= HandleAmmosChanged;
        GameEvents.OnHealthChanged -= HandleHealthChanged;
        GameEvents.OnThrowableAmountChanged -= HandleThrowableAmountChanged;
        GameEvents.OnPlayerHit -= HandlePlayerHit;
        GameEvents.OnPlayerHealed -= HandlePlayerHealed;
    }

    public void Initialized(PlayerController player)
    {
        currentHP = player.PlayerHealth.CurrentHP;
        hpThreshold = Mathf.CeilToInt(currentHP * hpThresholdPercentage);
        reloadKeyText.text = player.GameInput.ReloadBindingDisplay;
    }

    private void HandleWeaponSwitched(Sprite primaryGun, Sprite secondaryGun)
    {
        primaryGunImg.sprite = primaryGun;

        if (secondaryGun == null)
        {
            secondaryGunImg.gameObject.SetActive(false);
            return;
        }

        secondaryGunImg.sprite = secondaryGun;
        secondaryGunImg.gameObject.SetActive(true);
    }

    private void HandleAmmosChanged(int currentAmmos, int totalAmmos, int maxAmmos)
    {
        currentAmmosText.text = currentAmmos.ToString();
        totalAmmosText.text = totalAmmos.ToString();

        int reloadThreshold = Mathf.CeilToInt(maxAmmos * ammoThresholdPercentage);
        reloadUI.SetActive((currentAmmos <= reloadThreshold) && totalAmmos > 0);
        lowAmmoUI.SetActive((currentAmmos <= reloadThreshold) && (totalAmmos == 0 && currentAmmos > 0));
        noAmmoUI.SetActive(currentAmmos == 0 && totalAmmos == 0);
    }

    private void HandleHealthChanged(int currentHP)
    {
        this.currentHP = currentHP;
        hpText.text = currentHP.ToString();
    }

    private void HandleThrowableAmountChanged(ThrowableType throwableType, int amount)
    {
        if (throwableType == ThrowableType.Grenade)
            grenadeAmountText.text = amount.ToString();
        else
            molotovAmountText.text = amount.ToString();
    }

    private void HandlePlayerHit()
    {
        if (bloodEffectRoutine != null)
            StopCoroutine(bloodEffectRoutine);
        bloodEffectRoutine = StartCoroutine(ChangeAlphaRoutine(bloodEffectImg, 1f, 0f));

        if (currentHP <= hpThreshold && !enableBloodBlur)
        {
            enableBloodBlur = true;
            bloodBlurRoutine = StartCoroutine(BloodBlurRoutine());
        }
    }

    private void HandlePlayerHealed()
    {
        if (currentHP > hpThreshold && enableBloodBlur)
        {
            enableBloodBlur = false;
            if (bloodBlurRoutine != null)
                StopCoroutine(bloodBlurRoutine);
            UpdateImageColorAlpha(bloodBlurImg, 0f);
        }

        if (bloodEffectRoutine != null)
            StopCoroutine(bloodEffectRoutine);
        UpdateImageColorAlpha(bloodEffectImg, 0f);

        if (healScreenRoutine != null)
            StopCoroutine(healScreenRoutine);
        healScreenRoutine = StartCoroutine(ChangeAlphaRoutine(healScreenImg, 1f, 0f));
    }

    private IEnumerator BloodBlurRoutine()
    {
        float alpha = 1f;
        while (enableBloodBlur)
        {
            yield return ChangeAlphaRoutine(bloodBlurImg, alpha, alpha / 2f);
            yield return ChangeAlphaRoutine(bloodBlurImg, alpha / 2f, alpha);
        }
    }

    private IEnumerator ChangeAlphaRoutine(Image image, float startAlpha, float targetAlpha)
    {
        float time = 0;
        while (time < bloodScreenDuration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / bloodScreenDuration);
            UpdateImageColorAlpha(image, newAlpha);
            time += Time.deltaTime;
            yield return null;
        }

        UpdateImageColorAlpha(image, targetAlpha);
    }

    private void UpdateImageColorAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
