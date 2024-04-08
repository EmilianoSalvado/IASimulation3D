using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    Dictionary<T, float> _queue = new Dictionary<T, float>();

    public int Count { get { return _queue.Count; } }

    public void Enqueue(T elem, float value)
    {
        if (!_queue.ContainsKey(elem))
            _queue.Add(elem, value);
    }

    public T Dequeue()
    {
        if (_queue.Count == 0) return default;

        T elem = default;

        foreach (var item in _queue)
            elem = elem == null ? item.Key : _queue[elem] > item.Value ? item.Key : elem;

        _queue.Remove(elem);
        return elem;
    }
}
