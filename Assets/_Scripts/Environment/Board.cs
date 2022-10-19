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
        var tile = tiles[gridPos];

        return tile.elements.ToArray();
    }
}
