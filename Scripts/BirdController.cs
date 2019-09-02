using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BirdController : MonoBehaviour
{

    private Rigidbody2D rigidbodyBird;
    private State birdState;

    public float jumpSpeed;
    public event Action onDead;
    public event Action onBirdStart;
    public static BirdController instance;

    private enum State
    {
        WhitingToStart,
        Playing,
        Dead
    }

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rigidbodyBird = GetComponent<Rigidbody2D>();
        rigidbodyBird.bodyType = RigidbodyType2D.Static;
        birdState = State.WhitingToStart;
        
        // Change player properties depends on game mode
        ChangeBirdProperties();
    }
    void Update()
    {
        switch (birdState)
        {
            case State.WhitingToStart:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)
            || Input.touchCount > 0)
                {
                    birdState = State.Playing;
                    rigidbodyBird.bodyType = RigidbodyType2D.Dynamic;
                    onBirdStart.Invoke();
                }
                break;
            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)
            || Input.touchCount > 0)
                {
                    rigidbodyBird.velocity = Vector2.up * jumpSpeed;
                    soundManager.playSound(gameAssets.getInstance().birdJump);
                }
                transform.eulerAngles = new Vector3(0,0,rigidbodyBird.velocity.y * 0.2f);
                break;
        }
    }

    public static BirdController getInstance()
    {
        return instance;
    }

    public void ChangeBirdProperties()
    {
        if (GameMode.gameMode == GameMode.Modes.BirdInverse)
        {
            transform.localScale = new Vector3(-1,1);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        onDead.Invoke();
        rigidbodyBird.bodyType = RigidbodyType2D.Static;
        birdState = State.Dead;
        soundManager.playSound(gameAssets.getInstance().loss);
    }
}
