using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Rotator : BoardElement, TriggerResponse
{
    [SerializeField]
    private bool clockwise;
    SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();   
    }

    protected override void Start()
    {
        if (Application.IsPlaying(gameObject))
            base.Start();
    }

    private void Update()
    {
        _renderer.flipX = clockwise;
    }

    public void OnActivate()
    {
        Tile currTile;
        Board.tiles.TryGetValue(GridPosition, out currTile);

        if (!currTile)
            return;

        if (clockwise)
            currTile.RotateClockwise();
        else
            currTile.RotateCounterClockwise();
    }

    public void OnDeactivate()
    {
        //throw new System.NotImplementedException();
    }

    public void WhileActive()
    {
        //throw new System.NotImplementedException();
    }
}
