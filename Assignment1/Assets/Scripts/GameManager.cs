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
    
    public Text dh;
    public Text ipg;
    public Text s;
    public Text t;
    public Text r;
    public Text g;
    public Text b;
    
    private int _debrisHit;
    private int _score;
    private int _red;
    private int _green;
    private int _blue;
    private float _prevTime;
    private float _totalTime;
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
        
        // Update time
        if (currentGameState == GameState.inGame)
        {
            UpdateTime();
        }
    }

    public void UpdateTime()
    {
        float currentTime = Time.time;
        _totalTime += currentTime - _prevTime;
        _prevTime = currentTime;
        String minutes = (_totalTime / 60).ToString("f0");
        String seconds = (_totalTime % 60).ToString("f0");
        if (minutes.Length == 1) minutes = "0" + minutes;
        if (seconds.Length == 1) seconds = "0" + seconds;
        t.text = "T: " + minutes + ":" + seconds;
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
        _debrisHit = 0;
        _score = 0;
        _red = 0;
        _green = 0;
        _blue = 0;
        _prevTime = Time.time;
        _totalTime = 0;
        _prevScoreTime = Time.time;
        UpdateHUD();
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
        _prevTime = Time.time;
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
    }

    public void OnMenu()
    {
        currentGameState = GameState.menu;
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
            // Increase score
            if (matchSize >= 5) AddScore(30);  // +30
            else if (matchSize >= 3) AddScore(10);  // +10
            // Increase match count
            if (matchType == BallType.red) _red++;
            if (matchType == BallType.green) _green++;
            if (matchType == BallType.blue) _blue++;
            UpdateHUD();
        }
    }

    public void OnDebrisClean(GameObject debris)
    {
        // Destroy ball
        BallGenerator.instance.DestroyBall(debris);
        // Increase debris count
        _debrisHit++;
        UpdateHUD();
        // Increase score
        AddScore(5);
    }

    private void AddScore(int add)
    {
        _score += add;
        UpdateHUD();
        CheckWin();
    }

    private void UpdateHUD()
    {
        // Time is controlled separately
        dh.text = "DH: " + _debrisHit;
        ipg.text = "IPG: " + (_red + _green + _blue);
        s.text = "S: " + _score;
        r.text = "R: " + _red;
        g.text = "G: " + _green;
        b.text = "G: " + _blue;
    }

    private void CheckWin()
    {
        if (_score >= 100) OnGameOver(GameOverState.win);
    }
}
