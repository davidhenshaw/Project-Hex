using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using metakazz.Hex;

public class Board : MonoBehaviour
{
    public bool isFrozen;
    public readonly Dictionary<Vector3Int, Tile> tiles = new Dictionary<Vector3Int, Tile>();

    public Grid grid { get; private set; }

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

        Pipe pipe;
        destination.pipes.TryGetValue(HexUtil.Opposite(dir) , out pipe);

        return (pipe == null);
    }

    public bool CanMove(Vector3Int startPos, HexVertex dir)
    {
        if (isFrozen)
            return false;

        Vector3Int newPos = startPos.Neighbor(dir);
        Tile destination;
        tiles.TryGetValue(newPos, out destination);

        if (!destination)
            return false;

        Pipe pipe;
        destination.vertexPipes.TryGetValue(HexUtil.Opposite(dir), out pipe);

        return (pipe != null);
    }
}
