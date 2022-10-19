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

    private void Update()
    {
        HandleInputs();
    }

    void HandleInputs()
    {
        // In the original movement code, diagonal movements are confirmed by North and South button presses
        // meaning we don't need to check East and West at all
        if (Input.GetButtonDown("Move_N") || Input.GetButtonDown("Move_S"))
        {
            ResolveMoves();
        }
    }

    public void ResolveMoves()
    {
        var moveControllers = GetComponentsInChildren<MovementController>();

        foreach(MovementController moveController in moveControllers)
        {
            var nextMove = moveController.CalculateNextPosition();

            //if next move is a valid board position
            if(tiles.TryGetValue(nextMove, out Tile nextTile))
            {
                nextTile.speculativeElements
                    .Add(moveController.GetBoardElement());
            }
            else
            {
                moveController.NextMove = moveController.GetCurrentPosition();
            }
        }

        foreach(MovementController mover in moveControllers)
        {
            mover.ValidateNextMove();
        }

        foreach(MovementController mover in moveControllers)
        {
            mover.ExecuteMove();
        }

        foreach(Tile tile in tiles.Values)
        {
            tile.speculativeElements.Clear();
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
            if( b.gameObject.layer == LayerMask.NameToLayer("TileBlocking"))
            {
                return false;
            }
        }

        return true;
    }

    public bool CanMove(Vector3Int startPos, Vector3Int destPos)
    {
        if (isFrozen)
            return false;

        Tile destinationTile;
        tiles.TryGetValue(destPos, out destinationTile);

        // if there is no tile at the next grid position
        if (!destinationTile)
            return false;

        foreach (BoardElement b in destinationTile.elements)
        {
            // if the element is contained within the mask. If so, movement is blocked
            if (b.gameObject.layer == LayerMask.NameToLayer("TileBlocking"))
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
