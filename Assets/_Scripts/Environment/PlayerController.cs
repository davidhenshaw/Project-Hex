using System;
using UnityEngine;
using metakazz.Hex;
using DG.Tweening;

public interface IMover
{
    event Action<Vector3Int, Vector3Int> Moved;
    void Move(Vector3Int moveDir);
}

public class PlayerController : BoardElement, IMover
{
    public event Action Died;
    public event Action<Vector3Int, Vector3Int> Moved;

    private Collider2D _collider;

    [SerializeField]
    private GameObject _dirIndicator;

    [SerializeField]
    private float _moveSpeed = 0.3f;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        HandleInputs();
    }

    public void Move(Vector3Int to)
    {
        var worldPos = Board.grid.CellToWorld(to);

        Moved?.Invoke(GridPosition, to);

        SetGridPosition(to);

        _collider.enabled = false;
        transform
            .DOMove(worldPos, _moveSpeed)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _collider.enabled = true;
            });
    }

    private void MoveDir(HexDirection moveDir)
    {
        if (!Board.CanMove(GridPosition, moveDir))
            return;

        Vector3Int newPos = GridPosition.Neighbor(moveDir);

        Move(newPos);
    }

    void HandleInputs()
    {
        if (Input.GetButton("Move_E"))
        {
            if (Input.GetButtonDown("Move_N"))
            {
                SetDirIndicator(HexDirection.NORTHEAST);
                MoveDir(HexDirection.NORTHEAST);
            }
            else if(Input.GetButtonDown("Move_S"))
            {
                SetDirIndicator(HexDirection.SOUTHEAST);
                MoveDir(HexDirection.SOUTHEAST);
            }
            return;
        }

        if (Input.GetButton("Move_W"))
        {
            if (Input.GetButtonDown("Move_N"))
            {
                MoveDir(HexDirection.NORTHWEST);
                SetDirIndicator(HexDirection.NORTHWEST);
            }
            else if (Input.GetButtonDown("Move_S"))
            {
                MoveDir(HexDirection.SOUTHWEST);
                SetDirIndicator(HexDirection.SOUTHWEST);
            }
            return;
        }

        if(Input.GetButtonDown("Move_N"))
        {
            SetDirIndicator(HexDirection.NORTH);
            MoveDir(HexDirection.NORTH);
        }        
        
        if(Input.GetButtonDown("Move_S"))
        {
            SetDirIndicator(HexDirection.SOUTH);
            MoveDir(HexDirection.SOUTH);
        }
    }

    void SetDirIndicator(HexDirection dir)
    {
        _dirIndicator.transform.rotation = Quaternion.Euler(0,0, HexUtil.ToAngle(dir));
    }
    public void Interact()
    {
        Died?.Invoke();
    }
}
