using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [Header("Health References")]
    [SerializeField] private TMP_Text hpText;

    [Header("Weapon References")]
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

    private void HandleAmmosChanged(int currentAmmos, int totalAmmos)
    {
        currentAmmosText.text = currentAmmos.ToString();
        totalAmmosText.text = totalAmmos.ToString();
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
