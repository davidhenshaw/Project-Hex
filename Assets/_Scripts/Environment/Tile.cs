using System;
using System.Collections.Generic;
using UnityEngine;
using metakazz.Hex;

public class Tile : MonoBehaviour
{
    private Board _board;
    public Vector3Int GridPosition
    {
        get;
        private set;
    }
    private Vector3Int _currGridPos;
    public readonly List<BoardElement> elements = new List<BoardElement>();

    private void Awake()
    {
        _board = GetComponentInParent<Board>();
    }

    public void InitPosition()
    {
        GridPosition = _board.grid.WorldToCell(transform.position);
    }

    public void SnapToNearestCell(Vector3 inputPos)
    {
        if (!_board.grid)
            return;

        GridPosition = _board.grid.WorldToCell(
            inputPos);
    }

    public void Add(BoardElement b)
    {
        elements.Add(b);
    }

    public void Remove(BoardElement b)
    {
        elements.Remove(b);
    }
}
