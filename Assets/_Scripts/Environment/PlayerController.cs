using System;
using UnityEngine;
using metakazz.Hex;
using System.Collections.Generic;

public class PlayerController : EntityController
{
    private static List<PlayerController> AllPlayerControllers = new List<PlayerController>();

    bool _isInteracting = false;

    private BeeBehavior _beehaviour;
    DirectionSelectorWidget _dirSelector;

    public override event Action<ActionBase> NextActionCalculated;

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
        //_dirSelector.Selected += (dir) =>
        //{
        //    SetNextMoveRecursive(dir);
        //    _board.Tick();
        //};
    }

    private void OnInteractFinished()
    {
        _isInteracting = false;
    }

    public override void ExecuteMove(Vector3Int destination)
    {
        if (_isInteracting)
            return;
        
        base.ExecuteMove(destination);

        AudioManager.PlayOneShot(AudioManager.Instance.beeMoved);
    }
    
    public override ActionBase CalculateNextAction()
    {
        NextAction = null;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            NextAction = new InteractAction(_beehaviour, _beehaviour.Position);
            _isInteracting = true;
            return NextAction;
        }

        if(Input.GetButtonDown("Move_N"))
        {
            NextAction = SetNextMove(HexDirection.NORTH);
        }        
        
        if(Input.GetButtonDown("Move_S"))
        {
            NextAction = SetNextMove(HexDirection.SOUTH);
        }
        
        if(Input.GetButtonDown("Move_NE"))
        {
            NextAction = SetNextMove(HexDirection.NORTHEAST);
        }

        if (Input.GetButtonDown("Move_SE"))
        {
            NextAction = SetNextMove(HexDirection.SOUTHEAST);
        }

        if (Input.GetButtonDown("Move_NW"))
        {
            NextAction = SetNextMove(HexDirection.NORTHWEST);
        }

        if (Input.GetButtonDown("Move_SW"))
        {
            NextAction = SetNextMove(HexDirection.SOUTHWEST);
        }

        NextActionCalculated?.Invoke(NextAction);

        return NextAction;
    }

    public override void PostActionUpdate()
    {
        base.PostActionUpdate();

        _beehaviour.OnPostMoveUpdate();
    }

    ActionBase SetNextMove(HexDirection dir)
    {
        var action = new MoveAction(this, CurrentPosition, GetNeighborDirection(dir));
        return action;
    }

    Vector3Int GetNeighborDirection(HexDirection dir)
    {
        return _entity.GridPosition.Neighbor(dir);
    }

    /// <summary>
    /// Sets next move for ALL player controllers currently in existence
    /// </summary>
    /// <param name="dir"></param>
    void SetNextMoveRecursive(HexDirection dir)
    {
        NextAction = SetNextMove(dir);

        foreach (PlayerController pc in AllPlayerControllers)
        {
            if (pc.Equals(this))
                continue;

            pc.SetNextMove(dir);
        }
    }
}
