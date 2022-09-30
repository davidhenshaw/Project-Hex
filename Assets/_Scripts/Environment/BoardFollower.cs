using DG.Tweening;
using metakazz.Hex;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFollower : BoardElement, IMover
{
    private Collider2D _collider;

    [SerializeField]
    private float _moveSpeed = 0.3f;

    [SerializeField]
    BoardElement toFollow;
    
    IMover parentMover;

    public event Action<Vector3Int, Vector3Int> Moved;

    public void Move(Vector3Int to)
    {
        var worldPos = Board.grid.CellToWorld(to);

        Moved?.Invoke(GridPosition, to);

        _collider.enabled = false;
        transform
            .DOMove(worldPos, _moveSpeed)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                GridPosition = to;
                _collider.enabled = true;
            });
    }

    private void OnParentMoved(Vector3Int from, Vector3Int to)
    {
        Move(from);
    }

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        parentMover = toFollow.GetComponent<IMover>();
    }

    protected override void Start()
    {
        base.Start();
        parentMover.Moved += OnParentMoved;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
