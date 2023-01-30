using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : ActionBase
{
    public static MoveAction NullMove(MovementController controller)
    {
        return new MoveAction(controller, controller.GetCurrentPosition(), controller.GetCurrentPosition());
    }

    public MovementController Entity { get; private set; }
    public Vector3Int StartLocation { get; private set; }
    public Vector3Int EndLocation { get; private set; }

    public MoveAction(MovementController entity, Vector3Int startLocation, Vector3Int endLocation)
    {
        Entity = entity;
        StartLocation = startLocation;
        EndLocation = endLocation;
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }
}
