using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ResidentialZoneTests
{
    private ResidentialZone residentialZone;
    private Cell[,] cityMatrix;
    private Map map;
    [SetUp]
    public void SetUp()
    {
        GameObject residentialZoneObject = new GameObject();
        residentialZone = residentialZoneObject.AddComponent<ResidentialZone>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(residentialZone.gameObject);
    }

    [UnityTest]
    public IEnumerator TestSatisfactionStartValue()
    {
        yield return null;
        Assert.AreEqual(5, residentialZone.satisfaction);
    }

    [UnityTest]
    public IEnumerator TestWorkConnectionStartValue()
    {
        yield return null;
        Assert.AreEqual(false, residentialZone.workConnection);
    }

    [UnityTest]
    public IEnumerator TestMainRoadConnectionStartValue()
    {
        yield return null;
        Assert.AreEqual(false, residentialZone.mainRoadConnection);
    }
    [UnityTest]
    public IEnumerator TestAdjustPopulation()
    {
        ResidentialZone.AdjustPopulation(residentialZone);
        yield return null;
        Assert.AreEqual(0, residentialZone.population.Count);
    }

    [UnityTest]
    public IEnumerator TestBuildHouse()
    {
        GameObject houseObject = new GameObject();
        House house = houseObject.AddComponent<House>();
        residentialZone.housePrefab = house;
        residentialZone.BuildHouse();
        yield return null;
        Assert.AreEqual(1, residentialZone.transform.childCount);
    }

    //[UnityTest]
    //public IEnumerator TestDemolishHouse()
    //{
    //    GameObject houseObject = new GameObject();
    //    House house = houseObject.AddComponent<House>();
    //    residentialZone.housePrefab = house;
    //    residentialZone.buildHouse();
    //    residentialZone.demolishHouse();
    //    yield return null;
    //    Assert.AreEqual(0, residentialZone.transform.childCount);

    //    // Test demolishing a house when there are no houses
    //    residentialZone.demolishHouse();
    //    yield return null;
    //    Assert.AreEqual(0, residentialZone.transform.childCount);
    //}

    [UnityTest]
    public IEnumerator TestReplaceDeadCitizens()
    {
        var citizen = new Citizen(100);
        residentialZone.population.Add(citizen);

        Assert.AreEqual(100, citizen.Age);

        ResidentialZone.ReplaceDeadCitizens(residentialZone);
        yield return null;
        Assert.IsFalse(citizen.Age >= 18 && citizen.Age <= 60);
    }

    [UnityTest]
    public IEnumerator TestCalculatePensions()
    {
        var taxes = new Queue<int>();
        taxes.Enqueue(5);
        taxes.Enqueue(6);

        Assert.AreEqual(2, taxes.Count);
        Assert.AreEqual(5, taxes.Peek());

        var citizen = new Citizen(65);
        residentialZone.population.Add(citizen);

        int result = ResidentialZone.CalculatePensions(taxes, residentialZone);
        yield return null;
        Assert.AreEqual(0, result);
    }
}