using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPollinator
{
    FlowerType GetPollenType();
    void ClearPollen();
    void SetPollen(FlowerType flowerType);
}

public class PollinateAction : ActionBase
{
    public FlowerType PreviousPollen { get; private set; }
    public FlowerType IncomingPollen { get; private set; }
    public IPollinator Target { get; private set; }
    public FlowerBehavior Flower { get; private set; }

    public PollinateAction(IPollinator target, FlowerType incomingPollen)
    {
        PreviousPollen = target.GetPollenType();
        IncomingPollen = incomingPollen;
        Target = target;
    }

    public PollinateAction(IPollinator target, FlowerBehavior flower)
    {
        PreviousPollen = target.GetPollenType();
        IncomingPollen = flower.Type;
        Target = target;
    }

    public override void Execute()
    {
        if(IncomingPollen == null)
        {
            Target.ClearPollen();
            return;
        }

        Target.SetPollen(IncomingPollen);
    }

    public override ActionBase GetMimickAction(EntityController copyer)
    {
        return null;
    }

    public override void Undo()
    {
        Target.SetPollen(PreviousPollen);
    }

    public override bool Validate(Board board)
    {
        return true;
        //throw new System.NotImplementedException();
    }
}
