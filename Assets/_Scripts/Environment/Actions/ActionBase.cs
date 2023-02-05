using System;


public abstract class ActionBase
{
    public abstract void Undo();

    public abstract bool Validate(Board board);

    public abstract void Execute();

    public abstract ActionBase GetMimickAction(EntityController copyer);
}
