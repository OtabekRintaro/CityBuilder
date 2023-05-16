using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TMPro;
//using System.Numerics;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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

    private Transform selection;
    private RaycastHit hit;
    public Material selectionMaterial;
    private Material originalMaterial;
    ///////
    /// <summary>
    /// 
    /// </summary>
    /// 

    //  public DateHandler dateDisplay;
    public Button saveAndExitButton;
    public GameObject ConfirmationPanel;
    public Button YesButton;
    public Button NoButton;
    
    void Awake()
    {
        infoUI.SetActive(false);
        inst = this;
    }
    //public void changeColor()
    //{
    //    for (int i = 0; i < cellGrid.cells.GetLength(0); i++)
    //    {
    //        for (int k = 0; k < cellGrid.cells.GetLength(1); k++)
    //        {
    //            if (cellGrid.cells[i, k].X == curPlacementPos.x &&
    //            cellGrid.cells[i, k].Z == curPlacementPos.z)
    //            {

    //            }
    //        }
    //    }

    //}

    
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


    public static int Coverage(string type)
    {
        if (type.Equals("ResidentialZone") || type.Equals("IndustrialZone") || type.Equals("CommercialZone"))
        {
            return 3;
        }
        else if (type.Equals("Stadium")) return 5;
        else { return 1; }
    }

    

    public void BeginNewBuildingPlacement(BuildingPreset buildingPreset)
    {
        currentlyPlacing = true;
        curBuildingPreset = buildingPreset;
        placementIndicator.SetActive(true);
        int side = Coverage(buildingPreset.displayName);
        blueprintCells = new BlueprintCell[side * side];
        //foreach (var c in blueprintCells)
        //{
        //    if (c.GetComponent<BlueprintCell>() != null)
        //    {
        //        Destroy(c.gameObject);
        //    }
        //}
        
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

        Transform plane = placementIndicator.transform.GetChild(0);
        for(int index = 1; index < placementIndicator.transform.childCount; index++)
        {
            Destroy(placementIndicator.transform.GetChild(index).gameObject);
        }
        placementIndicator.transform.DetachChildren();
        plane.SetParent(placementIndicator.transform);

        placementIndicator.SetActive(false);
        //blueprintCells = null;
    }

    //bool isPlaceable(Vector3 curPlacementPos)
    //{
    //    int coverage = Coverage(curBuildingPreset.displayName);
    //    //float x = curPlacementPos.x / 10 - (coverage / 2);
    //    //float z = curPlacementPos.z / 10 - (coverage / 2);
    //    float x = curPlacementPos.x / 10f;
    //    float z = curPlacementPos.z / 10f;
        
    //    //for (int i = 0; i < coverage; i++)
    //    //{
    //    //    for (int k = 0; k < coverage; k++)
    //    //    {

    //    for (int i = -coverage / 2; i < coverage / 2; i++)
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
    //                if (c.Coordinate.X == x && c.Coordinate.Z == z && c.isFree == false)
    //                {
    //                    return false;
    //                }
    //            }
    //            x++;
    //        }
    //        z++;
    //    }
    //    return true;
    //}

    //public bool checkBoundaries()
    //{
    //    if(PlacementPos.x)
    //}

    public void PlaceBuilding()
    {
        bool isPlaceable = false;
        //Debug.Log(cellGrid.cells.GetLength(0));
        int row = 0; int col = 0;
        for (int i = 0; i < map.cells.GetLength(0); i++)
        {
            for (int k = 0; k < map.cells.GetLength(1); k++)
            {
                if (map.cells[i, k].X == curPlacementPos.x &&
                map.cells[i, k].Z == curPlacementPos.z)
                {
                    row = i; col = k;
                    isPlaceable = isPlaceable || assign_cells(map, curBuildingPreset, i, k);
                    //Debug.Log(assign_cells(i,k));
                }
                
            }
        }
        //   Debug.Log($"Placing building at: {curPlacementPos.x}, {curPlacementPos.z} ");
        int coverage = Coverage(curBuildingPreset.displayName);

        if (isPlaceable)
        {
            GameObject buildingObj = Instantiate(curBuildingPreset.prefab, curPlacementPos, Quaternion.identity);
            //foreach (var c in blueprintCells)
            //{
            //    Destroy(c.gameObject);
            //}
            //Debug.Log(buildingObj.GetInstanceID());
            map.addMapObject(buildingObj, coverage, row, col);
            attachToBuildings(buildingObj);
            CancelBuildingPlacement(); 
        }
    }

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

    public static string cropClone(string name, int id)
    {
        StringBuilder sb = new StringBuilder();

        for(int i = 0; i < name.Length; i++)
        {
            if(name[i] == '(')
            {
                break;
            }
            sb.Append(name[i]);
        }
        sb.Append(id.ToString());

        return sb.ToString();
    }
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

    //    int aux = coverage / 2;
    //    for (int x = -aux; x <= aux; x++)
    //    {
    //        for (int z = -aux; z <= aux; z++)
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
    //                if (c.Coordinate.X == curPlacementPos.x && c.Coordinate.Z == curPlacementPos.z)
    //                {
    //                    c.isFree = false;
    //                    c.Type = curBuildingPreset.displayName;

    //                }
    //            }
    //            x++;
    //        }
    ////        z++;
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

    public void selectObject()
    {   
        if (Input.GetMouseButtonDown(1))
        {           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (selection != null)
            {
                selection.GetComponent<MeshRenderer>().material = originalMaterial;
                selection = null;
                cellToBeDeleted = null;
                infoUI.SetActive(false);
            }
            if (Physics.Raycast(ray, out hit))
            {
           //     Debug.Log($"Placing building at: {curPlacementPos.x}, {curPlacementPos.z} ");
                selection = hit.transform;
                originalMaterial = selection.GetComponent<MeshRenderer>().material;
                if (selection.CompareTag("selectable"))
                {
                    selection.GetComponent<MeshRenderer>().material = selectionMaterial;
                    infoUI.SetActive(true);
                    var panelTransform = infoUI.transform;
                    var capacity = panelTransform.Find("Capacity").GetComponent<TextMeshProUGUI>();
                    Cell c = cellInfo();
                    capacity.text = $"Capacity is: {c.Type}";
                    cellToBeDeleted = c;
                }
                else
                {
                    selection = null;
                    infoUI.SetActive(false);
                    cellToBeDeleted = null;
                }
            }
        }
    }
    public void DeleteObject()
    {
        if(cellToBeDeleted != null)
        {
            int id = selection.gameObject.GetInstanceID();
            if(selection.gameObject.name.Split('-')[0].Equals("Cell"))
            {
                id = selection.gameObject.GetComponentInParent<Forest>().gameObject.GetInstanceID();
            }
            if (map.removeMapObject(id))
            {
                if (selection.gameObject.name.Split('-')[0].Equals("Cell"))
                {
                    Destroy(selection.gameObject.GetComponentInParent<Forest>().gameObject);
                }
                else
                {
                    Destroy(selection.gameObject);
                }
                int cellID = cellToBeDeleted.ID;
                foreach(var cell in map.cells)
                {
                    if( cell.ID == cellID)
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
    public Cell cellInfo()
    {
        for (int i = 0; i < map.cells.GetLength(0); i++)
        {
            for (int k = 0; k < map.cells.GetLength(1); k++)
            {
                if (map.cells[i, k].X == curPlacementPos.x &&
                map.cells[i, k].Z == curPlacementPos.z)
                {
                    return map.cells[i,k];
                }

            }
        }
        return null;
    }
    public static bool assign_cells(Map map, BuildingPreset curBuildingPreset, int row, int col)
    {
        int minValue = 1;
        int maxValue = 10000;
        int randomNumber = UnityEngine.Random.Range(minValue, maxValue);
        int half_cov = Coverage(curBuildingPreset.displayName) /2;
        int low_row = row - half_cov;
        int high_row = row + half_cov;
        int low_col = col - half_cov;
        int high_col = col + half_cov;
        //Debug.Log("low row: " + low_row + "high_row: " + high_row);
        //Debug.Log("low col: " + low_col + "high_col: " + high_col);
        //Debug.Log("-------------------------------------------------");
        if (low_row < 0 || high_row > map.cells.GetLength(0)-1 || low_col < 0 || high_col > map.cells.GetLength(1)-1)
        {
            return false;
        }
        else
        {
            for (int x = low_row; x <= high_row; x++)
            {
                for (int z = low_col; z <= high_col; z++)
                {
                    if (map.cells[x, z].isFree)
                    {
                        map.cells[x, z].isFree = false;
                        map.cells[x, z].Type = curBuildingPreset.displayName; 
                        map.cells[x, z].ID = randomNumber;
                        //Debug.Log(cellGrid.cells[x, z] + "id is: " + cellGrid.cells[x, z].ID);
                    }
                    else
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
                            for(tempCol = high_col; tempCol >= low_col; tempCol--)
                            {
                                map.cells[tempRow, tempCol].isFree = true;
                                map.cells[tempRow, tempCol].Type = "empty";
                                map.cells[tempRow, tempCol].ID = 0;
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }
    }

    //Cell GetCellAtPosition(int x, int y)
    //{
    //    check if the position is within the bounds of the grid
    //    if (x >= 0 && x < cellGrid.cells.GetLength(0) && y >= 0 && y < cellGrid.cells.GetLength(1))
    //    {
    //        Debug.Log("x:  " + x + " y: " + y);
    //        return cellGrid.cells[x, y];
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    //public bool SerializeJson()
    //{
    //    if (DataService.SaveData("/player-stats.json", cellGrid))
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //    //try
    //    //{
    //    //    PlayerStats data = DataService.LoadData<PlayerStats>("/player-stats.json", EncryptionEnabled);
    //    //    LoadTime = DateTime.Now.Ticks - startTime;
    //    //    InputField.text = "Loaded from file:\r\n" + JsonConvert.SerializeObject(data, Formatting.Indented);
    //    //    LoadTimeText.SetText($"Load Time: {(LoadTime / TimeSpan.TicksPerMillisecond):N4}ms");
    //    //}
    //    //catch (Exception e)
    //    //{
    //    //    Debug.LogError($"Could not read file! Show something on the UI here!");
    //    //    InputField.text = "<color=#ff0000>Error reading save file!</color>";
    //    //}
    //    //catch
    //    //{
    //    //    Debug.LogError("Could not save file!");
    //    //    //InputField.text = "<color=#ff0000>Error saving data!</color>";
    //    //}
    //}
    //public void DeserializeJSon()
    //{
    //    string path = Application.persistentDataPath + "/player-stats.json";
    //    if (File.Exists(path))
    //    {
    //        cellGrid = DataService.LoadData<CellGrid>("/player-stats.json");
    //        //InputField.text = "Loaded data goes here";
    //    }
    //    else
    //    {
    //        using FileStream stream = File.Create(path);
    //        stream.Close();
    //    }
    //    //try
    //    //{

    //    //}
    //    //catch (Exception)
    //    //{
    //    //    Debug.LogError($"Could not read file! Show something on the UI here!");           
    //    //}
    //}

    //public void ClearData()
    //{
    //    string path = Application.persistentDataPath + "/player-stats.json";
    //    if (File.Exists(path))
    //    {
    //        File.Delete(path);
    //        //InputField.text = "Loaded data goes here";
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        //string path = Application.persistentDataPath + "/player-stats.json";
        saveAndExitButton.onClick.AddListener(showPanel);
        ConfirmationPanel.gameObject.SetActive(false);
        ////savePath = "CurrentDate";
        ////SerializeJson();
        //if (new FileInfo(path).Length > 2) { 
        //    DeserializeJSon(); 
        //}
    }
    void showPanel()
    {
        ConfirmationPanel.gameObject.SetActive(true);
        YesButton.onClick.AddListener(SaveAndExit);
        NoButton.onClick.AddListener(Continue);
    }

    private void Continue()
    {
        ConfirmationPanel.gameObject.SetActive(false);
    }

    void SaveAndExit()
    {
        //SaveGame();
        //SceneManager.LoadScene("MenuScene");
        DataPersistenceManager.instance.SaveGame();
        Application.Quit();
    }
    //public void LoadData(GameData data)
    //{
    //    this.cellGrid = data.cellgrid;
    //}

    //public void SaveData(GameData data)
    //{
    //    data.cellgrid = this.cellGrid;
    //}
    //private void SaveGame()
    //{
    //    DateTime currentDate = DateTime.Now;
    //    PlayerPrefs.SetString("CurrentDate", currentDate.ToString("o"));
    //    PlayerPrefs.Save();
    //}

    //private void LoadGame()
    //{
    //    //if (PlayerPrefs.HasKey("CurrentDate"))
    //    //{
    //    //    string currentDateStr = PlayerPrefs.GetString(savePath);
    //    //    DateTime currentDate = DateTime.Parse(currentDateStr);
    //    //}
    //}

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

