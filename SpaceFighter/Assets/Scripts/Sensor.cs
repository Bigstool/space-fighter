using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public int row;
    public int column;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Initialize(int row, int column, Vector3 position)
    {
        this.row = row;
        this.column = column;
        GetComponent<Transform>().position = position;
    }

    public void OnPause()
    {
        
    }

    public void OnResume()
    {
        
    }
    
    // Assumes that only one ball can enter the trigger at a time
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball") SensorGrid.instance.OnCellEntry(row, column, col.gameObject);
    }
    
    // Assumes that only one ball can exit the trigger at a time
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball") SensorGrid.instance.OnCellExit(row, column);
    }
}
