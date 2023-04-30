using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHandler
{
    public int[,] Routes { get; set; }
    public int[,] MainRoad { get; set; }
    public int SizeOfGraph { get; set; }

    public RoadHandler(int sizeOfGraph)
    {
        this.SizeOfGraph = sizeOfGraph;
        Routes = new int[sizeOfGraph, sizeOfGraph];
        MainRoad = new int[sizeOfGraph, sizeOfGraph];
    }

    public bool findPublicRoad(MapObject mapObject)
    {
        bool found = false;

        int row = mapObject.Coordinate.x;
        int col = mapObject.Coordinate.z;

        Position bottomLeftCorner = new Position(row - (mapObject.coverage / 2), col - (mapObject.coverage / 2));

        //check left and right
        for(int z = 0; z < mapObject.coverage; z++)
        {
            found = found || predfs(bottomLeftCorner.z + z, bottomLeftCorner.x, new int[SizeOfGraph, SizeOfGraph]);
            found = found || predfs(bottomLeftCorner.z + z, bottomLeftCorner.x + mapObject.coverage - 1, new int[SizeOfGraph, SizeOfGraph]);
        }

        //check top and bottom
        for(int x = 0; x < mapObject.coverage; x++)
        {
            found = found || predfs(bottomLeftCorner.z, bottomLeftCorner.x + x, new int[SizeOfGraph, SizeOfGraph]);
            found = found || predfs(bottomLeftCorner.z + mapObject.coverage - 1, bottomLeftCorner.x + x, new int[SizeOfGraph, SizeOfGraph]);
            //Debug.Log((bottomLeftCorner.x + x) + " " + (bottomLeftCorner.z + mapObject.coverage - 1));
            //Debug.Log(predfs(bottomLeftCorner.z + mapObject.coverage - 1, bottomLeftCorner.x + x, new int[SizeOfGraph, SizeOfGraph]));
        }

        return found;
    }


    public bool predfs(int row, int col, int[,] used)
    {
        used[row, col] = 1;
        bool found = true;

        found = dfs(row - 1, col, used) || dfs(row, col - 1, used) || dfs(row + 1, col, used) || dfs(row, col + 1, used);

        return found;
    }

    public bool dfs(int row, int col, int[,] used)
    {
        if (row < 0 || row >= SizeOfGraph || col < 0 || col >= SizeOfGraph)
            return false;
        if (used[row, col] == 1)
            return false;   
        if (Routes[row, col] == 0)
            return false;
        if (MainRoad[row, col] == 1)
            return true;
        used[row, col] = 1;
        return true && (dfs(row - 1, col, used) || dfs(row, col - 1, used) || dfs(row + 1, col, used) || dfs(row, col + 1, used));
    }

    public void printRoutes()
    {
        Debug.Log(SizeOfGraph);
        for(int i = 0; i < SizeOfGraph; i++)
        {
            for(int j = 0; j < SizeOfGraph; j++)
            {
                Debug.Log(Routes[i,j] + "-");
            }
            Debug.Log(i);
        }
    }
}
