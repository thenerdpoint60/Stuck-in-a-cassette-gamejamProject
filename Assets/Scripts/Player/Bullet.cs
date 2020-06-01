using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    public float speed=3;
    public float damage;
    public float explosionRadius;
    private Vector2 pos;
    public float lifetime=10;
    private bool stop = false;
    private GameManager manager;
    public GameObject explosion;
   
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (stop)
        {
            return;
        }
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            gameObject.SetActive(false);
        }

        transform.Translate(speed * Time.deltaTime*manager.timeSpeed, 0, 0,  Space.Self);
    }
 

    private void OnCollisionStay2D(Collision2D collision)
    {
      
        

        if (collision.transform.tag != "Player" && collision.transform.tag != "Bullet")
        {
            if (explosionRadius > 0)
            {
                GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                Debug.Log("Enemies: " + allEnemies.Length);
                foreach(GameObject enemy in allEnemies)
                {
                    //Debug.Log("Distance " + Vector2.Distance(enemy.transform.position, transform.position));
                    if(Vector2.Distance(enemy.transform.position, transform.position)<=explosionRadius)
                    {
                        enemy.GetComponent<EnemyManager>().TakeDamage(damage);
                    }
                }

                Instantiate(explosion, transform.position, Quaternion.identity);
            }

            else if (collision.transform.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyManager>().TakeDamage(damage);
                //Destroy(gameObject);
            }

            Destroy(gameObject);
        }
        



    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "BossFeet")
        {
            collision.gameObject.GetComponent<Mech>().dealDamage(damage);
            Destroy(gameObject);
        }
        if (collision.transform.tag == "Boss1")
        {
            collision.gameObject.GetComponent<BossEnemy_1>().TakeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.transform.tag == "Boss3")
        {
            collision.gameObject.GetComponent<BossEnemy_3>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }




}
