using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public GameObject shopWindow;
    public GameObject UIBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    /// <summary>
    /// Changes the scene to game scene.
    /// </summary>
    // Update is called once per frame
    void Update()
    {
     //   Debug.Log(SceneManager.GetActiveScene());
        if(SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("GameScene")))
        {
            UIBar.SetActive(true);
            shopWindow.SetActive(true);
        }
    }
}
