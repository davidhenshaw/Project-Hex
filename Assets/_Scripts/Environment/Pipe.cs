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

    public readonly Tile anchor;
    [SerializeField]
    private HexDirection direction;

    public HexDirection FaceDirection {
        get => direction;
    }

    protected override void Start()
    {
        base.Start();

        //Edge tile at the anchor
        InitTile(GridPosition, faceTiles);

        //Edge tile opposite the pipe
        InitTileNeighbor(
            FaceDirection,
            faceTiles,
            false
            );

        InitVertexNeighbors();
    }

    void InitTile(Vector3Int tilePos, List<Tile> tileList)
    {
        Tile anchorTile;
        Board.tiles.TryGetValue(tilePos, out anchorTile);
        if (!anchorTile)
            return;

        tileList.Add(anchorTile);
        anchorTile.pipes.Add(FaceDirection, this);
    }

    void InitTileNeighbor(HexDirection neighborDir, List<Tile> tileList, bool isCorner)
    {
        Tile anchorTile;
        var anchorPosition = GridPosition.Neighbor(neighborDir);
        Board.tiles.TryGetValue(anchorPosition, out anchorTile);
        if (!anchorTile)
            return;

        tileList.Add(anchorTile);
        if(!isCorner)
            anchorTile.pipes.Add(HexUtil.Opposite(FaceDirection), this); 
    }

    void InitVertexNeighbors()
    {
        Tile anchorTile;
        var neighborVerts = HexUtil.EdgeToVertex(FaceDirection);

        //---CLOCKWISE NEIGHBOR---

        HexDirection neighborDir = HexUtil.Clockwise(FaceDirection);
        var tilePos = GridPosition.Neighbor(neighborDir);

        Board.tiles.TryGetValue(tilePos, out anchorTile);
        if (anchorTile)
        {
            vertexTiles.Add(anchorTile);
            //Inform the tile which vertex this pipe is touching
            anchorTile
                .vertexPipes
                .Add(neighborVerts[0], this);
        }

        //---COUNTER CLOCKWISE NEIGHBOR---

        neighborDir = HexUtil.CounterClockwise(FaceDirection);
        tilePos = GridPosition.Neighbor(neighborDir);

        Board.tiles.TryGetValue(tilePos, out anchorTile);
        if (anchorTile)
        {
            vertexTiles.Add(anchorTile);

            //Inform the tile which vertex this pipe is touching
            anchorTile
                .vertexPipes
                .Add(neighborVerts[1], this);
        }

    }

}
