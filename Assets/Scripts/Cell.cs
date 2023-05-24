using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Position Coordinate { get; set; }
    public float X { get; set; }
    public float Z { get; set; }
    public string Type { get; set; } = "empty";
    public bool isFree { get; set; } = true;
    public int ID { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
