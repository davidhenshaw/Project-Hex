using DG.Tweening;
using metakazz.Hex;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFollower : MonoBehaviour
{
    public BoardElement toFollow;
    
    ElementMovement _parentMover;
    ElementMovement _mover;

    private void OnParentMoved(Vector3Int from, Vector3Int to)
    {
        _mover.Move(from);
    }

    private void Awake()
    {
        _mover = GetComponent<ElementMovement>();
        _parentMover = toFollow.GetComponentInParent<ElementMovement>();
    }

    protected void Start()
    {
        _parentMover.Moved += OnParentMoved;
    }

    private void OnDisable()
    {
        _parentMover.Moved -= OnParentMoved;
    }
}
