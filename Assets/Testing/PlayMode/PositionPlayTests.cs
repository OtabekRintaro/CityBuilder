using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PositionTests : MonoBehaviour
{
    [Test]
    public void TestCalculateArea()
    {
        // Set up test conditions
        Position bottomLeft = new Position(0, 0);
        Position topRight = new Position(1, 1);

        // Call the calculateArea method
        Position[] area = bottomLeft.calculateArea(bottomLeft, topRight);

        // Check if the returned area is correct
        Assert.AreEqual(4, area.Length);
        Assert.AreEqual(new Position(0, 0), area[0]);
        Assert.AreEqual(new Position(1, 0), area[1]);
        Assert.AreEqual(new Position(0, 1), area[2]);
        Assert.AreEqual(new Position(1, 1), area[3]);
    }

    [Test]
    public void TestToString()
    {
        // Set up test conditions
        Position position = new Position(1, 2);

        // Call the toString method
        string result = position.toString();

        // Check if the returned string is correct
        Assert.AreEqual("x:1 z:2", result);
    }

    [Test]
    public void TestEquals()
    {
        // Set up test conditions
        Position position1 = new Position(1, 2);
        Position position2 = new Position(1, 2);
        Position position3 = new Position(2, 1);

        // Check if the Equals method returns the correct result
        Assert.IsTrue(position1.Equals(position2));
        Assert.IsFalse(position1.Equals(position3));
    }

    [Test]
    public void TestGetHashCode()
    {
        // Set up test conditions
        Position position1 = new Position(1, 2);
        Position position2 = new Position(1, 2);
        Position position3 = new Position(2, 1);

        // Check if the GetHashCode method returns the correct result
        Assert.AreEqual(position1.GetHashCode(), position2.GetHashCode());
        Assert.AreNotEqual(position1.GetHashCode(), position3.GetHashCode());
    }
    [Test]
    public void TestCalculateAreaWithNegativeCoordinates()
    {
        // Set up test conditions
        Position bottomLeft = new Position(-1, -1);
        Position topRight = new Position(1, 1);

        // Call the calculateArea method
        Position[] area = bottomLeft.calculateArea(bottomLeft, topRight);

        // Check if the returned area is correct
        Assert.AreEqual(9, area.Length);
    }

    [Test]
    public void TestCalculateAreaWithZeroWidth()
    {
        // Set up test conditions
        Position bottomLeft = new Position(0, 0);
        Position topRight = new Position(0, 1);

        // Call the calculateArea method
        Position[] area = bottomLeft.calculateArea(bottomLeft, topRight);

        // Check if the returned area is correct
        Assert.AreEqual(2, area.Length);
        Assert.AreEqual(new Position(0, 0), area[0]);
        Assert.AreEqual(new Position(0, 1), area[1]);
    }

    [Test]
    public void TestCalculateAreaWithZeroHeight()
    {
        // Set up test conditions
        Position bottomLeft = new Position(0, 0);
        Position topRight = new Position(1, 0);

        // Call the calculateArea method
        Position[] area = bottomLeft.calculateArea(bottomLeft, topRight);

        // Check if the returned area is correct
        Assert.AreEqual(2, area.Length);
        Assert.AreEqual(new Position(0, 0), area[0]);
        Assert.AreEqual(new Position(1, 0), area[1]);
    }
}
