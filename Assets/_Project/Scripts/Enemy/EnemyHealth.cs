using System;
using UnityEngine;

[System.Serializable]
public class EnemyHealth
{
    public event Action OnDeath;

    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentHP;

    private bool isDeath;

    public bool IsDeath => isDeath;

    public void Initialzied()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (isDeath) return;

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        if (currentHP <= 0)
        {
            isDeath = true;
            OnDeath?.Invoke();
        }
    }
}
