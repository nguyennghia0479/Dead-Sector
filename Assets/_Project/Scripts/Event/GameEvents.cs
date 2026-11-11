using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<Sprite, Sprite> OnWeaponSwitched;
    public static event Action<int, int, int> OnAmmosChanged;
    public static event Action<int> OnHealthChanged;
    public static event Action<ThrowableType, int> OnThrowableAmountChanged;
    public static event Action<EnemyController> OnEnemyDeath;
    public static event Action OnPlayerDeath;
    public static event Action OnPlayerHit;
    public static event Action OnPlayerHealed;

    public static void RaiseWeaponSwitched(Sprite primaryGun, Sprite secondaryGun)
    {
        OnWeaponSwitched?.Invoke(primaryGun, secondaryGun);
    }

    public static void RaiseAmmosChanged(int currentAmmos, int totalAmmos, int maxAmmos)
    {
        OnAmmosChanged?.Invoke(currentAmmos, totalAmmos, maxAmmos);
    }

    public static void RaiseHealthChanged(int currentHP)
    {
        OnHealthChanged?.Invoke(currentHP);
    }

    public static void RaiseThrowableAmountChanged(ThrowableType throwableType, int amount)
    {
        OnThrowableAmountChanged?.Invoke(throwableType, amount);
    }

    public static void RaiseEnemyDeath(EnemyController enemy)
    {
        OnEnemyDeath?.Invoke(enemy);
    }

    public static void RaisePlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    public static void RaisePlayerHit()
    {
        OnPlayerHit?.Invoke();
    }

    public static void RaisePlayerHealed()
    {
        OnPlayerHealed?.Invoke();
    }
}
