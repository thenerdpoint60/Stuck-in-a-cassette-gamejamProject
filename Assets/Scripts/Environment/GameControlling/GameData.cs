using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData 
{
    public static string currentLevel = "CurrentLevel";

    public static void SetTheCurrentLevel(int level)
    {
        PlayerPrefs.SetInt(currentLevel, level);
    }

    public static int GetTheCurrentLevel()
    {
        return PlayerPrefs.GetInt(currentLevel, 1);
    }


}
