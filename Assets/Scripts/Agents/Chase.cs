using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : State
{
    Transform _playerTransform;
    bool _hasAlerted;
    LayerMask _nodesLayer;
    Agent[] _agents;

    public Chase(Transform t, StateMachine sm, FOV fov, Transform pt, LayerMask nodesLayer, Agent[] agents) : base(t, sm, fov)
    {
        transform = t;
        _sm = sm;
        _fov = fov;
        _playerTransform = pt;
        _nodesLayer = nodesLayer;
        _agents = agents;
    }

    public override Func<Vector3> GetVector()
    {
        return ChaseTarget;
    }

    Vector3 ChaseTarget()
    {
        if (!_hasAlerted)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, _nodesLayer);
            Node closestNode = colliders[0].GetComponent<Node>();

            for (int i = 1; i < colliders.Length; i++)
            {
                if ((colliders[i].transform.position - transform.position).sqrMagnitude < (closestNode.transform.position - transform.position).sqrMagnitude && _fov.IsInLineOfSight(colliders[i].transform.position))
                    closestNode = colliders[i].GetComponent<Node>();
            }

            foreach (var agent in _agents)
            {
                agent.Alert(closestNode);
            }

            _hasAlerted = true;
        }

        if (!_fov.IsInLineOfSight(_playerTransform.position))
        {
            _hasAlerted = false;
            _sm.ChangeState(StateMachine.States.Patrolling, _count);
        }

        return (_playerTransform.position - transform.position).normalized;
    }
}
