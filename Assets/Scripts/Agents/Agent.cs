using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] FOV _fov;
    Pathfinding _pf;
    [SerializeField] StateMachine _sm;

    PFState _backToPatrolState;
    PFState _alertState;
    Patrolling _patrolling;
    Chase _chase;
    [SerializeField] Node[] _waypointsNodes;
    [SerializeField] LayerMask _nodesLayer;

    [SerializeField] Rigidbody _rb;
    [SerializeField] float _speed;
    [SerializeField] Transform _playerTransform;
    [SerializeField] Agent[] _agents;
    Vector3 _aux;

    private void Start()
    {
        _sm = new StateMachine();
        _pf = new Pathfinding();

        Vector3[] _waypoints = new Vector3[_waypointsNodes.Length];

        for (int i = 0; i < _waypointsNodes.Length; i++)
        {
            _waypoints[i] = _waypointsNodes[i].transform.position;
        }

        _patrolling = new Patrolling(transform, _sm, _fov, _waypoints, _playerTransform);
        _backToPatrolState = new PFState(transform, _sm, _fov, _pf, _waypointsNodes, _waypointsNodes[0], _nodesLayer, _playerTransform);
        _chase = new Chase(transform, _sm, _fov, _playerTransform, _nodesLayer, _agents);
        _alertState = new PFState(transform, _sm, _fov, _pf, _waypointsNodes, default, _nodesLayer, _playerTransform);

        _sm.AddState(StateMachine.States.Patrolling, _patrolling);
        _sm.AddState(StateMachine.States.Pathfinding, _backToPatrolState);
        _sm.AddState(StateMachine.States.Chasing, _chase);
        _sm.AddState(StateMachine.States.Alerted, _alertState);

        _sm.ChangeState(StateMachine.States.Patrolling);
    }

    public void Alert(Node node)
    {
        _alertState.ChangeTarget(node);
        _sm.ChangeState(StateMachine.States.Alerted);
    }

    private void Update()
    {
        _aux = _sm.CurrentState.GetVector().Invoke();
        transform.forward = _aux.magnitude > 0 ? _aux : Vector3.forward;
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position += _aux * (_speed * Time.fixedDeltaTime));
    }
}
