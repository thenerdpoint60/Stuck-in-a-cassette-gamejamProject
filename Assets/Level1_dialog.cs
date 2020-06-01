using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1_dialog : MonoBehaviour
{
    //public string [] Texts;
    int state = 0;
    public bool bossIsDead = false;
    public GameObject[] comics;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0 && bossIsDead)
        {
            state = 1;
            comics[state-1].SetActive(true);
            player.GetComponent<PlayerController>().enabled = false;
            player.transform.position = new Vector3(110.16f, -5.27f, 0);
            
        } 
        else if (state == 1 && Input.GetKeyDown(KeyCode.Space))
        {
            comics[state-1].SetActive(false);
            state = 2;
            comics[state-1].SetActive(true);
        }
    }
}
