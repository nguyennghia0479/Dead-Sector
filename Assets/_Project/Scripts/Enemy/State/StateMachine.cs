using UnityEngine;

public class StateMachine
{
    private EnemyState currentState;
    private bool canChangeState = true;

    public void Initialized(EnemyState state)
    {
        currentState = state;
        currentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        if (!canChangeState) return;

        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void CurrentStateUpdate()
    {
        currentState.Update();
    }

    public void StopStateMachine() => canChangeState = false;
}
