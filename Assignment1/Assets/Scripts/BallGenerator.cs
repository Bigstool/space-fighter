using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    public static BallGenerator instance;
    
    public float timeElapsed;
    public float generateInterval;
    // All the balls in the scene
    public List<BallController> balls = new List<BallController>();
    

    private void Awake()
    {
        instance = this;
        timeElapsed = 0f;
        generateInterval = 2f;
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
                // TODO: generate new ball
                timeElapsed -= generateInterval;
            }
        }
    }

    void Initialize()
    {
        // TODO: time elapsed to 0, clean balls list
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
}
