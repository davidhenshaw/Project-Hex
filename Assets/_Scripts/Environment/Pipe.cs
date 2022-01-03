using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using metakazz.Hex;

public class Pipe : BoardElement
{
    //End tiles are tiles touching the vertex of the pipe
    public readonly List<Tile> vertexTiles = new List<Tile>();
    
    //Edge tiles are tiles touching the face of the pipe
    public readonly List<Tile> faceTiles = new List<Tile>();

    Tile _anchor;

    [SerializeField]
    private HexDirection _direction;

    public HexDirection FaceDirection {
        get => _direction;
    }

    protected override void Start()
    {
        base.Start();

        Board.tiles.TryGetValue(GridPosition, out _anchor);

        InitFaceNeighbors();
        InitVertexNeighbors();
    }

    void InitFaceNeighbors()
    {
        Tile anchorTile;

        //Initialize anchor tile
        Board.tiles.TryGetValue(GridPosition, out anchorTile);
        if (anchorTile)
        {
            faceTiles.Add(anchorTile);

            anchorTile
                .pipes
                .Add(FaceDirection, this);
        }

        anchorTile = null;

        //Initialize neighbor to anchor tile
        var anchorPosition = GridPosition.Neighbor(FaceDirection);
        Board.tiles.TryGetValue(anchorPosition, out anchorTile);
        if (anchorTile)
        {
            faceTiles.Add(anchorTile);

            anchorTile
                .pipes
                .Add(HexUtil.Opposite(FaceDirection), this);
        }
    }

    void RemoveFaceNeighbors()
    {
        Tile anchorTile;

        //Initialize anchor tile
        Board.tiles.TryGetValue(GridPosition, out anchorTile);
        if (anchorTile)
        {
            faceTiles.Remove(anchorTile);
            anchorTile
                .pipes
                .Remove(FaceDirection);
        }

        anchorTile = null;

        //Initialize neighbor to anchor tile
        var anchorPosition = GridPosition.Neighbor(FaceDirection);
        Board.tiles.TryGetValue(anchorPosition, out anchorTile);
        if (anchorTile)
        {
            faceTiles.Remove(anchorTile);

            anchorTile
                .pipes
                .Remove(HexUtil.Opposite(FaceDirection));
        }
    }

    void InitVertexNeighbors()
    {
        Tile neighborTile;
        var neighborVerts = HexUtil.EdgeToVertex(FaceDirection);

        //---CLOCKWISE NEIGHBOR---

        HexDirection neighborDir = HexUtil.Clockwise(FaceDirection);
        var neighborPos = GridPosition.Neighbor(neighborDir);

        Board.tiles.TryGetValue(neighborPos, out neighborTile);
        if (neighborTile)
        {
            vertexTiles.Add(neighborTile);
            //Inform the tile which vertex this pipe is touching
            neighborTile
                .vertexPipes
                .Add(neighborVerts[0], this);
        }

        neighborTile = null;

        //---COUNTER CLOCKWISE NEIGHBOR---

        neighborDir = HexUtil.CounterClockwise(FaceDirection);
        neighborPos = GridPosition.Neighbor(neighborDir);

        Board.tiles.TryGetValue(neighborPos, out neighborTile);
        if (neighborTile)
        {
            vertexTiles.Add(neighborTile);

            //Inform the tile which vertex this pipe is touching
            neighborTile
                .vertexPipes
                .Add(neighborVerts[1], this);
        }

    }

    void RemoveVertexNeighbors()
    {
        Tile neighborTile;
        var neighborVerts = HexUtil.EdgeToVertex(FaceDirection);

        //---CLOCKWISE NEIGHBOR---

        HexDirection neighborDir = HexUtil.Clockwise(FaceDirection);
        var neighborPos = GridPosition.Neighbor(neighborDir);

        Board.tiles.TryGetValue(neighborPos, out neighborTile);
        if (neighborTile)
        {
            vertexTiles.Remove(neighborTile);
            //Inform the tile which vertex this pipe is touching
            neighborTile
                .vertexPipes
                .Remove(neighborVerts[0]);
        }

        neighborTile = null;

        //---COUNTER CLOCKWISE NEIGHBOR---

        neighborDir = HexUtil.CounterClockwise(FaceDirection);
        neighborPos = GridPosition.Neighbor(neighborDir);

        Board.tiles.TryGetValue(neighborPos, out neighborTile);
        if (neighborTile)
        {
            vertexTiles.Remove(neighborTile);

            //Inform the tile which vertex this pipe is touching
            neighborTile
                .vertexPipes
                .Remove(neighborVerts[1]);
        }

    }

    void InvertAnchorTile(Tile newAnchor)
    {
        _direction = HexUtil.Opposite(_direction);
        GridPosition = newAnchor.GridPosition;
        transform.Rotate(0, 0, 180);
        _anchor = newAnchor;
    }

    public void PreRotate()
    {
        RemoveFaceNeighbors();
        RemoveVertexNeighbors();        
    }

    public void PostRotate()
    {
        InitFaceNeighbors();
        InitVertexNeighbors();
    }

    public void RotateClockwise(Tile anchorTile)
    {
        if (_anchor != anchorTile)
            InvertAnchorTile(anchorTile);

        _direction = HexUtil.Clockwise(_direction);
        transform.Rotate(0, 0, -60);
    }

    public void RotateCounterClockwise(Tile anchorTile)
    {
        if(_anchor != anchorTile)
            InvertAnchorTile(anchorTile);

        _direction = HexUtil.CounterClockwise(_direction);
        transform.Rotate(0, 0, 60);
    }
}
