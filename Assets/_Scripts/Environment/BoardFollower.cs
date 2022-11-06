using DG.Tweening;
using metakazz.Hex;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardFollower : MovementController
{
    [FormerlySerializedAs("toFollow")]
    public GridEntity Leader;
    
    GridEntityMovement _parentMover;

    //private void OnParentMoved(Vector3Int from, Vector3Int to)
    //{
    //    _mover.Move(from);
    //}

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

    public override Vector3Int CalculateNextPosition()
    {
        if (_parentMover == null)
            return GetCurrentPosition();

        NextMove = _parentMover.GridPosition;
        return NextMove;
    }

}
