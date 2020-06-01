using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public int damage;
    public float speed;
    public bool shootingType, DroppingType;

    bool isMovingRight;
    
    GameManager manager;
    GameObject player;
    Vector3 whereIsPlayer;
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //Debug.Log("SHootingType+" + shootingType+ "   Moving RIght :"+isMovingRight);
        player = GameObject.FindGameObjectWithTag("Player");
        whereIsPlayer = player.transform.position;
    }

    private void Update()
    {
        if(shootingType)
        {
            if (isMovingRight)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime * manager.timeSpeed);
            }
            else
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime * manager.timeSpeed);
            }
        }
        else if (DroppingType)
        {
            transform.position = Vector3.MoveTowards(transform.position, whereIsPlayer-new Vector3(1,1,0), speed * Time.deltaTime);
        }
       
        
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Reduce the overall time
            Debug.Log("Player took a hit");
            manager.timeLeft -= damage;
            Destroy(this.gameObject);
        }
        if (collision.transform.tag == "Tilemap")
        { 
                    Destroy(this.gameObject);
        }
    }

    public void MovingDire(int state)
    {
        if(state==1)
        {
            isMovingRight = true;
        }
        else
        {
            isMovingRight = false;
        }
    }



    //public void SetTheBulletType(int state)
    //{
    //    if(state==1)
    //    {
    //        shootingType = true;
    //    }
    //    else
    //    {
    //        DroppingType = true;
    //    }
    //}



}
