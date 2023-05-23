using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour, IDataPersistence
{
    protected Transform _transform_map;

    public int width = 30;
    public int height = 30;

    public Cell cellPrefab;
    public Road roadPrefab;
    public Forest forestPrefab;
    public House housePrefab;
    public CommercialBuildings commercialBuildingsPrefab;
    public IndustrialBuildings IndustrialBuildingsPrefab;
    public ResidentialZone residentialZonePrefab;
    public CommercialZone commercialZonePrefab;
    public IndustrialZone industrialZonePrefab;
    public Police policePrefab;
    public FireDepartment fireDepartmentPrefab;
    public Stadium stadiumPrefab;
    public FirePrefab firePrefab;

    public BuildingPreset roadBuildingPreset;
    public BuildingPreset forestBuildingPreset;
    public BuildingPreset residentialZoneBuildingPreset;
    public BuildingPreset industrialZoneBuildingPreset;
    public BuildingPreset policeBuildingPreset;
    public BuildingPreset fireDepartmentBuildingPreset;
    public BuildingPreset stadiumBuildingPreset;
    public BuildingPreset commercialZoneBuildingPreset;


    public Cell[,] cells;
    public Road[,] mainRoad;
    public RoadHandler roadHandler;
    public List<MapObject> mapObjects = new ();
    public List<int> spentMoney = new ();
    public bool hasSomethingChanged = false;

    /// <summary>
    /// Hides the preplaced prefabs on the map, initializes the cells, main road and forest area 
    /// </summary>
    private void Awake()
    {
        HidePrefabs();
        _transform_map = this.transform;
        InitializeCells();
        CreateMainRoad();
        CreateForestArea();
    }

    /// <summary>
    /// Initializes the cells of the game map.
    /// </summary>
    private void InitializeCells()
    {
        cells = new Cell[height, width];
        mainRoad = new Road[height, width];
        roadHandler = new RoadHandler(height);

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                Vector3 position;
                position.x = -100 + col * 10;
                position.y = 0;
                position.z = -100 + row * 10;
                Cell cell = cells[row, col] = Instantiate<Cell>(cellPrefab);
                cells[row, col] = cell;
                cells[row, col].X = position.x;
                cells[row, col].Z = position.z;
                cells[row, col].isFree = true;

                cell.transform.SetParent(_transform_map, false);
                cell.transform.localPosition = position;
            }
        }
    }

    /// <summary>
    /// Creates the main road on the game map.
    /// </summary>
    private void CreateMainRoad()
    {
        for (int row = height / 2; row <= (height / 2) + 1; row++)
        {
            for (int col = 0; col < width; col++)
            {
                CreateRoad(row, col);
            }
        }
    }

    /// <summary>
    /// Creates a forest area on the game map.
    /// </summary>
    private void CreateForestArea()
    {
        for (int row = height - 1, amount = 12; row > height - 13; row--, amount--)
        {
            for (int col = 0; col < amount; col++)
            {
                CreateForest(row, col);
            }
        }

        for (int row = height - 1, amount = width - 12; row > height - 13; row--, amount++)
        {
            for (int col = width - 1; col > amount; col--)
            {
                CreateForest(row, col);
            }
        }
    }

    /// <summary>
    /// Loads data from a GameData object to initialize the game state.
    /// </summary>
    /// <param name="data"></param>
    public void LoadData(GameData data)
    {
        InitializeCells(data);
        int index = 0;
        int resIndex = 0;

        foreach (string name in data.gameObjects)
        {
            int row = data.positionsX[index];
            int col = data.positionsZ[index];
            Vector3 curPlacementPos = new Vector3(cells[row, col].X, 0.3f, cells[row, col].Z);

            if (name.Equals("ResidentialZone"))
            {
                CreateResidentialZone(row, col, curPlacementPos, data, ref resIndex);
            }
            else if (name.Equals("IndustrialZone"))
            {
                CreateIndustrialZone(row, col, curPlacementPos);
            }
            else if (name.Equals("CommercialZone"))
            {
                CreateCommercialZone(row, col, curPlacementPos);
            }
            else if (name.Equals("Police"))
            {
                CreatePolice(row, col, curPlacementPos);
            }
            else if (name.Equals("Stadium"))
            {
                CreateStadium(row, col, curPlacementPos);
            }
            else if (name.Equals("FireDepartment"))
            {
                CreateFireDepartment(row, col, curPlacementPos);
            }
            else if (name.Equals("Forest"))
            {
                CreateForest(row, col, curPlacementPos);
            }
            else if (name.Equals("Road"))
            {
                CreateRoad(row, col, curPlacementPos);
            }

            index++;
        }
    }

    /// <summary>
    /// Initializes the cells of the game map using data from a GameData object.
    /// </summary>
    /// <param name="data"></param>
    private void InitializeCells(GameData data)
    {
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                cells[row, col].isFree = data.FreeCells[width * row + col];
                cells[row, col].Type = data.TypeOfCells[width * row + col];
                cells[row, col].X = -100 + col * 10;
                cells[row, col].Z = -100 + row * 10;
                //cells[row, col].ID = data.IdOfCells[width * row + col];
            }
        }
    }

    /// <summary>
    /// Creates a residential zone on the game map at the specified row and column.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="curPlacementPos"></param>
    /// <param name="data"></param>
    /// <param name="resIndex"></param>
    private void CreateResidentialZone(int row, int col,
        Vector3 curPlacementPos,
        GameData data,
        ref int resIndex)
    {
        GameObject gameObject = Instantiate(residentialZoneBuildingPreset.prefab);
        int coverage = BuildingPlacer.Coverage(residentialZoneBuildingPreset.displayName);

        addMapObject(gameObject, coverage, row, col);

        ResidentialZone resZone = gameObject.GetComponent<ResidentialZone>();
        resZone.transform.localPosition = curPlacementPos;
        resZone.transform.rotation = Quaternion.identity;

        for (int i = (int)row - 1; i <= (int)row + 1; i++)
        {
            for (int j = (int)col - 1; j <= (int)col + 1; j++)
            {
                cells[i, j].ID = gameObject.GetInstanceID();
            }
        }

        resZone.position = new Position(row, col);
        int skipped = 0;
        for (int i = 0; i < resIndex; i++)
            skipped += data.ctzCount[i];
        for (int i = 0; i < data.ctzCount[resIndex]; i++){
            resZone.population.Add(new Citizen(data.ages[skipped + i]));
        }
        // resZone.population = data.population[resIndex];
        resZone.satisfaction = data.satisfaction[resIndex];
        resIndex++;

        BuildingPlacer.attachToBuildings(resZone.gameObject);
    }

    /// <summary>
    /// Creates an industrial zone on the game map at the specified row and column.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="curPlacementPos"></param>
    private void CreateIndustrialZone(int row,
                                      int col,
                                      Vector3 curPlacementPos)
    {
        GameObject gameObject = Instantiate(industrialZoneBuildingPreset.prefab);
        int coverage = BuildingPlacer.Coverage(industrialZoneBuildingPreset.displayName);

        addMapObject(gameObject, coverage, row, col);

        IndustrialZone indZone = gameObject.AddComponent<IndustrialZone>();
        indZone.transform.localPosition = curPlacementPos;
        indZone.transform.rotation = Quaternion.identity;

        indZone.position = new Position(row, col);

        BuildingPlacer.attachToBuildings(indZone.gameObject);
    }

    /// <summary>
    /// Creates a commercial zone on the game map at the specified row and column.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="curPlacementPos"></param>
    private void CreateCommercialZone(int row,
                                      int col,
                                      Vector3 curPlacementPos)
    {
        GameObject gameObject = Instantiate(commercialZoneBuildingPreset.prefab);
        int coverage = BuildingPlacer.Coverage(commercialZoneBuildingPreset.displayName);

        addMapObject(gameObject, coverage, row, col);

        CommercialZone comZone = gameObject.AddComponent<CommercialZone>();
        comZone.transform.localPosition = curPlacementPos;
        comZone.transform.rotation = Quaternion.identity;

        comZone.position = new Position(row, col);

        BuildingPlacer.attachToBuildings(comZone.gameObject);
    }

    /// <summary>
    /// Creates a police station on the game map at the specified row and column.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="curPlacementPos"></param>
    private void CreatePolice(int row,
                              int col,
                              Vector3 curPlacementPos)
    {
        GameObject gameObject = Instantiate(policeBuildingPreset.prefab);
        int coverage = BuildingPlacer.Coverage(policeBuildingPreset.displayName);

        addMapObject(gameObject, coverage, row, col);

        Police police = gameObject.AddComponent<Police>();
        police.transform.localPosition = curPlacementPos;
        police.transform.rotation = Quaternion.identity;

        police.position = new Position(row, col);

        BuildingPlacer.attachToBuildings(police.gameObject);
    }

    /// <summary>
    /// Creates a stadium on the game map at the specified row and column.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="curPlacementPos"></param>
    private void CreateStadium(int row,
                               int col,
                               Vector3 curPlacementPos)
    {
        GameObject gameObject = Instantiate(stadiumBuildingPreset.prefab);
        int coverage = BuildingPlacer.Coverage(stadiumBuildingPreset.displayName);

        addMapObject(gameObject, coverage, row, col);

        Stadium stadium = gameObject.AddComponent<Stadium>();
        stadium.transform.localPosition = curPlacementPos;
        stadium.transform.rotation = Quaternion.identity;

        stadium.position = new Position(row, col);

        BuildingPlacer.attachToBuildings(stadium.gameObject);
    }

    /// <summary>
    /// Creates a fire department on the game map at the specified row and column.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="curPlacementPos"></param>
    private void CreateFireDepartment(int row,
                                      int col,
                                      Vector3 curPlacementPos)
    {
        GameObject gameObject = Instantiate(fireDepartmentBuildingPreset.prefab);
        int coverage = BuildingPlacer.Coverage(fireDepartmentBuildingPreset.displayName);

        addMapObject(gameObject, coverage, row, col);

        FireDepartment fireDepartment = gameObject.AddComponent<FireDepartment>();
        fireDepartment.transform.localPosition = curPlacementPos;
        fireDepartment.transform.rotation = Quaternion.identity;

        fireDepartment.position = new Position(row, col);

        BuildingPlacer.attachToBuildings(fireDepartment.gameObject);
    }

    /// <summary>
    /// Creates a forest on the game map at the specified row and column.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="curPlacementPos"></param>
    private void CreateForest(int row,
                              int col,
                              Vector3 curPlacementPos)
    {
        GameObject gameObject = Instantiate(forestBuildingPreset.prefab);
        int coverage = BuildingPlacer.Coverage(forestBuildingPreset.displayName);

        addMapObject(gameObject, coverage, row, col);

        Forest forest = gameObject.AddComponent<Forest>();
        forest.transform.localPosition = curPlacementPos;
        forest.transform.rotation = Quaternion.identity;

        forest.position = new Position(row, col);

        BuildingPlacer.attachToBuildings(forest.gameObject);
    }

    /// <summary>
    /// Creates a road on the game map at the specified row and column.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="curPlacementPos"></param>
    private void CreateRoad(int row,
                            int col,
                            Vector3 curPlacementPos)
    {
        GameObject gameObject = Instantiate(roadBuildingPreset.prefab);
        int coverage = BuildingPlacer.Coverage(roadBuildingPreset.displayName);

        addMapObject(gameObject, coverage, row, col);

        Road road = gameObject.AddComponent<Road>();
        road.transform.localPosition = curPlacementPos;
        road.transform.rotation = Quaternion.identity;

        road.position = new Position(row, col);

        BuildingPlacer.attachToBuildings(road.gameObject);
    }

    /// <summary>
    /// Saves data from the game state to a GameData object.
    /// </summary>
    /// <param name="data"></param>
    public void SaveData(GameData data)
    {
        SaveCellData(data);
        SaveMapObjectData(data);
    }

    /// <summary>
    /// Saves cell data from the game state to a GameData object.
    /// </summary>
    /// <param name="data"></param>
    private void SaveCellData(GameData data)
    {
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                data.FreeCells[width * row + col] = cells[row, col].isFree;
                data.TypeOfCells[width * row + col] = cells[row, col].Type;
            }
        }
    }

    /// <summary>
    /// Saves map object data from the game state to a GameData object.
    /// </summary>
    /// <param name="data"></param>
    private void SaveMapObjectData(GameData data)
    {
        data.positionsX.Clear();
        data.positionsZ.Clear();
        data.gameObjects.Clear();
        data.satisfaction.Clear();
        data.ages.Clear();
        data.ctzCount.Clear();

        foreach (MapObject mapObj in mapObjects)
        {
            data.positionsX.Add(mapObj.position.x);
            data.positionsZ.Add(mapObj.position.z);
            data.gameObjects.Add(mapObj.name.Split('-')[0]);
            if (data.gameObjects[data.gameObjects.Count - 1].Equals("ResidentialZone"))
            {
                ResidentialZone resZone = (ResidentialZone)mapObj;
                data.ctzCount.Add(resZone.population.Count);
                foreach (var ctz in resZone.population)
                {
                    data.ages.Add(ctz.Age);
                }
                data.satisfaction.Add(resZone.satisfaction);
            }
        }
    }

    /// <summary>
    /// Hides prefabs in the game scene.
    /// </summary>
    void HidePrefabs()
    {
        int index = 0;
        Scene gameScene = SceneManager.GetSceneAt(index++);
        while (index < SceneManager.sceneCount && !gameScene.name.Equals("GameScene"))
            gameScene = SceneManager.GetSceneAt(index++);

        GameObject[] gameObjects = gameScene.GetRootGameObjects();

        foreach(GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Equals("Prefabs"))
                foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>())
                    transform.position = new Vector3(-99999, -99999, -99999);

        }
          
    }

    //void CreateCell(int x, int z, int i)
    //{

    //    Vector3 a = new Vector3(-100f, 0, -100f);
    //    //float b = _transform_CellGrid.localPosition.z;
    //    Vector3 position;
    //    position.x = a.x + (x * 10f);
    //    position.y = 0f;
    //    position.z = a.z + (z * 10f);
    //    //Debug.Log("cell content x:" + position.x + " z:" + position.z);
    //    Cell cell = cells[i] = Instantiate<Cell>(cellPrefab);
    //    //cells[i].position = new Position(x,z);
    //    cells[i].X = position.x;
    //    cells[i].Z = position.z;
    //    //Debug.Log("cell position (mgui) x:" + cells[i].X + " z:" + cells[i].Z);

    //    cell.transform.SetParent(_transform_CellGrid, false);
    //    cell.transform.localPosition = position;
    //}

    /// <summary>
    /// Creates a forest on the game map at the specified row and column.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    void CreateForest(int row, int col)
    {
        Vector3 position = GetPosition(row, col);
        Forest forest = Instantiate<Forest>(forestPrefab);
        cells[row, col].isFree = false;
        forest.position = new Position(row, col);
        forest.coverage = 1;
        forest.transform.SetParent(_transform_map, false);
        forest.transform.localPosition = position;
    }

    /// <summary>
    /// Creates a road on the game map at the specified row and column.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    void CreateRoad(int row, int col)
    {
        Vector3 position = GetPosition(row, col);
        Road road = mainRoad[row, col] = Instantiate<Road>(roadPrefab);
        cells[row, col].isFree = false;
        mainRoad[row, col].position = new Position(row, col);
        mainRoad[row, col].coverage = 1;
        roadHandler.Routes[row, col] = 1;
        roadHandler.MainRoad[row, col] = 1;
        road.transform.SetParent(_transform_map, false);
        road.transform.localPosition = position;
    }

    /// <summary>
    /// Returns the position of a cell on the game map at the specified row and column.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    Vector3 GetPosition(int row, int col)
    {
        Vector3 position;
        position.x = -100 + col * 10;
        position.y = 0.3f;
        position.z = -100 + row * 10;
        return position;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Updates the game state once per frame.
    /// </summary>
    void Update()
    {
        if(hasSomethingChanged)
        {
            resetMapObjects();
            hasSomethingChanged = false;
        }
        checkPublicRoadConnectivity();
    }

    /// <summary>
    /// Resets the map objects in the game state.
    /// </summary>
    public void resetMapObjects()
    {
        foreach (MapObject mapObject in mapObjects)
        {
            if (mapObject is not Forest && object.ReferenceEquals(mainRoad[mapObject.position.x, mapObject.position.z], null))
            {
                mapObject.publicRoads = 0;
                mapObject.connectToPublicRoad(false);
            }
            if (mapObject is not FireDepartment)
            {
                int count = MapObject.CheckRadius(cells,this, mapObject.position, 10, new FireDepartment());
                if(count != 0)
                {
                    mapObject.ConnectToFireDepartment(true);
                }
            }
        }
    }

    /// <summary>
    /// Checks the connectivity of public roads in the game state.
    /// </summary>
    public void checkPublicRoadConnectivity()
    {
        foreach(MapObject mapObject in mapObjects)
        {
            if(!mapObject.checkPublicRoadConnection())
            {
                    mapObject.connectToPublicRoad(roadHandler.findPublicRoad(mapObject));
            }
            else
            {
                //Debug.Log("Connected!");
            }
        }
    }

    //public GameObject findGameObject(Position position)
    //{
    //    GameObject gameObject = null;

    //    GameObject[] gameObjects = gameScene.GetRootGameObjects();

    //    foreach (GameObject gameObject in gameObjects)
    //    {
    //        if (gameObject.name.Equals("Prefabs"))
    //            foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>())
    //                transform.position = new Vector3(-99999, -99999, -99999);

    //    }

    //    return gameObject;
    //}

    /// <summary>
    /// Finds a map object in the game state with the specified ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public MapObject findMapObject(int id)
    {
        MapObject res = null;
        foreach (MapObject mapObject in mapObjects)
        {
            if (mapObject.ID.Equals(id))
            {
                res = mapObject;
            }
        }
        return res;
    }

    /// <summary>
    /// Finds a map object in the game state at the specified position.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public MapObject findMapObject(Position pos)
    {
        MapObject res = null;
        foreach(MapObject mapObject in mapObjects)
        {
            if(mapObject.position.Equals(pos))
            {
                res = mapObject;
            }
        }
        return res;
    }

    /// <summary>
    /// Adds a map object to the game state at the specified row and column.
    /// </summary>
    /// <param name="buildingObj"></param>
    /// <param name="coverage"></param>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public void addMapObject(GameObject buildingObj, int coverage, int x, int z)
    {
        MapObject mapObject = MapObject.getMapObject(buildingObj.name.Split('(')[0], buildingObj);
        mapObject.ID = buildingObj.GetInstanceID();
        mapObject.position = new Position(x, z);
        mapObject.coverage = coverage;
        mapObject.firePrefab = firePrefab;
        if(buildingObj.name.Split('(')[0].Equals("Road")){
            addRoad(mapObject);
        }
        if(mapObject.gameObject.GetComponent<ResidentialZone>() is not null){
            ResidentialZone zone = (ResidentialZone)mapObject;
            zone.housePrefab = this.housePrefab;
        }
        if (mapObject.gameObject.GetComponent<IndustrialZone>() is not null)
        {
            IndustrialZone zone = (IndustrialZone)mapObject;
            zone.factoryPrefab = this.IndustrialBuildingsPrefab;
        }
        if (mapObject.gameObject.GetComponent<CommercialZone>() is not null)
        {
            CommercialZone zone = (CommercialZone)mapObject;
            zone.commercialBuildingsPrefab = this.commercialBuildingsPrefab;
        }
        mapObject.connectToPublicRoad(false);
        mapObjects.Add(mapObject);
        hasSomethingChanged = true;
        //Debug.Log(MapObject.getCost(buildingPreset.displayName));
        spentMoney.Add(MapObject.getCost(buildingObj.name.Split('(')[0]));
    }

    /// <summary>
    /// Removes a map object from the game state with the specified ID.
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public bool removeMapObject(int ID)
    {
        bool canBeRemoved = true;
        int index = FindMapObjectIndex(ID);
        if (index == -1)
            return false;

        if (mapObjects[index].GetType() == typeof(Road) && mapObjects[index].checkPublicRoadConnection())
        {
            canBeRemoved = CheckIfRoadCanBeRemoved(index);
            if (canBeRemoved)
            {
                roadHandler.Routes[mapObjects[index].position.x, mapObjects[index].position.z] = 0;
                UpdatePublicRoads(index);
            }
        }

        if (canBeRemoved)
        {
            mapObjects.RemoveAt(index);
            hasSomethingChanged = true;
        }

        return canBeRemoved;
    }

    /// <summary>
    /// Finds the index of a map object in the game state with the specified ID.
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    int FindMapObjectIndex(int ID)
    {
        int index = -1;
        for (int i = 0; i < mapObjects.Count; i++)
        {
            if (mapObjects[i].ID == ID)
            {
                index = i;
            }
        }
        return index;
    }

    /// <summary>
    /// Checks if a road at the specified index in the game state can be removed.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    bool CheckIfRoadCanBeRemoved(int index)
    {
        bool canBeRemoved = true;
        foreach (MapObject mapObject in mapObjects)
        {
            if (mapObject is not Road && mapObject is not Forest && roadHandler.checkConnection(mapObject, mapObjects[index]) && mapObject.publicRoads == 1)
            {
                canBeRemoved = false;
            }
        }
        return canBeRemoved;
    }

    /// <summary>
    /// Updates the public roads in the game state after removing a road at the specified index.
    /// </summary>
    /// <param name="index"></param>
    void UpdatePublicRoads(int index)
    {
        foreach (MapObject mapObject in mapObjects)
        {
            if (mapObject is not Road && mapObject is not Forest && roadHandler.checkConnection(mapObject, mapObjects[index]))
            {
                mapObject.publicRoads--;
            }
        }
    }

    /// <summary>
    /// Adds a road to the game state.
    /// </summary>
    /// <param name="mapObject"></param>
    public void addRoad(MapObject mapObject)
    {
        roadHandler.Routes[mapObject.position.x, mapObject.position.z] = 1;
    }
}
