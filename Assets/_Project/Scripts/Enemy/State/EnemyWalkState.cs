using UnityEngine;

public class EnemyWalkState : EnemyState
{
    

    public EnemyWalkState(Enemy enemy, StateMachine stateMachine, string boolHash) : base(enemy, stateMachine, boolHash)
    {
    }

    public override void Enter()
    {
        base.Enter();

        agent.speed = enemy.WalkSpeed;
    }

    public override void Update()
    {
        base.Update();

        enemy.Agent.SetDestination(targetTransform.position);
        if (enemy.DistanceToPlayer < enemy.ChaseDistance)
            stateMachine.ChangeState(enemy.ChaseState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
