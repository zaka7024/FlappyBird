using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    const float CAMERA_ORTHO_SIZE = 50f;
    const float PIP_WIDTH = 7.3f;
    const float PIP_HEAD_HEIGHT = 3.75f;
    const float PIPE_MOVEMENT_SPEED = 30f;
    const float PIPE_MIN_X_MOVE = -130f;
    const float PIPE_MAX_X_POSITION = 130f;
    const float BIDR_X_POSITION = 0F;
    private const float GROUND_MIN_X_POSITION = -120f;
    private const float GROUND_MIN_Y_POSITION = -46f;
    private const float GROUND_SPEED = 30f;
    private const float CLOUD_MAX_X_POSITION= 130f;
    private const float CLOUD_WIDTH = 136f;
    private const float CLOUD_SPEED = 24f;

    private List<Pipe> pipeList;
    private List<Transform> clouds;
    private List<Transform> grounds;
    private float maxTimeToSpwn = 1f;
    private float pipeSpwnTimer;
    private float gapSize;
    private float spwnedPipeCount;
    private float passedPiepCount;
    private float maxCloudsTime = 5f;
    private float cloudsTimer;
    private float pipeChangeHeightMaxTime = 3f;
    private float pipeChangeHeightTimer;
    private State currentState;
    private float direction = 1f;
    private Pipe moveablePipe;

    public static LevelGenerator instance;
    private enum Difficulty
    {
        Easy,Meduim,Hard,Impossible
    }

    private enum State
    {
        WhitingToStart,
        Playing,
        BirdDead
    }

    public static LevelGenerator getInstance()
    {
        return instance;
    }
    private void Awake()
    {
        pipeList = new List<Pipe>();
        grounds = new List<Transform>();
        clouds = new List<Transform>();
        instance = this;
    }
    void Start()
    {
        gapSize = 20f;
        spwnedPipeCount = 0f;
        cloudsTimer = 0f;
        pipeChangeHeightTimer = pipeChangeHeightMaxTime;
        pipeSpwnTimer = maxTimeToSpwn;
        setDifficulty(Difficulty.Easy);

        currentState = State.WhitingToStart;

        // subscribe to onDead event in BirdController object
        BirdController.getInstance().onDead += () => {
            currentState = State.BirdDead;
        };

        BirdController.getInstance().onBirdStart += () =>
        {
            currentState = State.Playing;
        };
        
        // create initial objects int the scene
        createInitialGrounds();
        createIntialClouds();
        
        // Change space properties depends on game mode
        ChangeSpaceProperties();
    }

    private void Update()
    {

        HandleCloudsMove();
        
        if (currentState == State.Playing)
        {
            HandlePipeSpwn(); // First generates the item then move random pipe
            
            /*if (GameMode.gameMode == GameMode.Modes.PipeMove)
            {
                //TODO:: Complete the final mode game
            }*/
            
            HandleCloudsGenerate();
            HandlePipesMoving();
            HandleMoveGrounds();
        }
    }

    private void setDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gapSize = 50f;
                break;
            case Difficulty.Meduim:
                gapSize = 40f;
                break;
            case Difficulty.Hard:
                gapSize = 30f;
                break;
            case Difficulty.Impossible:
                gapSize = 20f;
                break;
        }
    }

    private Difficulty GetDifficulty()
    {
        if (spwnedPipeCount >= 30) return Difficulty.Impossible;
        if (spwnedPipeCount >= 20) return Difficulty.Hard;
        if (spwnedPipeCount >= 10) return Difficulty.Meduim;
        return Difficulty.Easy;
    }
    private void HandlePipeSpwn()
    {
        pipeSpwnTimer -= Time.deltaTime;
        if(pipeSpwnTimer < 0)
        {
            pipeSpwnTimer = maxTimeToSpwn;
            float confidentHeight = 10f;
            float minHeight = gapSize * 0.5f + confidentHeight;
            float maxHiehgt = CAMERA_ORTHO_SIZE * 2f - gapSize * 0.5f - confidentHeight;
            float height = Random.Range(minHeight, maxHiehgt);
            creatGapPipes(height, gapSize, direction * PIPE_MAX_X_POSITION);
        }
    }

    private void HandlePipesMoving()
    {
        for (int i = 0; i < pipeList.Count; i++)
        {
            Pipe pipe = pipeList[i];
            bool pipeIsRightToBird = direction * pipe.getPipePosition() >= BIDR_X_POSITION;
            pipe.Move(direction);

            if (pipe.getPipePosition() * direction < BIDR_X_POSITION && pipeIsRightToBird
                && pipe.isBottom())
            {
                passedPiepCount++;
                soundManager.playSound(gameAssets.getInstance().pipePass);
            }

            if (pipe.getPipePosition() < PIPE_MIN_X_MOVE)
            {
                pipe.DestroyPipes();
                pipeList.RemoveAt(i);
            }
        }
    }

    private void creatGapPipes(float Ygap, float sizeGap, float xPosition)
    {
        createPip(CAMERA_ORTHO_SIZE * 2f - Ygap - sizeGap * 0.5f, xPosition, false); // move 
        createPip(Ygap - sizeGap * 0.5f, xPosition, true); // move it down
        spwnedPipeCount++;
        setDifficulty(GetDifficulty());
    }
    private void createPip(float height, float xPosition, bool createBottom)
    {
        //create pipe body object
        var pipBody = Instantiate(gameAssets.getInstance().pipBody);
        //set pipe body position and height
        Vector3 pipBodyPosition;
        if (createBottom)
        {
            pipBodyPosition = new Vector3(xPosition, -CAMERA_ORTHO_SIZE);
        }
        else
        {
            pipBodyPosition = new Vector3(xPosition, CAMERA_ORTHO_SIZE);
            pipBody.transform.localScale = new Vector3(1, -1, 1);
        }
        pipBody.position = pipBodyPosition;
        
        SpriteRenderer pipBodySpriteRenderer = pipBody.gameObject.GetComponent<SpriteRenderer>();
        pipBodySpriteRenderer.size = new Vector2(PIP_WIDTH, height);

        //create pipe head object
        var pipHead = Instantiate(gameAssets.getInstance().pipHead);
        Vector3 pipHeadPosition;
        if (createBottom)
        {
            pipHeadPosition = new Vector3(xPosition, -CAMERA_ORTHO_SIZE + height - PIP_HEAD_HEIGHT * 0.5f);
        }
        else
        {
            pipHeadPosition = new Vector3(xPosition, CAMERA_ORTHO_SIZE - height + PIP_HEAD_HEIGHT * 0.5f);
        }
        pipHead.position = pipHeadPosition;

        //resize the box collider of pipe body
        BoxCollider2D pipeBodyBoxCollider = pipBody.gameObject.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PIP_WIDTH, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height / 2);

        Pipe pipe = new Pipe(pipBody, pipHead, createBottom);
        pipeList.Add(pipe);
    }

    private void createInitialGrounds()
    {
        Transform ground = Instantiate(gameAssets.getInstance().ground, new Vector3(0, GROUND_MIN_Y_POSITION, 0),
            Quaternion.identity);
        grounds.Add(ground);
        Transform ground1 = Instantiate(gameAssets.getInstance().ground, new Vector3(-GROUND_MIN_X_POSITION, GROUND_MIN_Y_POSITION, 0),
            Quaternion.identity);
        grounds.Add(ground1);
        Transform ground2 = Instantiate(gameAssets.getInstance().ground, new Vector3(-2 * GROUND_MIN_X_POSITION, GROUND_MIN_Y_POSITION, 0),
            Quaternion.identity);
        grounds.Add(ground2);
        Transform ground3 = Instantiate(gameAssets.getInstance().ground, new Vector3(-3 * GROUND_MIN_X_POSITION, GROUND_MIN_Y_POSITION, 0),
            Quaternion.identity);
        grounds.Add(ground3);
    }
    
    private void createIntialClouds()
    {
        Transform cloud = Instantiate(gameAssets.getInstance().clouds[Random.Range(0,3)], new Vector3(0,30f,0),
            Quaternion.identity);
        clouds.Add(cloud);
        Transform cloud2 = Instantiate(gameAssets.getInstance().clouds[Random.Range(0,3)], new Vector3(CLOUD_WIDTH,30f,0),
            Quaternion.identity);
        clouds.Add(cloud2);
    }

    private void HandleCloudsMove()
    {
        for (int i = 0; i < clouds.Count; i++)
        {

            clouds[i].position += Vector3.left * CLOUD_SPEED * Time.deltaTime;
            
            if ((clouds[i].position.x + CLOUD_WIDTH)  < -CLOUD_MAX_X_POSITION)
            {
                Destroy(clouds[i].gameObject);
                clouds.RemoveAt(i);
            }
        }
    }

    private void HandleCloudsGenerate()
    {
        if (cloudsTimer < 0)
        {
            cloudsTimer = maxCloudsTime;
            var gameObject = Instantiate(gameAssets.getInstance().clouds[Random.Range(0, 3)],
                new Vector3((CAMERA_ORTHO_SIZE + CLOUD_WIDTH), 30f, 0), Quaternion.identity);
            clouds.Add(gameObject);
        }
        else
        {
            cloudsTimer -= Time.deltaTime;
        }
    }
    
    public void ChangeSpaceProperties()
    {
        if (GameMode.gameMode == GameMode.Modes.SpaceInverse)
        {
            direction = -1;
        }
    }
    
    private void ChangePipeHeight()
    {
        
    }

    private void HandleMoveGrounds()
    {
        for (int i = 0; i < grounds.Count; i++)
        {
            grounds[i].position += Vector3.left * GROUND_SPEED * Time.deltaTime;
            
            
            if (grounds[i].position.x < GROUND_MIN_X_POSITION)
            {
                float max = -100f;
                for (int j = 0; j < grounds.Count; j++)
                {
                    if (grounds[j].position.x > max)
                    {
                        max = grounds[j].position.x;
                    }
                }
                grounds[i].position = new Vector3(max + 100f , GROUND_MIN_Y_POSITION, 0);
            }
        }
    }

    public float getPassedPipes()
    {
        return passedPiepCount;
    }

    private class Pipe
    {
        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool bottomPipe;
        public Pipe(Transform body, Transform head, bool isBottom)
        {
            pipeBodyTransform = body;
            pipeHeadTransform = head;
            bottomPipe = isBottom;
        }

        public void Move(float dir)
        {
            pipeBodyTransform.position += Vector3.left * dir * PIPE_MOVEMENT_SPEED * Time.deltaTime;
            pipeHeadTransform.position += Vector3.left * dir * PIPE_MOVEMENT_SPEED * Time.deltaTime;
        }

        public bool isBottom()
        {
            return bottomPipe;
        }

        public float getPipePosition()
        {
            return pipeBodyTransform.position.x;
        }

        public void DestroyPipes()
        {
            Destroy(pipeBodyTransform.gameObject);
            Destroy(pipeHeadTransform.gameObject);
        }

        public void ChangeHeight()
        {
            
        }
    }
    
}
