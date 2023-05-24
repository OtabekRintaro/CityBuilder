using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
    public Button pauseButton;

    /// <summary>
    /// If the player clicks on the save and exit button, the game will be saved and the application will be closed.
    /// </summary>
    public void SaveAndExit()
    {
        DataPersistenceManager.instance.SaveGame();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    /// <summary>
    /// When the pause button is clicked, it calls the save and exit function.
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        pauseButton.onClick.AddListener(SaveAndExit);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
