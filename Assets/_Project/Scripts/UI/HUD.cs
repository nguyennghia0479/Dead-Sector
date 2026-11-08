using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [Header("Reload UI Settings")]
    [SerializeField] private GameObject reloadUI;
    [SerializeField] private TMP_Text reloadKeyText;
    [SerializeField] private float thresholdPercentage = .3f;

    [Space]
    [SerializeField] private GameObject lowAmmoUI;
    [SerializeField] private GameObject noAmmoUI;

    [Header("Health References")]
    [SerializeField] private TMP_Text hpText;

    [Header("Weapon UI References")]
    [SerializeField] private Image primaryGunImg;
    [SerializeField] private Image secondaryGunImg;
    [SerializeField] private TMP_Text currentAmmosText;
    [SerializeField] private TMP_Text totalAmmosText;

    [Space]
    [SerializeField] private TMP_Text grenadeAmountText;
    [SerializeField] private TMP_Text molotovAmountText;

    private void OnEnable()
    {
        GameEvents.OnWeaponSwitched += HandleWeaponSwitched;
        GameEvents.OnAmmosChanged += HandleAmmosChanged;
        GameEvents.OnHealthChanged += HandleHealthChanged;
        GameEvents.OnThrowableAmountChanged += HandleThrowableAmountChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnWeaponSwitched -= HandleWeaponSwitched;
        GameEvents.OnAmmosChanged -= HandleAmmosChanged;
        GameEvents.OnHealthChanged -= HandleHealthChanged;
        GameEvents.OnThrowableAmountChanged -= HandleThrowableAmountChanged;
    }

    public void Initialized(GameInput gameInput)
    {
        reloadKeyText.text = gameInput.ReloadBindingDisplay;
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

        int reloadThreshold = Mathf.CeilToInt(maxAmmos * thresholdPercentage);
        reloadUI.SetActive((currentAmmos <= reloadThreshold) && totalAmmos > 0);
        lowAmmoUI.SetActive((currentAmmos <= reloadThreshold) && (totalAmmos == 0 && currentAmmos > 0));
        noAmmoUI.SetActive(currentAmmos == 0 && totalAmmos == 0);
    }

    private void HandleHealthChanged(int currentHP)
    {
        hpText.text = currentHP.ToString();
    }

    private void HandleThrowableAmountChanged(ThrowableType throwableType, int amount)
    {
        if (throwableType == ThrowableType.Grenade)
            grenadeAmountText.text = amount.ToString();
        else
            molotovAmountText.text = amount.ToString();
    }
}
