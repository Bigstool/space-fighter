using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallType
{
    red,
    green,
    blue,
    debris
}

public class BallController : MonoBehaviour
{
    public float velocity;
    public BallType ballType;

    private Rigidbody2D _rigidbody;
    private Vector2 _velocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSpawn()
    {
        
    }

    public void OnPause()
    {
        _velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y);
        _rigidbody.velocity = new Vector2(0, 0);
    }

    public void OnResume()
    {
        _rigidbody.velocity = _velocity;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.currentGameState == GameState.inGame)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, velocity * -1);
        }
    }
}
