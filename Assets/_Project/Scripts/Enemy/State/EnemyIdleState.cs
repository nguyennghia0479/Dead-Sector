using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyController enemy, StateMachine stateMachine, string boolHash) : base(enemy, stateMachine, boolHash)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.IdleDuration;
        agent.isStopped = true;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.Player.PlayerHealth.IsDeath)
            return;

        if (stateTimer <= 0)
            stateMachine.ChangeState(enemy.WalkState);
    }

    public override void Exit()
    {
        base.Exit();

        agent.isStopped = false;
    }
}
