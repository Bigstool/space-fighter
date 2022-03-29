using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    
    public int debrisHit;
    public int score;
    public int red;
    public int green;
    public int blue;
    
    private float _prevScoreTime;

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
        debrisHit = 0;
        score = 0;
        red = 0;
        green = 0;
        blue = 0;
        _prevScoreTime = Time.time;
        UIManager.instance.Initialize();
        FighterController.instance.Initialize();
        BallGenerator.instance.Initialize();
        SensorGrid.instance.Initialize();
    }

    public void OnPause()
    {
        currentGameState = GameState.pause;
        FighterController.instance.OnPause();
        BallGenerator.instance.OnPause();
        SensorGrid.instance.OnPause();
        UIManager.instance.OnPause();
    }
    
    public void OnResume()
    {
        UIManager.instance.OnResume();
        FighterController.instance.OnResume();
        BallGenerator.instance.OnResume();
        SensorGrid.instance.OnResume();
        currentGameState = GameState.inGame;
    }
    
    public void OnGameOver(GameOverState state)
    {
        OnPause();
        currentGameState = GameState.gameOver;
        gameOverState = state;
        UIManager.instance.OnGameOver(state);
    }

    public void OnMenu()
    {
        currentGameState = GameState.menu;
        Initialize();
    }
    
    public void OnMatch(List<GameObject> match)
    {
        float currentTime = Time.time;
        if (currentTime - _prevScoreTime > 0.01f)
        {
            _prevScoreTime = currentTime;
            int matchSize = match.Count;
            BallType matchType = match[0].GetComponent<BallController>().ballType;
            // Destroy balls
            for (int i = 0; i < matchSize; i++)
            {
                BallGenerator.instance.DestroyBall(match[i]);
            }
            // Increase score and match count
            if (matchSize >= 5)
            {
                AddScore(30);  // +30
                AddCount(matchType, 2);
            }
            else if (matchSize >= 3)
            {
                AddScore(10);  // +10
                AddCount(matchType, 1);
            }
            // Update HUD
            UIManager.instance.UpdateHUD();
        }
    }

    public void OnDebrisClean(GameObject debris)
    {
        // Destroy ball
        BallGenerator.instance.DestroyBall(debris);
        // Increase debris count
        debrisHit++;
        UIManager.instance.UpdateHUD();
        // Increase score
        AddScore(5);
    }

    private void AddScore(int add)
    {
        score += add;
        UIManager.instance.UpdateHUD();
        CheckWin();
    }

    private void AddCount(BallType type, int count)
    {
        if (type == BallType.red) red += count;
        if (type == BallType.green) green += count;
        if (type == BallType.blue) blue += count;
    }

    private void CheckWin()
    {
        if (score >= 100) OnGameOver(GameOverState.win);
    }
}
