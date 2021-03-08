using System;
using UnityEngine;

public class UnitPath : MonoBehaviour
{
    [SerializeField] float stopRadius = 0.005f;

    private Vector3[] nodes;
    private float[] distances;
    private float maxDist;

    public void Init(Vector3[] pathNodes)
    {
        nodes = new Vector3[pathNodes.Length];
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = pathNodes[i];
        }
        
        distances = new float[nodes.Length];
        distances[0] = 0;
        for (int i = 0; i < nodes.Length - 1; i++)
        {
            distances[i + 1] = distances[i] + Vector3.Distance(nodes[i], nodes[i + 1]);
        }

        maxDist = distances[distances.Length - 1];
    }

    public float GetParam(Vector3 position)
    {
        var closestSegment = GetClosestSegment(position);
        var param = this.distances[closestSegment] + GetParamForSegment(position,nodes[closestSegment],nodes[closestSegment+1]);
        return param;
    }

    public bool IsAtEndOfPath(Vector3 position,float param,out Vector3 finalDestination)
    {
        bool result;

        finalDestination = nodes[nodes.Length - 1];

        if (param >= this.distances[nodes.Length - 2])
        {
            var distance = Vector3.Distance(position, finalDestination); 
            result =  distance< stopRadius;
        }
        else
        {
            result = false;

        }

        return result;
    }

    public Vector3 GetPosition(float param)
    {
        if (param < 0)
        {
            param = 0;
        }
        else if (param > maxDist)
        {
            param = maxDist;
        }

        int i = 0;
        for (; i < distances.Length; i++)
        {
            if (distances[i] > param)
            {
                break;
            }
        }


        if (i > distances.Length - 2)
        {
            i = distances.Length - 2;
        }
        else
        {
            i -= 1;
        }

        float t = (param - distances[i]) / Vector3.Distance(nodes[i], nodes[i + 1]);

        return Vector3.Lerp(nodes[i], nodes[i + 1], t);
    }

    public void Reverse()
    {
        Array.Reverse(nodes);
    }
    
    private int GetClosestSegment(Vector3 position)
    {
        var closestDist = DistToSegment(position,nodes[0],nodes[1]);
        var closestSegment = 0;

        for (int i = 1; i < nodes.Length - 1; i++)
        {
            float dist = DistToSegment(position, nodes[i], nodes[i + 1]);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestSegment = i;
            }
        }

        return closestSegment;
    }

    private float GetParamForSegment(Vector3 p, Vector3 v, Vector3 w)
    {
        Vector3 vw = w - v;

        float l2 = Vector3.Dot(vw, vw);
        if (l2 == 0)
        {
            return 0;
        }

        float t = Vector3.Dot(p - v, vw) / l2;

        if (t < 0)
        {
            t = 0;
        }
        else if (t > 1)
        {
            t = 1;
        }

        return t * (v - w).magnitude;
    }

    private Vector3 GetClosestPointForSegment(Vector3 p,Vector3 v ,Vector3 w)
    {
        Vector3 vw = w - v;
        float l2 = Vector3.Dot(vw, vw);
        
        if (l2 == 0)
        {
            return v;
        }
        
        float t = Vector3.Dot(p - v, vw) / l2;

        if (t < 0)
        {
            return v;
        }
        else if (t > 1)
        {
            return w;
        }

        return Vector3.Lerp(v, w, t);
    }

    private float DistToSegment(Vector3 p,Vector3 v,Vector3 w)
    {
        Vector3 vw = w - v;
        float l2 = Vector3.Dot(vw, vw);
        if (l2 == 0)
        {
            return Vector3.Distance(p, v);
        }

        float t = Vector3.Dot(p - v, vw) / l2;

        if (t < 0)
        {
            return Vector3.Distance(p, v);
        }

        if (t > 1)
        {
            return Vector3.Distance(p, w);
        }

        Vector3 closestPoint = Vector3.Lerp(v, w, t);
        return Vector3.Distance(p, closestPoint);
    }
}
