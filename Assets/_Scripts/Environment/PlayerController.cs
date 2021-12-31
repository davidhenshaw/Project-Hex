using System;
using UnityEngine;
using metakazz.Hex;

public class PlayerController : BoardElement, IDeathTileInteractable
{
    public event Action Died;

    private void Update()
    {
        HandleInputs();
    }

    private void Move(HexDirection moveDir)
    {
        if (!Board.CanMove(GridPosition, moveDir))
            return;

        Vector3Int newPos = GridPosition.Neighbor(moveDir);
        GridPosition = newPos;
    }

    private void VertexMove(HexVertex moveDir)
    {
        if (!Board.CanMove(GridPosition, moveDir))
            return;

        Vector3Int newPos = GridPosition.Neighbor(moveDir);
        GridPosition = newPos;
    }

    void HandleInputs()
    {
        if(Input.GetKeyDown(KeyCode.Keypad6))
        {
            VertexMove(HexVertex.EAST);
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            VertexMove(HexVertex.WEST);
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
