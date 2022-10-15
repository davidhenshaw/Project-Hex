using System;
using UnityEngine;
using metakazz.Hex;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public event Action Died;
    private ElementMovement _mover;
    private BeeBehavior _beehaviour;

    [SerializeField]
    private GameObject _dirIndicator;

    private void Awake()
    {
        _mover = GetComponent<ElementMovement>();
        _beehaviour = GetComponent<BeeBehavior>();
    }

    private void Update()
    {
        HandleInputs();
    }

    void HandleInputs()
    {
        if (Input.GetButton("Move_E"))
        {
            if (Input.GetButtonDown("Move_N"))
            {
                SetDirIndicator(HexDirection.NORTHEAST);
                _mover.MoveDir(HexDirection.NORTHEAST);
            }
            else if(Input.GetButtonDown("Move_S"))
            {
                SetDirIndicator(HexDirection.SOUTHEAST);
                _mover.MoveDir(HexDirection.SOUTHEAST);
            }
            return;
        }

        if (Input.GetButton("Move_W"))
        {
            if (Input.GetButtonDown("Move_N"))
            {
                _mover.MoveDir(HexDirection.NORTHWEST);
                SetDirIndicator(HexDirection.NORTHWEST);
            }
            else if (Input.GetButtonDown("Move_S"))
            {
                _mover.MoveDir(HexDirection.SOUTHWEST);
                SetDirIndicator(HexDirection.SOUTHWEST);
            }
            return;
        }

        if(Input.GetButtonDown("Move_N"))
        {
            SetDirIndicator(HexDirection.NORTH);
            _mover.MoveDir(HexDirection.NORTH);
        }        
        
        if(Input.GetButtonDown("Move_S"))
        {
            SetDirIndicator(HexDirection.SOUTH);
            _mover.MoveDir(HexDirection.SOUTH);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            _beehaviour.TriggerInteract();
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
