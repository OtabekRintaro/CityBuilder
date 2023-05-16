using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
//using NSubstitute;
public class BuildingPlacerTest
{
    private BuildingPlacer buildingPlacer;

    [SetUp]
    public void Setup()
    {
        GameObject buildingPlacerObject = new GameObject();
        buildingPlacer = buildingPlacerObject.AddComponent<BuildingPlacer>();
        //buildingPlacer.map = Substitute.For<Map>();
        //buildingPlacer.blueprintCellPrefab = Substitute.For<BlueprintCell>();
        buildingPlacer.placementIndicator = new GameObject();
    }

    [Test]
    public void TestCoverage()
    {
        Assert.AreEqual(3, BuildingPlacer.Coverage("ResidentialZone"));
        Assert.AreEqual(3, BuildingPlacer.Coverage("IndustrialZone"));
        Assert.AreEqual(3, BuildingPlacer.Coverage("CommercialZone"));
        Assert.AreEqual(5, BuildingPlacer.Coverage("Stadium"));
        Assert.AreEqual(1, BuildingPlacer.Coverage("Other"));
    }
    //[Test]
    //public void TestBeginNewBuildingPlacement()
    //{
    //    BuildingPreset preset = new BuildingPreset();
    //    preset.displayName = "ResidentialZone";
    //    buildingPlacer.BeginNewBuildingPlacement(preset);
    //    Assert.IsTrue(buildingPlacer.IsCurrentlyPlacing);
    //    Assert.AreEqual(preset, buildingPlacer.IsCurrentlyPlacing);
    //    Assert.IsTrue(buildingPlacer.placementIndicator.activeSelf);
    //    Assert.AreEqual(9, buildingPlacer.blueprintCells.Length);
    //}

    //[Test]
    //public void TestCancelBuildingPlacement()
    //{
    //    BuildingPreset preset = new BuildingPreset();
    //    preset.displayName = "ResidentialZone";
    //    buildingPlacer.BeginNewBuildingPlacement(preset);
    //    buildingPlacer.CancelBuildingPlacement();
    //    Assert.IsFalse(buildingPlacer.IsCurrentlyPlacing);
    //    Assert.IsFalse(buildingPlacer.placementIndicator.activeSelf);
    //}
    [Test]
    public void TestCropClone()
    {
        string name = "Building(Clone)";
        int id = 12345;
        string expected = "Building12345";
        Assert.AreEqual(expected, BuildingPlacer.cropClone(name, id));
    }
}
