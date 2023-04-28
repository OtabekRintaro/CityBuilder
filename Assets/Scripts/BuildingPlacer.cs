using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;


public class BuildingPlacer : MonoBehaviour
{
    private bool currentlyPlacing;
    private BuildingPreset curBuildingPreset;
    private float placementIndicatorUpdateRate = 0.01f;
    private float lastUpdateTime;
    private Vector3 curPlacementPos;
    public GameObject placementIndicator;
    public static BuildingPlacer inst;
    public CellGrid cellGrid;
    public Cursor cursor;
    public BlueprintCell blueprintCellPrefab;
    public BlueprintCell[] blueprintCells;
    public LayerMask cellLayer;
    public Material cant_place;
    void Awake()
    {
        inst = this;
    }
    public void changeColor()
    {
        for (int i = 0; i < cellGrid.cells.GetLength(0); i++)
        {
            for (int k = 0; k < cellGrid.cells.GetLength(1); k++)
            {
                if (cellGrid.cells[i, k].X == curPlacementPos.x &&
                cellGrid.cells[i, k].Z == curPlacementPos.z)
                {
                    
                }
            }
        }
        
    }
    public void createPlane(int x, int z, int i)
    {
        Vector3 position;
        position.x = x * 10f;
        position.y = 0f;
        position.z = z * 10f;
        BlueprintCell plane = blueprintCells[i] = Instantiate<BlueprintCell>(blueprintCellPrefab);
        //if (position.x < -100 || position.x > 200 || position.z < -100 || position.z > 200)
        //{
        //    Renderer rend = blueprintCells[i].GetComponent<Renderer>();
        //    rend.material = cant_place;
        //    currentlyPlacing = false;
        //}
        //else
        //{
        plane.transform.SetParent(placementIndicator.transform, false);
        plane.transform.localPosition = position;
        //}
        //  Debug.Log($"the position of preview: {plane.transform.localPosition}");
    }


    public int Coverage(string type)
    {
        if (type.Equals("ResidentialZone") || type.Equals("IndustrialZone") || type.Equals("CommercialZone"))
            return 3;
        return 1;
    }

    

    public void BeginNewBuildingPlacement(BuildingPreset buildingPreset)
    {
        currentlyPlacing = true;
        curBuildingPreset = buildingPreset;
        placementIndicator.SetActive(true);
        int side = Coverage(buildingPreset.displayName);
        blueprintCells = new BlueprintCell[side * side];

        //Debug.Log(1);
        //Debug.Log(curBuildingPreset.mapObject.GetType());
        int aux = side / 2;
        for(int x = -aux,i = 0 ; x <= aux; x++)
        {
            for (int z = -aux; z <= aux; z++)
            {
                createPlane(x, z, i++);
                
           //     Debug.Log("x: " + x + " z: " + z + " i: " + i);
                //cellGrid.cells[(cursor.curPos.x + i) + (cursor.curPos.z + j) * cellGrid.width].IsFree = false;
            }
        }
    }

    public void CancelBuildingPlacement()
    {
        currentlyPlacing = false;
        placementIndicator.SetActive(false);
    }

    bool isPlaceable(Vector3 curPlacementPos)
    {
        int coverage = Coverage(curBuildingPreset.displayName);
        //float x = curPlacementPos.x / 10 - (coverage / 2);
        //float z = curPlacementPos.z / 10 - (coverage / 2);
        float x = curPlacementPos.x / 10f;
        float z = curPlacementPos.z / 10f;
        
        //for (int i = 0; i < coverage; i++)
        //{
        //    for (int k = 0; k < coverage; k++)
        //    {

        for (int i = -coverage / 2; i < coverage / 2; i++)
        {
            for (int k = -coverage / 2; k < coverage / 2; k++)
            {
                //for (int j = 0; j < cellGrid.cells.Length; j++)
                //{
                //    if (cellGrid.cells[j].Coordinate.X == x &&
                //        cellGrid.cells[j].Coordinate.Z == z)
                //    {
                //        cellGrid.cells[j].isFree = false;
                //        cellGrid.cells[j].Type = curBuildingPreset.displayName;

                //    }
                //}
                foreach (var c in cellGrid.cells)
                {
                    if (c.Coordinate.X == x && c.Coordinate.Z == z && c.isFree == false)
                    {
                        return false;
                    }
                }
                x++;
            }
            z++;
        }
        return true;
    }

    void PlaceBuilding()
    {
        for (int i = 0; i < cellGrid.cells.GetLength(0); i++)
        {
            for (int k = 0; k < cellGrid.cells.GetLength(1); k++)
            {
                if (cellGrid.cells[i,k].X == curPlacementPos.x &&
                cellGrid.cells[i, k].Z == curPlacementPos.z)
                {
                    Debug.Log("cell number is: " + k);
                    Debug.Log("cell content x:" + cellGrid.cells[i,k].X + " z:" + cellGrid.cells[i, k].Z);
                }
            }
        }
     //   Debug.Log($"Placing building at: {curPlacementPos.x}, {curPlacementPos.z} ");
        int coverage = Coverage(curBuildingPreset.displayName);
      //  Debug.Log("coverage divided by 2: " + coverage / 2);
        //RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out hit, Mathf.Infinity, cellLayer))
        //{
        //    // get the clicked cell   
        //    Cell clickedCell = hit.collider.gameObject.GetComponent<Cell>();

        //    // calculate the bottom-left cell of the 4x4 area
        //    int bottomLeftX = clickedCell.X - coverage / 2;
        //    int bottomLeftY = clickedCell.Z - coverage / 2;

        //    // loop through the cells that the house will occupy
        //    for (int x = 0; x < coverage; x++)
        //    {
        //        for (int y = 0; y < coverage; y++)
        //        {
        //            // calculate the position of the current cell
        //            int cellX = bottomLeftX + x;
        //            int cellY = bottomLeftY + y;

        //            // get the current cell
        //            Cell currentCell = GetCellAtPosition(cellX, cellY);

        //            // update its type
        //     //       currentCell.isFree = false;
        //            currentCell.Type = curBuildingPreset.displayName;
        //            //Debug.Log(GetCellAtPosition(cellX, cellY));
        //        }
        //    }


        //    GameObject buildingObj = Instantiate(curBuildingPreset.prefab, hit.collider.transform.position, Quaternion.identity);
        //}
        GameObject buildingObj = Instantiate(curBuildingPreset.prefab, curPlacementPos, Quaternion.identity);
        //if (!curBuildingPreset.displayName.Equals("road"))
        //{
        //    for()
        //}
        //float x = curPlacementPos.x / 10f;
        //float z = curPlacementPos.z / 10f;
        //Debug.Log("x:" + x + " z:" + z);
        //foreach (var c in cellGrid.cells)
        //{
        //    if (c.Coordinate.X == x && c.Coordinate.Z == z)
        //    {
        //        Debug.Log("cell is: " + c.);
        //    }
        //}

        //for (int i = -coverage/2; i < coverage/2; i++)
        //    {
        //        for (int k = -coverage / 2; k < coverage / 2; k++)
        //        {
        //            //for (int j = 0; j < cellGrid.cells.Length; j++)
        //            //{
        //            //    if (cellGrid.cells[j].Coordinate.X == x &&
        //            //        cellGrid.cells[j].Coordinate.Z == z)
        //            //    {
        //            //        cellGrid.cells[j].isFree = false;
        //            //        cellGrid.cells[j].Type = curBuildingPreset.displayName;

        //            //    }
        //            //}
        //            foreach (var c in cellGrid.cells)
        //            {
        //                if (c.Coordinate.X == x && c.Coordinate.Z == z)
        //                {
        //                    c.isFree = false;
        //                    c.Type = curBuildingPreset.displayName;
        //                }
        //            }
        //            x++;
        //        }
        //        z++;
        //    }

        //for(int i = 0; i < cellGrid.cells.Length; i++)
        //{
        //    if (cellGrid.cells[i].Coordinate.X== curPlacementPos.x &&
        //        cellGrid.cells[i].Coordinate.Z == curPlacementPos.z)
        //    {
        //        cellGrid.cells[i].isFree = false;
        //        cellGrid.cells[i].Type = curBuildingPreset.displayName;
        //    }
        //}
        //cellGrid.cells[]
        // City.inst.OnPlaceBuilding(curBuildingPreset);

        CancelBuildingPlacement();
        
    }
    Cell GetCellAtPosition(int x, int y)
    {
        // check if the position is within the bounds of the grid
        if (x >= 0 && x < cellGrid.cells.GetLength(0) && y >= 0 && y < cellGrid.cells.GetLength(1))
        {
            Debug.Log("x:  "+ x + " y: " + y);
            return cellGrid.cells[x, y];
        }
        else
        {
            return null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        curPlacementPos = Selector.inst.GetCurTilePosition();
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelBuildingPlacement();
        if (Time.time - lastUpdateTime > placementIndicatorUpdateRate && currentlyPlacing)
        {
            lastUpdateTime = Time.time;

            placementIndicator.transform.position = curPlacementPos;
        }
        if (currentlyPlacing && Input.GetMouseButtonDown(0))
        {
            PlaceBuilding();
        }
    }
}

