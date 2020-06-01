using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy_3 : MonoBehaviour
{
    private int state = 0;
    public float health;
    public float maxHealth=500;
    //float lastHealth = 400;
    public GameObject bulletPrefab;
    public GameObject shootingPoint;
    public float shootingDelay = 0.7f;
    float time = 0;
    public Transform LCar;
    public Transform RCar;
    public Transform swordPos;
    public float SwordMovementSpeed=5;
    bool movingL = true;
    Animator anim;
    public float swordRotationSpeed = 3f;
    Vector3 pos;
    public float carSpeed=1f;
    Vector3 targetPos;
    GameObject player;
    public float swordDelay = 1f;
    public GameObject lightning;
    public float lightningRate = 2f;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        anim = GetComponent<Animator>();
        anim.SetInteger("State", 0);
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0)
        {
            time += Time.deltaTime;
            if (time > shootingDelay)
            {
                GameObject ball= Instantiate(bulletPrefab, shootingPoint.transform.position, Quaternion.identity);
                ball.GetComponent<bouncyBall>().velocity.x = Random.Range(-2, -5);
                time = 0;
            }
        }
        if (state == 1)
        {
            pos.y = LCar.position.y;
            if (movingL)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                pos.x -= carSpeed * Time.deltaTime;
                if (pos.x < LCar.position.x)
                {
                    movingL = false;
                }
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                pos.x += carSpeed * Time.deltaTime;
                if (pos.x > RCar.position.x)
                {
                    movingL = true;
                }
            }
            transform.position = pos;
        }
        if (state == 2)
        {
            if (!movingL)
            {
                targetPos = new Vector3(player.transform.position.x, swordPos.position.y,0);

                Vector3 direction = targetPos - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                rotation *= Quaternion.Euler(0, 0, 90);
                rotation.x = 0;
                rotation.y = 0;
                //gun.transform.rotation = rotation;
                Quaternion Rot = Quaternion.Lerp(transform.rotation, rotation, swordRotationSpeed * Time.deltaTime);
                
                Rot.x = 0;
                Rot.y = 0;

                transform.rotation = Rot;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, SwordMovementSpeed*Time.deltaTime);
                if(Vector2.Distance(transform.position, targetPos) < 0.1f)
                {
                    movingL = true;
                    targetPos = player.transform.position;
                }
            }
            else
            {
                //targetPos = new Vector3(player.transform.position.x, swordPos.position.y, 0);

               
                Quaternion rotation = Quaternion.Euler(0, 0, 180);
                
                //gun.transform.rotation = rotation;
                Quaternion Rot = Quaternion.Lerp(transform.rotation, rotation, swordRotationSpeed * Time.deltaTime);

                Rot.x = 0;
                Rot.y = 0;

                transform.rotation = Rot;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, SwordMovementSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, targetPos) < 0.1f)
                {
                    time += Time.deltaTime;
                    if (time >= swordDelay)
                    {
                        movingL = false;
                        time = 0;
                    }
                   
                    
                }
            }
        }
        if (state == 3)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = new Vector3(player.transform.position.x, swordPos.position.y, 0);
            time += Time.deltaTime;
            if (time >= lightningRate)
            {
                Instantiate(lightning, new Vector3(transform.position.x + Random.Range(-2f, 2f), transform.position.y, 0f), Quaternion.identity);
                time = 0;
            }
        }
        else if (state == 4)
        {
            transform.position = startPos;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            time += Time.deltaTime;
            if (time > shootingDelay)
            {
                GameObject ball = Instantiate(bulletPrefab, shootingPoint.transform.position, Quaternion.identity);
                ball.GetComponent<bouncyBall>().velocity.x = Random.Range(-2, -5);
                time = 0;
            }
        }
        else if (state == 5)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= ((4 - state) * 100)){
            state++;
            time = 0;
            anim.SetInteger("State", state);
            movingL=false;
        }
    }
}
