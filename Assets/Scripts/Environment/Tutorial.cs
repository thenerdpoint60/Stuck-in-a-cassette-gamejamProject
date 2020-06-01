using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    public GameObject tutorialRobot;
    public Transform firstPos;
    public Transform secondPos;
    public GameObject enemyToKill;
    public GameObject LaserTilemap;
    public float speed;
    public string[] Texts;
    private int tutorialIndex = 0;
    public TMPro.TextMeshProUGUI tutorialText;
    private PlayerController controller;
    public GameObject[] robots1;
    public GameObject[] robots2;
    public GameObject[] instantReloads;
    public GameObject[] timeBonuses;
    public Chest chest;
    public GameObject tankReal;
    public GameObject heliReal;
    public GameObject timeStop;
    public GameObject timeWarp;
    public GameObject timeStopToSpawn;
    public Transform posToSpawnPowerUp;
    public GameObject power;
    //public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        controller= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        tutorialText.text = Texts[tutorialIndex];
        if (tutorialIndex <= 1)
        {
            Vector3 smoothedPosition = Vector3.Lerp(tutorialRobot.transform.position, firstPos.transform.position, speed);
            tutorialRobot.transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, -1f);
        }
        else if(tutorialIndex>1)
        {
            Vector3 smoothedPosition = Vector3.Lerp(tutorialRobot.transform.position, secondPos.transform.position, speed);
            tutorialRobot.transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, -1f);
        }

        if(tutorialIndex==0&&(Input.GetKeyDown(KeyCode.A)  || Input.GetKeyDown(KeyCode.D)))
        {
            tutorialIndex=1;
        }
        else if(tutorialIndex == 1 && controller.gameObject.transform.position.x>3)
        {
            tutorialIndex = 2;
        }
        else if (tutorialIndex == 2 && controller.currWeapon!=null)
        {
            tutorialIndex = 3;
        }
        else if (tutorialIndex == 3 && !robots1[0].activeSelf&& !robots1[1].activeSelf&&!robots1[2].activeSelf)
        {
            tutorialIndex = 4;
        }
        else if (tutorialIndex == 4 && chest.isOpen)
        {
            tutorialIndex = 5;
        }
        else if (tutorialIndex == 5 && controller.currWeapon.transform.name=="Sniper(Clone)")
        {
            tutorialIndex = 6;
        }
        else if(tutorialIndex==6 && !enemyToKill.activeSelf)
        {
            tutorialIndex = 7;
        }
        else if (tutorialIndex == 7 && !robots2[0].activeSelf && !robots2[1].activeSelf && !robots2[2].activeSelf)
        {
            tutorialIndex = 8;
        }
        else if (tutorialIndex == 8 && controller.gameObject.transform.position.x > 80)
        {
            tutorialIndex = 9;
        }
        else if (tutorialIndex == 9 && !tankReal.gameObject.activeSelf)
        {
            
            tutorialIndex = 10;
        }
        else if (tutorialIndex == 10 && !heliReal.gameObject.activeSelf)
        {
            tutorialIndex = 11;
        }
        else if (tutorialIndex == 11 && timeStop==null)
        {
            tutorialIndex = 12;
        }
        else if (tutorialIndex == 12 && timeWarp==null)
        {
            tutorialIndex = 13;
        }
        else if (tutorialIndex == 13 && controller.hasDied)
        {
            tutorialIndex = 14;
            
            power = Instantiate(timeStopToSpawn, posToSpawnPowerUp);
            power.transform.parent = null;
        }
        else if (tutorialIndex == 14 && instantReloads[0]==null&& instantReloads[1] == null && instantReloads[2] == null)
        {
            tutorialIndex = 15;
        }
        else if (tutorialIndex == 15 && timeBonuses[0] == null && timeBonuses[1] == null && timeBonuses[2] == null && timeBonuses[3] == null)
        {
            tutorialIndex = 16;
        }

        Debug.Log(tutorialIndex);
        if (!enemyToKill.activeSelf)
        {
            LaserTilemap.SetActive(false);
        }
    }
}
