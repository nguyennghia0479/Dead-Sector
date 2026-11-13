using UnityEngine;

public class EnemyWalkState : EnemyState
{
    public EnemyWalkState(EnemyController enemy, StateMachine stateMachine, string boolHash) : base(enemy, stateMachine, boolHash)
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

        if (enemy.Player.PlayerHealth.IsDeath)
            stateMachine.ChangeState(enemy.IdleState);

        enemy.Agent.SetDestination(targetTransform.position);
        if (enemy.DistanceToPlayer < enemy.ChaseDistance)
            stateMachine.ChangeState(enemy.ChaseState);

        if (!enemy.EnemyAudio.IsPlaying() && sfxTimer <= 0)
        {
            sfxTimer = enemy.EnemyAudio.GetDelayTimeWithClipLength(true);
            enemy.EnemyAudio.PlayGrowlSFX();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
