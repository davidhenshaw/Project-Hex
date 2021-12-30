using UnityEngine;

public abstract class BoardElement : MonoBehaviour
{
    private Board _board;
    private Vector3Int _gridPos;

    public Vector3Int GridPosition {
        get
        {
            _gridPos = Board.Grid
                        .WorldToCell(transform.position);
            return _gridPos;
        }
        set
        {

            Tile t;
            Board.tiles.TryGetValue(_gridPos, out t);
            t.Remove(this);

            _gridPos = value;
            transform.position = Board.Grid
                                    .CellToWorld(_gridPos);

            Board.tiles.TryGetValue(value, out t);
            t.Add(this);
        }
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
        _gridPos = Board.Grid
            .WorldToCell(transform.position);
        Tile t;
        Board.tiles.TryGetValue(_gridPos, out t);

        if (!t)
            return;

        t.Add(this);
    }

}
