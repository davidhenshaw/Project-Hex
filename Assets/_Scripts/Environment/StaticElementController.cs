using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticElementController : EntityController
{
    GridEntity staticElement;

    protected override void Awake()
    {
        base.Awake();
        staticElement = GetComponent<GridEntity>();
    }

    public override Vector3Int CurrentPosition => staticElement.GridPosition;

    public override bool ValidateNextAction()
    {
        return true;
    }

    public override ActionBase CalculateNextAction()
    {
        return null;
    }
}
