using System;
using System.Collections;

internal class DisableEntityAction : ActionBase
{
    private GridEntity gridEntity;
    private Action preCallback;
    private Action postCallback;
    private Action undoCallback;


    public DisableEntityAction(GridEntity gridEntity)
    {
        this.gridEntity = gridEntity;
    }

    public DisableEntityAction(GridEntity gridEntity, Action callbackFn)
    {
        this.gridEntity = gridEntity;
        this.preCallback = callbackFn;
        //this.postCallback = postCallbackFn;
    }

    public override void Execute()
    {
        gridEntity.StartCoroutine(ExecuteSequence());
    }

    IEnumerator ExecuteSequence()
    {
        preCallback?.Invoke();
        yield return null;
        gridEntity.gameObject.SetActive(false);
        yield return null;
        postCallback?.Invoke();
    }

    public override ActionBase GetMimickAction(EntityController copyer)
    {
        throw new System.NotImplementedException();
    }

    public override void Undo()
    {
        undoCallback?.Invoke();
        gridEntity.gameObject.SetActive(true);
    }

    public override bool Validate(Board board)
    {
        return true;
    }
}