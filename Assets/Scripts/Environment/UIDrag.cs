using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDrag : MonoBehaviour
{
    Vector2 offset;
    Quaternion qt;
    public GameObject cover;
    Cover coverScript;
    GameObject lvlLoader;
    int sceneId;
    bool touching = false;
    // Start is called before the first frame update
    void Start()
    {
        sceneId = GameData.GetTheCurrentLevel();
        if(gameObject.name== "NewGame")
        {
            
            sceneId = 2;
        }
        lvlLoader = GameObject.FindGameObjectWithTag("LvlLoader");
        qt = transform.rotation;
        coverScript = cover.GetComponent<Cover>();
    }

    private void Update()
    {
        if (transform.position.y <= 100)
        {
            if (!touching)
            {
                coverScript.touching++;
            }
            if (!coverScript.loading)
            {
                touching = true;
                cover.SetActive(true);
                qt.x = 0.5f;
            }
            
        }
        else
        {
            if (touching)
            {
                coverScript.touching--;
                if (coverScript.touching == 0)
                {
                    cover.SetActive(false);
                }
            }
            touching = false;
           
            qt.x = 0;
        }
        transform.rotation = qt;
    }

    public void BeginDrag()
    {
        offset = transform.position - Input.mousePosition;
        
    }

    public void EndDrag()
    {
        if (touching)
        {
            coverScript.touching--;
           
                cover.SetActive(false);
            coverScript.loading = true;
            if(gameObject.name== "SettingCasette")
            {
                lvlLoader.GetComponent<LevelLoader>().LoadLevel(6);
            }
            else if(gameObject.name == "Back")
            {
                lvlLoader.GetComponent<LevelLoader>().LoadLevel(0);
            }
            else
            {
                lvlLoader.GetComponent<LevelLoader>().LoadLevel(sceneId);
            }
            gameObject.SetActive(false);
            
        }
    }

    public void OnMouseDrag()
    {
        transform.position = new Vector3(offset.x + Input.mousePosition.x, offset.y + Input.mousePosition.y);
    }
}
