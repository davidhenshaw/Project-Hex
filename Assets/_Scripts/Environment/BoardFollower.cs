using DG.Tweening;
using metakazz.Hex;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardFollower : EntityController
{
    [FormerlySerializedAs("toFollow")]
    public GridEntity Leader;
    
    GridEntityMovement _parentMover;

    protected override void Awake()
    {
        base.Awake();
        if(Leader != null)
            _parentMover = Leader.GetComponent<GridEntityMovement>();
    }

    public void SetToFollow(GridEntity leader)
    {
        Leader = leader;
        _parentMover = leader.GetComponent<GridEntityMovement>();
    }

    public void ClearLeader()
    {
        Leader = null;
        _parentMover = null;
    }

    public override ActionBase CalculateNextAction()
    {
        if (_parentMover == null)
            return null;

        NextAction = new MoveAction(this, CurrentPosition, Leader.GridPosition);
        return NextAction;
    }
}
