using System;
using UnityEngine;
using metakazz.Hex;

public class PlayerController : MovementController
{
    public event Action Died;
    private BeeBehavior _beehaviour;

    [SerializeField]
    private GameObject _dirIndicator;

    public override event Action<Vector3Int, Vector3Int> MoveBlocked;

    protected override void Awake()
    {
        base.Awake();
        _beehaviour = GetComponent<BeeBehavior>();
    }

    private void Update()
    {
        HandleInputs();
    }

    void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _beehaviour.TriggerInteract();
            AudioManager.PlayOneShot(AudioManager.Instance.beeLand);
        }
    }

    public override void ExecuteMove()
    {
        base.ExecuteMove();
        AudioManager.PlayOneShot(AudioManager.Instance.beeMoved);
    }

    public override bool ValidateNextMove()
    {
        if(!ValidateBeeOverlap())
        {
            MoveBlocked?.Invoke(GetCurrentPosition(), NextMove);
            UpdateSpriteDir(GetCurrentPosition(), NextMove);

            NextMove = GetCurrentPosition();
            IsNextPositionDirty = false;

            return false;
        }
        return base.ValidateNextMove();
    }

    public bool ValidateBeeOverlap()
    {
        var board = Board.Instance;

        if (board.isFrozen)
            return false;

        Tile destinationTile;

        // if there is no tile at the next grid position, you can't move there
        if (!board.tiles.TryGetValue(NextMove, out destinationTile))
        {
            return false;
        }

        foreach (BoardElement b in destinationTile.elements)
        {
            // if the element is trying to move in the opposite direction (swap places) with me, validation fails
            if (b.TryGetComponent(out BeeBehavior other))
            {
                return false;
            }
        }

        return true;
    }

    public override Vector3Int GetCurrentPosition()
    {
        return _mover.GridPosition;
    }
    
    public override Vector3Int CalculateNextPosition()
    {
        if (Input.GetButton("Move_E"))
        {
            if (Input.GetButtonDown("Move_N"))
            {
                SetNextMove(HexDirection.NORTHEAST);
            }
            else if(Input.GetButtonDown("Move_S"))
            {
                SetNextMove(HexDirection.SOUTHEAST);
            }
            return NextMove;
        }

        if (Input.GetButton("Move_W"))
        {
            if (Input.GetButtonDown("Move_N"))
            {
                SetNextMove(HexDirection.NORTHWEST);
            }
            else if (Input.GetButtonDown("Move_S"))
            {
                SetNextMove(HexDirection.SOUTHWEST);
            }
            return NextMove;
        }

        if(Input.GetButtonDown("Move_N"))
        {
            SetNextMove(HexDirection.NORTH);
        }        
        
        if(Input.GetButtonDown("Move_S"))
        {
            SetNextMove(HexDirection.SOUTH);
        }

        return NextMove;
    }

    public override void PostMoveUpdate()
    {
        base.PostMoveUpdate();

        _beehaviour.OnPostMoveUpdate();
    }

    void SetNextMove(HexDirection dir)
    {
        SetDirIndicator(dir);
        NextMove = _mover.GridPosition.Neighbor(dir);
    }

    void SetDirIndicator(HexDirection dir)
    {
        if (!_dirIndicator)
            return;

        _dirIndicator.transform.rotation = Quaternion.Euler(0,0, HexUtil.ToAngle(dir));
    }

}
