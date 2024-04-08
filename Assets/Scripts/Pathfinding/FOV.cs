using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    [SerializeField] float _radius;
    [SerializeField] float _angle;
    [SerializeField] LayerMask _wallsLayer;

    public bool IsInFieldOfView(Vector3 target)
    {
        Vector3 dir = target - transform.position;

        if (dir.magnitude > _radius)
            return false;

        if (Vector3.Angle(transform.forward, dir) > _angle / 2)
            return false;

        if (!IsInLineOfSight(target)) return false;
            return true;
    }

    public bool IsInLineOfSight(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        return !Physics.Raycast(transform.position, dir, dir.magnitude, _wallsLayer);
    }

    public bool IsInLineOfSight(Vector3 target, float radius)
    {
        Vector3 dir = target - transform.position;
        return !Physics.Raycast(transform.position, dir, radius, _wallsLayer);
    }
}