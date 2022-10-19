using System;
using UnityEngine;
using metakazz.Hex;
using DG.Tweening;

public interface IMovementController
{
    Vector3Int NextMove { get; set; }

    bool IsNextPositionDirty
    {
        get;
    }

    Vector3Int CalculateNextPosition();

    void ValidateNextMove();

    void ExecuteMove();

    BoardElement GetBoardElement();
    Vector3Int GetCurrentPosition();
}

public class PlayerController : MovementController
{
    public event Action Died;
    private BeeBehavior _beehaviour;

    [SerializeField]
    private GameObject _dirIndicator;

    protected override void Awake()
    {
        base.Awake();
        _beehaviour = GetComponent<BeeBehavior>();
    }

    private void Update()
    {
        //Handle inputs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _beehaviour.TriggerInteract();
        }
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
