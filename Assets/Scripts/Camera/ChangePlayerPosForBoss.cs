using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangePlayerPosForBoss : MonoBehaviour
{
    public GameObject bossTileMap;
    public GameObject boss;
    public Vector2 playerPosNeeded;

    public GameObject player;





    private void Start()
    {
        enableBoss(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            enableBoss(true);
        }
    }

    void enableBoss(bool state)
    {
        boss.SetActive(state);
        bossTileMap.SetActive(state);
        if (state)
        {
            player.transform.position = playerPosNeeded;
        }
        
    }
}
