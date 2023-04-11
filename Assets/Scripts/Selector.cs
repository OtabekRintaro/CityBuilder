using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    private Camera cam;
    public static Selector inst;
    void Awake()
    {
        inst = this;
    }

    public Vector3 GetCurTilePosition()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return new Vector3(0, 0, 0);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        //Debug.Log(Input.mousePosition);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        float rayOut = 0.0f;
        if (plane.Raycast(cam.ScreenPointToRay(Input.mousePosition), out rayOut))
        {
           // Debug.Log(ray.GetPoint(rayOut));
            Vector3 newPos = ray.GetPoint(rayOut) - new Vector3(0.5f, 0f, 0.5f);
            return new Vector3(Mathf.CeilToInt(newPos.x) - Mathf.CeilToInt(newPos.x) % 10, 0.3f, Mathf.CeilToInt(newPos.z) - Mathf.CeilToInt(newPos.z) % 10);
        }
        return new Vector3(0, -99, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
