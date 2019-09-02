using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreWidnow : MonoBehaviour
{
    public Text scoreText;
    public Text highscoreText;
    void Start()
    {
        highscoreText.text = "HIGH SCORE: " + score.getHighScore();
    }
    void Update()
    {
        scoreText.text = LevelGenerator.getInstance().getPassedPipes().ToString();
    }
}
