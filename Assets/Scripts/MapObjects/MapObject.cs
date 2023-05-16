using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    protected bool isConnectedToPublicRoad = false;
    public int ID { get; set; }

    public int publicRoads = 0;
    public Position position;
    public int coverage;
    public int cost;

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
        if (type.Equals("Stadium"))
            return gameObject.AddComponent<Stadium>();
        if (type.Equals("Police"))
            return gameObject.AddComponent<Police>();
        if (type.Equals("Forest"))
            return gameObject.GetComponent<Forest>();
        if (type.Equals("FireDepartment"))
            return gameObject.AddComponent<FireDepartment>();
        return null;
    }

    public static int getCost(string type)
    {
        if (type.Equals("ResidentialZone"))
            return 200;
        if (type.Equals("IndustrialZone"))
            return 200;
        if (type.Equals("CommercialZone"))
            return 200;
        if (type.Equals("Road"))
            return 50;
        if (type.Equals("Stadium"))
            return 400;
        if (type.Equals("Police"))
            return 50;
        if (type.Equals("Forest"))
            return 50;
        return 0;
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
