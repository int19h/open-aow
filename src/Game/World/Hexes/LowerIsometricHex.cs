// Translated from IsoHex.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Hexes;

public partial class LowerIsometricHex(CustomNode owner) : IsometricHex(owner)
{
    public new const int ClassId = 131_442;

    public new class Resource(CustomNode owner) : IsometricHexResource(owner)
    {
        public new const int ClassId = 131_443;

        // IsoHex.pas:581
        public override int ResourceUserClassId() => LowerIsometricHex.ClassId;

        // IsoHex.pas:576
        public override string ResourceEditName() => s_lowerIsometricHexText;

        private static readonly string s_lowerIsometricHexText = "Lower Isometric Hexagon";

    }
}
