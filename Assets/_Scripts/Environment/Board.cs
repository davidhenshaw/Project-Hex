using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Grid grid;
    public readonly Dictionary<Vector3Int, Tile> tiles = new Dictionary<Vector3Int, Tile>();

    public Grid Grid {
        get {
            if (!grid)
                grid = GetComponent<Grid>();

            return grid;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        var boardElements = GetComponentsInChildren<Tile>();
        grid = GetComponent<Grid>();

        foreach (Tile t in boardElements)
        {
            tiles.Add(t.GridPosition, t);
        }
    }

    public bool CanMove(Vector3Int startPos, HexDirection dir)
    {
        Vector3Int newPos = startPos.Neighbor(dir);
        Tile destination;
        tiles.TryGetValue(newPos, out destination);

        if (!destination)
            return false;

        Wall wall;
        destination.walls.TryGetValue(Wall.Opposite(dir) , out wall);

        return (wall == null);
    }
}
