using System;
using UnityEngine;
using metakazz.Hex;
using DG.Tweening;

public class PlayerController : BoardElement, IDeathTileInteractable
{
    public event Action Died;
    private Collider2D _collider;

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

    private void Move(HexDirection moveDir)
    {
        if (!Board.CanMove(GridPosition, moveDir))
            return;

        Vector3Int newPos = GridPosition.Neighbor(moveDir);

        var worldPos = Board.grid.CellToWorld(newPos);

        _collider.enabled = false;
        transform
            .DOMove(worldPos, _moveSpeed)
            .SetEase(Ease.OutQuad)
            .OnComplete( ()=>
            {
                GridPosition = newPos;
                _collider.enabled = true;
            });
    }

    private void VertexMove(HexVertex moveDir)
    {
        if (!Board.CanMove(GridPosition, moveDir))
            return;

        Vector3Int newPos = GridPosition.Neighbor(moveDir);

        var worldPos = Board.grid.CellToWorld(newPos);

        _collider.enabled = false;
        transform
            .DOMove(worldPos, _moveSpeed)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                GridPosition = newPos;
                _collider.enabled = true;
            });
    }

    void HandleInputs()
    {
        if (Input.GetButton("Move_E"))
        {
            if (Input.GetButtonDown("Move_N"))
            {
                Move(HexDirection.NORTHEAST);
            }
            else if(Input.GetButtonDown("Move_S"))
            {
                Move(HexDirection.SOUTHEAST);
            }
            return;
        }

        if (Input.GetButton("Move_W"))
        {
            if (Input.GetButtonDown("Move_N"))
            {
                Move(HexDirection.NORTHWEST);
            }
            else if (Input.GetButtonDown("Move_S"))
            {
                Move(HexDirection.SOUTHWEST);
            }
            return;
        }

        if (Input.GetButtonDown("Move_N"))
        {
            Move(HexDirection.NORTH);
        }

        if (Input.GetButtonDown("Move_S"))
        {
            Move(HexDirection.SOUTH);
        }
    }

    public void Interact()
    {
        Died?.Invoke();
    }
}
