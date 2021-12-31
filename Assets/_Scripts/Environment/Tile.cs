using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using metakazz.Hex;

[ExecuteAlways]
public class Tile : BoardElement
{
    public readonly List<BoardElement> elements = new List<BoardElement>();
    public readonly Dictionary<HexDirection, Pipe> pipes = new Dictionary<HexDirection, Pipe>();
    public readonly Dictionary<HexVertex, Pipe> vertexPipes = new Dictionary<HexVertex, Pipe>();

    public void SnapToNearestCell()
    {
        if (!Board.Grid)
            return;

        GridPosition = Board.Grid
            .WorldToCell(transform.position);
    }

    public void SnapToNearestCell(Vector3 inputPos)
    {
        if (!Board.Grid)
            return;

        GridPosition = Board.Grid.WorldToCell(
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
