    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class PowerUpInfo
    {
        public GameObject[] PowerUpsPrefabsOfSameRarity;
        public int rarity;
    }

    public PowerUpInfo[] info;
    public float roundTime=2;
    public Slider Timeline;
    public TMPro.TextMeshProUGUI TimeLeft;
    public float timeLeft = 0;
    public float timeSpeed = 1;
    PlayerController controller;
    GameObject[] Enemies;
    Vector2[] EnemiesPos;
    float[] EnemiesTime;
    int fullRarity=0;
    int powerId;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < info.Length; i++)
        {
            fullRarity += (11 - info[i].rarity) * info[i].PowerUpsPrefabsOfSameRarity.Length;
        }
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        timeLeft = roundTime;
        //Timeline.maxValue = roundTime;
        controller.backupPos += Controller_backupPos;
        controller.rewindPos += Controller_rewindPos;
    }

private void Controller_rewindPos(object sender, System.EventArgs e)
{
    for (int i = 0; i < Enemies.Length; i++)
    {
            Enemies[i].transform.position = EnemiesPos[i];
            Enemies[i].GetComponent<EnemyManager>().currentTimeHealth = EnemiesTime[i];
            Enemies[i].SetActive(true);
        }
}

private void Controller_backupPos(object sender, System.EventArgs e)
{
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        EnemiesPos = new Vector2[Enemies.Length];
        EnemiesTime = new float[Enemies.Length];
        for (int i = 0; i < Enemies.Length; i++)
        {
            EnemiesPos[i] = Enemies[i].transform.position;
            EnemiesTime[i] = Enemies[i].GetComponent<EnemyManager>().currentTimeHealth;

        }
    }

// Update is called once per frame
void Update()
    {
        timeLeft -= Time.deltaTime*timeSpeed;
        //Timeline.value = roundTime- timeLeft;
        string minutes = Mathf.Floor(timeLeft / 60).ToString("00");
        string seconds = (timeLeft % 60).ToString("00");
        TimeLeft.text = minutes + ":" + seconds;
    }

    


    public GameObject getRandomPowerup()
    {
         int rand = Random.Range(1, fullRarity+1);
        Debug.Log(rand + " " + fullRarity);

            bool end = false;
            int earlierBonus = 0;

            for (int i = 0; i < info.Length; i++)
            {

                if (end == false)
                {
                    if (rand <= ((11-info[i].rarity)* info[i].PowerUpsPrefabsOfSameRarity.Length+earlierBonus))
                    {

                        end = true;
                        powerId = i;
                    }

                }
                earlierBonus += (11-info[i].rarity) * info[i].PowerUpsPrefabsOfSameRarity.Length;
            }


        return info[powerId].PowerUpsPrefabsOfSameRarity[Random.Range(0, info[powerId].PowerUpsPrefabsOfSameRarity.Length)];
    }
}

