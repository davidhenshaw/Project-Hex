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

    public readonly List<BoardElement> anchors = new List<BoardElement>();
    [SerializeField]
    public readonly HexDirection direction;

    private void Start()
    {
        base.Start();

        Tile anchor;
        Board.tiles.TryGetValue(GridPosition, out anchor);
        anchors.Add(anchor);
        anchor.Add(this);

        Board.tiles.TryGetValue(GridPosition.Neighbor(direction), out anchor);
        anchors.Add(anchor);
        anchor.Add(this);
    }
}
