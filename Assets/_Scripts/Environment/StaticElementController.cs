using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticElementController : MovementController
{
    GridEntity staticElement;

    protected override void Awake()
    {
        base.Awake();
        staticElement = GetComponent<GridEntity>();
    }

    public override Vector3Int GetCurrentPosition()
    {
        return staticElement.GridPosition;
    }

    public override bool ResolveNextMove()
    {
        return true;
    }

    public override GridEntity GetGridEntity()
    {
        return staticElement;
    }

    public override Vector3Int CalculateNextPosition()
    {
        return NextMove = staticElement.GridPosition;
    }

    public override bool WillBlockerMove(Tile destinationTile)
    {
        return false;
    }

    public override ActionBase ExecuteMove()
    {
        return MoveAction.NullMove(this);
    }
}
