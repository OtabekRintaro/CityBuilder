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

        if (mapObject.coverage == 1)
        {
            found = predfsForRoads(mapObject, row, col, new int[SizeOfGraph, SizeOfGraph], false) || found;
            found = predfsForRoads(mapObject, row, col, new int[SizeOfGraph, SizeOfGraph], true) || found;

            return found;
        }

        Position bottomLeftCorner = new Position(row - (mapObject.coverage / 2), col - (mapObject.coverage / 2));


        //check top and bottom
        for(int z = 0; z < mapObject.coverage; z++)
        {
            found = predfsForRoads(mapObject, bottomLeftCorner.x + mapObject.coverage - 1, bottomLeftCorner.z + z, new int[SizeOfGraph, SizeOfGraph], false) || found;
            found = predfsForRoads(mapObject, bottomLeftCorner.x, bottomLeftCorner.z + z, new int[SizeOfGraph, SizeOfGraph], false) || found;
        }

        //check left and right
        for(int x = 0; x < mapObject.coverage; x++)
        {
            found = predfsForRoads(mapObject, bottomLeftCorner.x + x, bottomLeftCorner.z, new int[SizeOfGraph, SizeOfGraph], true) || found;
            found = predfsForRoads(mapObject, bottomLeftCorner.x + x, bottomLeftCorner.z + mapObject.coverage - 1, new int[SizeOfGraph, SizeOfGraph], true) || found;
        }

        return found;
    }

    public bool predfsForRoads(MapObject mapObject, int row, int col, int[,] used, bool isSides)
    {
        used[row, col] = 1;
        bool found;

        if(isSides)
        {
            found = dfs(row, col - 1, used) || dfs(row, col + 1, used);
        }
        else
        {
            found = dfs(row - 1, col, used) || dfs(row + 1, col, used);
        }
        
        if(found && ( mapObject is not Road && mapObject is not Forest))
        {
            mapObject.publicRoads++;
        }

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

    public bool checkConnection(MapObject mapObject, MapObject road)
    {
        bool hasConnection;

        hasConnection = bfs(mapObject, road.position.x, road.position.z, new int[SizeOfGraph, SizeOfGraph]);

        return hasConnection;
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
