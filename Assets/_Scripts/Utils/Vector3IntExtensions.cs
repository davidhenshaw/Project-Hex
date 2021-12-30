using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3IntExtensions
{
    public static Dictionary<HexDirection, Vector3Int> evenDict = new Dictionary<HexDirection, Vector3Int>()
    {
        { HexDirection.NORTH, new Vector3Int(1, 0, 0)},
        { HexDirection.NORTHEAST, new Vector3Int(0, 1, 0) },
        { HexDirection.SOUTHEAST, new Vector3Int(-1, 1, 0)},
        { HexDirection.SOUTH, new Vector3Int(-1, 0, 0)},
        { HexDirection.SOUTHWEST, new Vector3Int(-1, -1, 0)},
        { HexDirection.NORTHWEST, new Vector3Int(0, -1, 0)},
    };

    public static Dictionary<HexDirection, Vector3Int> oddDict = new Dictionary<HexDirection, Vector3Int>()
    {
        { HexDirection.NORTH, new Vector3Int(1, 0, 0)},
        { HexDirection.NORTHEAST, new Vector3Int(1, 1, 0) },
        { HexDirection.SOUTHEAST, new Vector3Int(0, 1, 0)},
        { HexDirection.SOUTH, new Vector3Int(-1, 0, 0)},
        { HexDirection.SOUTHWEST, new Vector3Int(0, -1, 0)},
        { HexDirection.NORTHWEST, new Vector3Int(1, -1, 0)},
    };

    public static bool isEven(this Vector3Int vector)
    {
        return (vector.y % 2 == 0);
    }

    public static Vector3Int GetMovement(this Vector3Int vec, HexDirection dir)
    {
        return (vec.isEven()) ? evenDict[dir] : oddDict[dir];
    }

    public static Vector3Int Neighbor(this Vector3Int vec, HexDirection dir)
    {
        var movement = (vec.isEven()) ? evenDict[dir] : oddDict[dir];
        return vec + movement;
    }
}
