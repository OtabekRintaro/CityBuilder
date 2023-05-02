using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dictionary<Position, ResidentialZone> dictResidentialZones = new Dictionary<Position, ResidentialZone>();
    public int generalSatisfaction = 0;
    public int generalPopulation = 0;
    [SerializeField]
    CellGrid map;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(dictResidentialZones);
        Cell[,] cellGrid = map.cells;
        ResidentialZone[] resZones = GetResidentialZones(cellGrid);
        UpdateResidentialZones(resZones, dictResidentialZones);
        generalPopulation = 0;
        generalSatisfaction = 5;
        foreach (ResidentialZone zone in dictResidentialZones.Values)
        {
            ResidentialZone.CalculateSatisfaction(zone, cellGrid, 5, 100000);
            ResidentialZone.AdjustPopulation(zone);
            generalPopulation += zone.population;
            generalSatisfaction = (generalSatisfaction+zone.satisfaction)/2;
            Debug.Log(generalPopulation);
            Debug.Log(generalSatisfaction);
        }
    }


    // get array of residential zones
    public ResidentialZone[] GetResidentialZones(Cell[,] cellGrid)
    {
        List<ResidentialZone> residentialZones = new List<ResidentialZone>();
        
        Debug.Log(cellGrid[0, 0].X);
        for (int x = 1; x < cellGrid.GetLength(0) - 1; x += 1)
        {
            for (int z = 1; z < cellGrid.GetLength(1) - 1; z += 1)
            {
                if (cellGrid[x, z].Type == "ResidentialZone") 
                {
                    bool allIdsMatch = true;
                    for (int i = x - 1; i <= x + 1; i++)
                    {
                        for (int j = z - 1; j <= z + 1; j++)
                        {
                            if (cellGrid[i, j].ID != cellGrid[x, z].ID)
                            {
                                allIdsMatch = false;
                                break;
                            }
                        }
                        if (!allIdsMatch) break;
                    }

                    // if (map.findMapObject(new Position(x, z)) != null)

                    
                    if (allIdsMatch) // && map.findMapObject(new Position(x, z)) != null)
                    {
                        // Debug.Log(map.findMapObject(new Position(z, x)) );
                        ResidentialZone residentialZone = new ResidentialZone(new Position(x, z), 0, 5, false, false);
                        // ResidentialZone residentialZone = (ResidentialZone) map.findMapObject(new Position(x, z)); //new ResidentialZone(new Position(x, z), 0, 5, false, false);
                        // residentialZone.population = 0;
                        // residentialZone.satisfaction = 5;
                        // residentialZone.workConnection = false;
                        // residentialZone.mainRoadConnection = residentialZone.checkPublicRoadConnection();
                        residentialZones.Add(residentialZone);
                    }
                }
            }
        }
        
        return residentialZones.ToArray();
    }

    // update dictResidentialZones
    public void UpdateResidentialZones(ResidentialZone[] zonesArray, Dictionary<Position, ResidentialZone> zonesDict)
    {
        List<Position> toRemove = new List<Position>();

        // Iterate through keys in the dictionary
        foreach (Position position in zonesDict.Keys)
        {
            ResidentialZone zone = zonesDict[position];

            // If the value is not in the array, add it to the list of keys to remove
            if (!Array.Exists(zonesArray, element => zone.position.x == element.position.x && zone.position.z == element.position.z))
            {
                toRemove.Add(position);
            }
        }

        // Iterate through keys to remove and remove them from the dictionary
        foreach (Position position in toRemove)
        {
            zonesDict.Remove(position);
        }

        // Now, check which new keys to add
        List<Position> toAdd = new List<Position>();
        foreach (ResidentialZone rz in zonesArray)
        {
            if (!zonesDict.ContainsKey(rz.position))
            {
                toAdd.Add(rz.position);
            }
        }

        // Add the new keys to the dictionary
        foreach (Position key in toAdd)
        {
            zonesDict.Add(key, new ResidentialZone(key, 0, 5, false, false));
        }
    }     
}

    
