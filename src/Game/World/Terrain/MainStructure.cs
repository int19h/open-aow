// Translated from MainStructure.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Terrain;

public partial class MainStructure : CombatStructure
{
    public MainStructure(ECustomNode owner) : base(owner)
    { }

    // MainStructure.pas:37
    public new const int ClassId = 2229532;

    // MainStructure.pas:40
    public override int GetTerrainTypeImage(int terrainType) => terrainType;

    public new partial class Resource : CombatStructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // MainStructure.pas:43
        public new const int ClassId = 2229533;

        // MainStructure.pas:49
        public override int ResourceUserClassId() => 2229532;

        // MainStructure.pas:46
        public override string ResourceEditName() => "Combat Main Structure";

    }

}
