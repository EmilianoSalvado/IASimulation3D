using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected StateMachine _sm;
    protected FOV _fov;
    protected Transform transform;
    protected int _count;

    public State(Transform t, StateMachine sm, FOV fov) : base()
    {
        transform = t;
        _sm = sm;
        _fov = fov;
    }

    public abstract Func<Vector3> GetVector();

    public virtual void ChangeValue(int value)
    {
        _count = value;
    }
}