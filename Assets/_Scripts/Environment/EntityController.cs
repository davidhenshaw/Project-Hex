using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using metakazz.Hex;

public abstract class EntityController : MonoBehaviour
{
    /// <summary>
    /// arg1 = from  
    /// arg2 = to
    /// </summary>
    public virtual event Action<Vector3Int, Vector3Int> Moved;
    /// <summary>
    /// arg1 = from 
    /// arg2 = to
    /// </summary>
    public virtual event Action<Vector3Int, Vector3Int> MoveBlocked;

    public ActionBase NextAction { get; set; }
    public bool IsNextPositionDirty { get; set; } = true;
    public GridEntity GridEntity { get => _entity; }
    public virtual Vector3Int CurrentPosition => _entity.GridPosition;

    protected GridEntityMovement _entity;
    protected Board _board;

    protected virtual void Awake()
    {
        _entity = GetComponent<GridEntityMovement>();
        _board = GetComponentInParent<Board>();
    }

    public abstract ActionBase CalculateNextAction();
    
    public virtual void HandleInvalidAction(ActionBase action)
    {
        if(action is MoveAction)
        {
            MoveAction moveAction = (MoveAction)action;
            MoveBlocked?.Invoke(moveAction.Start, moveAction.Destination);
            NextAction = null;
        }
    }

    public virtual bool ValidateNextAction()
    {
        if (NextAction == null)
            return true;

        if(NextAction.Validate(_board))
            return true;
            
        HandleInvalidAction(NextAction);
        return false;
    }
    
    public virtual void ExecuteMove( Vector3Int destination )
    {
        var from = CurrentPosition;
        _entity.Move(destination);
        IsNextPositionDirty = true;

        Moved?.Invoke(from, destination);
    }

    public virtual void PostActionUpdate()
    {
        NextAction = null;
        IsNextPositionDirty = true;
    }


}
