// Translated from IsoHex.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Hexes;

public partial class LowerIsometricHex : IsometricHex
{
    public LowerIsometricHex(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131442;

    public new partial class Resource : IsometricHexResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        public new const int ClassId = 131443;

        // IsoHex.pas:581
        public override int ResourceUserClassId() => 131442;

        // IsoHex.pas:576
        public override string ResourceEditName() => s_lowerIsometricHexText;

        private static readonly string s_lowerIsometricHexText = "Lower Isometric Hexagon";

    }

}
