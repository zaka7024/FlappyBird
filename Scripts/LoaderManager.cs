using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoaderManager 
{

    public enum Scenes
    {
        MainScene,
        LoadingScene,
        MenuScene
    }

    public static Scenes target;
    public static void Load(Scenes scene)
    {

        target = scene;

        SceneManager.LoadScene(Scenes.LoadingScene.ToString());
        
    }

    public static void callTargetScene()
    {
        SceneManager.LoadScene(target.ToString());
    }
}
