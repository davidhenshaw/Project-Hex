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
    public BoardElement Leader;
    
    ElementMovement _parentMover;

    //private void OnParentMoved(Vector3Int from, Vector3Int to)
    //{
    //    _mover.Move(from);
    //}

    protected override void Awake()
    {
        base.Awake();
        if(Leader != null)
            _parentMover = Leader.GetComponent<ElementMovement>();
    }

    public void SetToFollow(BoardElement leader)
    {
        Leader = leader;
        _parentMover = leader.GetComponent<ElementMovement>();
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
