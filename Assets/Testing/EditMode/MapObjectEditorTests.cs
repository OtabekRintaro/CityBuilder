using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MapObjectTests
{
    private GameObject _gameObject;

    [SetUp]
    public void SetUp()
    {
        _gameObject = new GameObject();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_gameObject);
    }

    [Test]
    public void TestGetResidentialZoneMapObject()
    {
        // Call the getMapObject method
        MapObject residentialZone = MapObject.getMapObject("ResidentialZone", _gameObject);

        // Check if the returned object is of the correct type
        Assert.IsInstanceOf<ResidentialZone>(residentialZone);
    }

    [Test]
    public void TestGetIndustrialZoneMapObject()
    {
        // Call the getMapObject method
        MapObject industrialZone = MapObject.getMapObject("IndustrialZone", _gameObject);

        // Check if the returned object is of the correct type
        Assert.IsInstanceOf<IndustrialZone>(industrialZone);
    }

    [Test]
    public void TestGetCommercialZoneMapObject()
    {
        // Call the getMapObject method
        MapObject commercialZone = MapObject.getMapObject("CommercialZone", _gameObject);

        // Check if the returned object is of the correct type
        Assert.IsInstanceOf<CommercialZone>(commercialZone);
    }

    [Test]
    public void TestGetRoadMapObject()
    {
        // Call the getMapObject method
        MapObject road = MapObject.getMapObject("Road", _gameObject);

        // Check if the returned object is of the correct type
        Assert.IsInstanceOf<Road>(road);
    }

    [Test]
    public void TestGetStadiumMapObject()
    {
        // Call the getMapObject method
        MapObject stadium = MapObject.getMapObject("Stadium", _gameObject);

        // Check if the returned object is of the correct type
        Assert.IsInstanceOf<Stadium>(stadium);
    }

    [Test]
    public void TestGetPoliceMapObject()
    {
        // Call the getMapObject method
        MapObject police = MapObject.getMapObject("Police", _gameObject);

        // Check if the returned object is of the correct type
        Assert.IsInstanceOf<Police>(police);
    }

    [Test]
    public void TestGetResidentialZoneCost()
    {
        // Call the getCost method
        int residentialZoneCost = MapObject.getCost("ResidentialZone");

        // Check if the returned cost is correct
        Assert.AreEqual(200, residentialZoneCost);
    }

    [Test]
    public void TestGetIndustrialZoneCost()
    {
        // Call the getCost method
        int industrialZoneCost = MapObject.getCost("IndustrialZone");

        // Check if the returned cost is correct
        Assert.AreEqual(200, industrialZoneCost);
    }

    [Test]
    public void TestGetCommercialZoneCost()
    {
        // Call the getCost method
        int commercialZoneCost = MapObject.getCost("CommercialZone");

        // Check if the returned cost is correct
        Assert.AreEqual(200, commercialZoneCost);
    }

    [Test]
    public void TestGetRoadCost()
    {
        // Call the getCost method
        int roadCost = MapObject.getCost("Road");

        // Check if the returned cost is correct
        Assert.AreEqual(50, roadCost);
    }

    [Test]
    public void TestGetStadiumCost()
    {
        // Call the getCost method
        int stadiumCost = MapObject.getCost("Stadium");

        // Check if the returned cost is correct
        Assert.AreEqual(400, stadiumCost);
    }

    [Test]
    public void TestGetPoliceCost()
    {
        // Call the getCost method
        int policeCost = MapObject.getCost("Police");

        // Check if the returned cost is correct
        Assert.AreEqual(50, policeCost);
    }

    [Test]
    public void TestGetForestCost()
    {
        // Call the getCost method
        int forestCost = MapObject.getCost("Forest");

        // Check if the returned cost is correct
        Assert.AreEqual(50, forestCost);
    }

    [Test]
    public void TestCheckPublicRoadConnection()
    {
        // Set up test conditions
        MapObject mapObject = new MapObject();

        // Call the checkPublicRoadConnection method
        bool result = mapObject.checkPublicRoadConnection();

        // Check if the returned result is correct
        Assert.IsFalse(result);
    }

    [Test]
    public void TestConnectToPublicRoad()
    {
        // Set up test conditions
        MapObject mapObject = new MapObject();

        // Call the connectToPublicRoad method
        mapObject.connectToPublicRoad(true);

        // Check if the isConnectedToPublicRoad field was updated correctly
        Assert.IsTrue(mapObject.checkPublicRoadConnection());
    }
}