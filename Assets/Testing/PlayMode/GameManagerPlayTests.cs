//using System.Collections;
//using System.Collections.Generic;
//using NUnit.Framework;
//using UnityEngine;
//using UnityEngine.TestTools;

//public class GameManagerPlayTests
//{
//    private GameManager gameManager;

//    [SetUp]
//    public void SetUp()
//    {
//        GameObject gameManagerObject = new GameObject();
//        gameManager = gameManagerObject.AddComponent<GameManager>();
//    }

//    //[TearDown]
//    //public void TearDown()
//    //{
//    //    Object.Destroy(gameManager.gameObject);
//    //}

//    [UnityTest]
//    public IEnumerator TestGeneralBudgetStartValue()
//    {
//        yield return null;
//        Assert.AreEqual(20000, gameManager.generalBudget);
//    }

//    [UnityTest]
//    public IEnumerator TestGeneralPopulationStartValue()
//    {
//        yield return null;
//        Assert.AreEqual(0, gameManager.generalPopulation);
//    }

//    [UnityTest]
//    public IEnumerator TestGeneralSatisfactionStartValue()
//    {
//        yield return null;
//        Assert.AreEqual(0, gameManager.generalSatisfaction);
//    }

//    [UnityTest]
//    public IEnumerator TestLoadData()
//    {
//        yield return null;

//        GameData data = new GameData();
//        data.generalBudget = 10000;
//        data.generalPopulation = 5000;
//        data.generalSatisfaction = 3;

//        gameManager.LoadData(data);

//        Assert.AreEqual(10000, gameManager.generalBudget);
//        Assert.AreEqual(5000, gameManager.generalPopulation);
//        Assert.AreEqual(3, gameManager.generalSatisfaction);
//    }

//    [UnityTest]
//    public IEnumerator TestSaveData()
//    {
//        yield return null;

//        GameData data = new GameData();
//        gameManager.SaveData(data);

//        Assert.AreEqual(20000, data.generalBudget);
//        Assert.AreEqual(0, data.generalPopulation);
//        Assert.AreEqual(0, data.generalSatisfaction);
//    }
//}