using System;

namespace metakazz.Hex
{
    public class HexUtil
    {
        public static HexDirection Opposite(HexDirection incident)
        {
            HexDirection output;

            switch (incident)
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
        public static HexDirection Clockwise(HexDirection incident)
        {
            var outputDir = incident + 1;

            int output = ((int)outputDir > 5) ?
                (int)outputDir % 6
                :
                (int)outputDir;

            return (HexDirection)output;
        }
        public static HexDirection CounterClockwise(HexDirection incident)
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
        public static HexVertex Opposite(HexVertex incident)
        {
            HexVertex output;

            switch (incident)
            {
                case HexVertex.NORTHWEST:
                    output = HexVertex.SOUTHEAST;
                    break;

                case HexVertex.NORTHEAST:
                    output = HexVertex.SOUTHWEST;
                    break;

                case HexVertex.EAST:
                    output = HexVertex.WEST;
                    break;

                case HexVertex.SOUTHEAST:
                    output = HexVertex.NORTHWEST;
                    break;

                case HexVertex.SOUTHWEST:
                    output = HexVertex.NORTHEAST;
                    break;

                case HexVertex.WEST:
                    output = HexVertex.EAST;
                    break;

                default:
                    output = incident;
                    break;
            }

            return output;
        }

        /// <summary>
        /// <para>
        /// Returns the vertices of the neighboring hexes that touch the given <paramref name="hexFace"/>
        /// Element [0] is neighbor clockwise relative to the hex face
        /// Element [1] is neighbor counter-clockwise relative to the hex face
        /// </para>
        /// </summary>
        /// <param name="hexFace"></param>
        /// <param name="isClockwiseNeighbor"></param>
        public static HexVertex[] EdgeToVertex(HexDirection hexFace)
        {
            HexVertex[] vertices = new HexVertex[2];
            switch (hexFace)
            {
                case HexDirection.NORTH:
                    vertices = new HexVertex[] { HexVertex.WEST, HexVertex.EAST };
                    break;
                case HexDirection.SOUTH:
                    vertices = new HexVertex[] { HexVertex.EAST, HexVertex.WEST };
                    break;
                case HexDirection.NORTHEAST:
                    vertices = new HexVertex[] { HexVertex.NORTHWEST, HexVertex.SOUTHEAST };
                    break;
                case HexDirection.SOUTHEAST:
                    vertices = new HexVertex[] { HexVertex.NORTHEAST, HexVertex.SOUTHWEST };
                    break;
                case HexDirection.NORTHWEST:
                    vertices = new HexVertex[] { HexVertex.SOUTHWEST, HexVertex.NORTHEAST };
                    break;
                case HexDirection.SOUTHWEST:
                    vertices = new HexVertex[] { HexVertex.SOUTHEAST, HexVertex.NORTHWEST };
                    break;
            }

            return vertices;
        }
    }
}