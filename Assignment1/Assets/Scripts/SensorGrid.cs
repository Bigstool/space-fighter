using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorGrid : MonoBehaviour
{
    public static SensorGrid instance;
    
    public GameObject sensorPrefab;
    public int rows = 6;
    public int columns = 5;
    public GameObject[,] ballGrid;

    private void Awake()
    {
        instance = this;
        ballGrid = new GameObject[rows, columns];
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateSensors();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        ballGrid = new GameObject[rows, columns];
        DebugPrintGrid();
    }

    public void OnPause()
    {
        
    }

    public void OnResume()
    {
        
    }

    private void GenerateSensors()
    {
        float xStart = -2.25f;
        float xGap = 1f;
        float yBottom = -6f;
        float yMargin = sensorPrefab.transform.localScale.y;
        float yGap = BallGenerator.instance.ballPrefab.GetComponent<CircleCollider2D>().radius * 2f + yMargin;
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Vector3 position = new Vector3(xStart + (column * xGap),
                    yBottom + yMargin + ((5 - row) * yGap), 0);
                GameObject newSensor = Instantiate(sensorPrefab, this.transform);
                newSensor.GetComponent<Sensor>().Initialize(row, column, position);
            }
        }
    }

    private void DebugPrintGrid()
    {
        String gridString = "";
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                if (ballGrid[row, column] == null) gridString += "null \t";
                else gridString += ballGrid[row, column].GetComponent<BallController>().ballType + "\t";
            }
            gridString += "\n";
        }

        Debug.Log(gridString);
    }

    // TODO: handler for triggers
    public void OnCellEntry(int row, int column, GameObject ball)
    {
        ballGrid[row, column] = ball;
        DebugPrintGrid();
        // TODO: check
    }

    public void OnCellExit(int row, int column)
    {
        ballGrid[row, column] = null;
        DebugPrintGrid();
        // TODO: check
    }
    
    // TODO: check overflow, check debris, check matches
}
