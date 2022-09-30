using System;

namespace metakazz.Hex
{
    public class HexUtil
    {
        public static HexDirection Opposite(HexDirection incident)
        {
            var output = incident switch
            {
                HexDirection.NORTH => HexDirection.SOUTH,
                HexDirection.NORTHEAST => HexDirection.SOUTHWEST,
                HexDirection.SOUTHEAST => HexDirection.NORTHWEST,
                HexDirection.SOUTH => HexDirection.NORTH,
                HexDirection.SOUTHWEST => HexDirection.NORTHEAST,
                HexDirection.NORTHWEST => HexDirection.SOUTHEAST,
                _ => incident,
            };
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
                6 - Math.Abs((int)outputDir)
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
            var output = incident switch
            {
                HexVertex.NORTHWEST => HexVertex.SOUTHEAST,
                HexVertex.NORTHEAST => HexVertex.SOUTHWEST,
                HexVertex.EAST => HexVertex.WEST,
                HexVertex.SOUTHEAST => HexVertex.NORTHWEST,
                HexVertex.SOUTHWEST => HexVertex.NORTHEAST,
                HexVertex.WEST => HexVertex.EAST,
                _ => incident,
            };
            return output;
        }
        
        /// <summary>
        /// Converts Hex Direction to an angle in degrees (clockwise). 0 degrees is the same as Vector2.Up
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static float ToAngle(HexDirection dir)
        {
            return (int)dir * -60.0f;
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