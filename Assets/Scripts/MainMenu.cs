using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        //shopWindow.visible();
    }

    public void PlayGame()
    {
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Debug.Log(this.Get);
        //this.GetComponentInParent<Renderer>().enabled = false;
        //this.GetComponentInChildren<Renderer>().enabled = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
