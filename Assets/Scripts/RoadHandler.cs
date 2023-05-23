using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHandler
{
    public int[,] Routes { get; set; }
    public int[,] MainRoad { get; set; }
    public int SizeOfGraph { get; set; }

    /// <summary>
    /// Initializes a new instance of the RoadHandler class with a given size of graph.
    /// </summary>
    /// <param name="sizeOfGraph">The size of the graph.</param>
    public RoadHandler(int sizeOfGraph)
    {
        this.SizeOfGraph = sizeOfGraph;
        Routes = new int[sizeOfGraph, sizeOfGraph];
        MainRoad = new int[sizeOfGraph, sizeOfGraph];
    }

    /// <summary>
    /// Finds a public road for a given map object.
    /// </summary>
    /// <param name="mapObject">The map object to find a public road for.</param>
    /// <returns>True if a public road is found, false otherwise.</returns>
    public bool findPublicRoad(MapObject mapObject)
    {
        bool found = false;

        int row = mapObject.position.x;
        int col = mapObject.position.z;

        if (mapObject.coverage == 1)
        {
            found = CheckCoverageOne(mapObject, row, col) || found;
            return found;
        }

        Position bottomLeftCorner = new Position(row - (mapObject.coverage / 2), col - (mapObject.coverage / 2));

        found = CheckTopAndBottom(mapObject, bottomLeftCorner) || found;
        found = CheckLeftAndRight(mapObject, bottomLeftCorner) || found;

        return found;
    }

    /// <summary>
    /// Checks if a public road is found for a map object with coverage of one.
    /// </summary>
    /// <param name="mapObject">The map object to find a public road for.</param>
    /// <param name="row">The row of the map object's position.</param>
    /// <param name="col">The column of the map object's position.</param>
    /// <returns>True if a public road is found, false otherwise.</returns>
    public bool CheckCoverageOne(MapObject mapObject, int row, int col)
    {
        bool found = false;
        found = predfsForRoads(mapObject, row, col, new int[SizeOfGraph, SizeOfGraph], false) || found;
        found = predfsForRoads(mapObject, row, col, new int[SizeOfGraph, SizeOfGraph], true) || found;
        return found;
    }

    /// <summary>
    /// Checks if a public road is found for the top and bottom sides of a map object.
    /// </summary>
    /// <param name="mapObject">The map object to find a public road for.</param>
    /// <param name="bottomLeftCorner">The bottom left corner position of the map object.</param>
    /// <returns>True if a public road is found on the top or bottom side of the map object, false otherwise.</returns>
    public bool CheckTopAndBottom(MapObject mapObject, Position bottomLeftCorner)
    {
        bool found = false;
        for (int z = 0; z < mapObject.coverage; z++)
        {
            found = predfsForRoads(mapObject, bottomLeftCorner.x + mapObject.coverage - 1, bottomLeftCorner.z + z, new int[SizeOfGraph, SizeOfGraph], false) || found;
            found = predfsForRoads(mapObject, bottomLeftCorner.x, bottomLeftCorner.z + z, new int[SizeOfGraph, SizeOfGraph], false) || found;
        }
        return found;
    }

    /// <summary>
    /// Checks if a public road is found for the left and right sides of a map object.
    /// </summary>
    /// <param name="mapObject">The map object to find a public road for.</param>
    /// <param name="bottomLeftCorner">The bottom left corner position of the map object.</param>
    /// <returns>True if a public road is found on the left or right side of the map object, false otherwise.</returns>
    public bool CheckLeftAndRight(MapObject mapObject, Position bottomLeftCorner)
    {
        bool found = false;
        for (int x = 0; x < mapObject.coverage; x++)
        {
            found = predfsForRoads(mapObject, bottomLeftCorner.x + x, bottomLeftCorner.z, new int[SizeOfGraph, SizeOfGraph], true) || found;
            found = predfsForRoads(mapObject, bottomLeftCorner.x + x, bottomLeftCorner.z + mapObject.coverage - 1, new int[SizeOfGraph, SizeOfGraph], true) || found;
        }
        return found;
    }

    /// <summary>
    /// Performs a depth-first search to find a public road for a given map object.
    /// </summary>
    /// <param name="mapObject">The map object to find a public road for.</param>
    /// <param name="row">The row to start the search from.</param>
    /// <param name="col">The column to start the search from.</param>
    /// <param name="used">A 2D array to keep track of visited cells.</param>
    /// <param name="isSides">True if searching on the sides of the map object, false if searching on the top and bottom.</param>
    /// <returns>True if a public road is found, false otherwise.</returns>
    public bool predfsForRoads(MapObject mapObject, int row, int col, int[,] used, bool isSides)
    {
        used[row, col] = 1;
        bool found;

        if (isSides){
            found = dfs(row, col - 1, used) || dfs(row, col + 1, used);
        }
        else{
            found = dfs(row - 1, col, used) || dfs(row + 1, col, used);
        }
        if (found && (mapObject is not Road && mapObject is not Forest)){
            mapObject.publicRoads++;
        }

        return found;
    }

    /// <summary>
    /// Performs a depth-first search to find a public road.
    /// </summary>
    /// <param name="row">The row to start the search from.</param>
    /// <param name="col">The column to start the search from.</param>
    /// <param name="used">A 2D array to keep track of visited cells.</param>
    /// <returns>True if a public road is found, false otherwise.</returns>
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

    /// <summary>
    /// Checks if there is a connection between a given map object and a given road.
    /// </summary>
    /// <param name="mapObject">The map object to check for connection with the road.</param>
    /// <param name="row">The row of the road's position.</param>
    /// <param name="col">The column of the road's position.</param>
    /// <returns>True if there is a connection between the map object and the road, false otherwise.</returns>
    public bool checkConnection(MapObject mapObject, MapObject road)
    {
        bool hasConnection;
        hasConnection = bfs(mapObject, road.position.x, road.position.z, new int[SizeOfGraph, SizeOfGraph]);

        return hasConnection;
    }

    /// <summary>
    /// Performs a breadth-first search to find a connection between a given map object and a given road.
    /// </summary>
    /// <param name="mapObject">The map object to check for connection with the road.</param>
    /// <param name="row">The row to start the search from.</param>
    /// <param name="col">The column to start the search from.</param>
    /// <param name="used">A 2D array to keep track of visited cells.</param>
    /// <returns>True if there is a connection between the map object and the road, false otherwise.</returns>
    public bool bfs(MapObject mapObject, int row, int col, int[,] used)
    {
        if (row < 0 || row >= SizeOfGraph || col < 0 || col >= SizeOfGraph)
            return false;
        if (used[row, col] == 1)
            return false;
        if (MainRoad[row, col] == 1)
            return false;
        used[row, col] = 1;
        if (Routes[row, col] == 1)
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

    /// <summary>
    /// Prints the routes in the console for debugging purposes.
    /// </summary>
    public void printRoutes()
    {
        Debug.Log(SizeOfGraph);
        for (int i = 0; i < SizeOfGraph; i++)
        {
            for (int j = 0; j < SizeOfGraph; j++)
            {
                Debug.Log(Routes[i, j] + "-");
            }
            Debug.Log(i);
        }
    }
}
