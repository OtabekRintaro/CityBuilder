using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BuildingPlacerTests
{
    private BuildingPlacer buildingPlacer;
    private BuildingPreset buildingPreset;

    [SetUp]
    public void SetUp()
    {
        buildingPlacer = new GameObject().AddComponent<BuildingPlacer>();
        buildingPreset = ScriptableObject.CreateInstance<BuildingPreset>();
    }
    //[UnityTest]
    //public IEnumerator SelectObject_Sets_Selection_To_Null_When_Right_Mouse_Button_Is_Clicked()
    //{
    //    buildingPlacer.getSelection() = new GameObject().transform;
    //    buildingPlacer.selectObject();
    //    Assert.IsNull(buildingPlacer.getSelection());
    //    yield return null;
    //}
    //[UnityTest]
    //public IEnumerator BeginNewBuildingPlacement_Sets_CurrentlyPlacing_To_True()
    //{
    //    buildingPlacer.BeginNewBuildingPlacement(buildingPreset);
    //    Assert.IsTrue(buildingPlacer.IsCurrentlyPlacing);
    //    yield return null;
    //}

    //[UnityTest]
    //public IEnumerator BeginNewBuildingPlacement_Sets_CurBuildingPreset_To_Given_BuildingPreset()
    //{
    //    buildingPlacer.BeginNewBuildingPlacement(buildingPreset);
    //    Assert.AreEqual(buildingPreset, buildingPlacer.getCurrBuildingPreset());
    //    yield return null;
    //}

    //[UnityTest]
    //public IEnumerator CancelBuildingPlacement_Sets_CurrentlyPlacing_To_False()
    //{
    //    buildingPlacer.CancelBuildingPlacement();
    //    Assert.IsFalse(buildingPlacer.IsCurrentlyPlacing);
    //    yield return null;
    //}
}