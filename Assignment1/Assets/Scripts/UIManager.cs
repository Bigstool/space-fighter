using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    // UI views
    public Canvas hud;
    public Canvas menu;
    public Canvas popup;
    
    // HUD elements
    public Text dh;
    public Text ipg;
    public Text s;
    public Text t;
    public Text r;
    public Text g;
    public Text b;
    
    // Menu elements
    public Button start;
    
    // Popup elements
    public Text popupText;
    public Button resume;
    public Button restart;
    public Button toMenu;
    
    private float _prevTime;
    private float _totalTime;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        hud.enabled = true;
        menu.enabled = true;
        popup.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Update time
        if (GameManager.instance.currentGameState == GameState.inGame)
        {
            UpdateTime();
        }
    }

    public void Initialize()
    {
        _prevTime = Time.time;
        _totalTime = 0;
        t.text = "T: 00:00";
        UpdateHUD();
        menu.enabled = true;
        popup.enabled = false;
    }

    public void OnResume()
    {
        _prevTime = Time.time;
        menu.enabled = false;
        popup.enabled = false;
    }

    public void OnPause()
    {
        popupText.text = "Pause";
        popupText.color = new Color32((byte)255, (byte)255, (byte)255, (byte)255);
        resume.gameObject.SetActive(true);
        popup.enabled = true;
    }

    public void OnGameOver(GameOverState state)
    {
        if (state == GameOverState.win)
        {
            popupText.text = "Congratulations! You Won!";
            popupText.color = new Color32((byte)255, (byte)255, (byte)0, (byte)255);
        }
        else if (state == GameOverState.overflow)
        {
            popupText.text = "Oops! Ionised particles are not successfully lined in the storage!";
            popupText.color = new Color32((byte)255, (byte)165, (byte)0, (byte)255);
        }
        else if (state == GameOverState.debris)
        {
            popupText.text = "Oops! Debris are filled in the storage!";
            popupText.color = new Color32((byte)0, (byte)0, (byte)0, (byte)255);
        }
        else if (state == GameOverState.destroy)
        {
            popupText.text = "Oops! Space Fighter has been destroyed!";
            popupText.color = new Color32((byte)255, (byte)0, (byte)0, (byte)255);
        }
        resume.gameObject.SetActive(false);
        popup.enabled = true;
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
    
    public void UpdateHUD()
    {
        // Time is controlled separately
        dh.text = "DH: " + GameManager.instance.debrisHit;
        ipg.text = "IPG: " + (GameManager.instance.red + GameManager.instance.green + GameManager.instance.blue);
        s.text = "S: " + GameManager.instance.score;
        r.text = "R: " + GameManager.instance.red;
        g.text = "G: " + GameManager.instance.green;
        b.text = "B: " + GameManager.instance.blue;
    }
}
