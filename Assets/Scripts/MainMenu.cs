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
        DataPersistenceManager.instance.LoadGame();
    }

    void NewGame()
    {
       // b.ClearData();
        SceneManager.LoadScene("GameScene");
        DataPersistenceManager.instance.NewGame();
    }

    void QuitGame()
    {
        Application.Quit();
    }
}