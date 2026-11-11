using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyController enemy, StateMachine stateMachine, string boolHash) : base(enemy, stateMachine, boolHash)
    {
    }

    public override void Enter()
    {
        base.Enter();

        agent.speed = enemy.ChaseSpeed;
        agent.updateRotation = false;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.Player.PlayerHealth.IsDeath)
            stateMachine.ChangeState(enemy.IdleState);

        //enemy.transform.LookAt(targetTransform);
        enemy.HandleLookToPlayer();
        agent.SetDestination(targetTransform.position);
        if (enemy.DistanceToPlayer < enemy.AttackDistance)
            stateMachine.ChangeState(enemy.AttackState);

        if (enemy.DistanceToPlayer > enemy.ChaseDistance)
            stateMachine.ChangeState(enemy.WalkState);
    }

    public override void Exit()
    {
        base.Exit();

        agent.updateRotation = true;
    }
}
