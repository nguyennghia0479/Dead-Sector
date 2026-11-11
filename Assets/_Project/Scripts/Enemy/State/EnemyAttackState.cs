using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(EnemyController enemy, StateMachine stateMachine, string boolHash) : base(enemy, stateMachine, boolHash)
    {
    }

    public override void Enter()
    {
        base.Enter();

        agent.updateRotation = false;
        agent.isStopped = true;
        agent.ResetPath();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.Player.PlayerHealth.IsDeath)
            stateMachine.ChangeState(enemy.IdleState);

        enemy.HandleLookToPlayer();
        if (enemy.DistanceToPlayer > enemy.AttackDistance)
            stateMachine.ChangeState(enemy.ChaseState);
    }

    public override void Exit()
    {
        base.Exit();

        agent.updateRotation = true;
        agent.isStopped = false;
    }
}
