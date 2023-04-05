using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGrid : MonoBehaviour
{
    protected Transform _transform_CellGrid;

    public int width = 20;
    public int height = 20;

    public Cell cellPrefab;
    public Road roadPrefab;


    Cell[] cells;
    Road[] mainRoad;
    RoadHandler roadHandler;

    private void Awake()
    {
        _transform_CellGrid = this.transform;
        _transform_CellGrid.localPosition = new Vector3(-100f, 0, -100f);

        cells = new Cell[height * width];
        mainRoad = new Road[height * width];
        roadHandler = new RoadHandler(height);

        for(int z = 0, i = 0; z < height; z++)
        {
            for(int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }

        for(int z = height/2, j = 0; z <= (height/2) + 1; z++)
        {
            for(int x = 0; x < width; x++)
            {
                CreateRoad(x, z, j++);
            }
        }
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = x * 10f;
        position.y = 0f;
        position.z = z * 10f;

        Cell cell = cells[i] = Instantiate<Cell>(cellPrefab);
        cells[i].Coordinate = new Position(x,z);

        cell.transform.SetParent(_transform_CellGrid, false);
        cell.transform.localPosition = position;
    }

    void CreateRoad(int x, int z, int j)
    {
        Vector3 position;
        position.x = x * 10f;
        position.y = 0.3f;
        position.z = z * 10f;

        Road road = mainRoad[j] = Instantiate<Road>(roadPrefab);
        cells[(width * z - 1) + x + 1].IsFree = false;
        mainRoad[j].Coordinate = new Position(x,z);
        roadHandler.Routes[z, x] = 1;

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
        
    }
}
