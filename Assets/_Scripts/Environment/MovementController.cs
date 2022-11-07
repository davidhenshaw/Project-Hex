using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using metakazz.Hex;

public abstract class MovementController : MonoBehaviour
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

    public Vector3Int NextMove { get; set; }
    public bool IsNextPositionDirty { get; set; } = true;
    public MovementController MovementDependency { get; set; }
    public GridEntity GridEntity { get => _mover; }

    protected GridEntityMovement _mover;
    protected Board board;

    [SerializeField]
    GameObject _sprite;

    protected virtual void Awake()
    {
        _mover = GetComponent<GridEntityMovement>();
        board = GetComponentInParent<Board>();
    }

    public abstract Vector3Int CalculateNextPosition();
    
    public virtual void ExecuteMove()
    {
        var from = GetCurrentPosition();
        _mover.Move(NextMove);
        IsNextPositionDirty = true;

        Moved?.Invoke(from, NextMove);
    }

    public virtual void HandleInvalidMove()
    {
        NextMove = GridEntity.GridPosition;
        IsNextPositionDirty = false;
    }

    public virtual bool ResolveNextMove()
    {
        Tile destinationTile;

        // if there is no tile at the next grid position, you can't move there
        if (!board.tiles.TryGetValue(NextMove, out destinationTile))
        {
            IsNextPositionDirty = false;
            return false;
        }

        if (CanOverlapImmediate(destinationTile))
        {
            IsNextPositionDirty = false;
            return true;
        }


        MoveBlocked?.Invoke(GetCurrentPosition(), NextMove);
        _mover.OnMoveBlocked(NextMove);

        NextMove = GetCurrentPosition();
        IsNextPositionDirty = false;

        return false;
    }

    /// <summary>
    /// Can this entity overlap what is currently at the destinationTile 
    /// </summary>
    /// <param name="destinationTile"></param>
    /// <returns></returns>
    public virtual bool CanOverlapImmediate(Tile destinationTile)
    {
        IsNextPositionDirty = false;
        foreach (GridEntity otherEntity in destinationTile.entities)
        {
            //don't check if the object is yourself
            if (otherEntity.Equals(this.GridEntity))
                continue;

            if (GridEntity.CanOverlap(otherEntity))
                continue;

            if (!WillEntityMove(otherEntity))
                return false;
        }

        return true;
    }

    bool WillEntityMove(GridEntity other)
    {
        if (!other.TryGetComponent(out MovementController otherController))
            return false;

        //Protect from circular dependencies
        if (IsCircularDependency(otherController))
            return false;

        MovementDependency = otherController;

        return otherController.ResolveNextMove() && otherController.NextMove != NextMove;
    }

    bool IsCircularDependency(MovementController movementController)
    {
        MovementController currController = MovementDependency;
        while(currController != null)
        {
            if (currController.MovementDependency == this)
                return true;

            currController = currController.MovementDependency;
        }

        return false;
    }

    public virtual bool WillBlockerMove(Tile destinationTile)
    {
        foreach (GridEntity b in destinationTile.entities)
        {
            //don't check if the object is itself
            if (b.Equals(this.GridEntity))
                continue;

            // if the element is contained within the mask. If so, movement is blocked
            if (b.gameObject.layer == LayerMask.NameToLayer("TileBlocking"))
            {
                if(b.TryGetComponent(out MovementController blocker))
                {
                    if(blocker.IsNextPositionDirty)
                    {
                        IsNextPositionDirty = false;
                        return blocker.ResolveNextMove();
                    }
                    else
                    {
                        return blocker.NextMove != this.NextMove;
                    }
                }
            }
        }

        return true;
    }

    public virtual void PostMoveUpdate()
    {
        IsNextPositionDirty = true;
        MovementDependency = null;
    }

    public virtual GridEntity GetGridEntity()
    {
        return _mover;
    }

    public virtual Vector3Int GetCurrentPosition()
    {
        return _mover.GridPosition;
    }

}
