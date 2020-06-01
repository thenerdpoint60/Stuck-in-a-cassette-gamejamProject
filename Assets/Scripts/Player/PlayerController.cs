using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public event EventHandler backupPos;
    public event EventHandler rewindPos;

    public GameObject fire;
    SpriteRenderer rend;
    //Color fireColor;
    int collisionCount = 0;
    public float speed;
    public float jump;
    float moveVelocity;
    public GameObject currWeapon;
    private int bulletsLeft;
    private int maxBullets;
    private float fireRate;
    private float reloadTime;
    private float bulletSpread;
    private float bulletDamage;
    private GameObject bulletPrefab;
    private GameObject weaponTarget;
    private float bulletExplosionRadius;
    private bool bulletShow;
    GameObject currBullet;
    LineRenderer lineRend;
    GameObject lastEnemy;
    float timeShooting;
    public bool hasDied = false;
    public float laserDamage=60f;
    bool canJump = true;

    private weaponInfo info;
    private GameObject shootingPoint;
    private float shootingCooldown = 0;
    private string shotAudio;
    GameObject bullet;
    bool usedAction = false;
    Vector3 characterScale;
    public GameObject walljumpPoint;
    public float walljumpMultiplier=0.7f;
    public char lastWallJump = 'n';
    public bool canWallJump = false;

    private Animator anim;
    private float lastY;
    public GameObject raycastPoint;
    RaycastHit2D targetHit;
    private float baseScale;
    private BoxCollider2D gunCollider;
    private float powerTime = 0;
    private float time = 0;
    private Vector2 lastPos;
    private float lastTime;
    private bool hasSave = false;
    private GameObject lastGun;
    public TMPro.TextMeshProUGUI ammoText;
    public TMPro.TextMeshProUGUI instReloadText;
    public int instReloadsCount = 0;
    RaycastHit2D wallHit;
    bool end = false;
    public GameObject restartPanel;

    public TMPro.TextMeshProUGUI tutorialText;

    public GameObject weaponPoint;
    public Vector3 weaponOffsetFromPoint;
    public Quaternion weaponRot;
    private GameManager manager;
    private AudioManger audio;
    bool endGame;


    bool isGrounded = true;

    private void Start()
    {
        audio= GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManger>();
        rend = fire.GetComponent<SpriteRenderer>();
        UpdateInstReloadBar();

        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        characterScale = transform.lossyScale;
        baseScale = transform.lossyScale.x;
        lastY = transform.position.y;
        anim = GetComponentInChildren<Animator>();
        if (currWeapon != null)
        {
            OnPickupWeapon();
        }
        else
        {
            ammoText.text = "";
        }

    }

    void Update()
    {
        if (end)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }

        if (manager.timeLeft <= 0)
        {
            if (!hasSave)
            {
                restartPanel.SetActive(true);
                anim.SetBool("IsDead", true);
                manager.timeSpeed = 0;
                manager.timeLeft = 0;
                ammoText.text = "Game over";
                tutorialText.text = "Press R to restart";
                tutorialText.enabled = true;
                instReloadText.text = "";
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

                return;
            }
            else
            {
                powerTime = 0;
                hasDied = true;
                audio.Play("Time warp rewind", gameObject);
                fire.SetActive(false);
                currWeapon = lastGun;
                OnPickupWeapon();
                rewindPos?.Invoke(this, EventArgs.Empty);
                manager.timeSpeed = 1;
                manager.timeLeft = lastTime;
                transform.position = lastPos;
                hasSave = false;
            }
        }

        if (time < powerTime)
        {
            time += Time.deltaTime;
        } else if (time >= powerTime)
        {
            fire.SetActive(false);
            time = 0;
            powerTime = 0;
                manager.timeSpeed = 1;
            
            hasSave = false;
        }

        shootingCooldown += Time.deltaTime;

       

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canJump)
            {
                if (isGrounded)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jump);
                    isGrounded = false;
                }
                else if (canWallJump)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jump * walljumpMultiplier);
                    canWallJump = false;
                }
            }
        }


        moveVelocity = 0;

        //characterScale = transform.localScale;
        Vector3 weaponScale = transform.localScale;

        //Left Right Movement
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
        {
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                moveVelocity = -speed;
                anim.SetBool("IsRunning", true);
            }
            else
            {
                anim.SetBool("IsRunning", false);
            }

            characterScale.x = -baseScale;
            if (currWeapon != null)
            {
                weaponRot = Quaternion.Euler(0, 0, -45);
            }

        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                moveVelocity = -speed;
                anim.SetBool("IsRunning", true);
            }
            else
            {
                anim.SetBool("IsRunning", false);
            }
            characterScale.x = -baseScale;
            if (currWeapon != null)
            {
                weaponRot = Quaternion.Euler(0, 0, 0);
            }
        }


        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                moveVelocity = speed;
                anim.SetBool("IsRunning", true);
            }
            else
            {
                anim.SetBool("IsRunning", false);
            }
            characterScale.x = baseScale;
            if (currWeapon != null)
            {
                weaponRot = Quaternion.Euler(0, 0, 45);
            }

        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                moveVelocity = speed;
                anim.SetBool("IsRunning", true);
            }
            else
            {
                anim.SetBool("IsRunning", false);
            }

            characterScale.x = baseScale;
            if (currWeapon != null)
            {
                weaponRot = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (Input.GetKey(KeyCode.W))
        {

            characterScale.x = baseScale;
            if (currWeapon != null)
            {
                weaponRot = Quaternion.Euler(0, 0, 90);
            }
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }



        transform.localScale = characterScale;

        //tutorialText.transform.localScale = new Vector3(characterScale.x * 0.1f, characterScale.y * 0.1f, characterScale.z * 0.1f);

        if (currWeapon != null)
        {
            currWeapon.transform.localScale = new Vector3(characterScale.x * 0.7f, characterScale.y * 0.7f, characterScale.z * 0.7f);
        }
        if (currWeapon != null)
        {
            currWeapon.transform.rotation = weaponRot;
        }


        GetComponent<Rigidbody2D>().velocity = new Vector2(moveVelocity, GetComponent<Rigidbody2D>().velocity.y);

        if (currWeapon != null)
        {
            currWeapon.transform.position = weaponPoint.transform.position + weaponOffsetFromPoint;
        }


        if (Input.GetKey(KeyCode.Return))
        {
            if (currWeapon != null)
            {
                if (bulletsLeft > 0)
                {
                    if (bulletPrefab == null)
                    {
                        if (characterScale.x > 0)
                        {
                            targetHit = Physics2D.Raycast(shootingPoint.transform.position, currWeapon.transform.right);
                        }
                        else
                        {
                            targetHit = Physics2D.Raycast(shootingPoint.transform.position, -currWeapon.transform.right);
                        }
                        if (targetHit.collider != null)
                        {
                            lineRend.SetPosition(0, new Vector3(shootingPoint.transform.position.x, shootingPoint.transform.position.y, -1f));
                            lineRend.SetPosition(1, new Vector3(targetHit.point.x, targetHit.point.y, -1f));
                            lineRend.enabled = true;
                        }
                        else
                        {
                            lineRend.enabled = false;
                        }
                    }

                    if (shootingCooldown >= fireRate)
                    {
                        if (bulletPrefab != null)
                        {
                            if (weaponTarget == null)
                            {
                                if (!bulletShow)
                                {
                                    float spread = UnityEngine.Random.Range(-bulletSpread, bulletSpread);

                                    bulletsLeft--;
                                    shootingCooldown = 0;
                                    if (characterScale.x < 0)
                                    {
                                        bullet = Instantiate(bulletPrefab, shootingPoint.transform.position, weaponRot * Quaternion.Euler(0, 0, 180 + spread));
                                        bullet.GetComponent<Bullet>().damage = bulletDamage;
                                        bullet.GetComponent<Bullet>().explosionRadius = bulletExplosionRadius;
                                    }
                                    else
                                    {
                                        bullet = Instantiate(bulletPrefab, shootingPoint.transform.position, weaponRot * Quaternion.Euler(0, 0, spread));
                                        bullet.GetComponent<Bullet>().damage = bulletDamage;
                                        bullet.GetComponent<Bullet>().explosionRadius = bulletExplosionRadius;
                                    }
                                    audio.Play(shotAudio, gameObject);
                                    ammoText.text = bulletsLeft + "/" + maxBullets;
                                }

                                else
                                {
                                    if (currBullet != null)
                                    {
                                        bulletsLeft--;
                                        currBullet.transform.parent = null;
                                        if (characterScale.x < 0)
                                        {
                                            currBullet.transform.rotation *= Quaternion.Euler(0, 0, 180);
                                        }
                                        currBullet.GetComponent<Bullet>().enabled = true;
                                        currBullet.GetComponent<Bullet>().damage = bulletDamage;
                                        currBullet.GetComponent<Bullet>().explosionRadius = bulletExplosionRadius;
                                        audio.Play(shotAudio, gameObject);
                                        ammoText.text = bulletsLeft + "/" + maxBullets;
                                        currBullet = null;
                                    }

                                }
                            }
                            else
                            {
                                if (characterScale.x > 0)
                                {
                                    targetHit = Physics2D.Raycast(shootingPoint.transform.position, currWeapon.transform.right);
                                }
                                else
                                {
                                    targetHit = Physics2D.Raycast(shootingPoint.transform.position, -currWeapon.transform.right);
                                }
                                if (targetHit.collider != null)
                                {

                                    weaponTarget.transform.position = targetHit.point;
                                    weaponTarget.SetActive(true);
                                }
                                else
                                {
                                    weaponTarget.SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            bulletsLeft--;
                            ammoText.text = bulletsLeft + "/" + maxBullets;
                            shootingCooldown = 0;

                            

                            if (targetHit.transform.tag == "Enemy")
                            {
                                if (lastEnemy == targetHit.transform.gameObject)
                                {
                                    timeShooting += Time.deltaTime;
                                }
                                else
                                {
                                    timeShooting = 0;
                                    lastEnemy = targetHit.transform.gameObject;
                                }
                                    targetHit.transform.gameObject.GetComponent<EnemyManager>().TakeDamage(bulletDamage*timeShooting);
                            }
                            else
                            {
                                timeShooting = 0;
                                lastEnemy = null;
                            }
                        }
                    }

                }

                else
                {
                    if (bulletPrefab == null && currWeapon != null)
                    {
                        lineRend.enabled = false;
                    }
                    ammoText.text = "Reloading";
                }




            }
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (bulletPrefab == null&&currWeapon!=null)
            {
                lineRend.enabled = false;
                lastEnemy = null;
                timeShooting = 0;
            }
            if (weaponTarget != null)
            {
                weaponTarget.SetActive(false);
                if (currWeapon != null)
                {
                    if (bulletsLeft > 0)
                    {
                        if (shootingCooldown >= fireRate)
                        {
                            audio.Play(shotAudio, gameObject);
                            float spread = UnityEngine.Random.Range(-bulletSpread, bulletSpread);

                            bulletsLeft--;
                            shootingCooldown = 0;
                            if (characterScale.x < 0)
                            {
                                bullet = Instantiate(bulletPrefab, shootingPoint.transform.position, weaponRot * Quaternion.Euler(0, 0, 180 + spread));
                                bullet.GetComponent<Bullet>().damage = bulletDamage;
                            }
                            else
                            {
                                bullet = Instantiate(bulletPrefab, shootingPoint.transform.position, weaponRot * Quaternion.Euler(0, 0, spread));
                                bullet.GetComponent<Bullet>().damage = bulletDamage;
                            }
                            
                            ammoText.text = bulletsLeft + "/" + maxBullets;
                            
                        }
                    }
                }
            }
        }

        if (bulletsLeft <= 0 && shootingCooldown >= reloadTime)
        {
            shootingCooldown = fireRate;
            bulletsLeft = maxBullets;
            ammoText.text = maxBullets + "/" + maxBullets;
        }
        else if (bulletsLeft <= 0 && instReloadsCount > 0)
        {
            instReloadsCount--;
            UpdateInstReloadBar();
            shootingCooldown = fireRate;
            bulletsLeft = maxBullets;
            ammoText.text = maxBullets + "/" + maxBullets;
        }

        if (bulletShow && bulletsLeft > 0 && currBullet == null)
        {
            float spread = UnityEngine.Random.Range(-bulletSpread, bulletSpread);
            currBullet = Instantiate(bulletPrefab, shootingPoint.transform.position, weaponRot * Quaternion.Euler(0, 0, spread));
            currBullet.transform.parent = currWeapon.transform;
            currBullet.GetComponent<Bullet>().enabled = false;
        }

        if (!isGrounded)
        {
            anim.SetBool("IsJumping", true);
        }
        else
        {
            anim.SetBool("IsJumping", false);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            usedAction = false;
        }
    }
  
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag=="Win")
        {
            Win();
        }
     else if (collision.transform.tag == "Weapon")
        {
            collision.GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.gray);
            tutorialText.text = "Press E, to pick up "+collision.transform.name.Replace("(Clone)", "");
            tutorialText.enabled = true;
            if (Input.GetKey(KeyCode.E)&&!usedAction)
            {
                if (currWeapon == null)
                {
                    currWeapon = collision.gameObject;
                    OnPickupWeapon();
                }
                else {
                    gunCollider.enabled = true;
                    currWeapon.transform.position = collision.transform.position;
                    currWeapon = collision.gameObject;
                    OnPickupWeapon();
                }
                usedAction = true;
                collision.GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.white);
                tutorialText.enabled = false;
            }
        }
        else if(collision.transform.tag == "Powerup")
        {

            tutorialText.enabled = false;
            powerUpInfo info = collision.gameObject.GetComponent<powerUpInfo>();

            if (info.instantReload && instReloadsCount < 3)
            {
                Destroy(collision.gameObject);
                instReloadsCount++;
                UpdateInstReloadBar();
                audio.Play(info.powerUpSound, gameObject);
            }
            if (info.timeBonus)
            {
                Destroy(collision.gameObject);
                manager.timeLeft += 10;
                audio.Play(info.powerUpSound, gameObject);
            }
            if (powerTime == 0)
            {
                
                if (info.TimeStop)
                {
                    Color col = collision.GetComponent<powerUpInfo>().fireColor;
                    col.a = 0.75f;
                    rend.color = col;
                    fire.SetActive(true);
                    Destroy(collision.gameObject);
                    powerTime = info.time;
                    manager.timeSpeed = 0;
                    audio.Play(info.powerUpSound, gameObject);
                }
                if (info.TimeWarp)
                {
                    Color col = collision.GetComponent<powerUpInfo>().fireColor;
                    col.a = 0.75f;
                    rend.color = col;
                    fire.SetActive(true);
                    Destroy(collision.gameObject);
                    powerTime = info.time;
                    lastTime = manager.timeLeft;
                    lastPos = transform.position;
                    hasSave = true;
                    backupPos?.Invoke(this, EventArgs.Empty);
                    lastGun = currWeapon;
                    audio.Play(info.powerUpSound, gameObject);

                }
                
                
            }
            
            
            
        }

        else if (collision.transform.tag == "Chest")
        {
            collision.GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.gray);
            tutorialText.text = "Press E, to open a chest";
            tutorialText.enabled = true; 
            Chest chest = collision.GetComponent<Chest>();
            if (Input.GetKey(KeyCode.E))
            {
                tutorialText.enabled = false;
                chest.openChest();
            }

            usedAction = true;
        }
        else if(collision.transform.tag == "Button")
        {
            ButtonController btn = collision.gameObject.GetComponent<ButtonController>();
            if (!btn.isPressed)
            {
                tutorialText.text = "Press E, to press the button";
                tutorialText.enabled = true;
                if (Input.GetKey(KeyCode.E))
                {
                    tutorialText.enabled = false;
                    btn.pressTheButton();
                }
            }
        }
        else if (collision.transform.tag == "Laser")
        {
            manager.timeLeft -= laserDamage * Time.deltaTime;
        }
     else if(collision.transform.tag == "Elevator")
        {
            transform.parent = collision.transform;
        }
            
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "BossFeet")
        {
            if (collision.GetComponent<Mech>().jumped)
            {
                manager.timeLeft -= collision.GetComponent<Mech>().jumpDamage;
                characterScale.y = 1.5f;
                canJump = false;
            }

            //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Chest" || collision.transform.tag == "Weapon")
        {
            collision.GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.white);
            


        }
        else if (collision.transform.tag == "Elevator")
        {
            transform.parent = null;
        }
        else if (collision.transform.tag == "BossFeet")
        {
            characterScale.y = 5f;
            canJump = true;
            
            //GetComponent<Rigidbody2D>().constraints = ~RigidbodyConstraints2D.FreezePositionY;
        }
    }

    //Check if Grounded
    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastPoint.transform.position, -Vector2.up);
        if (hit.collider != null)
        {
            if (hit.distance < 0.1)
            {
                isGrounded = true;
                Debug.Log(hit.transform.name);
                canWallJump = false;
                lastWallJump = 'n';
            }
            else
            {
                isGrounded = false;
            }
        }
        else{
            isGrounded = false;
        }

        if (collisionCount == 0)
        {
            tutorialText.enabled = false;
        }

        if (characterScale.x > 0)
        {
             wallHit = Physics2D.Raycast(walljumpPoint.transform.position, Vector2.right);
        }
        else
        {
             wallHit = Physics2D.Raycast(walljumpPoint.transform.position, Vector2.left);
        }
        if (wallHit.collider != null)
        {
            if (wallHit.distance < 0.01)
            {
               if(lastWallJump=='r' && characterScale.x < 0 || lastWallJump == 'l' && characterScale.x > 0 || lastWallJump == 'n')
                {
                    canWallJump = true;
                    if(characterScale.x < 0)
                    {
                        lastWallJump = 'l';
                    }
                    else
                    {
                        lastWallJump = 'r';
                    }
                }
               
            }
            else
            {
                canWallJump = false;
            }
        }
        else
        {
            canWallJump = false;
        }


    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(collisionCount);
        collisionCount++;

    }

    void OnCollisionExit2D(Collision2D col)
    {
        collisionCount--;
        if (collisionCount == 0)
        {
            tutorialText.enabled = false;
        }
    }

    private void OnPickupWeapon()
    {
        timeShooting = 0;
        lastEnemy = null;
        gunCollider = currWeapon.GetComponent<BoxCollider2D>();
        gunCollider.enabled = false;
        info = currWeapon.GetComponent<weaponInfo>();
        maxBullets = info.maxBullets;
        bulletsLeft = maxBullets;
        reloadTime = info.reloadTime;
        bulletSpread = info.bulletSpread;
        bulletDamage = info.bulletDamage;
        fireRate = info.fireRateInSeconds;
        bulletPrefab = info.bullet;
        if (bulletPrefab == null)
        {
            lineRend = currWeapon.GetComponent<LineRenderer>();
            lineRend.enabled = false;
        }

        shootingPoint = info.shootingPoint;
        ammoText.text = maxBullets + "/" + maxBullets;
        weaponTarget = info.targetPrefab;
        shotAudio = info.bulletSound;
        bulletExplosionRadius = info.explosionRadius;
        bulletShow=info.bulletsStay;
        if (weaponTarget != null)
        {
            weaponTarget.transform.parent = null;
        }
       
    }

    private void UpdateInstReloadBar()
    {
        instReloadText.text = "Instant reloads: \n ";
        for (int i = 0; i < 3; i++)
        {
            
            if (i < instReloadsCount)
            {
                instReloadText.text += "X ";
            }
            else{
                instReloadText.text += "O ";
            }
        }
    }


    public void Win()
    {
        end = true;
        manager.timeSpeed = 0;
        ammoText.text = "You win";
        tutorialText.text = "Press R if you want to play this level again";
        tutorialText.enabled = true;
        instReloadText.text = "This game is in early develpment stage. More content coming soon!";
    }



}
