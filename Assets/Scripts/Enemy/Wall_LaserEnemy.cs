using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Wall_LaserEnemy : MonoBehaviour
{
    GameObject playerPosiiton;
    LineRenderer _laser;

    public float detectDistance;
    public float attackDistance;
    public Sprite[] laserSprite;
    public Transform partTorotate;


    void Start()
    {
        playerPosiiton = GameObject.FindGameObjectWithTag("Player");
        gameObject.GetComponent<SpriteRenderer>().sprite = laserSprite[0];
        _laser = GetComponentInChildren<LineRenderer>();
        
    }


    void Update()
    {
        float howFarIsPlayer = Vector3.Distance(playerPosiiton.transform.position, gameObject.transform.position);
        if(howFarIsPlayer<detectDistance)
        {
            _laser.enabled = true;
            //start attacking the player
            gameObject.GetComponent<SpriteRenderer>().sprite = laserSprite[1];
            if(howFarIsPlayer<attackDistance)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = laserSprite[2];

            }
           


            _laser.SetPosition(1, transform.position);
            _laser.SetPosition(0, playerPosiiton.transform.position);
        }
        else
        {
            _laser.enabled = false;
        }

      


    }
}
