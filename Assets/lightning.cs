using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightning : MonoBehaviour
{
    GameObject player;
    Vector3 pos;
   // Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pos.y -= 10f * Time.deltaTime;
        if (pos.y <= player.transform.position.y - 1)
        {
            Destroy(gameObject);
        }
            transform.position = pos;
    }
}
