using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticElementController : MovementController
{
    BoardElement staticElement;

    protected override void Awake()
    {
        base.Awake();
        staticElement = GetComponent<BoardElement>();
    }

    public override void ValidateNextMove()
    {
    }

    public override BoardElement GetBoardElement()
    {
        return staticElement;
    }

    public override Vector3Int CalculateNextPosition()
    {
        return staticElement.GridPosition;
    }

    public override bool CanMove(Vector3Int destPos)
    {
        return false;
    }

    public override void ExecuteMove()
    {
        
    }
}
