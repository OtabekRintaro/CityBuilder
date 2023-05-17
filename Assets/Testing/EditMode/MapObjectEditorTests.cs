using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MapObjectEditorTests
{
    private MapObject mapObject;

    [SetUp]
    public void SetUp()
    {
        GameObject mapObjectObject = new GameObject();
        mapObject = mapObjectObject.AddComponent<MapObject>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(mapObject.gameObject);
    }

    [Test]
    public void TestCheckPublicRoadConnection()
    {
        Assert.AreEqual(false, mapObject.checkPublicRoadConnection());
    }

    [Test]
    public void TestConnectToPublicRoad()
    {
        mapObject.connectToPublicRoad(true);
        Assert.AreEqual(true, mapObject.checkPublicRoadConnection());
    }
}
