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

    public GameState currentGameState = GameState.menu;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGameState = GameState.menu;
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartGame();
        }
    }

    // Called at game launch and game start
    public void Initialize()
    {
        FighterController.instance.Initialize();
    }

    public void StartGame()
    {
        // (Some initial setup)
        Resume();
    }
    
    public void Resume()
        {
            currentGameState = GameState.inGame;
        }

    public void Pause()
    {
        currentGameState = GameState.pause;
    }

    public void GameOver(GameOverState gameOverState)
    {
        currentGameState = GameState.gameOver;
    }

    public void BackToMenu()
    {
        currentGameState = GameState.menu;
    }
}
