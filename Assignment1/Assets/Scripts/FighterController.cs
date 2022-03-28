using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    public static FighterController instance;

    public float velocity;
    
    private Rigidbody2D _rigidbody;
    private float upperBound = 1.25f;
    private float lowerBound = -0.25f;

    private void Awake()
    {
        instance = this;
        _rigidbody = GetComponent<Rigidbody2D>();
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
            ControlUpdate();
        }
    }

    public void Initialize()
    {
        transform.position = new Vector3(0f, 0.5f, -1f);
    }

    public void OnPause()
    {
        _rigidbody.velocity = new Vector2(0f, 0f);
    }

    public void OnResume()
    {
        
    }

    private void ControlUpdate()
    {
        Vector3 currentPosition = transform.position;
        Vector2 newVelocity = new Vector2(0f, 0f);
        if (Input.GetKey(KeyCode.W) && currentPosition.y < upperBound)
        {
            newVelocity.y += velocity;
        }
        if (Input.GetKey(KeyCode.S) && currentPosition.y > lowerBound)
        {
            newVelocity.y -= velocity;
        }
        if (Input.GetKey(KeyCode.D))
        {
            newVelocity.x += velocity;
        }
        if (Input.GetKey(KeyCode.A))
        {
            newVelocity.x -= velocity;
        }

        _rigidbody.velocity = newVelocity;
    }
}