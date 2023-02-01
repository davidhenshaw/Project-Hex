using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : ActionBase
{
    public static MoveAction NullMove(EntityController controller)
    {
        return new MoveAction(controller, controller.CurrentPosition, controller.CurrentPosition);
    }

    public EntityController Controller { get; private set; }
    public Vector3Int Start { get; private set; }
    public Vector3Int Destination { get; private set; }

    public bool IsNextPositionDirty = true;
    public EntityController MovementDependency;

    public MoveAction(EntityController entity, Vector3Int startLocation, Vector3Int endLocation)
    {
        Controller = entity;
        Start = startLocation;
        Destination = endLocation;
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }

    public override void Execute()
    {
        Controller.ExecuteMove(Destination);
    }

    public override bool Validate(Board board)
    {
        Tile destinationTile;

        // if there is no tile at the next grid position, you can't move there
        if (!board.tiles.TryGetValue(Destination, out destinationTile))
        {
            IsNextPositionDirty = false;
            return false;
        }

        if (CanOverlapImmediate(destinationTile))
        {
            IsNextPositionDirty = false;
            return true;
        }

        Controller.HandleInvalidAction(this);
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
        var targetEntity = Controller.GridEntity;
        foreach (GridEntity otherEntity in destinationTile.entities)
        {
            //don't check if the object is yourself
            if (otherEntity.Equals(targetEntity))
                continue;

            if (targetEntity.CanOverlap(otherEntity))
                continue;

            if (!WillEntityMove(otherEntity))
                return false;
        }

        return true;
    }

    bool WillEntityMove(GridEntity other)
    {
        if (!other.TryGetComponent(out EntityController otherController))
            return false;

        //Protect from circular dependencies
        if (IsCircularDependency(otherController))
            return false;

        MovementDependency = otherController;

        if (otherController.NextAction is not MoveAction)
            return false;

        var otherMove = ((MoveAction)otherController.NextAction);
        return otherController.ValidateNextAction() && otherMove.Destination != this.Destination;
    }

    bool IsCircularDependency(EntityController controller)
    {
        EntityController currController = controller;
        while (currController != null)
        {
            if (currController.NextAction is not MoveAction)
                break;

            var currAction = currController.NextAction as MoveAction;
            if (currAction.MovementDependency == this.Controller)
                return true;

            currController = currAction.MovementDependency;
        }

        return false;
    }

}
