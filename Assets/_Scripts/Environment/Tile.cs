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
    public readonly List<BoardElement> elements = new List<BoardElement>();

    public void InitPosition()
    {
        _board = Board.Instance;
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
        foreach(BoardElement obj in elements)
        {
            if (obj.Equals(b))
                continue;

            obj.OnTileEnter(b);
        }
    }

    public void Remove(BoardElement b)
    {
        elements.Remove(b);
        foreach (BoardElement obj in elements)
        {
            if (obj.Equals(b))
                continue;

            obj.OnTileExit(b);
        }
    }
}
