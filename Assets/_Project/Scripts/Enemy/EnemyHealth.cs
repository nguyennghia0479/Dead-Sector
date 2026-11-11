using UnityEngine;

[System.Serializable]
public class EnemyHealth : IHealth
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentHP;

    private EnemyController enemy;
    private bool isDeath;

    public bool IsDeath => isDeath;

    public void Initialzied(EnemyController enemy)
    {
        this.enemy = enemy;
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
            GameEvents.RaiseEnemyDeath(enemy);
        }
    }
}
