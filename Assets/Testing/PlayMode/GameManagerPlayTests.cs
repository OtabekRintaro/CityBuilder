using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

//public class GameManagerPlayTests
//{
//    //private GameManager gameManager;
//    //private Map map;
//    //private Cell cellPrefab;

//    //[SetUp]
//    //public void SetUp()
//    //{
//    //    GameObject gameManagerObject = new GameObject();
//    //    gameManager = gameManagerObject.AddComponent<GameManager>();

//    //    GameObject mapObject = new GameObject();
//    //    map = mapObject.AddComponent<Map>();
//    //    gameManager.map = map;

//    //    cellPrefab = Resources.Load<Cell>("Assets/Cell.prefab");
//    //    map.cellPrefab = cellPrefab;
//    //}


//    //[TearDown]
//    //public void TearDown()
//    //{
//    //    Object.Destroy(gameManager.gameObject);
//    //    Object.Destroy(map.gameObject);
//    //}

//    ////[TearDown]
//    ////public void TearDown()
//    ////{
//    ////    Object.Destroy(gameManager.gameObject);
//    ////}

//    //[UnityTest]
//    //public IEnumerator TestGeneralBudgetStartValue()
//    //{
//    //    yield return null;
//    //    Assert.AreEqual(20000, gameManager.generalBudget);
//    //}

//    //[UnityTest]
//    //public IEnumerator TestGeneralPopulationStartValue()
//    //{
//    //    yield return null;
//    //    Assert.AreEqual(0, gameManager.generalPopulation);
//    //}

//    //[UnityTest]
//    //public IEnumerator TestGeneralSatisfactionStartValue()
//    //{
//    //    yield return null;
//    //    Assert.AreEqual(0, gameManager.generalSatisfaction);
//    //}

//    //[UnityTest]
//    //public IEnumerator TestLoadData()
//    //{
//    //    yield return null;

//    //    GameData data = new GameData();
//    //    data.generalBudget = 10000;
//    //    data.generalPopulation = 5000;
//    //    data.generalSatisfaction = 3;

//    //    gameManager.LoadData(data);

//    //    Assert.AreEqual(10000, gameManager.generalBudget);
//    //    Assert.AreEqual(5000, gameManager.generalPopulation);
//    //    Assert.AreEqual(3, gameManager.generalSatisfaction);
//    //}

//    //[UnityTest]
//    //public IEnumerator TestSaveData()
//    //{
//    //    yield return null;

//    //    GameData data = new GameData();
//    //    gameManager.SaveData(data);

//    //    Assert.AreEqual(20000, data.generalBudget);
//    //    Assert.AreEqual(0, data.generalPopulation);
//    //    Assert.AreEqual(0, data.generalSatisfaction);
//    //}

//    //[UnityTest]
//    //public IEnumerator TestPlayerLost_GeneralSatisfactionLessThanNegativeFour_ReturnsTrue()
//    //{
//    //    gameManager.generalSatisfaction = -5;
//    //    Assert.IsTrue(gameManager.PlayerLost());
//    //    yield return null;
//    //}

//    //[UnityTest]
//    //public IEnumerator TestPlayerLost_GeneralSatisfactionGreaterThanNegativeFour_ReturnsFalse()
//    //{
//    //    gameManager.generalSatisfaction = -3;
//    //    Assert.IsFalse(gameManager.PlayerLost());
//    //    yield return null;
//    //}

//    //[UnityTest]
//    //public IEnumerator TestLoadData_LoadsDataCorrectly()
//    //{
//    //    GameData data = new GameData();
//    //    data.generalBudget = 1000;
//    //    data.generalPopulation = 200;
//    //    data.generalSatisfaction = 5;

//    //    gameManager.LoadData(data);

//    //    Assert.AreEqual(1000, gameManager.generalBudget);
//    //    Assert.AreEqual(200, gameManager.generalPopulation);
//    //    Assert.AreEqual(5, gameManager.generalSatisfaction);

//    //    yield return null;
//    //}

//    //[UnityTest]
//    //public IEnumerator TestSaveData_SavesDataCorrectly()
//    //{
//    //    GameData data = new GameData();
//    //    gameManager.generalBudget = 1000;
//    //    gameManager.generalPopulation = 200;
//    //    gameManager.generalSatisfaction = 5;

//    //    gameManager.SaveData(data);

//    //    Assert.AreEqual(1000, data.generalBudget);
//    //    Assert.AreEqual(200, data.generalPopulation);
//    //    Assert.AreEqual(5, data.generalSatisfaction);

//    //    yield return null;
//    //}
//    //private GameManager gameManager;

//    //[SetUp]
//    //public void SetUp()
//    //{
//    //    GameObject gameManagerObject = new GameObject();
//    //    gameManager = gameManagerObject.AddComponent<GameManager>();
//    //}

//    //[TearDown]
//    //public void TearDown()
//    //{
//    //    Object.Destroy(gameManager.gameObject);
//    //}

//    //[UnityTest]
//    //public IEnumerator TestPanelIsNull_PassesTest()
//    //{
//    //    if (gameManager.panel == null)
//    //    {
//    //        Assert.Pass();
//    //    }
//    //    yield return null;
//    //}

//    private GameManager gameManager;
//    private Map map;
//    private GameObject panel;
//    private InfoBar infoBar;

//    [SetUp]
//    public void SetUp()
//    {
//        LogAssert.ignoreFailingMessages = true;

//        GameObject gameManagerObject = new GameObject();
//        gameManager = gameManagerObject.AddComponent<GameManager>();

//        panel = new GameObject();
//        gameManager.panel = panel;

//        infoBar = new GameObject().AddComponent<InfoBar>();
//        gameManager.infoBar = infoBar;

//        GameObject mapObject = new GameObject();
//        map = mapObject.AddComponent<Map>();
//        gameManager.map = map;
//    }

//    [TearDown]
//    public void TearDown()
//    {
//        Object.Destroy(gameManager.gameObject);
//        Object.Destroy(map.gameObject);
//        Object.Destroy(panel);
//        Object.Destroy(infoBar.gameObject);
//    }

//    [UnityTest]
//    public IEnumerator TestSpendMoney_DoesNotThrowException()
//    {
//        gameManager.spendMoney();
//        yield return null;
//    }
//}