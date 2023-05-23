using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    /// <summary>
    /// Assigns the instance to this object.
    /// </summary>
    private void Awake()
    {
        //Debug.Log(this.gameObject.name);
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        instance = this;
    }

    /// <summary>
    /// Initializes the data handler and finds all objects in the scene that implement the IDataPersistence interface.
    /// </summary>
    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadData();
    }

    /// <summary>
    /// When it is new game, initialize the game data to a new game.
    /// </summary>
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    /// <summary>
    /// Gets the deserialize game data using data handler's load function
    /// </summary>
    public void LoadData()
    {
        this.gameData = dataHandler.Load();
    }

    /// <summary>
    /// Loads the game data from a file using the data handler. If no data can be loaded, initialize to a new game.
    /// Push the loaded data to all other scripts that need it.
    /// </summary>
    public void LoadGame()
    {
        // load any saved data from a file using the data handler
        this.gameData = dataHandler.Load();

        // if no data can be loaded, initialize to a new game
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    /// <summary>
    /// Saves the game data by going through each data persistence object and calling their save data function.
    /// </summary>
    public void SaveGame()
    {
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }
        Debug.Log("saved date: " + gameData.currDate);
        //Debug.Log("saved population: " + gameData.population);
        // save that data to a file using the data handler
        dataHandler.Save(gameData);
    }

    //private void OnApplicationQuit()
    //{
    //    SaveGame();
    //    #if UNITY_EDITOR
    //            UnityEditor.EditorApplication.isPlaying = false;
    //    #endif
    //    Application.Quit();
    //}

    /// <summary>
    /// Find all objects in the scene that implement the IDataPersistence interface.
    /// </summary>
    /// <returns></returns>
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}