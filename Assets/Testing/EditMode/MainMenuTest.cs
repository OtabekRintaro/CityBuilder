using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MainMenuTest
{
    [Test]
    public void QuitGameTest()
    {
        MainMenu mainMenu = new GameObject().AddComponent<MainMenu>();
        mainMenu.QuitGame();
        // It is not possible to directly test the Application.Quit() method, so we cannot make assertions about it
        // However, this test ensures that the QuitGame() method is called without throwing any exceptions
    }
}
