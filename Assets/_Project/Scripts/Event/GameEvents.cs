using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static event Action<Sprite, Sprite> OnWeaponSwitched;
    public static event Action<int, int, int> OnAmmoChanged;
    public static event Action<int> OnHealthChanged;
    public static event Action<ThrowableType, int> OnThrowableAmountChanged;
    public static event Action<EnemyController> OnEnemyDeath;
    public static event Action OnPlayerDeath;
    public static event Action OnPlayerHit;
    public static event Action OnPlayerHealed;
    public static event Action<float> OnMedicalUsed;
    public static event Action<float> OnWaveStarted;
    public static event Action<int> OnWaveCompleted;
    public static event Action<List<Sprite>> OnItemsDropped;
    public static event Action<float> OnItemPicked;
    public static event Action<SoundType, Vector3, float> OnBulletHit;
    public static event Action OnSafeZoneEntered;
    public static event Action<PlayerController> OnSafeZoneExited;
    public static event Action<bool> OnPauseGameBtnPressed;

    public static void RaiseWeaponSwitched(Sprite primaryGun, Sprite secondaryGun)
    {
        OnWeaponSwitched?.Invoke(primaryGun, secondaryGun);
    }

    public static void RaiseAmmoChanged(int ammoInMag, int currentReserveAmmo, int magazineSize)
    {
        OnAmmoChanged?.Invoke(ammoInMag, currentReserveAmmo, magazineSize);
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

    public static void RaiseMedicalUsed(float healPercent)
    {
        OnMedicalUsed?.Invoke(healPercent);
    }

    public static void RaiseWaveStarted(float volume = 1f)
    {
        OnWaveStarted?.Invoke(volume);
    }

    public static void RaiseWaveCompleted(int currentWave)
    {
        OnWaveCompleted?.Invoke(currentWave);
    }

    public static void RaiseItemsDropped(List<Sprite> items)
    {
        OnItemsDropped?.Invoke(items);
    }

    public static void RaiseItemPicked(float volume = 1f)
    {
        OnItemPicked?.Invoke(volume);
    }

    public static void RaiseBulletHit(SoundType soundType, Vector3 position, float volume)
    {
        OnBulletHit?.Invoke(soundType, position, volume);
    }

    public static void RaiseSafeZoneEntered()
    {
        OnSafeZoneEntered?.Invoke();
    }

    public static void RaiseSafeZoneExited(PlayerController player)
    {
        OnSafeZoneExited?.Invoke(player);
    }

    public static void RaisePauseGameButtonPressed(bool isGamePaused)
    {
        OnPauseGameBtnPressed?.Invoke(isGamePaused);
    }
}
