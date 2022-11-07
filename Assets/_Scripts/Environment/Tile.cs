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
    public readonly List<GridEntity> entities = new List<GridEntity>();
    /// <summary>
    /// A list of BoardElements that want to move to this tile
    /// </summary>
    public readonly List<GridEntity> speculativeElements = new List<GridEntity>();

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

    public void Add(GridEntity b)
    {
        entities.Add(b);
        foreach(GridEntity obj in entities)
        {
            if (obj.Equals(b))
                continue;

            obj.OnTileEnter(b);
        }
    }

    public void Remove(GridEntity b)
    {
        entities.Remove(b);
        foreach (GridEntity obj in entities)
        {
            if (obj.Equals(b))
                continue;

            obj.OnTileExit(b);
        }
    }
}
