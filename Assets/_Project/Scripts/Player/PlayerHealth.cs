using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private int maxHP = 100;

    private int currentHP;
    private bool isDeath;

    public int CurrentHP => currentHP;
    public bool IsDeath => isDeath;

    private void Awake()
    {
        currentHP = maxHP;
    }

    private void Start()
    {
        UpdateHealthUI();
    }

    private void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
            Healing();

        if (Keyboard.current.nKey.wasPressedThisFrame)
            TakeDamage(10);
    }

    private void Healing()
    {
        currentHP += 10;
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
