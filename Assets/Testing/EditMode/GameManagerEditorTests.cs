using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GameManagerEditorTests
{
    private GameManager gameManager;

    [SetUp]
    public void SetUp()
    {
        GameObject gameManagerObject = new GameObject();
        gameManager = gameManagerObject.AddComponent<GameManager>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(gameManager.gameObject);
    }

    [Test]
    public void TestGeneralBudgetStartValue()
    {
        Assert.AreEqual(20000, gameManager.generalBudget);
    }

    [Test]
    public void TestGeneralPopulationStartValue()
    {
        Assert.AreEqual(0, gameManager.generalPopulation);
    }

    [Test]
    public void TestGeneralSatisfactionStartValue()
    {
        Assert.AreEqual(0, gameManager.generalSatisfaction);
    }
}