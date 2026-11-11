// Translated from IsoHex.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Hexes;

public partial class LowerIsometricHex : IsometricHex
{
    public LowerIsometricHex(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 131442;

    public new partial class Resource : IsometricHexResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 131443;

        // IsoHex.pas:581
        public override int ResourceUserClassID() => 131442;

        // IsoHex.pas:576
        public override string ResourceEditName() => LowerIsometricHexagonStr;

        private static readonly string LowerIsometricHexagonStr = "Lower Isometric Hexagon";

    }

}
