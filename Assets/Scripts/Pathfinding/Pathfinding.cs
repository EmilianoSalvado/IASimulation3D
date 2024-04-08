using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public List<Vector3> ASterisk(Node start, Node end)
    {
        List<Vector3> path = new List<Vector3>();
        if(start == null || end == null)
            return path;

        PriorityQueue<Node> pq = new PriorityQueue<Node>();
        pq.Enqueue(start, 0);
        Dictionary<Node, Node> currentPrevious = new Dictionary<Node, Node>();
        currentPrevious.Add(start, null);
        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(start, 0);

        Node current = default;
        while (pq.Count > 0)
        {
            current = pq.Dequeue();

            if (current == end)
                break;

            foreach (var next in current.Neighbours)
            {
                int newCost = costSoFar[current];
                if (!costSoFar.ContainsKey(next))
                {
                    pq.Enqueue(next, newCost + Heuristic(next.transform.position, end.transform.position));
                    currentPrevious.Add(next, current);
                    costSoFar.Add(next, newCost);
                }
                else if (newCost < costSoFar[next])
                {
                    pq.Enqueue(next, newCost + Heuristic(next.transform.position, end.transform.position));
                    currentPrevious[next] = current;
                    costSoFar[next] = newCost;
                }
            }
        }

        if (current != end) return path;

        while (current != start)
        {
            path.Add(current.transform.position);
            current = currentPrevious[current];
        }

        return path;
    }

    float Heuristic(Vector3 vA, Vector3 vB)
    {
        return (vA - vB).sqrMagnitude;
    }
}
