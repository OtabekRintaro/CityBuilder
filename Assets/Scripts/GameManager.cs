using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public Dictionary<Position, ResidentialZone> dictResidentialZones = new Dictionary<Position, ResidentialZone>();
    public int generalSatisfaction = 0;
    public int generalPopulation = 0;
    public int generalBudget = 20000;
    public InfoBar infoBar;
    private DateTime currentDate = new DateTime(1900, 1, 1);
    private DateTime currentMonth = new DateTime(1900, 1, 1);

    public Button okButton;
    public GameObject panel;

    [SerializeField]
    Map map;

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerLost())
        {
            LostPanelShow();
        }
        spendMoney();
        infoBar.budgetHandler.number = generalBudget;
        if (!infoBar.dateHandler.isPaused && infoBar.dateHandler.hasPassed3Seconds(currentDate))
        {
            currentDate = infoBar.dateHandler.currentDate;
            Cell[,] cellGrid = map.cells;
            IndustrialZone[] indZones = GetIndustrialZones(cellGrid);
            ResidentialZone[] resZones = GetResidentialZones(cellGrid);
            CommercialZone[] comZones = GetCommercialZones(cellGrid);
            UpdateResidentialZones(resZones, dictResidentialZones);
            generalPopulation = 0;
            generalSatisfaction = 5;
            foreach (ResidentialZone zone in dictResidentialZones.Values)
            {
                ResidentialZone.CalculateSatisfaction(zone, cellGrid, map, infoBar.taxHandler.taxValue, infoBar.budgetHandler.number);
                ResidentialZone.AdjustPopulation(zone);
                generalPopulation += zone.population;
                generalSatisfaction = (generalSatisfaction + zone.satisfaction) / 2;
            }
            foreach (IndustrialZone zone in indZones)
            {
                if (!zone.hasFactory() && zone.checkPublicRoadConnection())
                    zone.buildFactory();
            }
            foreach (CommercialZone zone in comZones)
            {
                if (!zone.hasCommercialBuildings() && zone.checkPublicRoadConnection())
                    zone.buildCommercialBuildings();
            }
            infoBar.populationHandler.number = generalPopulation;
            infoBar.satisfactionHandler.number = generalSatisfaction;
        }
        if (infoBar.dateHandler.hasPassedMonth(currentMonth))
        {
            generalBudget += infoBar.taxHandler.taxValue * 1000;
            infoBar.budgetHandler.number = generalBudget;
            currentMonth = infoBar.dateHandler.currentDate;
            currentDate = infoBar.dateHandler.currentDate;
        }
    }
    public void LostPanelShow()
    {
        panel.SetActive(true);
        okButton.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        DataPersistenceManager.instance.NewGame();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public bool PlayerLost()
    {
        if (generalSatisfaction <= -4)
        {
            return true;
        }
        return false;
    }

    public void LoadData(GameData data)
    {
        currentDate = infoBar.dateHandler.currentDate;
        currentMonth = infoBar.dateHandler.currentDate;
        generalBudget = data.generalBudget;
        generalPopulation = data.generalPopulation;
        generalSatisfaction = data.generalSatisfaction;
        infoBar.budgetHandler.number = generalBudget;
        infoBar.populationHandler.number = generalPopulation;
        infoBar.satisfactionHandler.number = generalSatisfaction;
    }

    public void SaveData(GameData data)
    {
        data.generalBudget = generalBudget;
        data.generalPopulation = generalPopulation;
        data.generalSatisfaction = generalSatisfaction;
    }

    public void setGame()
    {

    }

    public void spendMoney()
    {
        //Debug.Log(map.spentMoney.Count > 0);   
        if (map.spentMoney.Count > 0)
        {
            generalBudget -= map.spentMoney[0];
            map.spentMoney.RemoveAt(0);
        }
    }

    public CommercialZone[] GetCommercialZones(Cell[,] cellGrid)
    {
        List<CommercialZone> commercialZones = new List<CommercialZone>();
        for (int x = 1; x < cellGrid.GetLength(0) - 1; x++)
        {
            for (int z = 1; z < cellGrid.GetLength(1) - 1; z++)
            {
                MapObject mapObject;
                if ((mapObject = map.findMapObject(new Position(x, z))) is CommercialZone)
                {
                    CommercialZone commercialZone = (CommercialZone)mapObject;
                    commercialZone.connectToPublicRoad(commercialZone.checkPublicRoadConnection());
                    commercialZones.Add(commercialZone);
                }
            }
        }
        return commercialZones.ToArray();
    }

    public IndustrialZone[] GetIndustrialZones(Cell[,] cellGrid)
    {
        List<IndustrialZone> industrialZones = new List<IndustrialZone>();
        for (int x = 1; x < cellGrid.GetLength(0) - 1; x++)
        {
            for (int z = 1; z < cellGrid.GetLength(1) - 1; z++)
            {
                MapObject mapObject;
                if ((mapObject = map.findMapObject(new Position(x, z))) is IndustrialZone)
                {
                    IndustrialZone industrialZone = (IndustrialZone)mapObject;
                    industrialZone.connectToPublicRoad(industrialZone.checkPublicRoadConnection());
                    industrialZones.Add(industrialZone);
                }
            }
        }
        return industrialZones.ToArray();
    }

    // get array of residential zones
    public ResidentialZone[] GetResidentialZones(Cell[,] cellGrid)
    {
        List<ResidentialZone> residentialZones = new List<ResidentialZone>();

        for (int x = 1; x < cellGrid.GetLength(0) - 1; x += 1)
        {
            for (int z = 1; z < cellGrid.GetLength(1) - 1; z += 1)
            {
                MapObject mapObject;
                if ((mapObject = map.findMapObject(new Position(x, z))) is ResidentialZone)
                {
                    ResidentialZone residentialZone = (ResidentialZone)mapObject;
                    residentialZone.connectToPublicRoad(residentialZone.checkPublicRoadConnection());
                    residentialZone.mainRoadConnection = residentialZone.checkPublicRoadConnection();
                    residentialZones.Add(residentialZone);
                }

                //if (cellGrid[x, z].Type == "ResidentialZone") 
                //{
                //    bool allIdsMatch = true;
                //    for (int i = x - 1; i <= x + 1; i++)
                //    {
                //        for (int j = z - 1; j <= z + 1; j++)
                //        {
                //            if (cellGrid[i, j].ID != cellGrid[x, z].ID)
                //            {
                //                allIdsMatch = false;
                //                break;
                //            }
                //        }
                //        if (!allIdsMatch) break;
                //    }                  
                //    if (allIdsMatch)
                //    {
                //        ResidentialZone residentialZone = (ResidentialZone) map.findMapObject(new Position(x, z));
                //        residentialZone.connectToPublicRoad(residentialZone.checkPublicRoadConnection());
                //        residentialZone.mainRoadConnection = residentialZone.checkPublicRoadConnection();
                //        residentialZones.Add(residentialZone);
                //    }
                //}
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
            ResidentialZone residentialZone = zonesArray[Array.FindIndex(zonesArray, elem => elem.position.Equals(key))];
            //ResidentialZone residentialZone = new ResidentialZone(key, 0, 5, false, false);

            residentialZone = (ResidentialZone)map.findMapObject(key);
            residentialZone.mainRoadConnection = residentialZone.checkPublicRoadConnection();

            zonesDict.Add(key, residentialZone);
        }
    }
}


