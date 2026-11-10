// Translated from MainStructure.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Terrain;

namespace Aow.Game.World.Terrain;

public partial class MainStructure : CombatStructure
{
    public MainStructure(ECustomNode owner) : base(owner)
    { }

    // MainStructure.pas:37
    public new const int ClassID = 2229532;

    // MainStructure.pas:40
    public override int GetTerrainTypeImage(int TerrainType) => TerrainType;

    public new partial class Resource : CombatStructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // MainStructure.pas:43
        public new const int ClassID = 2229533;

        // MainStructure.pas:49
        public override int ResourceUserClassID() => 2229532;

        // MainStructure.pas:46
        public override string ResourceEditName() => "Combat Main Structure";

    }

}
