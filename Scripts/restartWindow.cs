using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class restartWindow : MonoBehaviour
{
    public Text scoreText;
    public Text highscoreHint;
    Button restartButton;
    GameObject restartWin;

    private void Awake()
    {
        restartButton = GameObject.Find("Button").GetComponent<Button>();
        highscoreHint = GameObject.Find("highscoreHint").GetComponent<Text>();
        restartWin = transform.Find("RestartWindow").gameObject;
        
        GameObject.Find("MainMenuButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            LoaderManager.Load(LoaderManager.Scenes.MenuScene);
        });
        
        hide();
    }
    void Start()
    {
        BirdController.getInstance().onDead += RestartWindow_onDead;
        restartButton.onClick.AddListener(() => {
            LoaderManager.Load(LoaderManager.Scenes.MainScene);
        });
    }

    private void RestartWindow_onDead()
    {
        show();
        scoreText.text = LevelGenerator.getInstance().getPassedPipes().ToString();
        if (LevelGenerator.getInstance().getPassedPipes() >= score.getHighScore())
        {
            highscoreHint.text = "NEW HIGH SCORE";
        }
        else
        {
            highscoreHint.text = "HIGH SCORE: " + score.getHighScore();
        }
    }

    void hide()
    {
        restartWin.SetActive(false);
    }

    void show()
    {
        restartWin.SetActive(true);
    }
}
