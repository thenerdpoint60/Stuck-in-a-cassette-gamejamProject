using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform target;
    public float speed=0.125f;
    public Vector3 offset;
    public float maxX;
    public float minX;

    private void Start()
    {
        if (minX == 0)
        {
            minX = -Mathf.Infinity;
        }
        if (maxX == 0)
        {
            maxX = Mathf.Infinity;
        }
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed);
        if (maxX != 0)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        }
        
        transform.position = smoothedPosition;
    }
}
