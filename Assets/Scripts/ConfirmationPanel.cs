using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
    public Button pauseButton;
    
    public void SaveAndExit()
    {
        DataPersistenceManager.instance.SaveGame();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

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
