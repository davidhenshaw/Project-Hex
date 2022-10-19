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

    public override Vector3Int GetCurrentPosition()
    {
        return staticElement.GridPosition;
    }

    public override bool ValidateNextMove()
    {
        return false;
    }

    public override BoardElement GetBoardElement()
    {
        return staticElement;
    }

    public override Vector3Int CalculateNextPosition()
    {
        return staticElement.GridPosition;
    }

    public override bool WillBlockerMove(Vector3Int destPos)
    {
        return false;
    }

    public override void ExecuteMove()
    {
        
    }
}
