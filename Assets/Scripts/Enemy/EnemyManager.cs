using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public float startTimeHealth;
    public Image timeBar;
    public GameObject Bullet;
    public GameObject explosionPrefab;
    public bool dropPowerUp=true;

    public float currentTimeHealth;

    private void Start()
    {

        currentTimeHealth = startTimeHealth;
    }


    public void TakeDamage(float damage)
    {
        //Debug.Log(damage);

        currentTimeHealth -= damage;
        timeBar.fillAmount = currentTimeHealth / startTimeHealth;
        if(currentTimeHealth<=0)
        {
            
            //You have died
            GameObject explosionObj = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosionObj, 0.5f);
            //Destroy(this.gameObject, 5f);
            
            GameObject randomPowerup =FindObjectOfType<GameManager>().getRandomPowerup();
            //Debug.Log("Power up name is :" + randomPowerup.name);
            if (dropPowerUp)
            {
                Instantiate(randomPowerup, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            }
            
            gameObject.SetActive(false);
        }
        //Debug.Log("Health left " + currentTimeHealth);
    }

    public void SetTheTImeAgain()
    {
        currentTimeHealth = startTimeHealth;
        timeBar.fillAmount = currentTimeHealth / startTimeHealth;
    }


   



}
