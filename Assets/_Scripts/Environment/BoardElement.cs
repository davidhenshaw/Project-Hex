using UnityEngine;

public abstract class BoardElement : MonoBehaviour
{
    private Board _board;
    public Vector3Int GridPosition 
    {
        get;
        private set;
    }


    public void SetGridPosition(Vector3Int value)
    {
        Tile fromTile;
        Tile toTile;
        Board.tiles.TryGetValue(GridPosition, out fromTile);
        Board.tiles.TryGetValue(value, out toTile);

        if (!toTile)
            return;

        fromTile.Remove(this);

        GridPosition = value;

        toTile.Add(this);
    }

    protected Board Board {
        get
        {
            if(!_board)
                _board = GetComponentInParent<Board>();

            return _board;
        }
        private set => _board = value;
    }

    protected virtual void Start()
    {
        GridPosition = Board.grid
            .WorldToCell(transform.position);
        Tile t;
        Board.tiles.TryGetValue(GridPosition, out t);

        if (!t)
            return;

        t.Add(this);

        //Snap to this tile's center
        transform.position = Board.grid
                                .CellToWorld(GridPosition);
    }

}
