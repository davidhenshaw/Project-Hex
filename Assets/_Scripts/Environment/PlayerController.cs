using System;
using UnityEngine;
using metakazz.Hex;
using System.Collections.Generic;

public class PlayerController : MovementController
{
    private static List<PlayerController> AllPlayerControllers = new List<PlayerController>();

    bool _isInteracting = false;

    private BeeBehavior _beehaviour;
    DirectionSelectorWidget _dirSelector;

    protected override void Awake()
    {
        base.Awake();
        AllPlayerControllers.Add(this);

        _dirSelector = GetComponentInChildren<DirectionSelectorWidget>();
        _beehaviour = GetComponent<BeeBehavior>();
        _beehaviour.InteractFinished += OnInteractFinished;
    }

    private void OnDisable()
    {
        AllPlayerControllers.Remove(this);
    }

    private void Start()
    {
        _dirSelector.Selected += (dir) =>
        {
            SetNextMoveRecursive(dir);
            board.Tick();
        };
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

        CalculateNextPosition();
    }

    public override ActionBase ExecuteMove()
    {
        if (_isInteracting)
            return new MoveAction(this, GetCurrentPosition(), GetCurrentPosition());

        AudioManager.PlayOneShot(AudioManager.Instance.beeMoved);
        return base.ExecuteMove();
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
        NextMove = _mover.GridPosition.Neighbor(dir);
    }

    /// <summary>
    /// Sets next move for ALL player controllers currently in existence
    /// </summary>
    /// <param name="dir"></param>
    void SetNextMoveRecursive(HexDirection dir)
    {
        NextMove = _mover.GridPosition.Neighbor(dir);

        foreach (PlayerController pc in AllPlayerControllers)
        {
            if (pc.Equals(this))
                continue;

            pc.SetNextMove(dir);
        }
    }
}
