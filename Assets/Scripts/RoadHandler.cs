using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHandler
{
    public int[,] Routes { get; set; }
    public int[,] MainRoad { get; set; }
    public int SizeOfGraph { get; set; }

    public static RoadHandler inst;
    /// <summary>
    /// Initializes a new instance of the RoadHandler class with a given size of graph.
    /// </summary>
    /// <param name="sizeOfGraph">The size of the graph.</param>
    public RoadHandler(int sizeOfGraph)
    {
        inst = this;
        this.SizeOfGraph = sizeOfGraph;
        Routes = new int[sizeOfGraph, sizeOfGraph];
        MainRoad = new int[sizeOfGraph, sizeOfGraph];
    }

    /// <summary>
    /// searching for closest fire departments and getting the closest path
    /// </summary>
    /// <param name="map"></param>
    /// <param name="cells"></param>
    /// <param name="src"></param>
    /// <returns></returns>
    public List<Position> bfs(Map map, Cell[,] cells, Position src)
    {
        List<Position> path = new();

        Queue<Position> queue = new();
        bool[,] used = new bool[SizeOfGraph, SizeOfGraph];
        
        used[src.x, src.z] = true;
        queue.Enqueue(new Position(src.x, src.z));

        Dictionary<Position, Position> parents = new Dictionary<Position, Position>();

        while (queue.Count > 0)
        {
            Position current = queue.Dequeue();

            if (cells[current.x, current.z].Type.Equals("FireDepartment") && !((FireDepartment)map.findMapObject(cells[current.x, current.z].ID)).isFireTruckSent)
            {
                // Construct the shortest path from start to fire department
                return ConstructPath(parents, src, current);
            }

            // Get the neighboring cells
            List<Position> neighbors = GetNeighbors(current);
            //Debug.Log(neighbors);
            foreach (var neighbor in neighbors)
            {
                if (!used[neighbor.x, neighbor.z])
                {
                    if (Routes[neighbor.x, neighbor.z] == 1 || cells[neighbor.x, neighbor.z].Type.Equals("FireDepartment"))
                    {
                        queue.Enqueue(neighbor);
                        parents[neighbor] = current;
                    }
                    used[neighbor.x, neighbor.z] = true;
                }
            }

        }


        return path;
    }

    /// <summary>
    /// Forms list of positions of the path that a fire truck should go with
    /// </summary>
    /// <param name="parents"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private List<Position> ConstructPath(Dictionary<Position, Position> parents, Position start, Position end)
    {
        List<Position> path = new List<Position>();
        Position current = end;

        while (!current.Equals(start))
        {
            path.Insert(0, current);
            current = parents[current];
        }

        path.Insert(0, start);

        return path;
    }

    /// <summary>
    /// checks if cell is in matrix range
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    private bool IsValidCell(int row, int col)
    {
        return row >= 0 && row < SizeOfGraph && col >= 0 && col < SizeOfGraph;
    }

    /// <summary>
    /// Gets neighbouring cells positions
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    private List<Position> GetNeighbors(Position cell)
    {
        List<Position> neighbors = new List<Position>();

        int[] dx = { -1, 0, 1, 0 };
        int[] dy = { 0, 1, 0, -1 };

        for (int i = 0; i < 4; i++)
        {
            int newRow = cell.x + dx[i];
            int newCol = cell.z + dy[i];

            if (IsValidCell(newRow, newCol))
            {
                neighbors.Add(new Position(newRow, newCol));
            }
        }

        return neighbors;
    }


    /// <summary>
    /// Finding closest path using dfs algorithm
    /// </summary>
    /// <param name="mapObject1"></param>
    /// <param name="mapObject2"></param>
    /// <returns></returns>
    public int FindNearestConnection(MapObject mapObject1, MapObject mapObject2)
    {
        return dfs(mapObject1, mapObject2.position.x, mapObject2.position.z, new int[SizeOfGraph, SizeOfGraph], 1);
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


    public int dfs(MapObject mapObject, int row, int col, int[,] used, int distance)
    {
        if (row < 0 || row >= SizeOfGraph || col < 0 || col >= SizeOfGraph)
            return int.MaxValue;
        if (used[row, col] == 1)
            return int.MaxValue;
        if (MainRoad[row, col] == 1)
            return int.MaxValue;
        used[row, col] = 1;
        if (Routes[row, col] == 1)
        {
            return Mathf.Min(dfs(mapObject, row + 1, col, used, distance+1),dfs(mapObject, row - 1, col, used, distance+1),dfs(mapObject, row, col + 1, used, distance+1),dfs(mapObject, row, col - 1, used, distance+1));
        }
        int lowerBoundRow = mapObject.position.x - mapObject.coverage / 2;
        int upperBoundRow = mapObject.position.x + mapObject.coverage / 2;
        int lowerBoundCol = mapObject.position.z - mapObject.coverage / 2;
        int upperBoundCol = mapObject.position.z + mapObject.coverage / 2;
        if (row >= lowerBoundRow && row <= upperBoundRow && col >= lowerBoundCol && col <= upperBoundCol)
            return distance;
        return int.MaxValue;
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
