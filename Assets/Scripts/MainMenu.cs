using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button continueButton;
    public Button newGameButton;
    public Button quitButton;

    /// <summary>
    /// Add listeners to the buttons.
    /// </summary>
    void Start()
    {
        continueButton.onClick.AddListener(ContinueGame);
        newGameButton.onClick.AddListener(NewGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    /// <summary>
    /// Switches to game scene and loads the game.
    /// </summary>
    public void ContinueGame()
    {
        //SceneManager.LoadScene("GameScene");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));
        DataPersistenceManager.instance.LoadGame();

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("MenuScene"));
    }

    /// <summary>
    /// When new game button is clicked, it clears the saved data and loads a new game and switches to game scene.
    /// </summary>
    public void NewGame()
    {
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadScene("GameScene");
        
    }

    /// <summary>
    /// When quit button is clicked, it quits the game.
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;

        #endif
        Application.Quit();
    }
}