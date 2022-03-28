using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallGenerator : MonoBehaviour
{
    public static BallGenerator instance;

    public GameObject ballPrefab;    // TODO: gameObject
    public float timeElapsed;
    public float generateInterval;
    // All the balls in the scene
    public List<GameObject> balls = new List<GameObject>();  // TODO: gameObject

    private BallType _previousBall;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.inGame)
        {
            timeElapsed += Time.deltaTime;
            // Generate new ball
            if (timeElapsed >= generateInterval)
            {
                GameObject newBall = Instantiate(ballPrefab, this.transform);
                // Make sure the new ball's color is not the same with the previous one
                BallType newBallType;
                while (true)
                {
                    newBallType = RandomType();
                    if (newBallType != _previousBall) break;
                }
                newBall.GetComponent<BallController>().Initialize(newBallType);
                _previousBall = newBallType;
                balls.Add(newBall);
                timeElapsed -= generateInterval;
            }
        }
    }

    private BallType RandomType()
    {
        BallType randomType = (BallType) Random.Range(0, Enum.GetValues(typeof(BallType)).Length);
        return randomType;
    }

    public void Initialize()
    {
        DestroyAll();
        timeElapsed = 2f;
        generateInterval = 2f;
        _previousBall = RandomType();
    }

    public void OnPause()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].GetComponent<BallController>().OnPause();
        }
    }

    public void OnResume()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].GetComponent<BallController>().OnResume();
        }
    }
    
    // TODO: a method for removing a given ball
    public void DestroyAll()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            Destroy(balls[i]);
        }
        balls = new List<GameObject>();
    }
}
