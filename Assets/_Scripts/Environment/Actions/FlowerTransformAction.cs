using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerTransformAction : ActionBase
{
    FlowerBehavior _original;
    GameObject _targetPrefab;
    GameObject _instantiatedPrefab;

    public event Action<FlowerType> OnExecute;
    public event Action OnValidate;

    public FlowerTransformAction(FlowerBehavior original, GameObject target)
    {
        _original = original;
        _targetPrefab = target;
    }

    public override void Execute()
    {
        _instantiatedPrefab = GameObject.Instantiate(_targetPrefab, _original.transform.position, _original.transform.rotation, _original.Board.grid.transform);
        //GameObject.Instantiate(pollenBurstPrefab, transform.position, transform.rotation, Board.grid.transform);

        _original.gameObject.SetActive(false);

        var offspringType = _targetPrefab.GetComponent<FlowerBehavior>().Type;
        OnExecute?.Invoke(offspringType);
    }

    public override ActionBase GetMimickAction(EntityController copyer)
    {
        throw new System.NotImplementedException();
    }

    public override void Undo()
    {
        _original.gameObject.SetActive(true);
        GameObject.Destroy(_instantiatedPrefab);
    }

    public override bool Validate(Board board)
    {
        return true;
    }
}
