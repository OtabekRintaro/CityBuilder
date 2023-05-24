using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MapObjectPlayTests
{
    private MapObject mapObject;
    private GameObject gameObject;
    [SetUp]
    public void SetUp()
    {
        GameObject mapObjectObject = new GameObject();
        mapObject = mapObjectObject.AddComponent<MapObject>();
        gameObject = new GameObject();
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
    [UnityTest]
    public IEnumerator TestMapObjectCreation()
    {
        var gameObject = new GameObject();
        var residentialZone = MapObject.getMapObject("ResidentialZone", gameObject);
        Assert.IsNotNull(residentialZone);
        Assert.IsInstanceOf<ResidentialZone>(residentialZone);

        var industrialZone = MapObject.getMapObject("IndustrialZone", gameObject);
        Assert.IsNotNull(industrialZone);
        Assert.IsInstanceOf<IndustrialZone>(industrialZone);

        var commercialZone = MapObject.getMapObject("CommercialZone", gameObject);
        Assert.IsNotNull(commercialZone);
        Assert.IsInstanceOf<CommercialZone>(commercialZone);

        var road = MapObject.getMapObject("Road", gameObject);
        Assert.IsNotNull(road);
        Assert.IsInstanceOf<Road>(road);

        var stadium = MapObject.getMapObject("Stadium", gameObject);
        Assert.IsNotNull(stadium);
        Assert.IsInstanceOf<Stadium>(stadium);

        var police = MapObject.getMapObject("Police", gameObject);
        Assert.IsNotNull(police);
        Assert.IsInstanceOf<Police>(police);

        var forest = MapObject.getMapObject("Forest", gameObject);
        Assert.IsNull(forest);

        var fireDepartment = MapObject.getMapObject("FireDepartment", gameObject);
        Assert.IsNotNull(fireDepartment);
        Assert.IsInstanceOf<FireDepartment>(fireDepartment);

        yield return null;
    }

    [Test]
    public void TestMapObjectCosts()
    {
        int residentialCost = MapObject.getCost("ResidentialZone");
        Assert.AreEqual(200, residentialCost);

        int industrialCost = MapObject.getCost("IndustrialZone");
        Assert.AreEqual(200, industrialCost);

        int commercialCost = MapObject.getCost("CommercialZone");
        Assert.AreEqual(200, commercialCost);

        int roadCost = MapObject.getCost("Road");
        Assert.AreEqual(50, roadCost);

        int stadiumCost = MapObject.getCost("Stadium");
        Assert.AreEqual(400, stadiumCost);

        int policeCost = MapObject.getCost("Police");
        Assert.AreEqual(50, policeCost);

        int forestCost = MapObject.getCost("Forest");
        Assert.AreEqual(50, forestCost);

        int fireDepartmentCost = MapObject.getCost("FireDepartment");
        Assert.AreEqual(0, fireDepartmentCost);
    }
    [UnityTest]
    public IEnumerator TestResidentialZoneCreation()
    {
        var residentialZone = MapObject.getMapObject("ResidentialZone", gameObject);
        Assert.IsNotNull(residentialZone);
        Assert.IsInstanceOf<ResidentialZone>(residentialZone);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestIndustrialZoneCreation()
    {
        var industrialZone = MapObject.getMapObject("IndustrialZone", gameObject);
        Assert.IsNotNull(industrialZone);
        Assert.IsInstanceOf<IndustrialZone>(industrialZone);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestCommercialZoneCreation()
    {
        var commercialZone = MapObject.getMapObject("CommercialZone", gameObject);
        Assert.IsNotNull(commercialZone);
        Assert.IsInstanceOf<CommercialZone>(commercialZone);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestRoadCreation()
    {
        var road = MapObject.getMapObject("Road", gameObject);
        Assert.IsNotNull(road);
        Assert.IsInstanceOf<Road>(road);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestStadiumCreation()
    {
        var stadium = MapObject.getMapObject("Stadium", gameObject);
        Assert.IsNotNull(stadium);
        Assert.IsInstanceOf<Stadium>(stadium);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestPoliceCreation()
    {
        var police = MapObject.getMapObject("Police", gameObject);
        Assert.IsNotNull(police);
        Assert.IsInstanceOf<Police>(police);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestForestCreation()
    {
        var forest = MapObject.getMapObject("Forest", gameObject);
        Assert.IsNull(forest);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestFireDepartmentCreation()
    {
        var fireDepartment = MapObject.getMapObject("FireDepartment", gameObject);
        Assert.IsNotNull(fireDepartment);
        Assert.IsInstanceOf<FireDepartment>(fireDepartment);
        yield return null;
    }

    [Test]
    public void TestResidentialCost()
    {
        int residentialCost = MapObject.getCost("ResidentialZone");
        Assert.AreEqual(200, residentialCost);
    }

    [Test]
    public void TestIndustrialCost()
    {
        int industrialCost = MapObject.getCost("IndustrialZone");
        Assert.AreEqual(200, industrialCost);
    }

    [Test]
    public void TestCommercialCost()
    {
        int commercialCost = MapObject.getCost("CommercialZone");
        Assert.AreEqual(200, commercialCost);
    }

    [Test]
    public void TestRoadCost()
    {
        int roadCost = MapObject.getCost("Road");
        Assert.AreEqual(50, roadCost);
    }

    [Test]
    public void TestStadiumCost()
    {
        int stadiumCost = MapObject.getCost("Stadium");
        Assert.AreEqual(400, stadiumCost);
    }

    [Test]
    public void TestPoliceCost()
    {
        int policeCost = MapObject.getCost("Police");
        Assert.AreEqual(50, policeCost);
    }

    [Test]
    public void TestForestCost()
    {
        int forestCost = MapObject.getCost("Forest");
        Assert.AreEqual(50, forestCost);
    }

    [Test]
    public void TestFireDepartmentCost()
    {
        int fireDepartmentCost = MapObject.getCost("FireDepartment");
        Assert.AreEqual(0, fireDepartmentCost);
    }
}