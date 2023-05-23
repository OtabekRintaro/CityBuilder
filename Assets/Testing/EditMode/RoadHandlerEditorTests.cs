using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RoadHandlerTests
{
    private RoadHandler _roadHandler;

    [SetUp]
    public void SetUp()
    {
        _roadHandler = new RoadHandler(10);
    }

    [Test]
    public void TestFindPublicRoad()
    {
        // Set up test conditions
        MapObject mapObject = new MapObject();
        mapObject.position = new Position(0, 0);
        mapObject.coverage = 1;

        // Call the findPublicRoad method
        bool result = _roadHandler.findPublicRoad(mapObject);

        // Check if the returned result is correct
        Assert.IsFalse(result);
    }

    [Test]
    public void TestCheckCoverageOne()
    {
        // Set up test conditions
        MapObject mapObject = new MapObject();
        mapObject.position = new Position(0, 0);
        mapObject.coverage = 1;

        // Call the CheckCoverageOne method
        bool result = _roadHandler.CheckCoverageOne(mapObject, 0, 0);

        // Check if the returned result is correct
        Assert.IsFalse(result);
    }

    //[Test]
    //public void TestCheckTopAndBottom()
    //{
    //    // Set up test conditions
    //    MapObject mapObject = new MapObject();
    //    mapObject.position = new Position(0, 0);
    //    mapObject.coverage = 2;

    //    // Call the CheckTopAndBottom method
    //    bool result = _roadHandler.CheckTopAndBottom(mapObject, new Position(-1, -1));

    //    // Check if the returned result is correct
    //    Assert.IsFalse(result);
    //}

    //[Test]
    //public void TestCheckLeftAndRight()
    //{
    //    // Set up test conditions
    //    MapObject mapObject = new MapObject();
    //    mapObject.position = new Position(0, 0);
    //    mapObject.coverage = 2;

    //    // Call the CheckLeftAndRight method
    //    bool result = _roadHandler.CheckLeftAndRight(mapObject, new Position(-1, -1));

    //    // Check if the returned result is correct
    //    Assert.IsFalse(result);
    //}
}
