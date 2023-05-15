using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentialZone : MapObject
{
    // public Position position;
    public int population;
    public int satisfaction;
    public bool workConnection;
    public bool mainRoadConnection;
    public House housePrefab;

    private List<House> houses = new ();

    public ResidentialZone(Position position, int population, int satisfaction, bool workConnection, bool mainRoadConnection)
    {
        this.position = position;
        this.population = population;
        this.satisfaction = satisfaction;
        this.workConnection = workConnection;
        this.mainRoadConnection = mainRoadConnection;
    }

    public ResidentialZone(){}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public static void CalculateSatisfaction(ResidentialZone resZone, Cell[,] cityMatrix, Map map, int tax, int budget)
    {
        int row = resZone.position.x;
        int col = resZone.position.z;

        // Check for police in radius 8
        int policeNearby = 0;
        for (int i = row - 8; i <= row + 8; i++)
        {
            if (i < 0 || i >= cityMatrix.GetLength(0)) continue;
            for (int j = col - 8; j <= col + 8; j++)
            {
                if (j < 0 || j >= cityMatrix.GetLength(1)) continue;
                if (map.findMapObject(new Position(i, j)) is Police)
                {
                    policeNearby++;
                }
            }
        }

        // Check for stadium in radius 12
        int stadiumNearby = 0;
        for (int i = row - 12; i <= row + 12; i++)
        {
            if (i < 0 || i >= cityMatrix.GetLength(0)) continue;
            for (int j = col - 12; j <= col + 12; j++)
            {
                if (j < 0 || j >= cityMatrix.GetLength(1)) continue;
                if (map.findMapObject(new Position(i, j)) is Stadium)
                {
                    stadiumNearby++;
                }
            }
        }

        // Check for no industrial zone in radius 10
        int industrialNearby = 0;
        for (int i = row - 10; i <= row + 10; i++)
        {
            if (i < 0 || i >= cityMatrix.GetLength(0)) continue;
            for (int j = col - 10; j <= col + 10; j++)
            {
                if (j < 0 || j >= cityMatrix.GetLength(1)) continue;
                if (map.findMapObject(new Position(i,j)) is IndustrialZone)
                {
                    industrialNearby++;
                }
            }
        }

        // Update satisfaction based on nearby zones and tax and budget
        int satisfaction = 5;
        while (policeNearby > 0)
        {
            satisfaction++; policeNearby--;
        }
        while (stadiumNearby > 0)
        {
            satisfaction++; stadiumNearby--;
        }
        while (industrialNearby > 0)
        {
            satisfaction--; industrialNearby--;
        }
        if (tax < 5) satisfaction++;
        else satisfaction--;
        if (budget < 0) satisfaction--;
        if (satisfaction > 10)
            satisfaction = 10;
        else if (satisfaction < 0)
            satisfaction = 0;

        resZone.satisfaction = satisfaction;
    }




    // version 0.3
    public static void AdjustPopulation(ResidentialZone zone) {
        if (zone.checkPublicRoadConnection())
        {
            if (zone.satisfaction >= 8) {
                zone.population += 10;
            } else if (zone.satisfaction >= 6) {
                zone.population += 5;
            } else if (zone.satisfaction >= 4) {
                zone.population += 2;
            } else if (zone.satisfaction >= 2 && zone.population > 1) {
                zone.population -= 2;
            } else{
                zone.population -= 5;
            }
            if (zone.population < 0)
                zone.population = 0;
            if (zone.population > 1000)
                zone.population = 1000;
            if (zone.houses.Count == 0 && zone.population > 500)
                zone.buildHouse();
            else if (zone.houses.Count == 1 && zone.population >= 1000)
                zone.buildHouse();
            else if (zone.houses.Count == 2 && zone.population < 1000)
                zone.demolishHouse();
            else if (zone.houses.Count == 1 && zone.population < 500)
                zone.demolishHouse();
        }
    }

    public void buildHouse()
    {
        House house = Instantiate<House>(housePrefab);

        Vector3 position;
        position.x = -2f + houses.Count * 4;
        position.y = 0;
        position.z = -3.5f + houses.Count * 7;

        houses.Add(house);
        house.transform.SetParent(this.transform);
        house.transform.localPosition = position;
    }

    public void demolishHouse()
    {
        Destroy(this.transform.GetChild(houses.Count).gameObject);
        houses.RemoveAt(houses.Count-1);
    }

    // // checks if two zones are connected
    // public float CalculateSatisfaction(int[,] city, int x, int y) {
    //     int m = city.GetLength(0);
    //     int n = city.GetLength(1);

    //     float satisfaction = 0f;

    //     // Calculate satisfaction based on police zone
    //     int policeRadius = 10;
    //     for (int i = Math.Max(0, x - policeRadius); i <= Math.Min(m - 1, x + policeRadius); i++) {
    //         for (int j = Math.Max(0, y - policeRadius); j <= Math.Min(n - 1, y + policeRadius); j++) {
    //             if (city[i, j] == 2) {  // If police zone
    //                 float distance = Mathf.Sqrt(Mathf.Pow(i - x, 2) + Mathf.Pow(j - y, 2));
    //                 if (distance <= policeRadius) {  // If within radius
    //                     if (IsConnected(city, x, y, i, j)) {  // If connected by road
    //                         satisfaction += (policeRadius - distance) / policeRadius;
    //                     }
    //                 }
    //             }
    //         }
    //     }

    //     // Calculate satisfaction based on stadium zone
    //     int stadiumRadius = 10;
    //     for (int i = Math.Max(0, x - stadiumRadius); i <= Math.Min(m - 1, x + stadiumRadius); i++) {
    //         for (int j = Math.Max(0, y - stadiumRadius); j <= Math.Min(n - 1, y + stadiumRadius); j++) {
    //             if (city[i, j] == 3) {  // If stadium zone
    //                 float distance = Mathf.Sqrt(Mathf.Pow(i - x, 2) + Mathf.Pow(j - y, 2));
    //                 if (distance <= stadiumRadius) {  // If within radius
    //                     if (IsConnected(city, x, y, i, j)) {  // If connected by road
    //                         satisfaction += (stadiumRadius - distance) / stadiumRadius;
    //                     }
    //                 }
    //             }
    //         }
    //     }

    //     // Calculate satisfaction based on industrial zone
    //     for (int i = 0; i < m; i++) {
    //         for (int j = 0; j < n; j++) {
    //             if (city[i, j] == 4) {  // If industrial zone
    //                 if (IsConnected(city, x, y, i, j)) {  // If connected by road
    //                     satisfaction += 1f / (m * n);
    //                 }
    //             }
    //         }
    //     }

    //     // Calculate satisfaction based on forest zone
    //     int forestRadius = 7;
    //     for (int i = Math.Max(0, x - forestRadius); i <= Math.Min(m - 1, x + forestRadius); i++) {
    //         for (int j = Math.Max(0, y - forestRadius); j <= Math.Min(n - 1, y + forestRadius); j++) {
    //             if (city[i, j] == 5) {  // If forest zone
    //                 float distance = Mathf.Sqrt(Mathf.Pow(i - x, 2) + Mathf.Pow(j - y, 2));
    //                 if (distance <= forestRadius) {  // If within radius
    //                     if (IsConnected(city, x, y, i, j)) {  // If connected by road
    //                         satisfaction += (forestRadius - distance) / forestRadius;
    //                     }
    //                 }
    //             }
    //         }
    //     }

    //     return Mathf.Clamp01(satisfaction);
    // }


    // // version main
    // public static float CalculateSatisfaction(int[,] city, int[,] residences, int[,] police, int[,] stadium, int[,] industrial, int[,] forest, int x, int y)
    // {
    //     float satisfaction = 0;
    //     float num_zones = 0;

    //     // Check police zone
    //     for (int i = Math.Max(0, x - 10); i <= Math.Min(city.GetLength(0) - 1, x + 10); i++)
    //     {
    //         for (int j = Math.Max(0, y - 10); j <= Math.Min(city.GetLength(1) - 1, y + 10); j++)
    //         {
    //             if (police[i, j] == 1 && IsConnected(city, x, y, i, j, out int distance))
    //             {
    //                 satisfaction += (11 - distance) / 10f; // Add to satisfaction, higher for closer police
    //                 num_zones++;
    //             }
    //         }
    //     }

    //     // Check stadium zone
    //     for (int i = Math.Max(0, x - 10); i <= Math.Min(city.GetLength(0) - 1, x + 10); i++)
    //     {
    //         for (int j = Math.Max(0, y - 10); j <= Math.Min(city.GetLength(1) - 1, y + 10); j++)
    //         {
    //             if (stadium[i, j] == 1 && IsConnected(city, x, y, i, j, out int distance))
    //             {
    //                 satisfaction += (11 - distance) / 10f; // Add to satisfaction, higher for closer stadium
    //                 num_zones++;
    //             }
    //         }
    //     }

    //     // Check industrial zone
    //     for (int i = 0; i < city.GetLength(0); i++)
    //     {
    //         for (int j = 0; j < city.GetLength(1); j++)
    //         {
    //             if (industrial[i, j] == 1 && IsConnected(city, x, y, i, j, out int distance))
    //             {
    //                 satisfaction += 1 / (1 + distance); // Add to satisfaction, higher for closer and connected industrial
    //                 num_zones++;
    //             }
    //         }
    //     }

    //     // Check forest zone
    //     for (int i = Math.Max(0, x - 7); i <= Math.Min(city.GetLength(0) - 1, x + 7); i++)
    //     {
    //         for (int j = Math.Max(0, y - 7); j <= Math.Min(city.GetLength(1) - 1, y + 7); j++)
    //         {
    //             if (forest[i, j] == 1 && IsConnected(city, x, y, i, j, out int distance))
    //             {
    //                 satisfaction += (8 - distance) / 10f; // Add to satisfaction, higher for closer forest
    //                 num_zones++;
    //             }
    //         }
    //     }

    //     // Average the satisfaction increase over the number of zones checked
    //     if (num_zones > 0)
    //     {
    //         satisfaction /= num_zones;
    //     }

    //     return satisfaction;
    // }


    // // population
    // public void AdjustPopulation(int[,] population, int[,] satisfaction, int x, int y) {
    //     // Determine the current population and satisfaction level
    //     int currPopulation = population[x, y];
    //     int currSatisfaction = satisfaction[x, y];

    //     // Calculate the new population based on the satisfaction level
    //     int newPopulation = currPopulation;
    //     if (currSatisfaction < 30) {
    //         newPopulation = (int)(currPopulation * 0.9);
    //     } else if (currSatisfaction > 70) {
    //         newPopulation = (int)(currPopulation * 1.1);
    //     }

    //     // Update the population in the residence zone
    //     population[x, y] = newPopulation;
    // }

    // // population main
    // public void AdjustPopulationBySatisfaction(int x, int y) {
    //     // Get the residence zone object based on the given coordinates
    //     ResidenceZone zone = GetResidenceZone(x, y);

    //     // Calculate the current satisfaction level of the zone
    //     float satisfaction = CalculateSatisfaction(zone);

    //     // Adjust the population based on the satisfaction level
    //     if (satisfaction >= 0.75f) {
    //         zone.Population += 10;
    //     } else if (satisfaction >= 0.5f) {
    //         zone.Population += 5;
    //     } else if (satisfaction >= 0.25f) {
    //         zone.Population -= 5;
    //     } else {
    //         zone.Population -= 10;
    //     }
    // }

    // public int IsConnected(int[,] city, int x1, int y1, int x2, int y2)
    // {
    //     int m = city.GetLength(0);
    //     int n = city.GetLength(1);

    //     bool[,] visited = new bool[m, n];
    //     Queue<(int, int, int)> queue = new Queue<(int, int, int)>();

    //     queue.Enqueue((x1, y1, 0));
    //     visited[x1, y1] = true;

    //     while (queue.Count > 0)
    //     {
    //         (int x, int y, int dist) = queue.Dequeue();

    //         if (x == x2 && y == y2 && city[x, y] == 1)
    //         {
    //             return dist;
    //         }

    //         if (x > 0 && city[x-1, y] == 1 && !visited[x-1, y])
    //         {
    //             queue.Enqueue((x-1, y, dist+1));
    //             visited[x-1, y] = true;
    //         }

    //         if (y > 0 && city[x, y-1] == 1 && !visited[x, y-1])
    //         {
    //             queue.Enqueue((x, y-1, dist+1));
    //             visited[x, y-1] = true;
    //         }

    //         if (x < m-1 && city[x+1, y] == 1 && !visited[x+1, y])
    //         {
    //             queue.Enqueue((x+1, y, dist+1));
    //             visited[x+1, y] = true;
    //         }

    //         if (y < n-1 && city[x, y+1] == 1 && !visited[x, y+1])
    //         {
    //             queue.Enqueue((x, y+1, dist+1));
    //             visited[x, y+1] = true;
    //         }
    //     }

    //     return -1;
    // }

    // // checking if there a specific zone in radius for residence zone 
    // public bool IsZoneInRadius(int[,] city, int zoneType, int zoneRadius, int x, int y)
    // {
    //     int m = city.GetLength(0);
    //     int n = city.GetLength(1);
        
    //     // zone validation
    //     if (zoneType < 0 || zoneType >= 5)
    //     {
    //         throw new ArgumentException("Invalid zone type.");
    //     }

    //     // check is it residence zone or not
    //     if (city[x, y] != 2)
    //     {
    //         throw new ArgumentException("Invalid residence zone position.");
    //     }

    //     // check if there is a zone of the specified type within the specified radius
    //     for (int i = Math.Max(x - zoneRadius, 0); i <= Math.Min(x + zoneRadius, m - 1); i++)
    //     {
    //         for (int j = Math.Max(y - zoneRadius, 0); j <= Math.Min(y + zoneRadius, n - 1); j++)
    //         {
    //             if (city[i, j] == zoneType)
    //             {
    //                 // check if the zone is connected to the residence zone by road
    //                 if (IsConnected(city, x, y, i, j))
    //                 {
    //                     return true;
    //                 }
    //             }
    //         }
    //     }

    //     // No zone of the specified type was found within the specified radius
    //     return false;
    // }
}
