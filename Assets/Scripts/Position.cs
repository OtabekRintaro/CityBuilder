using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Position
{
    public int x, z;
    public int X { get; set; }
    public int Z { get; set; }

    /// <summary>
    /// Constructor of the Position class
    /// </summary>
    /// <param name="x">x</param>
    /// <param name="z">z</param>
    public Position(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    /// <summary>
    /// Calculates the area of tiles between bottom left and top right position and returns an array of positions.
    /// </summary>
    /// <param name="bottomLeft"></param>
    /// <param name="topRight"></param>
    /// <returns>array of positions</returns>
    public Position[] calculateArea(Position bottomLeft, Position topRight)
    {
        int height = topRight.z - bottomLeft.z;
        int width = topRight.x - bottomLeft.x;
        Position[] AreaOfTiles = new Position[(height + 1) * (width + 1)];

        for (int z = 0, i = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++)
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

    /// <summary>
    /// Checks if given object is equal to this object.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj) { 
        if (obj == null)
            return false;
        if (obj is not Position)
            return false;
        Position o = (Position) obj;
        return o.x == x && o.z == z; 
    }
    
    /// <summary>
    /// Returns the hashcode of this object.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() { 
        return HashCode.Combine(x, z); 
    }
}
