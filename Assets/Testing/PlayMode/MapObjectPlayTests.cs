using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MapObjectPlayTests
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
        Object.Destroy(mapObject.gameObject);
    }

    [UnityTest]
    public IEnumerator TestCheckPublicRoadConnection()
    {
        yield return null;
        Assert.AreEqual(false, mapObject.checkPublicRoadConnection());
    }

    [UnityTest]
    public IEnumerator TestConnectToPublicRoad()
    {
        yield return null;
        mapObject.connectToPublicRoad(true);
        Assert.AreEqual(true, mapObject.checkPublicRoadConnection());
    }
}