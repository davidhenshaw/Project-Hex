using DG.Tweening;
using metakazz.Hex;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFollower : MovementController
{
    public BoardElement toFollow;
    
    ElementMovement _parentMover;

    //private void OnParentMoved(Vector3Int from, Vector3Int to)
    //{
    //    _mover.Move(from);
    //}

    protected override void Awake()
    {
        base.Awake();
        if(toFollow != null)
            _parentMover = toFollow.GetComponent<ElementMovement>();
    }

    //protected void Start()
    //{
    //    _parentMover.Moved += OnParentMoved;
    //}

    //private void OnDisable()
    //{
    //    _parentMover.Moved -= OnParentMoved;
    //}

    public void SetToFollow(BoardElement leader)
    {
        toFollow = leader;
        _parentMover = leader.GetComponent<ElementMovement>();
    }

    public void ClearLeader()
    {
        toFollow = null;
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
