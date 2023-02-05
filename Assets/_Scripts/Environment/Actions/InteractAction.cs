using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : ActionBase
{
    public IInteractive Caller { get; private set; }
    public Vector3Int Position { get; private set; }
    public ActionBase[] ResultingActions { get; private set; }

    public InteractAction(IInteractive caller, Vector3Int position)
    {
        Caller = caller;
        Position = position;
    }

    public override void Undo()
    {
        foreach(ActionBase action in ResultingActions)
        {
            action.Undo();
        }
    }

    public override bool Validate(Board board)
    {
        return board.IsValid(Position);
    }

    public override void Execute()
    {
        ResultingActions = Caller.TriggerInteract();
        foreach(ActionBase action in ResultingActions)
        {
            if (action == null)
                continue;

            action.Execute();
        }
    }

    public override ActionBase GetMimickAction(EntityController copyer)
    {
        return new InteractAction(copyer.GetComponent<IInteractive>(), copyer.CurrentPosition);
    }
}
