using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterEnemy : EnemyManager
{

    public float reloadTime;
    public float moveSpeed;
    public float playerRange;
    public Transform groundDetect;
    public float rayDist;
    public GameObject Muzzle;
    public string beep;
    public float beepWaitTime;


    private AudioManger audio;
    float stopRange;
    GameObject playerPosiiton;
    Vector3 startPosition;
    float Rtime;
    GameManager manager;
    float beepWait=0;



    private void Start()
    {
        audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManger>();
        playerPosiiton = GameObject.FindGameObjectWithTag("Player");
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        SetTheTImeAgain();
        stopRange = 2f;
        startPosition = transform.position;
        Rtime = reloadTime;
    }

    private void Update()
    {
        RaycastHit2D groundCheck = Physics2D.Raycast(groundDetect.position, Vector2.down, rayDist);
        float howFarIsPlayer = Vector3.Distance(playerPosiiton.transform.position, gameObject.transform.position);
        //Debug.Log("Distance between us :" + howFarIsPlayer);
        if(howFarIsPlayer<playerRange )
        {
            beepWait += Time.deltaTime;
            if (beepWaitTime <= beepWait)
            {
                beepWait = 0;
                audio.Play(beep, gameObject);
            }
            if(groundCheck.collider == false && howFarIsPlayer > stopRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerPosiiton.transform.position, moveSpeed * Time.deltaTime * manager.timeSpeed);
                TakeDamage(0.02f);
            }
            else if(howFarIsPlayer < stopRange)
            {
                if (manager.timeSpeed != 0)
                {
                    shootBullet();
                }
               
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + RotateAsPerPlayer(), moveSpeed * Time.deltaTime * manager.timeSpeed);
            }
            //shootPlayer
            
        }
        else 
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed*Time.deltaTime*manager.timeSpeed);
            SetTheTImeAgain();
        }
    
    
    
    }


    Vector3 RotateAsPerPlayer()
    {
        Vector3 poss= new Vector3();
        if (playerPosiiton.transform.position.x > gameObject.transform.position.x)
        {
            poss = new Vector3(2, 0, 0);
        }
        else if (playerPosiiton.transform.position.x < gameObject.transform.position.x)
        {
            poss = new Vector3(-2, 0, 0);
        }
        return poss;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }

    void shootBullet()
    {
        if (manager.timeSpeed != 0)
        {
            reloadTime -= Time.deltaTime;
            if (reloadTime < 0)
            {
                //Shoot Bullets
                GameObject _bullet = Instantiate(Bullet, Muzzle.transform.position, Quaternion.identity);
                Destroy(_bullet, 5f);
                Debug.Log("Shoot Bullet");
                reloadTime = Rtime;
            }
        }

    }

}
