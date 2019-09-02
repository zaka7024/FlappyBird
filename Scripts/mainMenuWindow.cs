using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainMenuWindow : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("PlayButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            LoaderManager.Load(LoaderManager.Scenes.MainScene);
        });       
        
        transform.Find("QuitButton").GetComponent<Button>().onClick.AddListener(Application.Quit);

        Button changeModeButton = transform.Find("ChangeModeButton").GetComponent<Button>();
        Text buttonText = changeModeButton.transform.Find("Text").GetComponent<Text>();
        buttonText.text = GameMode.GetGameModeName();
        changeModeButton.onClick.AddListener(() =>
        {
            GameMode.gameMode = (GameMode.Modes)(Convert.ToInt32(GameMode.gameMode) % 3 + 1);
            buttonText.text = GameMode.GetGameModeName();
        });
    }
    
}
