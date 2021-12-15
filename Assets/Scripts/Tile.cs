using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
    private Grid _grid;
    [Randomize(0, 10)]
    public int myFloat;

    // Start is called before the first frame update
    void Start()
    {
        _grid = GetComponentInParent<Grid>();
    }

    public void SnapToNearestCell()
    {
        Vector3Int cellPos = _grid.WorldToCell(
            transform.position);

        var newPos = _grid.CellToWorld(cellPos);

        transform.position = newPos;
    }

    public void SnapToNearestCell(Vector3 inputPos)
    {
        Vector3Int cellPos = _grid.WorldToCell(
            inputPos);

        var newPos = _grid.CellToWorld(cellPos);

        transform.position = newPos;
    }

    public Vector3Int GetCoordinates()
    {
        return _grid.WorldToCell(transform.position);
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawIcon(transform.position, "pointer.png", true, Color.cyan);
        
    //}
}
