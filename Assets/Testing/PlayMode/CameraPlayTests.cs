//using System.Collections;
//using System.Collections.Generic;
//using NUnit.Framework;
//using UnityEngine;
//using UnityEngine.TestTools;

//public class CameraControllerTests : MonoBehaviour
//{
//    private CameraController _cameraController;
//    private GameObject _cameraGameObject;

//    [SetUp]
//    public void SetUp()
//    {
//        _cameraController.CameraDisabled = false;
//        _cameraGameObject = new GameObject();
//        _cameraController = _cameraGameObject.AddComponent<CameraController>();
//    }

//    [TearDown]
//    public void TearDown()
//    {
//        Object.Destroy(_cameraGameObject);
//    }

//    [UnityTest]
//    public IEnumerator TestMove()
//    {
//        // Set up test conditions
//        Vector3 initialPosition = _cameraController.transform.position;

//        // Call the Move method directly
//        yield return new WaitForSeconds(1);
//        _cameraController.CameraDisabled = false;
//        yield return new WaitForSeconds(1);
//        _cameraController.Move();
//        yield return new WaitForSeconds(1);

//        // Check if the camera moved
//        Assert.AreNotEqual(initialPosition, _cameraController.transform.position);
//    }

//    [UnityTest]
//    public IEnumerator TestZoom()
//    {
//        // Set up test conditions
//        float initialDistance = Vector3.Distance(_cameraController.transform.position, _cameraController.GetTransform());

//        // Call the Zoom method directly
//        yield return new WaitForSeconds(1);
//        _cameraController.Zoom();
//        yield return new WaitForSeconds(1);

//        // Check if the camera zoomed in
//        float finalDistance = Vector3.Distance(_cameraController.transform.position, _cameraController.GetTransform());
//        Assert.Less(finalDistance, initialDistance);
//    }

//    [UnityTest]
//    public IEnumerator TestSetDefault()
//    {
//        // Set up test conditions
//        Vector3 initialPosition = _cameraController.transform.position;
//        Quaternion initialRotation = _cameraController.transform.rotation;

//        // Call the setDefault method directly
//        yield return new WaitForSeconds(1);
//        _cameraController.setDefault();
//        yield return new WaitForSeconds(1);

//        // Check if the camera position and rotation were reset to default values
//        Assert.AreNotEqual(initialPosition, _cameraController.transform.position);
//        Assert.AreNotEqual(initialRotation, _cameraController.transform.rotation);
//    }
//}
