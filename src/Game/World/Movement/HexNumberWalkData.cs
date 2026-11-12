// Translated from HSEngine.pas.
using System.Runtime.InteropServices;

using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// { State of a walk around a centre hexagon (InitHNWalk, WalkToNextHN): the hexagon numbers
//     ring after ring, 6 * Radius hexagons on ring Radius }
[StructLayout(LayoutKind.Explicit, Size = 32)]
public struct HexNumberWalkData
{
    [FieldOffset(0)]
    public int Side;
    [FieldOffset(4)]
    public int Step;
    [FieldOffset(8)]
    public int HexNumber;
    [FieldOffset(12)]
    public int Radius;
    [FieldOffset(16)]
    public int X;
    [FieldOffset(20)]
    public int Y;
    [FieldOffset(24)]
    public int CenterX;
    [FieldOffset(28)]
    public int CenterY;

    internal static void WalkToNextHexNumber(ref HexNumberWalkData walk)
    {
        walk.HexNumber = walk.HexNumber + 1;
        int parity = walk.X & 1;
        int side = walk.Side;
        walk.Step = walk.Step + 1;
        if (walk.Step < walk.Radius || side != 5)
        {
            walk.X = walk.X + HexGeometry.HexNumberWalkTable[parity][side].X;
            walk.Y = walk.Y + HexGeometry.HexNumberWalkTable[parity][side].Y;
            if (walk.Step >= walk.Radius)
            {
                walk.Side = side + 1;
                walk.Step = 0;
            }
        }
        else
        {
            walk.Radius = walk.Radius + 1;
            walk.Side = 0;
            walk.Step = 0;
            walk.X = walk.CenterX;
            walk.Y = walk.CenterY - walk.Radius;
        }
    }

    internal static void InitializeHexNumberWalk(ref HexNumberWalkData walk, int hexX, int hexY, int radius)
    {
        if (radius == 0)
        {
            walk.HexNumber = 0;
            walk.Side = 5;
        }
        else
        {
            walk.HexNumber = (radius - 1) * radius * 3 + 1;
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
