using UnityEngine;

public class EnemyAnimationBridge : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public void CallHitFinished()
    {
        enemy.OnHitAnimationFinished();
    }
}
