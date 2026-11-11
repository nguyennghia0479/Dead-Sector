using UnityEngine;
using UnityEngine.AI;

public class EnemyState
{
    protected EnemyController enemy;
    protected StateMachine stateMachine;
    protected string boolHash;
    protected Animator animator;
    protected NavMeshAgent agent;
    protected Transform targetTransform;
    protected float stateTimer;
    protected bool triggerCalled;

    public EnemyState(EnemyController enemy, StateMachine stateMachine,  string boolHash)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.boolHash = boolHash;

        animator = enemy.Animator;
        agent = enemy.Agent;
        targetTransform = enemy.Player.transform;
    }

    public virtual void Enter()
    {
        animator.SetBool(boolHash, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        animator.SetBool(boolHash, false);
    }

    public void OnTriggerCalled() => triggerCalled = true;
}
