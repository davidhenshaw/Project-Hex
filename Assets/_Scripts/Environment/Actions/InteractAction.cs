using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : ActionBase
{
    public IInteractive Caller { get; private set; }
    public Vector3Int Position { get; private set; }
    public readonly ActionBase[] ResultingActions;

    public InteractAction(IInteractive caller, Vector3Int position)
    {
        Caller = caller;
        Position = position;
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }

    public override bool Validate(Board board)
    {
        return board.IsValid(Position);
    }

    public override void Execute()
    {
        Caller.TriggerInteract();
    }
}
