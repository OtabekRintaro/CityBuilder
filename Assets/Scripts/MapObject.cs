using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    protected bool isConnectedToPublicRoad = false;
    public int ID { get; set; }

    public Position position;
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
        if (type.Equals("Stadium"))
            return gameObject.AddComponent<Stadium>();
        if (type.Equals("Police"))
            return gameObject.AddComponent<Police>();
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
