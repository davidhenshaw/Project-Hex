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

    protected ElementMovement _mover;

    [SerializeField]
    GameObject _sprite;

    bool isFacingRight = true;

    protected virtual void Awake()
    {
        _mover = GetComponent<ElementMovement>();
    }

    public abstract Vector3Int CalculateNextPosition();

    public virtual void ExecuteMove()
    {
        var from = GetCurrentPosition();
        _mover.Move(NextMove);
        IsNextPositionDirty = true;

        if(from != NextMove)
            UpdateSpriteDir(from, NextMove);
        
        Moved?.Invoke(from, NextMove);
    }

    public virtual bool ValidateNextMove()
    {
        if(!ValidateMovementOverlap())
        {
            IsNextPositionDirty = false;
            NextMove = GetCurrentPosition();
            return false;
        }

        if (CanMoveImmediate(NextMove))
        {
            IsNextPositionDirty = false;
            return true;
        }

        if (WillBlockerMove(NextMove))
        {
            IsNextPositionDirty = false;
            return true;
        }

        MoveBlocked?.Invoke(GetCurrentPosition(), NextMove);
        
        NextMove = GetCurrentPosition();
        IsNextPositionDirty = false;

        return false;
    }

    public virtual BoardElement GetBoardElement()
    {
        return _mover;
    }

    public virtual Vector3Int GetCurrentPosition()
    {
        return _mover.GridPosition;
    }

    public virtual bool CanMoveImmediate(Vector3Int destPos)
    {
        var board = Board.Instance;

        Tile destinationTile;

        // if there is no tile at the next grid position, you can't move there
        if (!board.tiles.TryGetValue(destPos, out destinationTile))
        {
            return false;
        }

        foreach (BoardElement b in destinationTile.elements)
        {
            //don't check if the object is itself
            if (b.Equals(this.GetBoardElement()))
                continue;

            // if the element is contained within the mask. If so, movement is blocked
            if (b.gameObject.layer == LayerMask.NameToLayer("TileBlocking"))
            {
                return false;
            }
        }

        return true;
    }

    public virtual bool WillBlockerMove(Vector3Int destPos)
    {
        var board = Board.Instance;

        Tile destinationTile;

        // if there is no tile at the next grid position, you can't move there
        if (!board.tiles.TryGetValue(destPos, out destinationTile))
        {
            return false;
        }

        foreach (BoardElement b in destinationTile.elements)
        {
            //don't check if the object is itself
            if (b.Equals(this.GetBoardElement()))
                continue;

            // if the element is contained within the mask. If so, movement is blocked
            if (b.gameObject.layer == LayerMask.NameToLayer("TileBlocking"))
            {
                if(b.TryGetComponent(out MovementController blocker))
                {
                    if(blocker.IsNextPositionDirty)
                    {
                        IsNextPositionDirty = false;
                        return blocker.ValidateNextMove();
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

    public bool ValidateMovementOverlap()
    {
        var board = Board.Instance;

        Tile destinationTile;

        // if there is no tile at the next grid position, you can't move there
        if (!board.tiles.TryGetValue(NextMove, out destinationTile))
        {
            return false;
        }

        foreach (BoardElement b in destinationTile.elements)
        {
            // if the element is trying to move in the opposite direction (swap places) with me, validation fails
            if (b.TryGetComponent(out MovementController other))
            {
                Vector3 moveDir = NextMove - GetCurrentPosition();
                Vector3 otherMoveDir = other.NextMove - other.GetCurrentPosition();
                
                if(Vector3.Dot(moveDir.normalized, otherMoveDir.normalized) <= -0.9f)
                {
                    return false;
                }

            }
        }

        return true;
    }

    public virtual void PostMoveUpdate()
    {

    }

    protected void UpdateSpriteDir(Vector3Int from, Vector3Int to)
    {
        var moveDir = to.YXZ() - from.YXZ();
        var dotProduct = Vector3.Dot(Vector3.right, moveDir);

        if (dotProduct > 0 && !isFacingRight || dotProduct < 0 && isFacingRight)
            FlipSprite();
    }

    [ContextMenu("Flip")]
    protected void FlipSprite()
    {
        if (!_sprite)
            return;

        Vector3 target = isFacingRight ? new Vector3(0, 0, 180) : Vector3.zero ;

        _sprite.transform
            .DOLocalRotate(target, 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                isFacingRight = !isFacingRight;
            });
    }
}
