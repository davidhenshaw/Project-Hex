using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerTransformAction : ActionBase
{
    FlowerBehavior Original;
    GameObject TargetObject;

    public event Action<FlowerType> OnExecute;
    public event Action OnValidate;

    public FlowerTransformAction(FlowerBehavior original, GameObject target)
    {
        Original = original;
        TargetObject = target;
    }

    public override void Execute()
    {
        GameObject.Instantiate(TargetObject, Original.transform.position, Original.transform.rotation, Original.Board.grid.transform);
        //GameObject.Instantiate(pollenBurstPrefab, transform.position, transform.rotation, Board.grid.transform);

        Original.gameObject.SetActive(false);

        var offspringType = TargetObject.GetComponent<FlowerBehavior>().Type;
        OnExecute?.Invoke(offspringType);
    }

    public override ActionBase GetMimickAction(EntityController copyer)
    {
        throw new System.NotImplementedException();
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }

    public override bool Validate(Board board)
    {
        throw new System.NotImplementedException();
    }
}
