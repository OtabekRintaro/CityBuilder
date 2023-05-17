using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ResidentialZoneTests
{
    private ResidentialZone residentialZone;

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
    public IEnumerator TestPopulationStartValue()
    {
        yield return null;
        Assert.AreEqual(0, residentialZone.population);
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
}