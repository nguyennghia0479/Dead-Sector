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
    public static event Action<int> OnWaveCompleted;
    public static event Action<List<Sprite>> OnItemsDropped;
    public static event Action<AudioClip, Vector3, float> OnBulletHit;
    public static event Action OnSafeZoneEntered;
    public static event Action<PlayerController> OnSafeZoneExited;

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

    public static void RaiseWaveCompleted(int currentWave)
    {
        OnWaveCompleted?.Invoke(currentWave);
    }

    public static void RaiseItemsDropped(List<Sprite> items)
    {
        OnItemsDropped?.Invoke(items);
    }

    public static void RaiseBulletHit(AudioClip clip, Vector3 position, float volume)
    {
        OnBulletHit?.Invoke(clip, position, volume);
    }

    public static void RaiseSafeZoneEntered()
    {
        OnSafeZoneEntered?.Invoke();
    }

    public static void RaiseSafeZoneExited(PlayerController player)
    {
        OnSafeZoneExited?.Invoke(player);
    }
}
