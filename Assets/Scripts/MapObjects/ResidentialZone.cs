using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResidentialZone : MapObject
{
    // public Position position;
    public List<Citizen> population = new List<Citizen>();
    public int satisfaction = 5;
    public bool workConnection = false;
    public bool mainRoadConnection = false;
    public House housePrefab;

    private List<House> houses = new ();

    public ResidentialZone(Position position, List<Citizen> population, int satisfaction, bool workConnection, bool mainRoadConnection)
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

        // Check for commercial zone in radius 8
        int commercialNearby = 0;
        for (int i = row - 8; i <= row + 8; i++)
        {
            if (i < 0 || i >= cityMatrix.GetLength(0)) continue;
            for (int j = col - 8; j <= col + 8; j++)
            {
                if (j < 0 || j >= cityMatrix.GetLength(1)) continue;
                if (map.findMapObject(new Position(i, j)) is CommercialZone)
                {
                    commercialNearby++;
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

        // Update satisfaction based on nearby zones, tax and budget
        int satisfaction = 5;
        while (policeNearby > 0)
        {
            satisfaction++; policeNearby--;
        }
        while (commercialNearby > 0)
        {
            satisfaction++; commercialNearby--;
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

        // Pensions
        int numberOfPensions = 0;
        foreach (Citizen ctz in resZone.population) {
            if (ctz.IsRetired)
                numberOfPensions++;
        }
        // satisfaction += (int)10*numberOfPensions/(resZone.population.Count+1);
        if((int)100*numberOfPensions/(resZone.population.Count+1) > 20)
            satisfaction++;

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
            int i = 0;
            System.Random random = new System.Random();
            if (zone.satisfaction >= 8) {
                // zone.population += 10;
                while (i<10 && zone.population.Count < 1000) {
                    zone.population.Add(new Citizen(random.Next(18, 61)));
                    //zone.population.Add(new Citizen(65));
                    i++;
                }
            } else if (zone.satisfaction >= 6) {
                // zone.population += 5;
                while (i<5 && zone.population.Count < 1000) {
                    zone.population.Add(new Citizen(random.Next(18, 61)));
                    //zone.population.Add(new Citizen(65));
                    i++;
                }
            } else if (zone.satisfaction >= 4) {
                // zone.population += 2;
                while (i<2 && zone.population.Count < 1000) {
                    zone.population.Add(new Citizen(random.Next(18, 61)));
                    //zone.population.Add(new Citizen(65));
                    i++;
                }
            } else if (zone.satisfaction >= 2) {
                // zone.population -= 2;
                while (i<2 && zone.population.Count > 0) {
                    zone.population.RemoveAt(zone.population.Count-1);
                    i++;
                }
            } else {
                // zone.population -= 5;
                while (i<5 && zone.population.Count > 0) {
                    zone.population.RemoveAt(zone.population.Count-1);
                    i++;
                }
            }
            // if (zone.population < 0)
            //     zone.population = 0;
            // if (zone.population > 1000)
            //     zone.population = 1000;
            if (zone.houses.Count == 0 && zone.population.Count > 500)
                zone.BuildHouse();
            else if (zone.houses.Count == 1 && zone.population.Count >= 1000)
                zone.BuildHouse();
            else if (zone.houses.Count == 2 && zone.population.Count < 1000)
                zone.DemolishHouse();
            else if (zone.houses.Count == 1 && zone.population.Count < 500)
                zone.DemolishHouse();
        }
    }

    public void BuildHouse()
    {
        if (fire is not null)
            return;
        House house = Instantiate<House>(housePrefab);

        Vector3 position;
        position.x = -2f + houses.Count * 4;
        position.y = 0;
        position.z = -3.5f + houses.Count * 7;

        houses.Add(house);
        house.transform.SetParent(this.transform);
        house.transform.localPosition = position;
    }

    public void DemolishHouse()
    {
        if (houses.Count == 0)
            return;
        Destroy(this.transform.GetChild(houses.Count).gameObject);
        houses.RemoveAt(houses.Count-1);
    }

    public static void ReplaceDeadCitizens(ResidentialZone resZone) {
        foreach(Citizen ctz in resZone.population) {
            if (ctz.IsDead) {
                System.Random random = new System.Random();
                ctz.Age = random.Next(18, 61);
            }
        }
    }

    public static int CalculatePensions(Queue<int> taxes, ResidentialZone zone) {
        int avgTax = 0;
        int sum = 0;
        foreach (int x in taxes) {
            sum += x;
        }
        if (taxes.Count > 0) 
            avgTax = sum/taxes.Count;
        int totalPension = 0;
        foreach (Citizen ctz in zone.population) {
            if (ctz.IsRetired)
                totalPension += 150*avgTax;
        }
        return totalPension;
    }
}
