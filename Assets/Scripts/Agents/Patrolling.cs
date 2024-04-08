using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling : State
{
    Vector3[] _waypoints;
    public int Count { get { return _count; } }
    Transform _playerTransform;

    public Patrolling(Transform t, StateMachine sm, FOV fov, Vector3[] waypoints, Transform pt) : base (t, sm, fov)
    {
        transform = t;
        _sm = sm;
        _fov = fov;
        _waypoints = waypoints;
        _playerTransform = pt;
    }

    public override Func<Vector3> GetVector()
    {
        return Patrol;
    }

    Vector3 Patrol()
    {
        if (_fov.IsInLineOfSight(_playerTransform.position))
            _sm.ChangeState(StateMachine.States.Chasing, _count);
        if (!_fov.IsInLineOfSight(_waypoints[_count]))
            _sm.ChangeState(StateMachine.States.Pathfinding, _count);

        if (Vector3.SqrMagnitude(_waypoints[_count] - transform.position) < .3)
            _count++;
        if (_count >= _waypoints.Length)
            _count = 0;

        return (_waypoints[_count] - transform.position).normalized;
    }
}
