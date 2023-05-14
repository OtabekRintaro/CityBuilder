using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //public Map cellgrid;

    public String currDate;
   
    //private int width = 30;
    //private int height = 30;
    //private Cell[,] cells ;
    //private Road[,] mainRoad;
    //private RoadHandler roadHandler;
    //private Transform _transform_CellGrid;
    //private Cell cellPrefab;
    //private Road roadPrefab;
    public GameData()
    {
      //  cellgrid.generateObjects();
        currDate = (new DateTime(1900, 1, 1)).ToString("O");
        //cells = new Cell[height, width];
        //mainRoad = new Road[height, width];
        //roadHandler = new RoadHandler(height);

        //for (int row = 0; row < height; row++)
        //{
        //    for (int col = 0; col < width; col++)
        //    {
        //        //CreateCell(x, z, i++);
        //        //Debug.Log("cell content x:" + x + " z:" + z + " i:" + i);

        //        Vector3 position;
        //        position.x = -100 + col * 10;
        //        position.y = 0;
        //        position.z = -100 + row * 10;
        //        cells[row, col] = Instantiate<>(cellPrefab);
        //        Cell cell = cells[row, col] = Instantiate<>(cellPrefab);
        //        //cells[i].Coordinate = new Position(x,z);

        //        //cells[x,z].X = (int)position.x;
        //        //cells[x,z].Z = (int)position.z;

        //        cells[row, col] = cell;
        //        cells[row, col].X = position.x;
        //        cells[row, col].Z = position.z;

        //        //Debug.Log("cell coordinate (mgui) x:" + cells[i].X + " z:" + cells[i].Z);

        //        cell.transform.SetParent(_transform_CellGrid, false);
        //        cell.transform.localPosition = position;
        //    }
        //}
    }
}
