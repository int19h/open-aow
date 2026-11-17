// Translated from HSEngine.pas.
using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// { State of a walk around a centre hexagon (InitHNWalk, WalkToNextHN): the hexagon numbers
//     ring after ring, 6 * Radius hexagons on ring Radius }
public struct HexNumberWalkData
{
    public int Side { get; private set; }
    public int Step { get; private set; }
    public int HexNumber { get; private set; }
    public int Radius { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public int CenterX { get; internal set; }
    public int CenterY { get; internal set; }

    internal static void WalkToNextHexNumber(ref HexNumberWalkData walk)
    {
        walk.HexNumber++;
        int parity = walk.X & 1;
        int side = walk.Side;
        walk.Step++;
        if (walk.Step < walk.Radius || side != 5)
        {
            walk.X += HexGeometry.HexNumberWalkTable[parity][side].X;
            walk.Y += HexGeometry.HexNumberWalkTable[parity][side].Y;
            if (walk.Step >= walk.Radius)
            {
                walk.Side = side + 1;
                walk.Step = 0;
            }
        }
        else
        {
            walk.Radius++;
            walk.Side = 0;
            walk.Step = 0;
            walk.X = walk.CenterX;
            walk.Y = walk.CenterY - walk.Radius;
        }
    }

    internal static void InitializeHexNumberWalk(out HexNumberWalkData walk, int hexX, int hexY, int radius)
    {
        // The out value is assigned before its members are written.
        walk = default;
        if (radius == 0)
        {
            walk.HexNumber = 0;
            walk.Side = 5;
        }
        else
        {
            walk.HexNumber = ((radius - 1) * radius * 3) + 1;
            walk.Side = 0;
        }

        walk.CenterX = hexX;
        walk.CenterY = hexY;
        walk.Radius = radius;
        walk.X = hexX;
        walk.Y = hexY - radius;
        walk.Step = 0;
    }

}
