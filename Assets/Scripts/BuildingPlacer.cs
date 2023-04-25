using System.Collections;
using System.Collections.Generic;
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
    void Awake()
    {
        inst = this;
    }

    public void createPlane(int x, int z, int i)
    {
        Vector3 position;
        position.x = x * 10f;
        position.y = 0f;
        position.z = z * 10f;

        BlueprintCell plane = blueprintCells[i] = Instantiate<BlueprintCell>(blueprintCellPrefab);
        

        plane.transform.SetParent(placementIndicator.transform, true);
        plane.transform.localPosition = position;
    }

    public void BeginNewBuildingPlacement(BuildingPreset buildingPreset)
    {

        currentlyPlacing = true;
        curBuildingPreset = buildingPreset;
        placementIndicator.SetActive(true);
        //blueprintCells = new BlueprintCell[curBuildingPreset.prefab.coverage * curBuildingPreset.mapObject.coverage];

        //Debug.Log(1);
        //Debug.Log(curBuildingPreset.mapObject.GetType());

        //for(int x = 0, i = 0 ; x < curBuildingPreset.mapObject.coverage ; x++)
        //{
        //    for(int z = 0; z < curBuildingPreset.mapObject.coverage; z++)
        //    {
        //        createPlane(x, z, i++);
        //        //cellGrid.cells[(cursor.curPos.x + i) + (cursor.curPos.z + j) * cellGrid.width].IsFree = false;
        //    }
        //}
    }

    public void CancelBuildingPlacement()
    {
        currentlyPlacing = false;
        placementIndicator.SetActive(false);
    }

    void PlaceBuilding()
    {
        GameObject buildingObj = Instantiate(curBuildingPreset.prefab, curPlacementPos, Quaternion.identity);
        
        // City.inst.OnPlaceBuilding(curBuildingPreset);

        CancelBuildingPlacement();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelBuildingPlacement();
        if (Time.time - lastUpdateTime > placementIndicatorUpdateRate && currentlyPlacing)
        {
            lastUpdateTime = Time.time;
            curPlacementPos = Selector.inst.GetCurTilePosition();
            placementIndicator.transform.position = curPlacementPos;
        }
        if (currentlyPlacing && Input.GetMouseButtonDown(0))
        {
            PlaceBuilding();
        }
    }
}

