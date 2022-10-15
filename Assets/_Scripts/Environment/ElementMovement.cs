using DG.Tweening;
using metakazz.Hex;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementMovement : BoardElement
{
    public event Action<Vector3Int, Vector3Int> Moved;
    Collider2D _collider;

    [SerializeField]
    float _moveSpeed = 0.3f;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void MoveDir(HexDirection moveDir)
    {
        if (!Board.CanMove(GridPosition, moveDir))
            return;

        Vector3Int newPos = GridPosition.Neighbor(moveDir);

        Move(newPos);
    }

    public void Move(Vector3Int to)
    {
        var worldPos = Board.grid.CellToWorld(to);

        Moved?.Invoke(GridPosition, to);

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
}
