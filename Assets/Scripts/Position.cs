using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position
{
    int x, z;
    public int X { get; set; }
    public int Z { get; set; }

    public Position(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public Position[] calculateArea(Position bottomLeft, Position topRight)
    {
        int height = topRight.Z - bottomLeft.Z;
        int width = topRight.X - bottomLeft.X;
        Position[] AreaOfTiles = new Position[height * width];

        for(int z = 0, i = 0; z <= height; z++)
        {
            for(int x = 0; x <= width; x++)
            {
                Position p = new Position(x + bottomLeft.X, z + bottomLeft.Z);
                AreaOfTiles[i++] = p;
            }
        }

        return AreaOfTiles;
    }

    public string toString()
    {
        return "x:"+ x + " z:" + z;
    }
}
