using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using metakazz.Hex;

public class Board : Singleton<Board>
{
    public bool isFrozen;
    public readonly Dictionary<Vector3Int, Tile> tiles = new Dictionary<Vector3Int, Tile>();

    public Grid grid { get; private set; }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        var boardElements = GetComponentsInChildren<Tile>();
        grid = GetComponent<Grid>();

        foreach (Tile t in boardElements)
        {
            t.InitPosition();
            tiles.Add(t.GridPosition, t);
        }
    }

    public bool CanMove(Vector3Int startPos, HexDirection dir)
    {
        if (isFrozen)
            return false;

        Vector3Int newPos = startPos.Neighbor(dir);
        Tile destination;
        tiles.TryGetValue(newPos, out destination);

        // if there is no tile at the next grid position
        if (!destination)
            return false;

        foreach(BoardElement b in destination.elements)
        {
            // if the element is contained within the mask. If so, movement is blocked
            if( b.gameObject.layer == LayerMask.NameToLayer("Air"))
            {
                return false;
            }
        }

        return true;
    }

    public BoardElement[] GetObjectsAtPosition(Vector3Int gridPos)
    {
        var tile = tiles[gridPos];

        return tile.elements.ToArray();
    }
}
