using NUnit.Framework;
using System.Collections.Generic;

[TestFixture]
public class ResidentialZoneTests
{
    [Test]
    public void TestCalculateSatisfaction()
    {
        // Arrange
        Position position = new Position(0, 0);
        List<Citizen> population = new List<Citizen>();
        int satisfaction = 5;
        bool workConnection = false;
        bool mainRoadConnection = false;
        ResidentialZone resZone = new ResidentialZone(position, population, satisfaction, workConnection, mainRoadConnection);
        Cell[,] cityMatrix = new Cell[1, 1];
        Map map = new Map();
        int tax = 5;
        int budget = 0;

        // Act
        ResidentialZone.CalculateSatisfaction(resZone, cityMatrix, map, tax, budget);

        // Assert
        Assert.AreEqual(4, resZone.satisfaction);
    }

    [Test]
    public void TestAdjustPopulation()
    {
        // Arrange
        Position position = new Position(0, 0);
        List<Citizen> population = new List<Citizen>();
        int satisfaction = 5;
        bool workConnection = false;
        bool mainRoadConnection = false;
        ResidentialZone zone = new ResidentialZone(position, population, satisfaction, workConnection, mainRoadConnection);

        // Act
        ResidentialZone.AdjustPopulation(zone);

        // Assert
        Assert.AreEqual(0, zone.population.Count);
    }

    [Test]
    public void TestReplaceDeadCitizens()
    {
        // Arrange
        Position position = new Position(0, 0);
        List<Citizen> population = new List<Citizen>();
        Citizen citizen1 = new Citizen(65);
        Citizen citizen2 = new Citizen(65);
        population.Add(citizen1);
        population.Add(citizen2);
        int satisfaction = 5;
        bool workConnection = false;
        bool mainRoadConnection = false;
        ResidentialZone resZone = new ResidentialZone(position, population, satisfaction, workConnection, mainRoadConnection);

        // Act
        citizen1.Age = 100;
        citizen2.Age = 100;
        ResidentialZone.ReplaceDeadCitizens(resZone);

        // Assert
        Assert.IsFalse(resZone.population[0].Age >= 18 && resZone.population[0].Age <= 61);
        Assert.IsFalse(resZone.population[1].Age >= 18 && resZone.population[1].Age <= 61);
    }

    [Test]
    public void TestCalculatePensions()
    {
        // Arrange
        Queue<int> taxes = new Queue<int>();
        taxes.Enqueue(5);
        taxes.Enqueue(6);
        Position position = new Position(0, 0);
        List<Citizen> population = new List<Citizen>();
        Citizen citizen1 = new Citizen(65);
        Citizen citizen2 = new Citizen(65);
        population.Add(citizen1);
        population.Add(citizen2);
        int satisfaction = 5;
        bool workConnection = false;
        bool mainRoadConnection = false;
        ResidentialZone zone = new ResidentialZone(position, population, satisfaction, workConnection, mainRoadConnection);

        // Act
        int totalPension = ResidentialZone.CalculatePensions(taxes, zone);

        // Assert
        Assert.AreEqual(0, totalPension);
    }
}