using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button continueButton;
    public Button newGameButton;
    public Button quitButton;
    GameManager gameManager;
    void Start()
    {
        continueButton.onClick.AddListener(ContinueGame);
        newGameButton.onClick.AddListener(NewGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void ContinueGame()
    {
        //SceneManager.LoadScene("GameScene");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));
        DataPersistenceManager.instance.LoadGame();

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("MenuScene"));
    }

    public void NewGame()
    {
        // b.ClearData();
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadScene("GameScene");
        //dataPersistanceManager = new DataPersistenceManager();
        // Debug.Log(DataPersistenceManager.instance);
        
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;

        #endif
        Application.Quit();
    }
}