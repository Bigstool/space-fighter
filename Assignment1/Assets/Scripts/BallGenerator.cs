using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    // TODO: a list holding all balls in the scene
    public float timeElapsed;

    public float generateInterval;

    private void Awake()
    {
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
        
    }
    
    // TODO: a method for removing a given ball
}
