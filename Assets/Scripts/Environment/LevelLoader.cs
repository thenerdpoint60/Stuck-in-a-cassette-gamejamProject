using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameObject[] CassetesAsLevel;

    private void Start()
    {
        if(SceneManager.GetActiveScene().name=="Menu")
        {
            if (GameData.GetTheCurrentLevel() == 1)
            {
                CassetesAsLevel[0].SetActive(true);
                CassetesAsLevel[1].SetActive(false);
            }
            else
            {
                CassetesAsLevel[0].SetActive(false);
                CassetesAsLevel[1].SetActive(true);
            }
        }
       
    }



    public void LoadLevel(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
    }

    //IEnumerator LoadAsynchronously(int sceneIndex)
    //{
        
        //while(!operation.isDone)
    //}
}
