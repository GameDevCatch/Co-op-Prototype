using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{

    public LayerMask WhatIsIntersectable;
    public LineRenderer lr;

    public float minLength, maxLength;
    public int maxBounces;

    public float length;

    private void Update()
    {
        RaycastHit hit;
        List<Ray> raysFired = new();
        int timesRayBounced = 0;

        lr.positionCount = 2;

        raysFired.Add(new Ray(transform.position, transform.up.normalized * length));
        Debug.DrawRay(transform.position, transform.up.normalized * length, Color.red);

        while (timesRayBounced < maxBounces)
        {
            if (Physics.Raycast(raysFired[raysFired.Count - 1], out hit, length, WhatIsIntersectable))
            {
                raysFired.Add(new Ray(hit.point, hit.normal.normalized * length));
                Debug.DrawRay(hit.point, hit.normal.normalized * length, Color.red);
                timesRayBounced++;
            }
            else
                break;
        }

        lr.positionCount = raysFired.Count + 1;

        lr.SetPosition(0, transform.position);

        for (int i = 0; i < raysFired.Count; i++)
        {
            if (raysFired.Count - 1 == i)
            {
                lr.SetPosition(i + 1, new Vector3(raysFired[i].origin.x + raysFired[i].direction.x * length,
                                  raysFired[i].origin.y + raysFired[i].direction.y * length, transform.position.z));
            }
            else
                lr.SetPosition(i + 1, raysFired[i + 1].origin);
        }
    }
}