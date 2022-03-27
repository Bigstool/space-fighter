using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDown : MonoBehaviour
{
    public float velocity;

    private Rigidbody2D _rigidbody;

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

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, velocity);
    }
}
