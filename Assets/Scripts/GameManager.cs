using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public Dictionary<Position, ResidentialZone> dictResidentialZones = new Dictionary<Position, ResidentialZone>();
    public int generalSatisfaction = 5;
    public int generalPopulation = 0;
    public int generalBudget = 20000;
    public InfoBar infoBar;
    private DateTime currentDate = new DateTime(1900, 1, 1);
    private DateTime currentMonth = new DateTime(1900, 1, 1);
    private DateTime currentYear = new DateTime(1900, 1, 1);
    private Queue<int> lastTaxes = new Queue<int>();

    public Button okButton;
    public GameObject panel;

    [SerializeField]
    Map map;

    /// <summary>
    /// Sets the panel to inactive.
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        HandlePlayerLost();
        SpendMoney();
        UpdateBudget();
        CheckFire();
        if (ShouldUpdateZones())
        {
            UpdateZones();
            SpreadFire();
        }
        if (HasPassedMonth())
        {
            UpdateBudgetForNewMonth();
        }
        if (HasPassedYear())
        {
            AgeCitizens();
            UpdateTaxes();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            SpreadFire();
        }
    }


    /// <summary>
    /// Checks fire condition in every map object
    /// </summary>
    void CheckFire()
    {
        foreach(MapObject mapObj in map.mapObjects)
        {
            if (mapObj.fire is not null && mapObj is ResidentialZone or IndustrialZone or CommercialZone && infoBar.dateHandler.currentDate.Subtract(mapObj.fire.startOfFire).Days >= 30)
            {
                MapObject.DestroyBuildings(mapObj);
            }
        }
    }

    /// <summary>
    /// Checks for available fire departments and send the closest fire truck available
    /// </summary>
    public void SendFireTruck()
    {
        int id = BuildingPlacer.inst.CellToBeDeleted.ID;
       // Debug.Log(BuildingPlacer.inst.CellToBeDeleted is null);
        MapObject mapObject = map.findMapObject(id);

        //Debug.Log(id);
        //FireDepartment fireDepartment = (FireDepartment) map.FindNearestFireTruck(mapObject);
        if(mapObject.fire is not null && !mapObject.IsFireInformed)
        {
            mapObject.IsFireInformed = true;
            FireDepartment.SendFireTruck(infoBar.dateHandler, map, map.cells,mapObject);
        }
    }
    /// <summary>
    /// Spreads fire randomly across the map objects
    /// </summary>
    void SpreadFire()
    {
        System.Random chanceGenerator = new System.Random();
        foreach(MapObject mapObject in map.mapObjects)
        {
            if(mapObject is FireDepartment or Road or Forest || mapObject.fire is not null)
            {
                continue;
            }

            int min = 1;
            int max = 100;
            if(mapObject is IndustrialZone)
            {
                max += 5;
            }
            if(mapObject.IsFireDepartmentNearby)
            {
                max -= 3;
            }
            int chance = chanceGenerator.Next(min, max);
            
            if(chance >= 95)
            {
                mapObject.SetFire(infoBar.dateHandler);
            }
        
        }
    }

    /// <summary>
    /// Checks if the player has lost and shows the lost panel if they have.
    /// </summary>
    void HandlePlayerLost()
    {
        if (PlayerLost())
        {
            LostPanelShow();
        }
    }

    /// <summary>
    /// Spends money.
    /// </summary>
    void SpendMoney()
    {
        spendMoney();
    }

    /// <summary>
    /// Updates the budget.
    /// </summary>
    void UpdateBudget()
    {
        infoBar.budgetHandler.number = generalBudget;
    }

    /// <summary>
    /// Checks if the zones should be updated.
    /// </summary>
    bool ShouldUpdateZones()
    {
        return !infoBar.dateHandler.isPaused && infoBar.dateHandler.hasPassed3Seconds(currentDate);
    }

    /// <summary>
    /// Updates the zones.
    /// </summary>
    void UpdateZones()
    {
        currentDate = infoBar.dateHandler.currentDate;
        Cell[,] cellGrid = map.cells;
        IndustrialZone[] indZones = GetIndustrialZones(cellGrid);
        ResidentialZone[] resZones = GetResidentialZones(cellGrid);
        CommercialZone[] comZones = GetCommercialZones(cellGrid);
        UpdateResidentialZones(resZones, dictResidentialZones);
        generalPopulation = 0;
        int numberOfResZones = 0;
        int satisfactionSum = 0;

        foreach (ResidentialZone zone in dictResidentialZones.Values)
        {
            UpdateResidentialZone(zone, cellGrid);
            generalPopulation += zone.population.Count;
            numberOfResZones++;
            satisfactionSum += zone.satisfaction;
        }

        if (numberOfResZones > 0)
            generalSatisfaction = satisfactionSum / numberOfResZones;

        foreach (IndustrialZone zone in indZones)
        {
            UpdateIndustrialZone(zone);
        }

        foreach (CommercialZone zone in comZones)
        {
            UpdateCommercialZone(zone);
        }

        infoBar.populationHandler.number = generalPopulation;
        infoBar.satisfactionHandler.number = generalSatisfaction;
    }

    /// <summary>
    /// Updates a residential zone.
    /// </summary>
    void UpdateResidentialZone(ResidentialZone zone, Cell[,] cellGrid)
    {
        ResidentialZone.ReplaceDeadCitizens(zone);
        ResidentialZone.CalculateSatisfaction(zone, cellGrid, map, infoBar.taxHandler.taxValue, infoBar.budgetHandler.number);
        ResidentialZone.AdjustPopulation(zone);
    }

    /// <summary>
    /// Updates an industrial zone.
    /// </summary>
    void UpdateIndustrialZone(IndustrialZone zone)
    {
        if (!zone.hasFactory() && zone.checkPublicRoadConnection())
            zone.buildFactory();
    }

    /// <summary>
    /// Updates a commercial zone.
    /// </summary>
    void UpdateCommercialZone(CommercialZone zone)
    {
        if (!zone.hasCommercialBuildings() && zone.checkPublicRoadConnection())
            zone.buildCommercialBuildings();
    }

    /// <summary>
    /// Checks if a month has passed.
    /// </summary>
    bool HasPassedMonth()
    {
        return infoBar.dateHandler.hasPassedMonth(currentMonth);
    }

    /// <summary>
    /// Updates the budget for a new month.
    /// </summary>
    void UpdateBudgetForNewMonth()
    {
        foreach (ResidentialZone zone in dictResidentialZones.Values)
        {
            generalBudget -= ResidentialZone.CalculatePensions(lastTaxes, zone);
        }

        generalBudget += 5000 * infoBar.taxHandler.taxValue;
        infoBar.budgetHandler.number = generalBudget;
        currentMonth = infoBar.dateHandler.currentDate;
        currentDate = infoBar.dateHandler.currentDate;
    }

    /// <summary>
    /// Checks if a year has passed.
    /// </summary>
    bool HasPassedYear()
    {
        return infoBar.dateHandler.hasPassedYear(currentYear);
    }

    /// <summary>
    /// Ages the citizens by one year.
    /// </summary>
    void AgeCitizens()
    {
        foreach (ResidentialZone zone in dictResidentialZones.Values)
        {
            foreach (Citizen ctz in zone.population)
            {
                ctz.AgeOneYear();
                //Debug.Log(ctz.Age);
            }
        }

        currentYear = infoBar.dateHandler.currentDate;
        currentMonth = infoBar.dateHandler.currentDate;
        currentDate = infoBar.dateHandler.currentDate;
    }

    /// <summary>
    /// Updates the taxes.
    /// </summary>
    void UpdateTaxes()
    {
        lastTaxes.Enqueue(infoBar.taxHandler.taxValue);
        if (lastTaxes.Count > 20)
            lastTaxes.Dequeue();
    }

    /// <summary>
    /// Shows the lost panel and adds a listener to the okButton to quit the game.
    /// </summary>
    public void LostPanelShow()
    {
        panel.SetActive(true);
        okButton.onClick.AddListener(QuitGame);
    }

    /// <summary>
    /// Quits the game and starts a new game.
    /// </summary>
    private void QuitGame()
    {
        DataPersistenceManager.instance.NewGame();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    /// <summary>
    /// Checks if the player has lost by checking if the general satisfaction is less than or equal to -4.
    /// </summary>
    /// <returns></returns>
    public bool PlayerLost()
    {
        if (generalSatisfaction <= -4)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Loads data from a GameData object.
    /// </summary>
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

    /// <summary>
    /// Saves data to a GameData object.
    /// </summary>
    public void SaveData(GameData data)
    {
        data.generalBudget = generalBudget;
        data.generalPopulation = generalPopulation;
        data.generalSatisfaction = generalSatisfaction;
    }

    /// <summary>
    /// Spends money by removing the first element from the spentMoney list and subtracting it from the general budget.
    /// </summary>
    public void spendMoney()
    {
        if (map.spentMoney.Count > 0)
        {
            generalBudget -= map.spentMoney[0];
            map.spentMoney.RemoveAt(0);
        }
    }
    /// <summary>
    /// Gets an array of commercial zones from a cell grid.
    /// </summary>
    /// <param name="cellGrid"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Gets an array of industrial zones from a cell grid.
    /// </summary>
    /// <param name="cellGrid"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Gets an array of residential zones from a cell grid.
    /// </summary>
    /// <param name="cellGrid"></param>
    /// <returns></returns>

    // get array of residential zones
    public ResidentialZone[] GetResidentialZones(Cell[,] cellGrid){
        List<ResidentialZone> residentialZones = new List<ResidentialZone>();
        for (int x = 1; x < cellGrid.GetLength(0) - 1; x += 1){
            for (int z = 1; z < cellGrid.GetLength(1) - 1; z += 1){
                MapObject mapObject;
                if ((mapObject = map.findMapObject(new Position(x, z))) is ResidentialZone){
                    ResidentialZone residentialZone = (ResidentialZone)mapObject;
                    residentialZone.connectToPublicRoad(residentialZone.checkPublicRoadConnection());
                    residentialZone.mainRoadConnection = residentialZone.checkPublicRoadConnection();
                    residentialZones.Add(residentialZone);
                }
            }
        }

        return residentialZones.ToArray();
    }

    /// <summary>
    /// This function updates the dictionary of residential zones by removing old zones that are not in the array and adding
    /// new zones that are in the array but not in the dictionary.
    /// </summary>
    /// <param name="zonesArray"></param>
    /// <param name="zonesDict"></param>
    // update dictResidentialZones
    public void UpdateResidentialZones(ResidentialZone[] zonesArray, Dictionary<Position, ResidentialZone> zonesDict)
    {
        RemoveOldZones(zonesArray, zonesDict);
        AddNewZones(zonesArray, zonesDict);
    }

    /// <summary>
    /// This function removes old zones from the dictionary that are not in the array.
    /// </summary>
    /// <param name="zonesArray"></param>
    /// <param name="zonesDict"></param>
    void RemoveOldZones(ResidentialZone[] zonesArray, Dictionary<Position, ResidentialZone> zonesDict)
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
    }

    /// <summary>
    /// This function adds new zones to the dictionary that are in the array but not in the dictionary.
    /// </summary>
    /// <param name="zonesArray"></param>
    /// <param name="zonesDict"></param>
    void AddNewZones(ResidentialZone[] zonesArray, Dictionary<Position, ResidentialZone> zonesDict)
    {
        // Now, check which new keys to add
        List<Position> toAdd = new List<Position>();
        foreach (ResidentialZone rz in zonesArray){
            if (!zonesDict.ContainsKey(rz.position)){
                toAdd.Add(rz.position);
            }
        }

        // Add the new keys to the dictionary
        foreach (Position key in toAdd){
            ResidentialZone residentialZone = zonesArray[Array.FindIndex(zonesArray, elem => elem.position.Equals(key))];
            residentialZone = (ResidentialZone)map.findMapObject(key);
            residentialZone.mainRoadConnection = residentialZone.checkPublicRoadConnection();
            zonesDict.Add(key, residentialZone);
        }
    }
}


