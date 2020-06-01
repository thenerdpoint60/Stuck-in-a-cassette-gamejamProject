using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.UI;

public class Mech : MonoBehaviour
{
    public float jumpDamage=10;
    GameObject player;
    Animator anim;
    Vector3 pos;
    public float vSpeed=1;
    public float jumpSpeed = 1;
    bool jumping;
    public bool jumped;
    float pause;
    Vector3 playerPos;
    public float minTime;
    public float maxTime;
    bool longJumpB;
    bool shortJumpB;
    public GameObject gun;
    public Transform shootingPoint;
    public float fireRate = 0.1f;
    public GameObject bulletPrefab;
    public float rotationSpeed = 1f;
    float reloadTime = 0;
    public string shootSound;
    public float maxHealth=600;
    float health;
    public Image lifebar;
    public GameObject explosion;

    float time;
    //GameManager gm;
   
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        //gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        pos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        doRandomAttack();
    }

    // Update is called once per frame
    void Update()
    {
        RotateAsPerPlayer();
        handleJump();
        
       
    }

    void shortJump()
    {
        playerPos = player.transform.position;
        anim.SetBool("jumpLow", true);
        jumping = true;
        jumped = false;
        shortJumpB = true;
   }

    void RotateAsPerPlayer()
    {
        if (player.transform.position.x+1 < gameObject.transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (player.transform.position.x-1 >= gameObject.transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
    }

    void longJump()
    {
        anim.SetBool("jumpHigh", true);
        jumping = true;
        jumped = false;
        longJumpB = true;
        shortJumpB = false;
        time = 0;
    }

    void handleJump()
    {
        if (jumping)
        {
            if (transform.position.y > 8)
            {
                if (shortJumpB)
                {
                    jumped = true;
                }
                else
                {
                    Vector3 direction = player.transform.position - gun.transform.position;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    if (transform.rotation.y != 0)
                    {
                        rotation *= Quaternion.Euler(0, 0, 180);
                    }
                    rotation.x = gun.transform.rotation.x;
                    rotation.y = gun.transform.rotation.y;
                    //gun.transform.rotation = rotation;
                    Quaternion gunRot = Quaternion.Lerp(gun.transform.rotation, rotation, rotationSpeed*Time.deltaTime);
                    gunRot.x = 0;
                    gunRot.y = 0;

                    gun.transform.rotation = gunRot;
                    time += Time.deltaTime;
                    reloadTime += Time.deltaTime;
                    if (reloadTime >= fireRate)
                    {
                        reloadTime = 0;
                        
                        GameObject bullet =Instantiate(bulletPrefab, shootingPoint.position, gun.transform.rotation);
                        //GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManger>().Play(shootSound, );

                    }
                    if (time >= 4f)
                    {
                        playerPos = player.transform.position;
                        jumped = true;
                    }
                }
                

            }
            if (transform.position.y >= 2.95)
            {
                if (jumped)
                {
                    if (playerPos.x < gameObject.transform.position.x)
                    {

                        pos.x -= Time.deltaTime * vSpeed;
                    }
                    else if (playerPos.x > gameObject.transform.position.x)
                    {

                        pos.x += Time.deltaTime * vSpeed;
                    }
                }



            }
            else
            {
                if (jumped)
                {
                    GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManger>().Play("Mech fall", gameObject);
                    CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 1f);
                    if (longJumpB)
                    {
                        anim.SetBool("jumpHigh", false);
                    }
                    else
                    {
                        anim.SetBool("jumpLow", false);
                    }
                    longJumpB = false;
                    shortJumpB = false;
                    pause = Random.Range(minTime, maxTime);
                    time = 0;
                    jumping = false;
                    jumped = false;
                }

            }


            if (jumped)
            {
                Quaternion rot = Quaternion.Lerp(gun.transform.rotation, Quaternion.Euler(0,0,0), rotationSpeed * Time.deltaTime);
                rot.x = 0;
                rot.y = 0;
                gun.transform.rotation = rot;
                pos.y -= jumpSpeed * Time.deltaTime;

            }
            else if (transform.position.y < 8)
            {

                pos.y += jumpSpeed * Time.deltaTime;
            }
            transform.position = pos;
        }
        else
        {
            if (transform.rotation.y == 0)
            {
                Quaternion rot = Quaternion.Lerp(gun.transform.rotation, Quaternion.Euler(0, 0, 0), rotationSpeed * Time.deltaTime);
                rot.x = 0;
                rot.y = 0;
                gun.transform.rotation = rot;
            }
            else
            {
                Quaternion rot = Quaternion.Lerp(gun.transform.rotation, Quaternion.Euler(0, 0, 180), rotationSpeed * Time.deltaTime);
                rot.x = 0;
                rot.y = 0;
                gun.transform.rotation = rot;
            }
            
            
            time += Time.deltaTime;
            if (time >= pause)
            {
                doRandomAttack();
            }
        }
    }

    void doRandomAttack()
    {
        int rand = Random.Range(1, 3);
        //Debug.Log("Rand " + rand);
        if (rand == 1)
        {
            shortJump();
        }
        else
        {
            longJump();
        }
    }

    public void dealDamage(float damage)
    {
        health -= damage;
        UpdateSlider();
        Debug.Log("health " + health);
        if (health <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            
        }
    }

    void UpdateSlider()
    {
        lifebar.fillAmount = health / maxHealth;
    }
}
