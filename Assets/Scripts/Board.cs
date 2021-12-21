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

    public bool CanMove(Tile source, HexDirection dir)
    {
        Tile destination;
        var destPos = source.GridPosition.Neighbor(dir);

        tiles.TryGetValue(destPos, out destination);

        if (!destination)
            return false;

        var destWalls = destination.GetWalls();

        //If any one wall is anchored to the source tile and the destination tile,
        // it will block the desired movement.
        foreach(BoardElement b in destWalls)
        {
            Wall w = (Wall)b;

            if (w.anchors.Contains(source) && w.anchors.Contains(destination))
                return false;
        }

        return true;
    }

    public bool CanMove(Vector3Int sourcePos, HexDirection dir)
    {
        Tile source;
        tiles.TryGetValue(sourcePos, out source);
        //TODO: Might need to remove this later since it could soft-lock the game
        if (!source)
            return false;

        Tile destination;
        var destPos = source.GridPosition.Neighbor(dir);

        tiles.TryGetValue(destPos, out destination);

        if (!destination)
            return false;

        var destWalls = destination.GetWalls();

        //If any one wall is anchored to the source tile and the destination tile,
        // it will block the desired movement.
        foreach (BoardElement b in destWalls)
        {
            Wall w = (Wall)b;

            if (w.anchors.Contains(source) && w.anchors.Contains(destination))
                return false;
        }

        return true;
    }

}
