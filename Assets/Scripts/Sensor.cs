using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sensor : MonoBehaviour
{

    public List<int> list = new List<int>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        List<Vector3> hits = new List<Vector3>();
        List<int> newList = new List<int>();

        for (int i = -135; i < 136; i++)
        {
            RaycastHit hit;
            Ray downRay = new Ray(transform.position, Quaternion.Euler(0, i, 0) * transform.forward);

            // Cast a ray straight downwards.
            if (Physics.Raycast(downRay, out hit))
            {
                newList.Add(Math.Min(10000, (int)(hit.distance * 1000)));
                hits.Add(hit.point);
                hits.Add(transform.position);
            }
            else
            {
                newList.Add(10000);
            }
        }

        list = newList;
        lineRenderer.SetPositions(hits.ToArray());

        string s = "[";
        foreach (var l in list)
        {
            s += l + ",";
        }

        s += "]";

        Debug.Log(s);
        // Debug.Log(list.Count);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = -135; i < 136; i++)
        {
            RaycastHit hit;
            Ray downRay = new Ray(transform.position, Quaternion.Euler(0, i, 0) * transform.forward);

            // Cast a ray straight downwards.
            if (Physics.Raycast(downRay, out hit))
            {
                // Gizmos.DrawLine(transform.position, hit.point);
            }
        }
    }
}


