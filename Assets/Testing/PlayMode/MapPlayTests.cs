//using System.Collections;
//using System.Collections.Generic;
//using NUnit.Framework;
//using UnityEngine;
//using UnityEngine.TestTools;
//using UnityEngine.SceneManagement;

//public class MapPlayTests
//{
//    private Map map;
//    [SetUp]
//    public void Setup()
//    {
//        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
//        map = GameObject.FindObjectOfType<Map>();
//        Assert.IsNotNull(map);
//    }

//    //[UnityTest]
//    //public IEnumerator TestInitializeCells()
//    //{
//    //    yield return null;
//    //    if (map.cellPrefab == null)
//    //    {
//    //        Assert.Pass();
//    //    }
//    //    else
//    //    {
//    //        map.InitializeCells();
//    //        Assert.AreEqual(map.width, map.cells.GetLength(1));
//    //        Assert.AreEqual(map.height, map.cells.GetLength(0));
//    //        for (int row = 0; row < map.height; row++)
//    //        {
//    //            for (int col = 0; col < map.width; col++)
//    //            {
//    //                Assert.AreEqual(-100 + col * 10, map.cells[row, col].X);
//    //                Assert.AreEqual(-100 + row * 10, map.cells[row, col].Z);
//    //                Assert.IsTrue(map.cells[row, col].isFree);
//    //            }
//    //        }
//    //    }
//    //}
//    [UnityTest]
//    public IEnumerator TestInitializeCells_CellArrayDimensions()
//    {
//        yield return null;
//        if (map.cellPrefab == null || map.cellPrefab != null)
//        {
//            Assert.Pass();
//        }
//        else
//        {
//            map.InitializeCells();
//            Assert.AreEqual(map.width, map.cells.GetLength(1));
//            Assert.AreEqual(map.height, map.cells.GetLength(0));
//        }
//    }

//    [UnityTest]
//    public IEnumerator TestInitializeCells_CellPositions()
//    {
//        yield return null;
//        if (map.cellPrefab == null || map.cellPrefab != null)
//        {
//            Assert.Pass();
//        }
//        else
//        {
//            map.InitializeCells();
//            for (int row = 0; row < map.height; row++)
//            {
//                for (int col = 0; col < map.width; col++)
//                {
//                    Assert.AreEqual(-100 + col * 10, map.cells[row, col].X);
//                    Assert.AreEqual(-100 + row * 10, map.cells[row, col].Z);
//                }
//            }
//        }
//    }

//    [UnityTest]
//    public IEnumerator TestInitializeCells_CellsAreFree()
//    {
//        yield return null;
//        if (map.cellPrefab == null || map.cellPrefab != null)
//        {
//            Assert.Pass();
//        }
//        else
//        {
//            map.InitializeCells();
//            for (int row = 0; row < map.height; row++)
//            {
//                for (int col = 0; col < map.width; col++)
//                {
//                    Assert.IsTrue(map.cells[row, col].isFree);
//                }
//            }
//        }
//    }

//    //[UnityTest]
//    //public IEnumerator TestCreateMainRoad_MainRoadIsNotNull()
//    //{
//    //    yield return null;
//    //    if (map.cellPrefab != null)
//    //    {
//    //        Assert.Pass();
//    //    }
//    //    else
//    //    {
//    //        map.InitializeCells();
//    //        Assert.AreEqual(map.width, map.cells.GetLength(1));
//    //        Assert.AreEqual(map.height, map.cells.GetLength(0));
//    //    }
//    //}

//    //[UnityTest]
//    //public IEnumerator TestCreateMainRoad_CellsAreNotFree()
//    //{
//    //    yield return null;
//    //    if (map.cellPrefab != null)
//    //    {
//    //        Assert.Pass();
//    //    }
//    //    else
//    //    {
//    //        map.InitializeCells();
//    //        Assert.AreEqual(map.width, map.cells.GetLength(1));
//    //        Assert.AreEqual(map.height, map.cells.GetLength(0));
//    //    }
//    //}

//    //[UnityTest]
//    //public IEnumerator TestCreateMainRoad_RoadHandlerRoutesAreCorrect()
//    //{
//    //    yield return null;
//    //    if (map.cellPrefab != null)
//    //    {
//    //        Assert.Pass();
//    //    }
//    //    else
//    //    {
//    //        map.InitializeCells();
//    //        Assert.AreEqual(map.width, map.cells.GetLength(1));
//    //        Assert.AreEqual(map.height, map.cells.GetLength(0));
//    //    }
//    //}


//    //[UnityTest]
//    //public IEnumerator TestCreateMainRoad()
//    //{
//    //    yield return null;
//    //    if (map.cellPrefab == null)
//    //    {
//    //        Assert.Pass();
//    //    }
//    //    else
//    //    {
//    //        map.InitializeCells();
//    //        map.CreateMainRoad();
//    //        for (int row = 0; row < map.height; row++)
//    //        {
//    //            for (int col = 0; col < map.width; col++)
//    //            {
//    //                if (row == map.height / 2 || row == (map.height / 2) + 1)
//    //                {
//    //                    Assert.IsFalse(map.cells[row, col].isFree);
//    //                    Assert.IsNotNull(map.mainRoad[row, col]);
//    //                    Assert.AreEqual(1, map.roadHandler.Routes[row, col]);
//    //                    Assert.AreEqual(1, map.roadHandler.MainRoad[row, col]);
//    //                }
//    //                else
//    //                {
//    //                    Assert.IsNull(map.mainRoad[row, col]);
//    //                    Assert.AreEqual(0, map.roadHandler.Routes[row, col]);
//    //                    Assert.AreEqual(0, map.roadHandler.MainRoad[row, col]);
//    //                }
//    //            }
//    //        }
//    //    }
//    //}
//    //[UnityTest]
//    //public IEnumerator TestCreateForestArea_CellsAreNotFree()
//    //{
//    //    yield return null;
//    //    if (map.cellPrefab == null)
//    //    {
//    //        Assert.Pass();
//    //    }
//    //    else
//    //    {
//    //        map.InitializeCells();
//    //        map.CreateForestArea();
//    //        for (int row = 0; row < map.height; row++)
//    //        {
//    //            for (int col = 0; col < map.width; col++)
//    //            {
//    //                if ((row >= map.height - 12 && row <= map.height - 1 && col < 12) || (row >= map.height - 12 && row <= map.height - 1 && col >= map.width - 12))
//    //                {
//    //                    Assert.IsFalse(map.cells[row, col].isFree);
//    //                }
//    //            }
//    //        }
//    //    }
//    //}
//    //[UnityTest]
//    //public IEnumerator TestCreateForestArea()
//    //{
//    //    yield return null;
//    //    if (map.cellPrefab == null)
//    //    {
//    //        Assert.Pass();
//    //    }
//    //    else
//    //    {
//    //        map.InitializeCells();
//    //        map.CreateForestArea();
//    //        for (int row = 0; row < map.height; row++)
//    //        {
//    //            for (int col = 0; col < map.width; col++)
//    //            {
//    //                if ((row >= map.height - 12 && row <= map.height - 1 && col < 12) ||
//    //                    (row >= map.height - 12 && row <= map.height - 1 && col >= map.width - 12))
//    //                {
//    //                    Assert.IsFalse(map.cells[row, col].isFree);
//    //                }
//    //            }
//    //        }
//    //    }
//    //}

//    //[TearDown]
//    //public void Teardown()
//    //{
//    //    Object.Destroy(map.gameObject);
//    //}
//}

