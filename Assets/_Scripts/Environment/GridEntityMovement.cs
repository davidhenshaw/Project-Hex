using DG.Tweening;
using metakazz.Hex;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEntityMovement : GridEntity
{
    Collider _collider;
    [SerializeField]
    float _moveSpeed = 0.3f;


    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Move(Vector3Int to)
    {
        if (to.Equals(GridPosition))
            return;

        var worldPos = Board.grid.CellToWorld(to);

        SetGridPosition(to);

        _collider.enabled = false;
        transform
            .DOMove(worldPos, _moveSpeed)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _collider.enabled = true;
            });
    }

    public void OnMoveBlocked(Vector3Int to)
    {
        var worldPos = Board.grid.CellToWorld(to);
        var originalPos = transform.position;

        var positionExtreme = (worldPos - transform.position) * 0.2f;
        positionExtreme += transform.position;

        _collider.enabled = false;

        if (DOTween.IsTweening(transform, true))
            return;

        transform
            .DOMove(positionExtreme, _moveSpeed/2)
            .SetEase(Ease.OutQuad)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                _collider.enabled = true;
                transform.position = originalPos;
            });
    }

}
