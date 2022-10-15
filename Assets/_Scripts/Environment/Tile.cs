using System;
using System.Collections.Generic;
using UnityEngine;
using metakazz.Hex;

//[ExecuteAlways]
public class Tile : BoardElement
{
    public readonly List<BoardElement> elements = new List<BoardElement>();

    public void SnapToNearestCell()
    {
        if (!Board.grid)
            return;

        GridPosition = Board.grid
            .WorldToCell(transform.position);
    }

    public void SnapToNearestCell(Vector3 inputPos)
    {
        if (!Board.grid)
            return;

        GridPosition = Board.grid.WorldToCell(
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
