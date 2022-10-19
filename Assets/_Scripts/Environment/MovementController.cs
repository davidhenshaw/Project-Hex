using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementController : MonoBehaviour
{
    public Vector3Int NextMove { get; set; }
    public bool IsNextPositionDirty { get; set; }

    protected ElementMovement _mover;

    protected virtual void Awake()
    {
        _mover = GetComponent<ElementMovement>();
    }

    public abstract Vector3Int CalculateNextPosition();

    public virtual void ExecuteMove()
    {
        _mover.Move(NextMove);
    }

    public virtual void ValidateNextMove()
    {
        if (!CanMove(NextMove))
            NextMove = _mover.GridPosition;
    }

    public virtual BoardElement GetBoardElement()
    {
        return _mover;
    }

    public virtual Vector3Int GetCurrentPosition()
    {
        return _mover.GridPosition;
    }

    public virtual bool CanMove(Vector3Int destPos)
    {
        var board = Board.Instance;

        if (board.isFrozen)
            return false;

        Tile destinationTile;

        // if there is no tile at the next grid position, you can't move there
        if (!board.tiles.TryGetValue(destPos, out destinationTile))
        {
            return false;
        }

        foreach (BoardElement b in destinationTile.speculativeElements)
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

}
