using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using metakazz.Hex;

public class Board : Singleton<Board>
{
    public readonly Dictionary<Vector3Int, Tile> tiles = new Dictionary<Vector3Int, Tile>();

    Stack<Turn> _turnActions = new Stack<Turn>();

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

    public void Tick()
    {
        if (GameSession.Instance.IsPaused)
            return;

        var moveControllers = GetComponentsInChildren<MovementController>();

        foreach(MovementController moveController in moveControllers)
        {
            if (moveController is PlayerController)
                continue;

            var nextPos = moveController.CalculateNextPosition();
            RegisterSpeculativeMove(nextPos, moveController.GridEntity);
        }

        foreach(MovementController mover in moveControllers)
        {
            if(!mover.ResolveNextMove())
            {
                mover.HandleInvalidMove();
            }
        }

        foreach(MovementController mover in moveControllers)
        {
            mover.ExecuteMove();
        }

        foreach (MovementController mover in moveControllers)
        {
            mover.PostMoveUpdate();
        }

        ClearSpeculativeMoves();
    }

    public void UndoLastTurn()
    {
        if (!_turnActions.TryPop(out Turn turn))
            return;


        foreach(ActionBase action in turn.Actions)
        {
            action.Undo();
        }
    }

    void ClearSpeculativeMoves()
    {
        foreach(Tile tile in tiles.Values)
        {
            tile.speculativeElements.Clear();
        }
    }

    void RegisterSpeculativeMove(Vector3Int moveTo, GridEntity entity)
    {
        if(tiles.TryGetValue(moveTo, out Tile tile))
        {
            tile.speculativeElements.Add(entity);
        }

    }

    public GridEntity[] GetObjectsAtPosition(Vector3Int gridPos)
    {
        if (!tiles.TryGetValue(gridPos, out Tile tile))
            return null;

        return tile.entities.ToArray();
    }
}

struct Turn
{
    public readonly ActionBase[] Actions;

    //public Turn(ActionBase[] actions)
    //{
    //    this.actions = actions;
    //}

    public Turn(ICollection<ActionBase> actions)
    {
        this.Actions = null;
        actions.CopyTo(this.Actions, 0);
    }
}
