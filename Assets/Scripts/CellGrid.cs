using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CellGrid : MonoBehaviour
{
    protected Transform _transform_CellGrid;

    public int width = 30;
    public int height = 30;

    public Cell cellPrefab;
    public Road roadPrefab;


<<<<<<< HEAD
    Cell[] cells;
    Road[] mainRoad;
    // Road[] roads; 
    RoadHandler roadHandler;
=======
    public Cell[,] cells;
    public Road[] mainRoad;
    public RoadHandler roadHandler;
>>>>>>> origin/cellgrid_matrix_version

    private void Awake()
    {
        _transform_CellGrid = this.transform;
      //  _transform_CellGrid.localPosition = new Vector3(-100f, 0, -100f);

        cells = new Cell[height,width];
        mainRoad = new Road[height * width];
        roadHandler = new RoadHandler(height);

        for(int row = 0; row < height; row++)
        {
            for(int col = 0; col < width; col++)
            {
                //CreateCell(x, z, i++);
                //Debug.Log("cell content x:" + x + " z:" + z + " i:" + i);

                Vector3 position;
                position.x = -100 + col * 10 ;
                position.y = 0;
                position.z = -100 + row * 10;
                Cell cell = cells[row,col] = Instantiate<Cell>(cellPrefab);
                //cells[i].Coordinate = new Position(x,z);

                //cells[x,z].X = (int)position.x;
                //cells[x,z].Z = (int)position.z;
                
                cells[row,col] = cell;
                cells[row, col].X = position.x;
                cells[row, col].Z = position.z;

                //Debug.Log("cell coordinate (mgui) x:" + cells[i].X + " z:" + cells[i].Z);

                cell.transform.SetParent(_transform_CellGrid, false);
                cell.transform.localPosition = position;
            }
        }

        //for(int z = height/2, j = 0; z <= (height/2) + 1; z++)
        //{
        //    for(int x = 0; x < width; x++)
        //    {
        //        CreateRoad(x, z, j++);
        //    }
        //}
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
    //    //cells[i].Coordinate = new Position(x,z);
    //    cells[i].X = position.x;
    //    cells[i].Z = position.z;
    //    //Debug.Log("cell coordinate (mgui) x:" + cells[i].X + " z:" + cells[i].Z);

    //    cell.transform.SetParent(_transform_CellGrid, false);
    //    cell.transform.localPosition = position;
    //}

<<<<<<< HEAD
=======
    //void CreateRoad(int x, int z, int j)
    //{
    //    Vector3 position;
    //    position.x = x * 10f;
    //    position.y = 0.3f;
    //    position.z = z * 10f;

    //    Road road = mainRoad[j] = Instantiate<Road>(roadPrefab);
    //    cells[(width * z - 1) + x + 1].isFree = false;
    //    mainRoad[j].Coordinate = new Position(x,z);
    //    roadHandler.Routes[z, x] = 1;

    //    road.transform.SetParent(_transform_CellGrid, false);
    //    road.transform.localPosition = position;
    //}
>>>>>>> origin/cellgrid_matrix_version

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
