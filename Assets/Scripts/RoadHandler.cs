using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHandler
{
    public int[,] Routes { get; set; }
    public int SizeOfGraph { get; set; }

    public RoadHandler(int sizeOfGraph)
    {
        this.SizeOfGraph = sizeOfGraph;
        Routes = new int[this.SizeOfGraph, this.SizeOfGraph];
    }
}
