using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentHP;

    private void Start()
    {
        currentHP = maxHP;
        UpdateHealthUI();
    }

    private void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
            IncreaseHealth();

        if (Keyboard.current.nKey.wasPressedThisFrame)
            DecreaseHealth();
    }

    private void IncreaseHealth()
    {
        currentHP += 10;
        currentHP = Mathf.Min(currentHP, maxHP);
        UpdateHealthUI();
    }

    private void DecreaseHealth()
    {
        currentHP -= 10;
        currentHP = Mathf.Max(currentHP, 0);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        GameEvents.RaiseHealthChanged(currentHP);
    }
}
