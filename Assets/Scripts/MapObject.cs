using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    protected bool isConnectedToPublicRoad = false;
    Position coordinate;

    public Position Coordinate { get; set; }
    public int coverage;

    public static MapObject getMapObject(string type, GameObject gameObject)
    {
        if (type.Equals("ResidentialZone"))
            return gameObject.AddComponent<ResidentialZone>();
        if (type.Equals("IndustrialZone"))
            return gameObject.AddComponent<IndustrialZone>();
        if (type.Equals("CommercialZone"))
            return gameObject.AddComponent<CommercialZone>();
        if (type.Equals("Road"))
            return gameObject.AddComponent<Road>();
        return null;
    }

    public bool checkPublicRoadConnection()
    {
        return isConnectedToPublicRoad;
    }

    public void connectToPublicRoad(bool value)
    {
        isConnectedToPublicRoad = value;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
}
