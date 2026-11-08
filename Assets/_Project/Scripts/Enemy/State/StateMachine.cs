using UnityEngine;

public class StateMachine
{
    private EnemyState currentState;

    public void Initialized(EnemyState state)
    {
        currentState = state;
        currentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void CurrentStateUpdate()
    {
        currentState.Update();
    }
}
