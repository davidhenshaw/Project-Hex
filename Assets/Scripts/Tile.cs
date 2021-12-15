using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tile : MonoBehaviour
{
    private Grid _grid;

    // Start is called before the first frame update
    void Start()
    {
        _grid = GetComponentInParent<Grid>();

        if (!_grid)
        {
            transform.parent = FindObjectOfType<Grid>().transform;
        }

        SnapToNearestCell();
    }

    public void SnapToNearestCell()
    {
        if (!_grid)
            return;

        Vector3Int cellPos = _grid.WorldToCell(
            transform.position);

        var newPos = _grid.CellToWorld(cellPos);

        transform.position = newPos;
    }

    public void SnapToNearestCell(Vector3 inputPos)
    {
        if (!_grid)
            return;

        Vector3Int cellPos = _grid.WorldToCell(
            inputPos);

        var newPos = _grid.CellToWorld(cellPos);

        transform.position = newPos;
    }

    public Vector3Int GetCoordinates()
    {
        if (!_grid)
            return Vector3Int.zero;

        return _grid.WorldToCell(transform.position);
    }

}
