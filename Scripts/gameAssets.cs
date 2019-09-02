using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameAssets : MonoBehaviour
{

    public static gameAssets instance;

    private void Awake()
    {
        instance = this;
    }

    public static gameAssets getInstance()
    {
        return instance;
    }

    public Sprite pipe;
    public Transform pipHead;
    public Transform pipBody;
    public AudioClip birdJump;
    public AudioClip pipePass;
    public AudioClip loss;
    public AudioClip buttonHover;
    public AudioClip buttonClick;
    public Transform ground;
    public Transform[] clouds;
}
