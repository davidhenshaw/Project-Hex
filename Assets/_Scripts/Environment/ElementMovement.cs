using DG.Tweening;
using metakazz.Hex;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementMovement : BoardElement
{
    /// <summary>
    /// arg1 = from  
    /// arg2 = to
    /// </summary>
    public event Action<Vector3Int, Vector3Int> Moved;
    /// <summary>
    /// arg1 = from 
    /// arg2 = to
    /// </summary>
    public event Action<Vector3Int, Vector3Int> MoveBlocked;

    public Vector3Int NextPosition
    {
        get;
        private set;
    }

    public bool IsNextPositionDirty {
        get;
        private set;
    }

    Collider _collider;
    [SerializeField]
    float _moveSpeed = 0.3f;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
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
