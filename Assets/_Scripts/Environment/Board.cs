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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetButtonDown("Move_N")   ||
            Input.GetButtonDown("Move_NE")  ||
            Input.GetButtonDown("Move_SE")  ||
            Input.GetButtonDown("Move_S")   ||
            Input.GetButtonDown("Move_SW")  ||
            Input.GetButtonDown("Move_NW"))
        {
            Tick();
        }
    }

    public void Tick()
    {
        if (GameSession.Instance.IsPaused)
            return;

        var controllers = GetComponentsInChildren<EntityController>();
        List<ActionBase> actions = new List<ActionBase>();

        foreach(EntityController controller in controllers)
        {
            //if (controller is PlayerController)
            //    continue;

            controller.CalculateNextAction();
        }

        foreach(EntityController controller in controllers)
        {
            controller.ValidateNextAction();
        }

        foreach(EntityController controller in controllers)
        {
            if (controller.NextAction == null)
                continue;

            controller.NextAction.Execute();
            actions.Add(controller.NextAction);
        }

        foreach (EntityController controller in controllers)
        {
            controller.PostActionUpdate();
        }

        _turnActions.Push( new Turn(actions) );

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

    public Turn(ICollection<ActionBase> actions)
    {
        this.Actions = new ActionBase[actions.Count];
        actions.CopyTo(this.Actions, 0);
    }
}
