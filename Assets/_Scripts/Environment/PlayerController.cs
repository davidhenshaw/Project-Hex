using System;
using UnityEngine;
using metakazz.Hex;
using DG.Tweening;

public class PlayerController : BoardElement, IDeathTileInteractable
{
    public event Action Died;
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
            if (Input.GetButton("Move_N"))
            {
                SetDirIndicator(HexDirection.NORTHEAST);
                Move(HexDirection.NORTHEAST);
            }
            else if(Input.GetButton("Move_S"))
            {
                SetDirIndicator(HexDirection.SOUTHEAST);
                Move(HexDirection.SOUTHEAST);
            }
            return;
        }

        if (Input.GetButton("Move_W"))
        {
            if (Input.GetButton("Move_N"))
            {
                Move(HexDirection.NORTHWEST);
                SetDirIndicator(HexDirection.NORTHWEST);
            }
            else if (Input.GetButton("Move_S"))
            {
                Move(HexDirection.SOUTHWEST);
                SetDirIndicator(HexDirection.SOUTHWEST);
            }
            return;
        }

        if(Input.GetButtonDown("Move_N"))
        {
            SetDirIndicator(HexDirection.NORTH);
            Move(HexDirection.NORTH);
        }        
        
        if(Input.GetButtonDown("Move_S"))
        {
            SetDirIndicator(HexDirection.SOUTH);
            Move(HexDirection.SOUTH);
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
