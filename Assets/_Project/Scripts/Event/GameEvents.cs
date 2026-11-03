using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<Sprite, Sprite> OnWeaponSwitched;
    public static event Action<int, int> OnAmmosChanged;
    public static event Action<int> OnHealthChanged;
    public static event Action<ThrowableType, int> OnThrowableAmountChanged;

    public static void RaiseWeaponSwitched(Sprite primaryGun, Sprite secondaryGun)
    {
        OnWeaponSwitched?.Invoke(primaryGun, secondaryGun);
    }

    public static void RaiseAmmosChanged(int currentAmmos, int totalAmmos)
    {
        OnAmmosChanged?.Invoke(currentAmmos, totalAmmos);
    }

    public static void RaiseHealthChanged(int currentHP)
    {
        OnHealthChanged?.Invoke(currentHP);
    }

    public static void RaiseThrowableAmountChanged(ThrowableType throwableType, int amount)
    {
        OnThrowableAmountChanged?.Invoke(throwableType, amount);
    }
}
