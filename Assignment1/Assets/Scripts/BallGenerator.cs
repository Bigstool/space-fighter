using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    public static BallGenerator instance;

    public BallController ballPrefab;
    public float timeElapsed;
    public float generateInterval;
    // All the balls in the scene
    public List<BallController> balls = new List<BallController>();
    

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
            if (timeElapsed >= generateInterval)
            {
                BallController newBall = Instantiate(ballPrefab);
                newBall.Initialize();
                balls.Add(newBall);
                timeElapsed -= generateInterval;
            }
        }
    }

    public void Initialize()
    {
        DestroyAll();
        timeElapsed = 2f;
        generateInterval = 2f;
    }

    public void OnPause()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].OnPause();
        }
    }

    public void OnResume()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].OnResume();
        }
    }
    
    // TODO: a method for removing a given ball
    public void DestroyAll()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            Destroy(balls[i].gameObject);
        }
        balls = new List<BallController>();
    }
}
