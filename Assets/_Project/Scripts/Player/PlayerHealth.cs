using UnityEngine;

[System.Serializable]
public class PlayerHealth : IHealth
{
    [SerializeField] private int maxHP = 100;

    private int currentHP;
    private bool isDeath;

    public int CurrentHP => currentHP;
    public bool IsDeath => isDeath;

    public void ResetHealth()
    {
        isDeath = false;
        currentHP = maxHP;
        UpdateHealthUI();
    }  

    public void HandleMedicalUsed(float healPercent)
    {
        int healAmount = Mathf.CeilToInt(maxHP * healPercent);
        Healing(healAmount);
    }

    private void Healing(int healAmount)
    {
        currentHP += healAmount;
        currentHP = Mathf.Min(currentHP, maxHP);
        UpdateHealthUI();
        GameEvents.RaisePlayerHealed();
    }

    public void TakeDamage(int damage)
    {
        if (isDeath) return;

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        if (currentHP <= 0 && !isDeath)
        {
            isDeath = true;
            GameEvents.RaisePlayerDeath();
        }

        UpdateHealthUI();
        GameEvents.RaisePlayerHit();
    }

    private void UpdateHealthUI()
    {
        GameEvents.RaiseHealthChanged(currentHP);
    }
}
