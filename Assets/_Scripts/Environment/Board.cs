using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using metakazz.Hex;

public class Board : Singleton<Board>
{
    public readonly Dictionary<Vector3Int, Tile> tiles = new Dictionary<Vector3Int, Tile>();

    Stack<Turn> _turnActions = new Stack<Turn>();
    public readonly List<ActionBase> TileEnterActions = new List<ActionBase>();
    public readonly List<ActionBase> TileExitActions = new List<ActionBase>();

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

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            UndoLastTurn();
        }
    }

    public bool IsValid(Vector3Int pos)
    {
        return tiles.ContainsKey(pos);
    }

    public void Tick()
    {
        if (GameSession.Instance.IsPaused)
            return;

        var controllers = GetComponentsInChildren<EntityController>();
        List<ActionBase> pastActions = new List<ActionBase>();

        foreach(EntityController controller in controllers)
        {
            controller.CalculateNextAction();
        }

        foreach(EntityController controller in controllers)
        {
            controller.ValidateNextAction();
        }

        foreach(ActionBase action in TileEnterActions)
        {
            if (action != null)
            {
                action.Execute();
                pastActions.Add(action);
            }
        }

        foreach(EntityController controller in controllers)
        {
            if (controller.NextAction == null)
                continue;

            controller.NextAction.Execute();
            pastActions.Add(controller.NextAction);
        }

        foreach (ActionBase action in TileExitActions)
        {
            if (action != null)
            {
                action.Execute();
                pastActions.Add(action);
            }
        }

        foreach (EntityController controller in controllers)
        {
            controller.PostActionUpdate();
        }

        if(pastActions.Count > 0)
            _turnActions.Push( new Turn(pastActions) );

        ClearSpeculativeMoves();
    }

    public void UndoLastTurn()
    {
        if (!_turnActions.TryPop(out Turn turn))
        {
            Debug.Log("No more turns to undo");
            return;
        }


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
    public ActionBase[] Actions { get; private set; }

    public Turn(ICollection<ActionBase> actions)
    {
        this.Actions = new ActionBase[actions.Count];
        actions.CopyTo(this.Actions, 0);
    }

    public void Undo()
    {
        if (Actions == null)
        {
            Debug.Log("Cannot undo action-less Turn");
            return;
        }

        foreach (ActionBase action in Actions)
        {
            action.Undo();
        }

        Actions = null;
    }
}
