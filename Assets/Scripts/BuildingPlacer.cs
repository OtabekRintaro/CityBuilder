using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;

public class BuildingPlacer : MonoBehaviour
{
    private bool currentlyPlacing;
    private BuildingPreset curBuildingPreset;
    private float placementIndicatorUpdateRate = 0.01f;
    private float lastUpdateTime;
    private Vector3 curPlacementPos;
    public GameObject placementIndicator;
    public static BuildingPlacer inst;
    public Map map;
    public Cursor cursor;
    public BlueprintCell blueprintCellPrefab;
    public BlueprintCell[] blueprintCells;
    public LayerMask cellLayer;
    public Material cant_place;
    public GameObject infoUI;

    private Cell cellToBeDeleted;
    public Cell CellToBeDeleted { get { return cellToBeDeleted; } }

    private Transform selection;
    private RaycastHit hit;
    public Material selectionMaterial;
    private Material originalMaterial;

    public Button saveAndExitButton;
    public GameObject ConfirmationPanel;
    public Button YesButton;
    public Button NoButton;

    /// <summary>
    /// awake is called when the script instance is being loaded. Assigns variable to this instance.
    /// </summary>
    void Awake()
    {
        inst = this;
    }

    /// <summary>
    /// getter for curBuildingPreset
    /// </summary>
    /// <returns>BuildingPreset</returns>
    public BuildingPreset getCurrBuildingPreset()
    {
        return curBuildingPreset;
    }
    /// <summary>
    /// getter for IsCurrentlyPlacing
    /// </summary>
    public bool IsCurrentlyPlacing
    {
        get { return currentlyPlacing; }
    }
    /// <summary>
    /// getter for selection
    /// </summary>
    /// <returns></returns>
    public Transform getSelection()
    {
        return selection;
    }
    /// <summary>
    /// Gets cell information and translates it to world position and creates plane's cell where it shows the preview of the size of the building.
    /// </summary>
    /// <param name="x">Cell col num</param>
    /// <param name="z">Cell row num</param>
    /// <param name="i">Cell index</param>
    public void createPlane(int x, int z, int i)
    {
        Vector3 position;
        position.x = x * 10f;
        position.y = 0f;
        position.z = z * 10f;
        BlueprintCell plane = blueprintCells[i] = Instantiate<BlueprintCell>(blueprintCellPrefab);

        plane.transform.SetParent(placementIndicator.transform, false);
        plane.transform.localPosition = position;
    }

    /// <summary>
    /// Returns the coverage of the given prefab.
    /// </summary>
    /// <param name="type">Given prefab's type</param>
    /// <returns></returns>
    public static int Coverage(string type)
    {
        if (type.Equals("ResidentialZone") || type.Equals("IndustrialZone") || type.Equals("CommercialZone"))
        {
            return 3;
        }
        else if (type.Equals("Stadium")) return 5;
        else { return 1; }
    }

    /// <summary>
    /// Creates a preview of the prefab before placing it on the map by calling createPlane function to create each of the plane's cells.
    /// </summary>
    /// <param name="buildingPreset"></param>
    public void BeginNewBuildingPlacement(BuildingPreset buildingPreset)
    {
        currentlyPlacing = true;
        curBuildingPreset = buildingPreset;
        placementIndicator.SetActive(true);
        int side = Coverage(buildingPreset.displayName);
        blueprintCells = new BlueprintCell[side * side];

        int aux = side / 2;
        for (int x = -aux, i = 0; x <= aux; x++)
        {
            for (int z = -aux; z <= aux; z++)
            {
                createPlane(x, z, i++);
            }
        }
    }

    /// <summary>
    /// When the player cancels the placement of the building, the preview of the building is destroyed.
    /// </summary>
    public void CancelBuildingPlacement()
    {
        currentlyPlacing = false;

        Transform plane = placementIndicator.transform.GetChild(0);
        for (int index = 1; index < placementIndicator.transform.childCount; index++)
        {
            Destroy(placementIndicator.transform.GetChild(index).gameObject);
        }
        placementIndicator.transform.DetachChildren();
        plane.SetParent(placementIndicator.transform);

        placementIndicator.SetActive(false);
    }

    /// <summary>
    /// When player places building on the map it checks if the building is placeable and if it is, it places the building on the map.
    /// If it is not, it destroys the preview of the building.
    /// </summary>
    public void PlaceBuilding()
    {
        bool isPlaceable = false;
        int row = 0; int col = 0;
        GameObject buildingObj = Instantiate(curBuildingPreset.prefab, curPlacementPos, Quaternion.identity);
        for (int i = 0; i < map.cells.GetLength(0); i++){
            for (int k = 0; k < map.cells.GetLength(1); k++){
                if (map.cells[i, k].X == curPlacementPos.x && map.cells[i, k].Z == curPlacementPos.z){
                    row = i; col = k;
                    isPlaceable = isPlaceable || assign_cells(map, curBuildingPreset, i, k, buildingObj.GetInstanceID());
                    //Debug.Log(assign_cells(i,k));
                }
            }
        }
        //   Debug.Log($"Placing building at: {curPlacementPos.x}, {curPlacementPos.z} ");
        int coverage = Coverage(curBuildingPreset.displayName);
        if (isPlaceable){
            map.addMapObject(buildingObj, coverage, row, col);
            attachToBuildings(buildingObj);
            CancelBuildingPlacement();
        }
        else{
            Destroy(buildingObj);
        }
    }

    /// <summary>
    /// Gets all the game objects and attaches their ID to the end of their name. 
    /// </summary>
    /// <param name="gameObject"></param>
    public static void attachToBuildings(GameObject gameObject)
    {
        int index = 0;
        Scene gameScene = SceneManager.GetSceneAt(index++);
        while (index < SceneManager.sceneCount && !gameScene.name.Equals("GameScene"))
            gameScene = SceneManager.GetSceneAt(index++);

        GameObject[] gameObjects = gameScene.GetRootGameObjects();

        foreach (GameObject gameObject_0 in gameObjects)
        {
            if (gameObject_0.name.Equals("GameObjects"))
            {
                gameObject.name = cropClone(gameObject.name, gameObject.GetInstanceID());
                gameObject.transform.SetParent(gameObject_0.transform);

                break;
            }
        }
    }

    /// <summary>
    /// Returns the string of the name of the building without the "(Clone)" part.
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="id">id</param>
    /// <returns>string</returns>
    public static string cropClone(string name, int id)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < name.Length; i++)
        {
            if (name[i] == '(')
            {
                break;
            }
            sb.Append(name[i]);
        }
        sb.Append(id.ToString());

        return sb.ToString();
    }

    /// <summary>
    /// Function to select the selectable game objects when the player right clicks on them in play-mode.
    /// </summary>
    public void selectObject()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            ResetSelection();
            if (Physics.Raycast(ray, out hit))
            {
                selection = hit.transform;
                originalMaterial = selection.GetComponent<MeshRenderer>().material;
                if (selection.CompareTag("selectable"))
                {
                    SelectObject();
                }
                else
                {
                    DeselectObject();
                }
            }
        }
    }

    /// <summary>
    /// If the player selects another selectable game object, the previous one is deselected.
    /// </summary>
    private void ResetSelection()
    {
        if (selection != null)
        {
            selection.GetComponent<MeshRenderer>().material = originalMaterial;
            selection = null;
            cellToBeDeleted = null;
            infoUI.SetActive(false);
        }
    }

    /// <summary>
    /// Selected object is highlighted and the info panel is activated where it shows multiple information about the selected object.
    /// </summary>
    private void SelectObject()
    {
        selection.GetComponent<MeshRenderer>().material = selectionMaterial;
        infoUI.SetActive(true);
        var panelTransform = infoUI.transform;
        var capacity = panelTransform.Find("Capacity").GetComponent<TextMeshProUGUI>();
        var satisfaction = panelTransform.Find("SatisfactionText").GetComponent<TextMeshProUGUI>();
        Cell c = cellInfo();
        satisfaction.text = map.findMapObject(c.ID) is ResidentialZone ? "Satisfaction: " + ((ResidentialZone)map.findMapObject(c.ID)).satisfaction.ToString() : "";
        capacity.text = capacityReturn(c);
        cellToBeDeleted = c;
    }

    /// <summary>
    /// If it is not selected anymore, the object is deselected and the info panel is deactivated.
    /// </summary>
    private void DeselectObject()
    {
        selection = null;
        infoUI.SetActive(false);
        cellToBeDeleted = null;
    }

    /// <summary>
    /// Returns capacity string for each of the zones.
    /// </summary>
    /// <param name="c">The cell for which to return the capacity string.</param>
    /// <returns>A string representing the capacity of the specified zone.</returns>
    public string capacityReturn(Cell c)
    {
        if (c.Type == "ResidentialZone")
        {
            return "Capacity: 1000";
        }
        else if (c.Type == "IndustrialZone")
        {
            return "Capacity: 500";
        }
        else if (c.Type == "CommercialZone")
        {
            return "Capacity: 500";
        }
        return "";
    }

    /// <summary>
    /// When the player wants to delete an object, the object is removed from the map and the game in play-mode. 
    /// </summary>
    public void DeleteObject()
    {
        if (cellToBeDeleted != null)
        {
            int id = selection.gameObject.GetInstanceID();
            if (selection.gameObject.name.Split('-')[0].Equals("Cell"))
            {
                id = selection.gameObject.GetComponentInParent<Forest>().gameObject.GetInstanceID();
            }
            if (map.removeMapObject(id))
            {
                if (selection.gameObject.name.Split('-')[0].Equals("Cell")){
                    Destroy(selection.gameObject.GetComponentInParent<Forest>().gameObject);
                }else{
                    Destroy(selection.gameObject);
                }
                int cellID = cellToBeDeleted.ID;
                foreach (var cell in map.cells)
                {
                    if (cell.ID == cellID)
                    {
                        cell.isFree = true;
                        cell.ID = 0;
                        cell.Type = "empty";
                    }
                }
                infoUI.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Translates the mouse position to the position of the cell in the map matrix.
    /// </summary>
    /// <returns>Cell contained in the map matrix</returns>
    public Cell cellInfo()
    {
        for (int i = 0; i < map.cells.GetLength(0); i++)
        {
            for (int k = 0; k < map.cells.GetLength(1); k++)
            {
                if (map.cells[i, k].X == curPlacementPos.x &&
                map.cells[i, k].Z == curPlacementPos.z)
                {
                    return map.cells[i, k];
                }

            }
        }
        return null;
    }

    /// <summary>
    /// When the user wants to put a building on the map, the building is instantiated and added to the map matrix.
    /// It assigns the position of the preview plane to the position of the building on the map.
    /// </summary>
    /// <param name="map">map cell matrix</param>
    /// <param name="curBuildingPreset">prefab of the placing object</param>
    /// <param name="row">col num</param>
    /// <param name="col">row num</param>
    /// <param name="id">cell id</param>
    /// <returns></returns>
    public static bool assign_cells(Map map, BuildingPreset curBuildingPreset, int row, int col, int id)
    {
        int half_cov = Coverage(curBuildingPreset.displayName) / 2;
        int low_row = row - half_cov;
        int high_row = row + half_cov;
        int low_col = col - half_cov;
        int high_col = col + half_cov;

        if (low_row < 0 || high_row > map.cells.GetLength(0) - 1 || low_col < 0 || high_col > map.cells.GetLength(1) - 1){
            return false;
        }
        else{
            for (int x = low_row; x <= high_row; x++){
                for (int z = low_col; z <= high_col; z++){
                    if (map.cells[x, z].isFree){
                        AssignCell(map, curBuildingPreset, x, z, id);
                    }else{
                        ResetCells(map, low_row, low_col, high_col, x, z);
                        return false;
                    }
                }
            }
            return true;
        }
    }

    /// <summary>
    /// If the area is free, then assign cell info to building info.
    /// </summary>
    /// <param name="map">map cell matrix</param>
    /// <param name="curBuildingPreset">prefab of the placing object</param>
    /// <param name="x">col number</param>
    /// <param name="z">row number</param>
    /// <param name="id">cell id</param>
    private static void AssignCell(Map map, BuildingPreset curBuildingPreset, int x, int z, int id)
    {
        map.cells[x, z].isFree = false;
        map.cells[x, z].Type = curBuildingPreset.displayName;
        map.cells[x, z].ID = id;
    }

    /// <summary>
    /// If it is not placeable, then reset the cells.
    /// </summary>
    /// <param name="map">map cell matrix</param>
    /// <param name="low_row">lower row num</param>
    /// <param name="low_col">lower col num</param>
    /// <param name="high_col">higher col num</param>
    /// <param name="x">col num</param>
    /// <param name="z">row num</param>
    private static void ResetCells(Map map, int low_row, int low_col, int high_col, int x, int z)
    {
        int tempRow = x - 1;
        int tempCol = z - 1;
        for (; tempCol >= low_col; tempCol--)
        {
            map.cells[x, tempCol].isFree = true;
            map.cells[x, tempCol].Type = "empty";
            map.cells[x, tempCol].ID = 0;
        }
        for (; tempRow >= low_row; tempRow--)
        {
            for (tempCol = high_col; tempCol >= low_col; tempCol--)
            {
                map.cells[tempRow, tempCol].isFree = true;
                map.cells[tempRow, tempCol].Type = "empty";
                map.cells[tempRow, tempCol].ID = 0;
            }
        }
    }

    /// <summary>
    /// Hides the panels.
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        infoUI.SetActive(false);
        saveAndExitButton.onClick.AddListener(showPanel);
        ConfirmationPanel.SetActive(false);
    }

    /// <summary>
    /// Shows the confirmation panel and listener to the button.
    /// </summary>
    void showPanel()
    {
        ConfirmationPanel.SetActive(true);
        NoButton.onClick.AddListener(Continue);
    }

    /// <summary>
    /// If no button is clicked then hides the confirmation panel.
    /// </summary>
    private void Continue()
    {
        ConfirmationPanel.SetActive(false);
    }
    
    /// <summary>
    /// Tracks the mouse position and updates the position of the preview plane.
    /// </summary>
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
        selectObject();
    }
}

