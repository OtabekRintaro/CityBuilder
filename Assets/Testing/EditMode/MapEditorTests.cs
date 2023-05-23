using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class MapEditorTests
{
    private Map map;

    [SetUp]
    public void SetUp()
    {
        GameObject mapObject = new GameObject();
        map = mapObject.AddComponent<Map>();

        // Create a new Cell object
        GameObject cellObject = new GameObject();
        Cell cell = cellObject.AddComponent<Cell>();

        GameObject roadObject = new GameObject();
        Road road = roadObject.AddComponent<Road>();
        map.roadPrefab = road;


        // Assign the Cell object to the cellPrefab field
        map.cellPrefab = cell;
        map.roadPrefab = road;
    }

    [Test]
    public void TestInitializeCells()
    {
        map.InitializeCells();

        Assert.AreEqual(map.height, map.cells.GetLength(0));
        Assert.AreEqual(map.width, map.cells.GetLength(1));
        Assert.AreEqual(map.height, map.mainRoad.GetLength(0));
        Assert.AreEqual(map.width, map.mainRoad.GetLength(1));

        for (int row = 0; row < map.height; row++)
        {
            for (int col = 0; col < map.width; col++)
            {
                Assert.IsNotNull(map.cells[row, col]);
                Assert.AreEqual(-100 + col * 10, map.cells[row, col].X);
                Assert.AreEqual(-100 + row * 10, map.cells[row, col].Z);
                Assert.IsTrue(map.cells[row, col].isFree);
            }
        }
    }
    [Test]
    public void TestInitializeCells_CellsArrayHasCorrectNumberOfRows()
    {
        map.InitializeCells();

        Assert.AreEqual(map.height, map.cells.GetLength(0));
    }

    [Test]
    public void TestInitializeCells_CellsArrayHasCorrectNumberOfColumns()
    {
        map.InitializeCells();

        Assert.AreEqual(map.width, map.cells.GetLength(1));
    }

    [Test]
    public void TestInitializeCells_MainRoadArrayHasCorrectNumberOfRows()
    {
        map.InitializeCells();

        Assert.AreEqual(map.height, map.mainRoad.GetLength(0));
    }

    [Test]
    public void TestInitializeCells_MainRoadArrayHasCorrectNumberOfColumns()
    {
        map.InitializeCells();

        Assert.AreEqual(map.width, map.mainRoad.GetLength(1));
    }

    [Test]
    public void TestInitializeCells_AllCellsAreNotNull()
    {
        map.InitializeCells();

        for (int row = 0; row < map.height; row++)
        {
            for (int col = 0; col < map.width; col++)
            {
                Assert.IsNotNull(map.cells[row, col]);
            }
        }
    }

    [Test]
    public void TestInitializeCells_AllCellsHaveCorrectXValues()
    {
        map.InitializeCells();

        for (int row = 0; row < map.height; row++)
        {
            for (int col = 0; col < map.width; col++)
            {
                Assert.AreEqual(-100 + col * 10, map.cells[row, col].X);
            }
        }
    }

    [Test]
    public void TestInitializeCells_AllCellsHaveCorrectZValues()
    {
        map.InitializeCells();

        for (int row = 0; row < map.height; row++)
        {
            for (int col = 0; col < map.width; col++)
            {
                Assert.AreEqual(-100 + row * 10, map.cells[row, col].Z);
            }
        }
    }

    [Test]
    public void TestInitializeCells_AllCellsAreFree()
    {
        map.InitializeCells();

        for (int row = 0; row < map.height; row++)
        {
            for (int col = 0; col < map.width; col++)
            {
                Assert.IsTrue(map.cells[row, col].isFree);
            }
        }
    }

    [Test]
    public void TestCreateMainRoad()
    {
        map.InitializeCells();
        map.CreateMainRoad();

        for (int row = 0; row < map.height; row++)
        {
            for (int col = 0; col < map.width; col++)
            {
                if (row == map.height / 2 || row == (map.height / 2) + 1)
                {
                    Assert.IsNotNull(map.mainRoad[row, col]);
                }
                else
                {
                    Assert.IsNull(map.mainRoad[row, col]);
                }
            }
        }
    }

    [Test]
    public void TestCreateMainRoad_MainRoadIsNotNullOnMiddleRows()
    {
        map.InitializeCells();
        map.CreateMainRoad();

        for (int row = 0; row < map.height; row++)
        {
            if (row == map.height / 2 || row == (map.height / 2) + 1)
            {
                for (int col = 0; col < map.width; col++)
                {
                    Assert.IsNotNull(map.mainRoad[row, col]);
                }
            }
        }
    }

    [Test]
    public void TestCreateMainRoad_MainRoadIsNullOnOtherRows()
    {
        map.InitializeCells();
        map.CreateMainRoad();

        for (int row = 0; row < map.height; row++)
        {
            if (row != map.height / 2 && row != (map.height / 2) + 1)
            {
                for (int col = 0; col < map.width; col++)
                {
                    Assert.IsNull(map.mainRoad[row, col]);
                }
            }
        }
    }
    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(map.gameObject);
    }
}
