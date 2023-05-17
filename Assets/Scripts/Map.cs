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

    private void Awake()
    {
        HidePrefabs();
        _transform_map = this.transform;
        //  _transform_CellGrid.localPosition = new Vector3(-100f, 0, -100f);

        generateObjects();
        //roadHandler.printRoutes();
        cells = new Cell[height, width];
        mainRoad = new Road[height, width];
        roadHandler = new RoadHandler(height);

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                //CreateCell(x, z, i++);
                //Debug.Log("cell content x:" + x + " z:" + z + " i:" + i);

                Vector3 position;
                position.x = -100 + col * 10;
                position.y = 0;
                position.z = -100 + row * 10;
                Cell cell = cells[row, col] = Instantiate<Cell>(cellPrefab);
                //cells[i].Coordinate = new Position(x,z);

                //cells[x,z].X = (int)position.x;
                //cells[x,z].Z = (int)position.z;

                cells[row, col] = cell;
                cells[row, col].X = position.x;
                cells[row, col].Z = position.z;
                cells[row, col].isFree = true;

                //Debug.Log("cell position (mgui) x:" + cells[i].X + " z:" + cells[i].Z);

                cell.transform.SetParent(_transform_map, false);
                cell.transform.localPosition = position;
            }
        }

        for (int row = height / 2; row <= (height / 2) + 1; row++)
        {
            for (int col = 0; col < width; col++)
            {
                CreateRoad(row, col);
            }
        }
        
        for (int row = height - 1, amount = 12; row > height - 13; row--, amount--)
        {
            for(int col = 0; col < amount; col++)
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

    public void LoadData(GameData data)
    {
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                cells[row, col].isFree = data.FreeCells[width * row + col];
                cells[row, col].Type = data.TypeOfCells[width * row + col];
                cells[row, col].ID = data.IdOfCells[width * row + col];
            }
        }

        int index = 0;
        int resIndex = 0;

        foreach(string name in data.gameObjects)
        {

            int row = data.positionsX[index];
            int col = data.positionsZ[index];
            Vector3 curPlacementPos = new Vector3(cells[row,col].X, 0.3f, cells[row, col].Z);
            if(name.Equals("ResidentialZone"))
            {
                GameObject gameObject = Instantiate(residentialZoneBuildingPreset.prefab);
                int coverage = BuildingPlacer.Coverage(residentialZoneBuildingPreset.displayName);
                
                addMapObject(gameObject, coverage, row, col);

                ResidentialZone resZone = gameObject.GetComponent<ResidentialZone>();
                resZone.transform.localPosition = curPlacementPos;
                resZone.transform.rotation = Quaternion.identity;

                resZone.position = new Position(row, col);
                foreach(var age in data.ages)
                {
                    resZone.population.Add(new Citizen(age));
                }
                // resZone.population = data.population[resIndex];
                resZone.satisfaction = data.satisfaction[resIndex];
                resIndex++;

                BuildingPlacer.attachToBuildings(resZone.gameObject);
            }
            else if(name.Equals("IndustrialZone"))
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
            else if (name.Equals("CommercialZone"))
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
            else if (name.Equals("Police"))
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
            else if (name.Equals("Stadium"))
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
            else if (name.Equals("FireDepartment"))
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
            else if (name.Equals("Forest"))
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
            else if (name.Equals("Road"))
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

            index++;
        }
    }

    public void SaveData(GameData data)
    {
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                data.FreeCells[width * row + col] = cells[row, col].isFree;
                data.TypeOfCells[width * row + col] = cells[row, col].Type;
                data.IdOfCells[width * row + col] = cells[row, col].ID;
            }
        }

        data.positionsX.Clear();
        data.positionsZ.Clear();
        data.gameObjects.Clear();
        //data.population.Clear();
        data.satisfaction.Clear();
        data.ages.Clear();

        foreach(MapObject mapObj in mapObjects)
        {
            data.positionsX.Add(mapObj.position.x);
            data.positionsZ.Add(mapObj.position.z);
            data.gameObjects.Add(mapObj.name.Split('-')[0]);
            if(data.gameObjects[data.gameObjects.Count - 1].Equals("ResidentialZone"))
            {
                ResidentialZone resZone = (ResidentialZone)mapObj;
                foreach(var ctz in resZone.population)
                {
                    data.ages.Add(ctz.Age);
                }
                data.satisfaction.Add(resZone.satisfaction);
            }
        }
    }
    public void generateObjects()
    {
        
    }
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

    void CreateForest(int row, int col)
    {
        Vector3 position;
        position.x = -100 + col * 10;
        position.y = 0.3f;
        position.z = -100 + row * 10;

        
        Forest forest = Instantiate<Forest>(forestPrefab);
        cells[row, col].isFree = false;
        forest.position = new Position(row, col);
        forest.coverage = 1;

        forest.transform.SetParent(_transform_map, false);
        forest.transform.localPosition = position;
        //addMapObject(forest.gameObject, forestBuildingPreset, 1, row, col);
    }

    void CreateRoad(int row, int col)
    {
        Vector3 position;
        position.x = -100 + col * 10;
        position.y = 0.3f;
        position.z = -100 + row * 10;

        Road road = mainRoad[row, col] = Instantiate<Road>(roadPrefab);
        cells[row, col].isFree = false;
        mainRoad[row, col].position = new Position(row, col);
        mainRoad[row, col].coverage = 1;
        roadHandler.Routes[row, col] = 1;
        roadHandler.MainRoad[row, col] = 1;

        road.transform.SetParent(_transform_map, false);
        road.transform.localPosition = position;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasSomethingChanged)
        {
            resetMapObjects();
            hasSomethingChanged = false;
        }
        checkPublicRoadConnectivity();
    }

    public void resetMapObjects()
    {
        foreach (MapObject mapObject in mapObjects)
        {
            if (mapObject is not Forest && object.ReferenceEquals(mainRoad[mapObject.position.x, mapObject.position.z], null))
            {
                mapObject.publicRoads = 0;
                mapObject.connectToPublicRoad(false);
            }
        }
    }

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

    public void addMapObject(GameObject buildingObj, int coverage, int x, int z)
    {
        MapObject mapObject = MapObject.getMapObject(buildingObj.name.Split('(')[0], buildingObj);
        mapObject.ID = buildingObj.GetInstanceID();
        mapObject.position = new Position(x, z);
        mapObject.coverage = coverage;
        if(buildingObj.name.Split('(')[0].Equals("Road"))
        {
            addRoad(mapObject);
        }
        if(mapObject.gameObject.GetComponent<ResidentialZone>() is not null)
        {
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

    public bool removeMapObject(int ID)
    {
        bool canBeRemoved = true;
        int index = -1;
        //Debug.Log(ID);
        for(int i = 0; i < mapObjects.Count; i++)
        {
            if(mapObjects[i].ID == ID)
            {
                index = i;
            }
        }
        if (index == -1)
            return false;

        if (mapObjects[index].GetType() == typeof(Road) && mapObjects[index].checkPublicRoadConnection())
        {
            foreach(MapObject mapObject in mapObjects)
            {
                if(mapObject is not Road && mapObject is not Forest && roadHandler.checkConnection(mapObject, mapObjects[index]) && mapObject.publicRoads == 1)
                {
                    canBeRemoved = false;
                }
            }

            if(canBeRemoved)
            {
                roadHandler.Routes[mapObjects[index].position.x, mapObjects[index].position.z] = 0;
                foreach (MapObject mapObject in mapObjects)
                {
                    if (mapObject is not Road && mapObject is not Forest && roadHandler.checkConnection(mapObject, mapObjects[index]))
                    {
                        mapObject.publicRoads--;
                    }
                }
            }
        }

        if(canBeRemoved)
        {
            mapObjects.RemoveAt(index);
            hasSomethingChanged = true;
        }

        return canBeRemoved;
    }


    public void addRoad(MapObject mapObject)
    {
        roadHandler.Routes[mapObject.position.x, mapObject.position.z] = 1;
    }

}
