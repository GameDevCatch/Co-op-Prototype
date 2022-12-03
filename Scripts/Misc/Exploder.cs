using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{

    [SerializeField]
    private float force;

    public void Trigger(Color parentColor)
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().material.color = parentColor;
            child.GetComponent<Rigidbody>().AddForce(Random.insideUnitCircle.normalized * force);
        }
    }
}