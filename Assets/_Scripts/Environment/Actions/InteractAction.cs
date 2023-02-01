using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : ActionBase
{
    public IInteractive Caller { get; private set; }
    public IInteractable Target { get; private set; }
    public Vector3Int Position { get; private set; }

    public InteractAction(IInteractive caller, IInteractable target, Vector3Int position)
    {
        Caller = caller;
        Target = target;
        Position = position;
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }

    public override bool Validate(Board board)
    {
        throw new System.NotImplementedException();
    }

    public override void Execute()
    {
        throw new System.NotImplementedException();
    }
}
