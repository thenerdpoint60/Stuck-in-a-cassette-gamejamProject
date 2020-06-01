using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    [System.Serializable]
    public class WeaponInfo
    {
        public GameObject[] GunsPrefabsOfSameRarity;
        public int chansesOfGetting;
    }

    private int weaponId;
    private bool end;

    public WeaponInfo[] weaponsYouCanGet;
    Animator anim;
    private int chanses = 0;
    public bool isOpen;
    private int earlierBonus;
    private BoxCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {

        collider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        for (int i = 0; i < weaponsYouCanGet.Length; i++)
        {
            chanses += weaponsYouCanGet[i].chansesOfGetting* weaponsYouCanGet[i].GunsPrefabsOfSameRarity.Length;
        }
    }

    public void openChest()
    {
        
        
        if (!isOpen)
        {
            isOpen = true;
            Debug.Log("Open");
            int rand = Random.Range(1, chanses+1);

            end = false;
            earlierBonus = 0;

            for (int i = 0; i < weaponsYouCanGet.Length; i++)
            {

                if (end == false)
                {
                    if (rand <= (weaponsYouCanGet[i].chansesOfGetting* weaponsYouCanGet[i].GunsPrefabsOfSameRarity.Length+earlierBonus))
                    {

                        end = true;
                        weaponId = i;
                    }

                }
                earlierBonus += weaponsYouCanGet[i].chansesOfGetting * weaponsYouCanGet[i].GunsPrefabsOfSameRarity.Length;
            }


            Instantiate(weaponsYouCanGet[weaponId].GunsPrefabsOfSameRarity[Random.Range(0, weaponsYouCanGet[weaponId].GunsPrefabsOfSameRarity.Length)], transform.position, transform.rotation);

           
            anim.SetBool("IsOpen", true);
            collider.enabled=false;
        }
        else
        {

            Debug.Log("Already Open");
        }
        
    }
}
