using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RoadHandlerPlayTests : MonoBehaviour
{
    private RoadHandler roadHandler;
    private MapObject mapObject;

    [SetUp]
    public void SetUp()
    {
        roadHandler = new RoadHandler(10);
        mapObject = new MapObject();
        mapObject.position = new Position(5, 5);
        mapObject.coverage = 1;
    }

    [Test]
    public void TestFindPublicRoad()
    {
        bool result = roadHandler.findPublicRoad(mapObject);
        Assert.IsFalse(result);
    }

    [Test]
    public void TestCheckCoverageOne()
    {
        bool result = roadHandler.CheckCoverageOne(mapObject, 5, 5);
        Assert.IsFalse(result);
    }

    [Test]
    public void TestCheckTopAndBottom()
    {
        bool result = roadHandler.CheckTopAndBottom(mapObject, new Position(4, 4));
        Assert.IsFalse(result);
    }

    [Test]
    public void TestCheckLeftAndRight()
    {
        bool result = roadHandler.CheckLeftAndRight(mapObject, new Position(4, 4));
        Assert.IsFalse(result);
    }

    [Test]
    public void TestPredfsForRoads()
    {
        bool result = roadHandler.predfsForRoads(mapObject, 5, 5, new int[10, 10], false);
        Assert.IsFalse(result);
    }

    [Test]
    public void TestDfs()
    {
        bool result = roadHandler.dfs(5, 5, new int[10, 10]);
        Assert.IsFalse(result);
    }

    [Test]
    public void TestCheckConnection()
    {
        var road = new MapObject();
        road.position = new Position(6, 6);

        bool result = roadHandler.checkConnection(mapObject, road);
        Assert.IsFalse(result);
    }

    [Test]
    public void TestBfs()
    {
        bool result = roadHandler.bfs(mapObject, 6, 6, new int[10, 10]);
        Assert.IsFalse(result);
    }
}
