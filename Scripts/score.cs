using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class score
{
    
    public static void Start()
    {
        BirdController.getInstance().onDead += () =>
            {
                tryToSaveScore((int) LevelGenerator.getInstance().getPassedPipes());
            };
    }
    
    public static bool tryToSaveScore(int score)
    {
        int lastScore = PlayerPrefs.GetInt("highscore", 0);
        if(score > lastScore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }

    public static int getHighScore()
    {
        return PlayerPrefs.GetInt("highscore", 0);
    }
}