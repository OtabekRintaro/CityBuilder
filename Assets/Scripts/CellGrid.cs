using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class CellGrid : MonoBehaviour
{
    protected Transform _transform_CellGrid;

    public int width = 30;
    public int height = 30;

    public Cell cellPrefab;
    public Road roadPrefab;


    public Cell[,] cells;
    public Road[,] mainRoad;
    public RoadHandler roadHandler;
    public List<MapObject> mapObjects = new ();
    public List<int> spentMoney = new ();

    private void Awake()
    {
        HidePrefabs();
        _transform_CellGrid = this.transform;
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

                //Debug.Log("cell position (mgui) x:" + cells[i].X + " z:" + cells[i].Z);

                cell.transform.SetParent(_transform_CellGrid, false);
                cell.transform.localPosition = position;
            }
        }

        for (int row = height / 2, index = 0; row <= (height / 2) + 1; row++)
        {
            for (int col = 0; col < width; col++)
            {
                CreateRoad(row, col, index++);
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

    void CreateRoad(int row, int col, int index)
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

        road.transform.SetParent(_transform_CellGrid, false);
        road.transform.localPosition = position;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkPublicRoadConnectivity();
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

    public void addMapObject(GameObject buildingObj, BuildingPreset buildingPreset, int coverage, int x, int z)
    {
        MapObject mapObject = MapObject.getMapObject(buildingPreset.displayName, buildingObj);
        mapObject.ID = buildingObj.GetInstanceID();
        mapObject.position = new Position(x, z);
        mapObject.coverage = coverage;
        if(buildingPreset.displayName.Equals("Road"))
        {
            addRoad(mapObject);
        }
        mapObjects.Add(mapObject);
        Debug.Log(MapObject.getCost(buildingPreset.displayName));
        spentMoney.Add(MapObject.getCost(buildingPreset.displayName));
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
                if(mapObject is not Road && roadHandler.checkConnection(mapObject, mapObjects[index]))
                {
                    canBeRemoved = false;
                }
            }
        }
        if(canBeRemoved)
            mapObjects.RemoveAt(index);
        return canBeRemoved;
    }


    public void addRoad(MapObject mapObject)
    {
        roadHandler.Routes[mapObject.position.x, mapObject.position.z] = 1;
    }

}
