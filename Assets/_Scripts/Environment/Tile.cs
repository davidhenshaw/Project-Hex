using System;
using System.Collections.Generic;
using UnityEngine;
using metakazz.Hex;

//[ExecuteAlways]
public class Tile : BoardElement
{
    public readonly List<BoardElement> elements = new List<BoardElement>();
    public readonly Dictionary<HexDirection, Pipe> pipes = new Dictionary<HexDirection, Pipe>();
    public readonly Dictionary<HexVertex, Pipe> vertexPipes = new Dictionary<HexVertex, Pipe>();

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

    public void UnbindNeighborFace(HexDirection neighborDir)
    {
        var neighborPos  = GridPosition.Neighbor(neighborDir);
        Tile neighbor;

        Board.tiles.TryGetValue(neighborPos, out neighbor);
        if (!neighbor)
            return;

        neighbor
            .pipes
            .Remove(
            HexUtil.Opposite(neighborDir)
            );
    }

    [ContextMenu("Rotate Clockwise")]
    public void RotateClockwise()
    {
        //Pre-Rotate
        UnbindNeighborFace(HexDirection.NORTH);
        UnbindNeighborFace(HexDirection.NORTHEAST);
        UnbindNeighborFace(HexDirection.SOUTHEAST);
        UnbindNeighborFace(HexDirection.SOUTH);
        UnbindNeighborFace(HexDirection.SOUTHWEST);
        UnbindNeighborFace(HexDirection.NORTHWEST);
        var pipesTemp = new Dictionary<HexDirection, Pipe>(pipes);
        foreach (Pipe p in pipesTemp.Values)
        {
            p.PreRotate();
        }

        //Perform rotation
        foreach (Pipe p in pipesTemp.Values)
        {
            p.RotateClockwise(this);
        }

        //Post Rotate
        pipes.Clear();
        foreach(Pipe p in pipesTemp.Values)
        {
            p.PostRotate();
        }
    }

    [ContextMenu("Rotate Counter-Clockwise")]
    public void RotateCounterClockwise()
    {
        //Pre-Rotate
        UnbindNeighborFace(HexDirection.NORTH);
        UnbindNeighborFace(HexDirection.NORTHEAST);
        UnbindNeighborFace(HexDirection.SOUTHEAST);
        UnbindNeighborFace(HexDirection.SOUTH);
        UnbindNeighborFace(HexDirection.SOUTHWEST);
        UnbindNeighborFace(HexDirection.NORTHWEST);
        var pipesTemp = new Dictionary<HexDirection, Pipe>(pipes);
        foreach (Pipe p in pipesTemp.Values)
        {
            p.PreRotate();
        }

        //Perform rotation
        foreach (Pipe p in pipesTemp.Values)
        {
            p.RotateCounterClockwise(this);
        }

        //Post Rotate
        pipes.Clear();
        foreach (Pipe p in pipesTemp.Values)
        {
            p.PostRotate();
        }
    }
}
