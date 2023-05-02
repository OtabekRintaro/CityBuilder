using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dictionary<Vector2, ResidentialZone> dictResidentialZones = new Dictionary<Vector2, ResidentialZone>();

    CellGrid map;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ResidentialZone resZones = GetResidentialZones(cellGrid);
        UpdateResidentialZones(resZones, dictResidentialZones);
        CalculateSatisfaction(dictResidentialZones, cellGrid, 5, 100000);
        AdjustPopulation(dictResidentialZones);
        foreach (ResidentialZone zone in dictResidentialZones.Values)
        {
            Console.WriteLine(zone.Satisfaction);
            Console.WriteLine(zone.Population);
        }
    }

    // get array of residential zones
    public ResidentialZone[] GetResidentialZones(int[,] cellGrid)
    {
        List<ResidentialZone> residentialZones = new List<ResidentialZone>();
        
        for (int x = 1; x < cellGrid.GetLength(0) - 1; x += 3)
        {
            for (int z = 1; z < cellGrid.GetLength(1) - 1; z += 3)
            {
                if (cellGrid[x][z] == "ResidentialZone") 
                {
                    bool allIdsMatch = true;
                    for (int i = x - 1; i <= x + 1; i++)
                    {
                        for (int j = z - 1; j <= z + 1; j++)
                        {
                            if (cellGrid[i, j].ID != cellGrid[x][z].ID)
                            {
                                allIdsMatch = false;
                                break;
                            }
                        }
                        if (!allIdsMatch) break;
                    }
                    
                    if (allIdsMatch)
                    {
                        ResidentialZone residentialZone = new ResidentialZone(new Vector2(x, z), 0, 5, false, false);
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
            if (!zonesArray.Contains(zone))
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
            if (!dictZones.ContainsKey(rz.position))
            {
                toAdd.Add(rz.position);
            }
        }

        // Add the new keys to the dictionary
        foreach (Position key in toAdd)
        {
            dictZones.Add(key, new ResidentialZone(key, 0, 5, false, false));
        }
    }     
}

    
