using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireDepartment : MapObject
{
    public FireTruck fireTruck;
    public bool isFireTruckSent = false;
    List<Position> path = new();
    DateTime timeOfLeave = new();
    DateHandler dateHandler;
    int index = 0;
    Position firePosition;
    Map map;
    public static void SendFireTruck(DateHandler dateHandler, Map map, Cell[,] cells, MapObject mapObject)
    {
        List<Position> path = new(); //RoadHandler.inst.bfs(cells, mapObject);
        int half = mapObject.coverage / 2;
        for (int row = mapObject.position.x - half; row <= mapObject.position.x + half; row++)
        {
            for(int col = mapObject.position.z - half; col <= mapObject.position.z + half; col++)
            {
                List<Position> tempPath = RoadHandler.inst.bfs(map, cells, new Position(row, col));
                if (tempPath.Count != 0 && tempPath.Count < path.Count || path.Count == 0)
                {
                    path = tempPath;
                }
            }
        }

        if (path.Count == 0)
        {
            mapObject.IsFireInformed = false;
            return;
        }

        FireDepartment fireDep = (FireDepartment)map.findMapObject(cells[path[path.Count - 1].x, path[path.Count - 1].z].ID);
        fireDep.isFireTruckSent = true;
        path.Reverse();
        fireDep.firePosition = path[path.Count - 1];
        path.RemoveAt(path.Count - 1);
        path.RemoveAt(0);
        fireDep.path = path;
        fireDep.dateHandler = dateHandler;
        fireDep.timeOfLeave = dateHandler.currentDate;
        fireDep.map = map;

        //FireTruck fireTruck = ((FireDepartment)map.findMapObject(cells[path[path.Count-1].x, path[path.Count - 1].z].ID)).fireTruck;
        //path.Reverse();
        //path.RemoveAt(path.Count -1);
        //System.DateTime time = dateHandler.currentDate;
        //Debug.Log(time);
        //StartCoroutine(FireDepartment.GoTruck(fireTruck,));
        //foreach (Position pos in path)
        //{
        //    int i = 0;
        //    while(i < 100000000)
        //    {
        //        i++;
        //    }
        //    Debug.Log($"{time.Day} {dateHandler.currentDate.Day}" );
        //    Vector3 position = new Vector3();
        //    position.x = -100 + pos.z * 10;
        //    position.y = 0.3f;
        //    position.z = -100 + pos.x * 10;
        //    fireTruck.transform.position = position;
        //}
    }


    void Awake()
    {
        fireTruck = this.GetComponentInChildren<FireTruck>();    
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isFireTruckSent && index < path.Count)
        {
            if(Mathf.Abs(timeOfLeave.Subtract(dateHandler.currentDate).Days) >= 1)
            {
                timeOfLeave = dateHandler.currentDate;
                Vector3 position = new Vector3();
                position.x = -100 + path[index].z * 10;
                position.y = 0.3f;
                position.z = -100 + path[index].x * 10;
                fireTruck.transform.position = position;
                index++;
            }
        }
        else if(isFireTruckSent && index == path.Count)
        {
            MapObject mapObject = map.findMapObject(map.cells[firePosition.x, firePosition.z].ID);
            Destroy(mapObject.fire.transform.gameObject);
            mapObject.IsFireInformed = false;
            mapObject.fire = null;
            Vector3 position = new Vector3();
            position.x = 6f;
            position.y = 0.3f;
            position.z = 3f;
            index = 0;
            fireTruck.transform.localPosition = position;
            isFireTruckSent = false;
        }
    }
}
