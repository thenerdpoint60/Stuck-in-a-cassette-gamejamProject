using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy_1 : MonoBehaviour
{
    public Image timeBar;
    public float currentTimeHealth;
    public float startTimeHealth;
    public GameObject explosionPrefab;
    [System.Serializable]
    public class BossAttacks
    {
        public GameObject Bullet;
        public int coolDownTime;
        public int numberOfBullets;
    }

    float speed;
    bool movingRight;
    Animator animator;
    GameObject playerPosiiton;
    bool isShooting;
    float currentTimer;
    public GameObject[] Muzzle;
    GameManager manager;

    public BossAttacks[] bossAttacks;
    int currentColldownState;




    // Start is called before the first frame update
    void Start()
    {
        currentColldownState = 0;
        isShooting = false;
        currentTimer = 0f;
        speed = 0.5f;
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        playerPosiiton = GameObject.FindGameObjectWithTag("Player");
        currentTimeHealth = startTimeHealth;

    }

    // Update is called once per frame
    void Update()
    {
        currentTimer += Time.deltaTime;
        //Debug.Log("Current Time is :" + currentTimer);
        if (manager.timeSpeed == 0)
        {
            animator.SetBool("isWalking", false);
            return;
        }
        float howFarIsPlayer = Vector3.Distance(playerPosiiton.transform.position, gameObject.transform.position);
        transform.Translate(Vector2.right * speed * Time.deltaTime * manager.timeSpeed);
        RotateAsPerPlayer();
        if(!isShooting)
        {
            if (currentColldownState==bossAttacks.Length)
            {
                currentColldownState = 0;
                return;
            }
            else
            {
                if (bossAttacks.Length - 1 < currentColldownState)
                {
                    return;
                }
                else if (bossAttacks[currentColldownState].coolDownTime < currentTimer)
                {
                    isShooting = true;
                    shootBullet(bossAttacks[currentColldownState].numberOfBullets, bossAttacks[currentColldownState].Bullet);
                    currentColldownState += 1;
                    currentTimer = 0f;
                }
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

    void shootBullet(int bulletsNumber,GameObject bulletPrefab)
    {
        if (manager.timeSpeed != 0)
        {
                //Shoot Bullets
                for (int i = 0; i < bulletsNumber; i++)
                {
                    int a = Random.Range(0,5);
                    GameObject _bullet = Instantiate(bulletPrefab, Muzzle[a].transform.position, Quaternion.identity);
                    Destroy(_bullet, 5f);
                    Debug.Log("Shoot Bullet");
                }
        }
        isShooting = false;

    }


    public void TakeDamage(float damage)
    {
        //Debug.Log(damage);

        currentTimeHealth -= damage;
        timeBar.fillAmount = currentTimeHealth / startTimeHealth;
        if (currentTimeHealth <= 0)
        {
            transform.position = new Vector3(110.97f, -6.26f, 0);
            transform.rotation = Quaternion.Euler(0, 0, 90);
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<Level1_dialog>().bossIsDead = true;
            timeBar.GetComponentInParent<Image>().gameObject.SetActive(false);
            enabled = false;

            //You have died

            //GameObject explosionObj = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            //Destroy(explosionObj, 0.5f);
            //Destroy(this.gameObject, 5f);

            //gameObject.SetActive(false);
        }
        //Debug.Log("Health left " + currentTimeHealth);
    }

}
