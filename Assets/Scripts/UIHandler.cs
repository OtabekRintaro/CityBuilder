using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public GameObject shopWindow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     //   Debug.Log(SceneManager.GetActiveScene());
        if(SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("GameScene")))
        {
            shopWindow.SetActive(true);
        }
    }
}
