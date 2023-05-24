using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections;

public class CitizenTests
{
    private Citizen citizen;

    [SetUp]
    public void SetUp()
    {
        citizen = new Citizen(30);
    }

    [Test]
    public void TestAgeOneYear()
    {
        citizen.AgeOneYear();
        Assert.AreEqual(31, citizen.Age);
    }

    [Test]
    public void TestIsRetired()
    {
        for (int i = 0; i < 35; i++)
        {
            citizen.AgeOneYear();
        }
        Assert.AreEqual(true, citizen.IsRetired);
    }

    [Test]
    public void TestIsDead()
    {
        for (int i = 0; i < 100; i++)
        {
            citizen.AgeOneYear();
        }
        Assert.AreEqual(true, citizen.IsDead);
    }

    [Test]
    public void TestAgeAfterDeath()
    {
        for (int i = 0; i < 100; i++)
        {
            citizen.AgeOneYear();
        }
        int ageBefore = citizen.Age;
        citizen.AgeOneYear();
        int ageAfter = citizen.Age;
        Assert.AreEqual(ageBefore, ageAfter);
    }

    [Test]
    public void TestRetirementAfterDeath()
    {
        for (int i = 0; i < 100; i++)
        {
            citizen.AgeOneYear();
        }
        bool isRetiredBefore = citizen.IsRetired;
        citizen.AgeOneYear();
        bool isRetiredAfter = citizen.IsRetired;
        Assert.AreEqual(isRetiredBefore, isRetiredAfter);
    }
}
