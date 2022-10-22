using System;

namespace metakazz.Hex
{
    public enum HexDirection
    {
        NORTH = 0, NORTHEAST = 1, SOUTHEAST = 2, SOUTH = 3, SOUTHWEST = 4, NORTHWEST = 5, NONE = 6
    }

    public enum HexVertex
    {
        NORTHWEST, NORTHEAST, EAST, SOUTHEAST, SOUTHWEST, WEST
    }
}