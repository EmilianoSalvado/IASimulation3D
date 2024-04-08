using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] float _detectionRadius;
    [SerializeField] LayerMask _nodesLayer;
    [SerializeField] FOV _fov;

    [SerializeField] List<Node> _neighbours = new List<Node>();
    public List<Node> Neighbours { get { return _neighbours; } }

    private void Start()
    {
        GetNeighbours();
    }

    public void GetNeighbours()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _detectionRadius, _nodesLayer);

        foreach (Collider collider in colliders)
        {
            if (!_fov.IsInLineOfSight(collider.transform.position))
                continue;

            if (collider.TryGetComponent<Node>(out var node))
            {
                if (node != this)
                    _neighbours.Add(node);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
