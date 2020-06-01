using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTilemap : MonoBehaviour
{
    public float flashTime=3;
    public float stopTime=2;
    public GameObject laserTilemap;
    public GameManager gm;
    public float damagePerSecond=10;
    private float timer=0;
    private bool on = true;
    // Start is called before the first frame update
    void Start()
    {

        laserTilemap.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        if (on && timer >= flashTime)
        {
            on = false;
            laserTilemap.SetActive(false);
            
            timer = 0;
            Debug.Log("off");

        }
        else if(!on && timer >= stopTime)
        {
            on = true;
            laserTilemap.SetActive(true);

            timer = 0;
            Debug.Log("On");
        }
        
            
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.transform.name);
        if (collision.transform.tag == "Player")
        {
            gm.timeLeft -= damagePerSecond;
        }
    }


}
