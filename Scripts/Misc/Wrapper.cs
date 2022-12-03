using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrapper : MonoBehaviour
{

    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        var positiveXPos = _mainCam.ViewportToWorldPoint(new Vector3(1, 0, 15.52f)).x;
        var negitiveXPos = _mainCam.ViewportToWorldPoint(new Vector3(0, 0, 15.52f)).x;

        var positiveYPos = _mainCam.ViewportToWorldPoint(new Vector3(0, 1, 15.52f)).y;
        var negitiveYPos = _mainCam.ViewportToWorldPoint(new Vector3(0, 0, 15.52f)).y;

        if (transform.position.x > positiveXPos)
            transform.position = new Vector3(negitiveXPos, transform.position.y, transform.position.z);
        else if (transform.position.x < negitiveXPos)
            transform.position = new Vector3(positiveXPos, transform.position.y, transform.position.z);
        else if (transform.position.y > positiveYPos)
            transform.position = new Vector3(transform.position.x, negitiveYPos, transform.position.z);
        else if (transform.position.y < negitiveYPos)
            transform.position = new Vector3(transform.position.x, positiveYPos, transform.position.z);
    }
}