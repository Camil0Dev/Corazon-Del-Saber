using System;
using System.Collections.Generic;

public class BossStateMachine
{
    private Dictionary<Type, IBossState> _states = new Dictionary<Type, IBossState>();

    public IBossState CurrentState { get; private set; }

    public void AddState(IBossState state)
    {
        _states[state.GetType()] = state;
    }

    public void ChangeState<T>() where T : class, IBossState
    {
        if (_states.TryGetValue(typeof(T), out IBossState newState))
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }

    public void Update() => CurrentState?.Update();

    public void FixedUpdate() => CurrentState?.FixedUpdate();
}