using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Position
{
    public int x, z;
    public int X { get; set; }
    public int Z { get; set; }

    public Position(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public Position[] calculateArea(Position bottomLeft, Position topRight)
    {
        int height = topRight.z - bottomLeft.z;
        int width = topRight.x - bottomLeft.x;
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

    public override bool Equals(object obj) { 
        if (obj == null)
            return false;
        if (obj is not Position)
            return false;
        Position o = (Position) obj;
        return o.x == x && o.z == z; 
    }
    public override int GetHashCode() { 
        return HashCode.Combine(x, z); 
    }
}
