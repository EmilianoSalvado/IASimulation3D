using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public enum States { Patrolling, Chasing, Pathfinding, Alerted}

    Dictionary<States, State> _states = new Dictionary<States, State>();
    State _currentState;
    public State CurrentState { get { return _currentState; } }

    public void AddState(States k, State v)
    {
        if (!_states.ContainsKey(k))
            _states.Add(k, v);
    }

    public void ChangeState(States k)
    {
        if (_states.ContainsKey(k))
            _currentState = _states[k];
    }

    public void ChangeState(States k, int i)
    {
        if (_states.ContainsKey(k))
            _currentState = _states[k];
        _currentState.ChangeValue(i);
    }
}