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
        _parentMover = toFollow.GetComponentInParent<ElementMovement>();
    }

    //protected void Start()
    //{
    //    _parentMover.Moved += OnParentMoved;
    //}

    //private void OnDisable()
    //{
    //    _parentMover.Moved -= OnParentMoved;
    //}

    public override Vector3Int CalculateNextPosition()
    {
        NextMove = _parentMover.GridPosition;
        return NextMove;
    }

}
