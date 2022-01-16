using System;
using UnityEngine;
using metakazz.Hex;
using DG.Tweening;

public class PlayerController : BoardElement, IDeathTileInteractable
{
    public event Action Died;
    private Collider2D _collider;

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
            .DOMove(worldPos, 0.3f)
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
            .DOMove(worldPos, 0.3f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                GridPosition = newPos;
                _collider.enabled = true;
            });
    }

    void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            VertexMove(HexVertex.NORTHEAST);
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            VertexMove(HexVertex.EAST);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            VertexMove(HexVertex.SOUTHEAST);
        }

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            VertexMove(HexVertex.NORTHWEST);
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            VertexMove(HexVertex.WEST);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            VertexMove(HexVertex.SOUTHWEST);
        }

        if (Input.GetButtonDown("Move_N"))
        {
            Move(HexDirection.NORTH);
        }

        if (Input.GetButtonDown("Move_NE"))
        {
            Move(HexDirection.NORTHEAST);
        }

        if (Input.GetButtonDown("Move_SE"))
        {
            Move(HexDirection.SOUTHEAST);
        }

        if (Input.GetButtonDown("Move_S"))
        {
            Move(HexDirection.SOUTH);
        }

        if (Input.GetButtonDown("Move_SW"))
        {
            Move(HexDirection.SOUTHWEST);
        }

        if (Input.GetButtonDown("Move_NW"))
        {
            Move(HexDirection.NORTHWEST);
        }
    }

    public void Interact()
    {
        Died?.Invoke();
    }
}
