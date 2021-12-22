using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : BoardElement
{
    public static HexDirection Opposite( HexDirection incident)
    {
        HexDirection output;

        switch(incident)
        {
            case HexDirection.NORTH:
                output = HexDirection.SOUTH;
                break;

            case HexDirection.NORTHEAST:
                output = HexDirection.SOUTHWEST;
                break;

            case HexDirection.SOUTHEAST:
                output = HexDirection.NORTHWEST;
                break;

            case HexDirection.SOUTH:
                output = HexDirection.NORTH;
                break;

            case HexDirection.SOUTHWEST:
                output = HexDirection.NORTHEAST;
                break;

            case HexDirection.NORTHWEST:
                output = HexDirection.SOUTHEAST;
                break;

            default:
                output = incident;
                break;
        }

        return output;
    }
    public static HexDirection Clockwise( HexDirection incident )
    {
        var outputDir = incident + 1;

        int output = ((int)outputDir > 5) ?
            (int)outputDir % 6
            :
            (int)outputDir;

        return (HexDirection)output;
    }
    private static HexDirection CounterClockwise(HexDirection incident)
    {
        var outputDir = incident - 1;

        int output = ((int)outputDir < 0) ?
            Math.Abs((int)outputDir)
            :
            (int)outputDir;

        return (HexDirection)output;
    }
    public static bool IsDiagonal(HexDirection incident)
    {
        return incident.Equals(HexDirection.NORTHWEST) ||
               incident.Equals(HexDirection.SOUTHEAST) ||
               incident.Equals(HexDirection.SOUTHWEST) ||
               incident.Equals(HexDirection.NORTHWEST); ;
    }

    //End tiles are tiles touching the 'endpoints' of the wall
    public readonly List<Tile> endTiles = new List<Tile>();
    
    //Edge tiles are tiles touching the face of the wall
    public readonly List<Tile> edgeTiles = new List<Tile>();

    public readonly Tile anchor;
    [SerializeField]
    private HexDirection direction;

    public HexDirection Direction {
        get => direction;
    }

    private void Start()
    {
        base.Start();

        //Edge tile at the anchor
        InitTile(GridPosition, edgeTiles);

        //Edge tile opposite the anchor
        InitTileNeighbor(
            Direction,
            edgeTiles,
            false
            );

        //End tile clockwise from anchor
        InitTileNeighbor(
            Clockwise(Direction),
            endTiles,
            true
            );
        //End tile counter clockwise from anchor
        InitTileNeighbor(
            CounterClockwise(Direction),
            endTiles,
            true
            );
    }

    void InitTile(Vector3Int tilePos, List<Tile> tileList)
    {
        Tile anchorTile;
        Board.tiles.TryGetValue(tilePos, out anchorTile);
        if (!anchorTile)
            return;

        tileList.Add(anchorTile);
        anchorTile.walls.Add(Direction, this);
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
            anchorTile.walls.Add(Opposite(Direction), this);
    }

}
