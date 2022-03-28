using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    menu,
    inGame,
    pause,
    gameOver
}

public enum GameOverState
{
    win,
    overflow,
    debris,
    destroy
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState currentGameState;
    public GameOverState gameOverState;
    public int score;
    
    private float _prevTime;

    private void Awake()
    {
        instance = this;
        currentGameState = GameState.menu;
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnStart();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentGameState == GameState.inGame)
            {
                OnPause();
            }
            else if (currentGameState == GameState.pause)
            {
                OnResume();
            }
        }
    }
    
    public void OnStart()
    {
        Initialize();
        OnResume();
    }
    
    // Called at game launch and game start
    public void Initialize()
    {
        FighterController.instance.Initialize();
        BallGenerator.instance.Initialize();
        SensorGrid.instance.Initialize();
        score = 0;
        _prevTime = Time.time;
    }

    public void OnPause()
    {
        FighterController.instance.OnPause();
        BallGenerator.instance.OnPause();
        SensorGrid.instance.OnPause();
        currentGameState = GameState.pause;
    }
    
    public void OnResume()
    {
        FighterController.instance.OnResume();
        BallGenerator.instance.OnResume();
        SensorGrid.instance.OnResume();
        currentGameState = GameState.inGame;
    }
    
    public void OnGameOver(GameOverState gameOverState)
    {
        OnPause();
        currentGameState = GameState.gameOver;
        this.gameOverState = gameOverState;
    }

    public void OnMenu()
    {
        currentGameState = GameState.menu;
    }
    
    public void OnMatch(List<GameObject> match)
    {
        float currentTime = Time.time;
        if (currentTime - _prevTime > 0.01f)
        {
            _prevTime = currentTime;
            int matchSize = match.Count;
            // Destroy balls
            for (int i = 0; i < matchSize; i++)
            {
                BallGenerator.instance.DestroyBall(match[i]);
            }
            // Increase score
            if (matchSize >= 5) AddScore(30);  // +30
            else if (matchSize >= 3) AddScore(10);  // +10
        }
    }

    private void AddScore(int add)
    {
        score += add;
        CheckWin();
    }

    private void CheckWin()
    {
        if (score >= 100) OnGameOver(GameOverState.win);
    }
}
