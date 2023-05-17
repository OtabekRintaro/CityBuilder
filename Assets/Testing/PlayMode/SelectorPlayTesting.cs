//using System.Collections;
//using System.Collections.Generic;
//using NUnit.Framework;
//using UnityEngine;
//using UnityEngine.TestTools;

//public class SelectorPlayTesting
//{
//    private Selector selector;

//    [SetUp]
//    public void SetUp()
//    {
//        GameObject selectorObject = new GameObject();
//        selector = selectorObject.AddComponent<Selector>();
//    }

//    [TearDown]
//    public void TearDown()
//    {
//        Object.Destroy(selector.gameObject);
//    }

//    [UnityTest]
//    public IEnumerator TestGetCurTilePosition()
//    {
//        yield return null;
//        Assert.AreEqual(new Vector3(0, -99, 0), selector.GetCurTilePosition());
//    }
//}