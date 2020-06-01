using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossObjectSpawn : MonoBehaviour
{
    public float speed;
    public int DestroyAfter;
    public float minX,maxX,minY,maxY;
    public GameObject objectToSpawn;
    public GameObject explosion;
    float randomX, randomY;
    bool objectSapwned;




    GameManager manager;
    private void Start()
    {
        objectSapwned = false;
        randomX = UnityEngine.Random.Range(minX,maxX);
        randomY = UnityEngine.Random.Range(minY, maxY);
        Debug.Log(randomX + ":" + randomY);
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }


    private void FixedUpdate()
    {
        float time = 0f;
        time += Time.deltaTime;
        Debug.Log(time);
        //Vector3.MoveTowards(transform.position, new Vector3(randomX, randomY, 0), 1f);
        //transform.Translate(new Vector3(randomX, randomY, 0)*Time.deltaTime*speed);
        transform.Translate(new Vector2(randomX, randomY) * speed * Time.deltaTime * manager.timeSpeed);
        if(!objectSapwned)
        {
            if (time < DestroyAfter /*|| (transform.position.x == randomX && transform.position.y == randomY*/)
            {
                objectSapwned = true;
                StartCoroutine("SpawnObject");
                
            }
        }
        
    }

    IEnumerator SpawnObject()
    {
        yield return new WaitForSeconds(1f);
        GameObject a = Instantiate(explosion, transform.position, quaternion.identity);
        Destroy(a, 2f);
        Destroy(this,1f);
        yield return new WaitForSeconds(0.5f);
        Instantiate(objectToSpawn, transform.position, quaternion.identity);
        
    }
}

