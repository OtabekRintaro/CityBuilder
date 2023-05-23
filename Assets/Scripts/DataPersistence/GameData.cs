using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //public Map cellgrid;

    public int generalPopulation;
    public int generalSatisfaction;
    public int generalBudget;
    public string currDate;

    public List<int> positionsX;
    public List<int> positionsZ;
    public List<int> satisfaction;
    public List<int> ages;
    public List<string> gameObjects;
    public List<int> ctzCount;

    public bool[] FreeCells;
    public string[] TypeOfCells;
    //public int[] IdOfCells;

    int size = 30;

    /// <summary>
    /// Game data constructor where it initializes the datas to be saved if it haven't been created before.
    /// </summary>
    public GameData()
    {
        positionsX = new();
        positionsZ = new();
        gameObjects = new();
        currDate = (new DateTime(1900, 1, 1)).ToString("O");
        FreeCells = new bool[size * size];
        TypeOfCells = new string[size * size];
    }
}
