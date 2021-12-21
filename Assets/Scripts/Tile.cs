using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[ExecuteAlways]
public class Tile : BoardElement
{
    public readonly List<BoardElement> elements = new List<BoardElement>();

    // Start is called before the first frame update
    void Start()
    {
        SnapToNearestCell();
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

    public List<BoardElement> GetWalls()
    {
        var output = elements.FindAll(
            (element) =>
           {
               return element.GetType().Equals(typeof(Wall));
           }
         );

        return output;
    }

    public List<BoardElement> GetAll(Type type)
    {
        var output = elements.FindAll(
            (element) =>
            {
                return element.GetType().Equals(type);
            }
         );

        return output;
    }
}
