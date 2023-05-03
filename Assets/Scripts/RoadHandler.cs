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

        int row = mapObject.position.x;
        int col = mapObject.position.z;

        //Debug.Log(row + " " + col);

        Position bottomLeftCorner = new Position(row - (mapObject.coverage / 2), col - (mapObject.coverage / 2));

        //Debug.Log(bottomLeftCorner.toString());

        //check top and bottom
        for(int z = 0; z < mapObject.coverage; z++)
        {
            found = found || predfsForRoads(bottomLeftCorner.x, bottomLeftCorner.z + z, new int[SizeOfGraph, SizeOfGraph]);
            found = found || predfsForRoads(bottomLeftCorner.x + mapObject.coverage - 1, bottomLeftCorner.z + z, new int[SizeOfGraph, SizeOfGraph]);
        }

        //check left and right
        for(int x = 0; x < mapObject.coverage; x++)
        {
            found = found || predfsForRoads(bottomLeftCorner.x + x, bottomLeftCorner.z, new int[SizeOfGraph, SizeOfGraph]);
            found = found || predfsForRoads(bottomLeftCorner.x + x, bottomLeftCorner.z + mapObject.coverage - 1, new int[SizeOfGraph, SizeOfGraph]);
        }

        return found;
    }

    public bool checkConnection(MapObject mapObject, MapObject road)
    {
        bool hasConnection;

        hasConnection = bfs(mapObject, road.position.x, road.position.z, new int[SizeOfGraph, SizeOfGraph]);

        return hasConnection;
    }

    public bool predfsForRoads(int row, int col, int[,] used)
    {
        used[row, col] = 1;
        bool found;

        found = dfs(row - 1, col, used) || dfs(row, col - 1, used) || dfs(row + 1, col, used) || dfs(row, col + 1, used);

        return found;
    }

    public bool bfs(MapObject mapObject, int row, int col, int[,] used)
    {
        if (row < 0 || row >= SizeOfGraph || col < 0 || col >= SizeOfGraph)
            return false;
        if (used[row, col] == 1)
            return false;
        if (MainRoad[row, col] == 1)
            return false;
        used[row, col] = 1;
        if(Routes[row,col] == 1)
        {
            return bfs(mapObject, row + 1, col, used) || bfs(mapObject, row - 1, col, used) || bfs(mapObject, row, col + 1, used) || bfs(mapObject, row, col - 1, used);
        }
        int lowerBoundRow = mapObject.position.x - mapObject.coverage / 2;
        int upperBoundRow = mapObject.position.x + mapObject.coverage / 2;
        int lowerBoundCol = mapObject.position.z - mapObject.coverage / 2;
        int upperBoundCol = mapObject.position.z + mapObject.coverage / 2;
        if (row >= lowerBoundRow && row <= upperBoundRow && col >= lowerBoundCol && col <= upperBoundCol)
            return true;
        return false;
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
