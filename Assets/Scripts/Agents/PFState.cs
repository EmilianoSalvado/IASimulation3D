using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFState : State
{
    List<Vector3> _path;
    bool _pathCalculated = false;
    Node _waypoint;
    Pathfinding _pf;
    LayerMask _nodesLayer;
    Transform _playerTransform;
    Node[] _array;

    public PFState(Transform t, StateMachine sm, FOV fov, Pathfinding pf, Node[] array, Node waypoint, LayerMask mask, Transform pt) : base(t, sm, fov)
    {
        transform = t;
        _sm = sm;
        _fov = fov;
        _array = array;
        _waypoint = waypoint;
        _pf = pf;
        _nodesLayer = mask;
        _playerTransform = pt;
    }

    public override Func<Vector3> GetVector()
    {
        return GoToNextNode;
    }

    public void ChangeTarget(int i)
    {
        _waypoint = _array[i];
    }

    public void ChangeTarget(Node node)
    {
        _waypoint = node;
    }

    Vector3 GoToNextNode()
    {
        if (!_pathCalculated)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, _nodesLayer);
            Node startNode = colliders[0].GetComponent<Node>();

            for (int i = 1; i < colliders.Length; i++)
            {
                if ((colliders[i].transform.position - transform.position).sqrMagnitude < (startNode.transform.position - transform.position).sqrMagnitude && _fov.IsInLineOfSight(colliders[i].transform.position))
                    startNode = colliders[i].GetComponent<Node>();
            }

            _path = _pf.ASterisk(startNode, _waypoint);

            _pathCalculated = true;
        }

        if (_path.Count < 1)
            return Vector3.zero;

        if (Vector3.SqrMagnitude(_path[_path.Count - 1] - transform.position) < .25f)
            _path.RemoveAt(_path.Count - 1);

        if (_path.Count < 1)
        {
            _pathCalculated = false;
            _sm.ChangeState(StateMachine.States.Patrolling);
            return Vector3.zero;
        }

        if (_fov.IsInFieldOfView(_playerTransform.position))
        {
            _pathCalculated = false;
            _sm.ChangeState(StateMachine.States.Chasing);
        }

        return (_path[_path.Count - 1] - transform.position).normalized;
    }

    public override void ChangeValue(int value)
    {
        base.ChangeValue(value);
        ChangeTarget(value);
    }
}