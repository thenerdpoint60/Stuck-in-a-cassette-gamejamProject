using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private Vector3 startPos;
    public Transform endPos;
    public float speed=1f;
    bool movingNext = true;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingNext)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos.position, speed);
            if (Vector2.Distance(transform.position, endPos.position) <= 0.1)
            {
                movingNext = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed);
            if (Vector2.Distance(transform.position, startPos) <= 0.1)
            {
                movingNext = true;
            }
        }
        
    }
}
