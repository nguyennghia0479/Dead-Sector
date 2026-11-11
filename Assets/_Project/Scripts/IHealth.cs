using UnityEngine;

public interface IHealth
{
    public bool IsDeath { get; }
    public void TakeDamage(int damage);
}
