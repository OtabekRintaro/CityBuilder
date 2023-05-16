using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button continueButton;
    public Button newGameButton;
    public Button quitButton;
    private BuildingPlacer b;
    DataPersistenceManager dataPersistanceManager;
    void Start()
    {
        continueButton.onClick.AddListener(ContinueGame);
        newGameButton.onClick.AddListener(NewGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    void ContinueGame()
    {
        //b.DeserializeJSon();
        SceneManager.LoadScene("GameScene");
        //dataPersistanceManager = new DataPersistenceManager();
        DataPersistenceManager.instance.LoadGame();
    }

    void NewGame()
    {
        // b.ClearData();
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadScene("GameScene");
        //dataPersistanceManager = new DataPersistenceManager();
        // Debug.Log(DataPersistenceManager.instance);
        
    }

    void QuitGame()
    {
        #if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;

        #endif
        Application.Quit();
    }
}