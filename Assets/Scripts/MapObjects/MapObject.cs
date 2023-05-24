using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    protected bool isConnectedToPublicRoad = false;
    protected bool isFireDepartmentNearby = false;

    public bool IsFireDepartmentNearby { get { return isFireDepartmentNearby; } }
    public int ID { get; set; }

    public int publicRoads = 0;
    public Position position;
    public int coverage;
    public int cost;

    public FirePrefab firePrefab;
    public FirePrefab fire;
    

    /// <summary>
    /// Connects map object to fire department
    /// </summary>
    public void ConnectToFireDepartment(bool connect)
    {
        this.isFireDepartmentNearby = connect;
    }

    /// <summary>
    /// Checks the given radius for certain object
    /// </summary>
    public static int CheckRadius(Cell[,] mapGrid, Map map, Position position, int radius, MapObject obj)
    {
        int count = 0;
        int row = position.x;
        int col = position.z;

        MapObject temp;
        System.Type type = obj.GetType();
        for (int i = row - radius; i <= row + radius; i++)
        {
            if (i < 0 || i >= mapGrid.GetLength(0)) continue;
            for (int j = col - radius; j <= col + radius; j++)
            {
                if (j < 0 || j >= mapGrid.GetLength(1)) continue;
                if ((temp = map.findMapObject(new Position(i, j))) is not null && temp.GetType() == type)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public void SetFire(DateHandler dateHandler)
    {
        fire = Instantiate<FirePrefab>(firePrefab);
        Vector3 position = new Vector3();
        position.x = 0;
        position.y = 1f;
        position.z = 0;
        fire.transform.localPosition = position;
        fire.startOfFire = dateHandler.currentDate;
        fire.transform.SetParent(this.transform, false);
    }
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

    public static void DestroyBuildings(MapObject mapObject)
    {
        if (mapObject is ResidentialZone resZone)
        {
            resZone.DemolishHouse();
        }
        else if (mapObject is IndustrialZone indZone)
        {
            indZone.DemolishFactory();
        }
        else if (mapObject is CommercialZone comZone)
        {
            comZone.DemolishBuildings();
        }
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
