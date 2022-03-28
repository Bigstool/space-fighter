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
        CheckGrid();
    }

    public void OnCellExit(int row, int column)
    {
        ballGrid[row, column] = null;
        CheckGrid();
    }
    
    // Checks overflow, debris, and match TODO: check valid match (no mid-air matches)
    private void CheckGrid()
    {
        // Check overflow
        for (int column = 0; column < columns; column++)  // Check every column
        {
            if (ballGrid[0, column] != null)  // If the overflow column is not empty
            {
                bool overflow = true;
                for (int row = 1; row < rows; row++)  // Look into each row of the column to see if there's still empty space
                {
                    if (ballGrid[row, column] == null)
                    {
                        overflow = false;
                        break;
                    }
                }

                if (overflow) GameManager.instance.OnGameOver(GameOverState.overflow);  // if overflow, end the game
            }
        }
        
        // check debris
        int numDebris = 0;
        for (int row = 1; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                if (ballGrid[row, column] != null && ballGrid[row, column].GetComponent<BallController>().ballType == BallType.debris && CheckValid(row, column)) numDebris++;
            }
        }
        if (numDebris > 5) GameManager.instance.OnGameOver(GameOverState.debris);
        
        // Check match
        List<GameObject> match = new List<GameObject>();
        
        // check column match
        for (int row = 1; row < rows; row++)
        {
            BallType previousType = BallType.debris;
            int sameCount = 1;
            bool found = false;
            
            for (int column = 0; column < columns; column++)
            {
                if (ballGrid[row, column] != null && CheckValid(row, column))
                {
                    BallType nextType = ballGrid[row, column].GetComponent<BallController>().ballType;
                    if (nextType != BallType.debris && previousType == nextType) sameCount++;
                    else
                    {
                        previousType = nextType;
                        sameCount = 1;
                    }
                }
                else
                {
                    previousType = BallType.debris;
                    sameCount = 1;
                }
                if (sameCount == 3)
                {
                    for (int i = 0; i < sameCount; i++) match.Add(ballGrid[row, column - i]);
                    found = true;
                    break;
                }
            }

            if (found) break;
        }
        
        // check row match
        for (int column = 0; column < columns; column++)
        {
            BallType previousType = BallType.debris;
            int sameCount = 1;
            bool found = false;

            for (int row = 5; row > 0; row--)
            {
                if (ballGrid[row, column] != null && CheckValid(row, column))
                {
                    BallType nextType = ballGrid[row, column].GetComponent<BallController>().ballType;
                    if (nextType != BallType.debris && previousType == nextType) sameCount++;
                    else
                    {
                        previousType = nextType;
                        sameCount = 1;
                    }
                }
                else
                {
                    previousType = BallType.debris;
                    sameCount = 1;
                }
                if (sameCount == 3)
                {
                    for (int i = 0; i < sameCount; i++) if (!match.Contains(ballGrid[row + i, column])) match.Add(ballGrid[row + i, column]);
                    found = true;
                    break;
                }
            }

            if (found) break;
        }
        
        if (match.Count > 0) GameManager.instance.OnMatch(match);
    }

    // check valid match (no mid-air matches)
    private bool CheckValid(int row, int column)
    {
        for (; row < rows; row++) if (ballGrid[row, column] == null) return false;
        return true;
    }
}
