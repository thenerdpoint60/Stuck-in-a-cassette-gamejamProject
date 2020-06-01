using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : EnemyManager
{
    public float speed;
    public float rayDist;
    public Transform groundDetect;
    public bool isGroundDetecting;
    public Transform WallDetect;
    public bool isWallDetecting;
    public int stopDistance;
    public float reloadTime;
    public GameObject Muzzle;
    GameManager manager;


    public string beep;
    public float beepWaitTime;
    float rotatingTime;

    private AudioManger audio;


    private float beepWait;
    bool movingRight;
    bool isMoving;
    Animator animator;
    GameObject playerPosiiton;
    float Rtime;
    private void Start()
    {
        audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManger>();
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        isMoving = false;
        animator = GetComponent<Animator>();
        playerPosiiton = GameObject.FindGameObjectWithTag("Player");
        Rtime = reloadTime;
        SetTheTImeAgain();
        rotatingTime = 0f;
    }


    private void Update()
    {
        if (manager.timeSpeed == 0)
        {
            animator.SetBool("isWalking", false);
            return;
        }

        float howFarIsPlayer = Vector3.Distance(playerPosiiton.transform.position, gameObject.transform.position);
        //Debug.Log("This far is the player :" + howFarIsPlayer);
        if(howFarIsPlayer<stopDistance)
        {
            beepWait += Time.deltaTime;
            if (beepWaitTime <= beepWait)
            {
                beepWait = 0;
                audio.Play(beep, gameObject);
            }
            if (manager.timeSpeed != 0)
            {
                animator.SetBool("isWalking", false);
                isMoving = false;
                Debug.Log("Player is so close");
                TakeDamage(0.01f);
                RotateAsPerPlayer();
                shootBullet();
            }
         
        }
        else
        {
            PatrollingTheRegion();
            SetTheTImeAgain();
        }
        
    }

    void PatrollingTheRegion()
    {
        
        isMoving = true;
        transform.Translate(Vector2.right * speed * Time.deltaTime * manager.timeSpeed);
        animator.SetBool("isWalking", true);
        if (isGroundDetecting)
        {
            RaycastHit2D groundCheck = Physics2D.Raycast(groundDetect.position, Vector2.down, rayDist);
            rotatingTime += Time.deltaTime;
            if ((groundCheck.collider == false || groundCheck.distance>1) && rotatingTime > 1f)
            {
                if (movingRight)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    movingRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingRight = true;
                }
                rotatingTime = 0f;
            }
        }
        if(isWallDetecting)
        {
            RaycastHit2D wallCheck;
            if (movingRight)
            {
               wallCheck = Physics2D.Raycast(WallDetect.position, Vector2.right, rayDist);
            }
            else
            {
                wallCheck = Physics2D.Raycast(WallDetect.position, Vector2.left, rayDist);;
            }
           
            rotatingTime += Time.deltaTime;
            if ((wallCheck.collider == true) && rotatingTime > 2f)
            {
                if (movingRight)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    movingRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingRight = true;
                }
                rotatingTime = 0f;
            }
        }
        
       
    }

    void RotateAsPerPlayer()
    {
        if (playerPosiiton.transform.position.x > gameObject.transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            movingRight = true;
        }
        else if (playerPosiiton.transform.position.x < gameObject.transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            movingRight = false;
        }
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
           
                if (movingRight)
                {
                    _bullet.GetComponent<BulletManager>().MovingDire(1);
                }
                else
                {
                    _bullet.GetComponent<BulletManager>().MovingDire(0);
                }
                Destroy(_bullet, 5f);
                Debug.Log("Shoot Bullet");
                reloadTime = Rtime;
            }
        }
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, rayDist);
    }


}
