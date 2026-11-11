using UnityEngine;

public class EnemyAnimationBridge : MonoBehaviour
{
    private EnemyController enemy;

    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }

    public void CallHitFinished()
    {
        enemy.OnHitAnimationFinished();
    }
}
