using System;
using UnityEngine;
using metakazz.Hex;

public class PlayerController : MovementController
{
    bool _isInteracting = false;

    private BeeBehavior _beehaviour;

    [SerializeField]
    private GameObject _dirIndicator;

    //public override event Action<Vector3Int, Vector3Int> MoveBlocked;

    protected override void Awake()
    {
        base.Awake();
        _beehaviour = GetComponent<BeeBehavior>();
        _beehaviour.InteractFinished += OnInteractFinished;
    }

    private void OnInteractFinished()
    {
        _isInteracting = false;
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
            _isInteracting = true;
            AudioManager.PlayOneShot(AudioManager.Instance.beeLand);
        }
    }

    public override void ExecuteMove()
    {
        if (_isInteracting)
            return;

        base.ExecuteMove();
        AudioManager.PlayOneShot(AudioManager.Instance.beeMoved);
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
