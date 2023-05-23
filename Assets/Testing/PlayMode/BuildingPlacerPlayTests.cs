////using System.Collections;
////using NUnit.Framework;
////using UnityEngine;
////using UnityEngine.TestTools;

////public class BuildingPlacerTests
////{
////    private BuildingPlacer buildingPlacer;
////    private BuildingPreset buildingPreset;

////    [SetUp]
////    public void SetUp()
////    {
////        buildingPlacer = new GameObject().AddComponent<BuildingPlacer>();
////        buildingPreset = ScriptableObject.CreateInstance<BuildingPreset>();
////    }
////    //[UnityTest]
////    //public IEnumerator SelectObject_Sets_Selection_To_Null_When_Right_Mouse_Button_Is_Clicked()
////    //{
////    //    buildingPlacer.getSelection() = new GameObject().transform;
////    //    buildingPlacer.selectObject();
////    //    Assert.IsNull(buildingPlacer.getSelection());
////    //    yield return null;
////    //}
////    //[UnityTest]
////    //public IEnumerator BeginNewBuildingPlacement_Sets_CurrentlyPlacing_To_True()
////    //{
////    //    buildingPlacer.BeginNewBuildingPlacement(buildingPreset);
////    //    Assert.IsTrue(buildingPlacer.IsCurrentlyPlacing);
////    //    yield return null;
////    //}

////    //[UnityTest]
////    //public IEnumerator BeginNewBuildingPlacement_Sets_CurBuildingPreset_To_Given_BuildingPreset()
////    //{
////    //    buildingPlacer.BeginNewBuildingPlacement(buildingPreset);
////    //    Assert.AreEqual(buildingPreset, buildingPlacer.getCurrBuildingPreset());
////    //    yield return null;
////    //}

////    //[UnityTest]
////    //public IEnumerator CancelBuildingPlacement_Sets_CurrentlyPlacing_To_False()
////    //{
////    //    buildingPlacer.CancelBuildingPlacement();
////    //    Assert.IsFalse(buildingPlacer.IsCurrentlyPlacing);
////    //    yield return null;
////    //}
////}

//using NUnit.Framework;
//using UnityEngine;
//using UnityEngine.TestTools;
//using System.Collections;

//public class BuildingPlacerTests
//{
//    private BuildingPlacer buildingPlacer;

//    [SetUp]
//    public void SetUp()
//    {
//        LogAssert.ignoreFailingMessages = true;
//        GameObject buildingPlacerObject = new GameObject();
//        buildingPlacer = buildingPlacerObject.AddComponent<BuildingPlacer>();
//    }

//    [TearDown]
//    public void TearDown()
//    {
//        Object.Destroy(buildingPlacer.gameObject);
//        LogAssert.ignoreFailingMessages = false;
//    }

//    [UnityTest]
//    public IEnumerator TestCoverage()
//    {
//        int result = BuildingPlacer.Coverage("ResidentialZone");
//        yield return null;
//        Assert.AreEqual(3, result);

//        result = BuildingPlacer.Coverage("Stadium");
//        yield return null;
//        Assert.AreEqual(5, result);

//        result = BuildingPlacer.Coverage("Police");
//        yield return null;
//        Assert.AreEqual(1, result);
//    }

//    [UnityTest]
//    public IEnumerator TestCropClone()
//    {
//        string result = BuildingPlacer.cropClone("ResidentialZone(Clone)", 12345);
//        yield return null;
//        Assert.AreEqual("ResidentialZone12345", result);
//    }
//}
