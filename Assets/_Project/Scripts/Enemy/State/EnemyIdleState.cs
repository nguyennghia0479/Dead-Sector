using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private float idleTime = 1f;

    public EnemyIdleState(Enemy enemy, StateMachine stateMachine, string boolHash) : base(enemy, stateMachine, boolHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = idleTime;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            stateMachine.ChangeState(enemy.WalkState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
