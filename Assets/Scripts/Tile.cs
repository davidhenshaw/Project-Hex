using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[ExecuteAlways]
public class Tile : BoardElement
{
    public readonly List<BoardElement> elements = new List<BoardElement>();
    public readonly Dictionary<HexDirection, Wall> walls = new Dictionary<HexDirection, Wall>();
    public readonly Dictionary<HexDirection, Wall> cornerWalls = new Dictionary<HexDirection, Wall>();

    // Start is called before the first frame update
    void Start()
    {
        //SnapToNearestCell();
    }

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
