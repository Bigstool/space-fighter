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
    private Vector2 _velocityCache;
    // TODO: a int counting how many balls in collision, and another int counting how many players in collision

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

    public void Initialize()
    {
        transform.position = new Vector3(0f, 6.5f, 0f);
        _rigidbody.velocity = new Vector2(0f, 0f);
        _velocityCache = new Vector2(0f, 0f);
        // TODO: counters
    }

    public void OnPause()
    {
        _velocityCache = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y);
        _rigidbody.velocity = new Vector2(0, 0);
    }

    public void OnResume()
    {
        _rigidbody.velocity = _velocityCache;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.currentGameState == GameState.inGame)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, velocity * -1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // TODO: increase corresponding counter, check both counters, check y position relative to player
    }

    private void OnCollisionExit(Collision other)
    {
        // TODO: decrease corresponding counter
    }
}
