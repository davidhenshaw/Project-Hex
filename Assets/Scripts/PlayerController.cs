using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BoardElement
{
    Grid _grid;

    // Start is called before the first frame update
    void Start()
    {
        _grid = GetComponentInParent<Grid>();
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
        GridPosition = newPos;
    }

    void HandleInputs()
    {
        if(Input.GetButtonDown("Move_N"))
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
}
