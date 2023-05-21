using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    private Camera cam;
    public static Selector inst;

    /// <summary>
    /// The function is called when it is first awaken and assigns the instance to this object.
    /// </summary>
    void Awake()
    {
        inst = this;
    }

    /// <summary>
    /// The function returns the current tile position of the mouse.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCurTilePosition()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return new Vector3(0, 0, 0);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(cam.ScreenPointToRay(Input.mousePosition), out float rayOut))
        {
            Vector3 newPos = ray.GetPoint(rayOut);
            return new Vector3((float)(Math.Round(newPos.x / 10.0) * 10), 0.3f, (float)Math.Round(newPos.z / 10.0) * 10);
        }
        return new Vector3(0, -99, 0);
    }

    /// <summary>
    /// Configures the camera and sets the instance using singleton design pattern.
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        if (inst is null)
            inst = this;
        cam = Camera.main;
    }

    /// <summary>
    /// Checks if instance is null and sets it to this object once per frame.
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        if (inst is null)
            inst = this;
    }
}
