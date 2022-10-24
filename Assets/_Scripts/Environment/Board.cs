using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using metakazz.Hex;

public class Board : Singleton<Board>
{
    public readonly Dictionary<Vector3Int, Tile> tiles = new Dictionary<Vector3Int, Tile>();

    public Grid grid { get; private set; }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        var boardElements = GetComponentsInChildren<Tile>();
        List<Tile> rejected = new List<Tile>();

        grid = GetComponent<Grid>();

        foreach (Tile t in boardElements)
        {
            t.InitPosition();
            
            if(!tiles.TryAdd(t.GridPosition, t))
            {//If the position is already taken, destroy the tile
                rejected.Add(t);
            }
        }

        foreach(Tile t in rejected)
        {
            Destroy(t);
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
        if (GameSession.Instance.IsPaused)
            return;

        var moveControllers = GetComponentsInChildren<MovementController>();

        foreach(MovementController moveController in moveControllers)
        {
            moveController.CalculateNextPosition();
        }

        foreach(MovementController mover in moveControllers)
        {
            mover.ValidateNextMove();
        }

        foreach(MovementController mover in moveControllers)
        {
            mover.ExecuteMove();
        }

        foreach (MovementController mover in moveControllers)
        {
            mover.PostMoveUpdate();
        }
    }

    public BoardElement[] GetObjectsAtPosition(Vector3Int gridPos)
    {
        if (!tiles.TryGetValue(gridPos, out Tile tile))
            return null;

        return tile.elements.ToArray();
    }
}
