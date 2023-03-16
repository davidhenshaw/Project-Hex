using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeelineRemoveAction : ActionBase
{
    BeelineController _beeline;
    BeeBehavior _bee;
    List<BeeBehavior> _beeList = new List<BeeBehavior>();

    public BeelineRemoveAction(BeeBehavior bee)
    {
        this._beeline = bee.GetBeelineController();
        this._bee = bee;
        this._beeList = _beeline.GetBeesAsList();
    }

    public override void Execute()
    {
        _beeline.StartCoroutine(
            BeelineController.HandleBeelineSplit(_bee)
        );
    }

    public override ActionBase GetMimickAction(EntityController copyer)
    {
        throw new System.NotImplementedException();
    }

    public override void Undo()
    {
        //bee.transform.SetParent(beeline.transform);
        _bee.gameObject.SetActive(true);
        _beeline.AddBee(_bee);

        _beeline.AddBees(_beeList);
    }

    public override bool Validate(Board board)
    {
        return true;
    }
}
