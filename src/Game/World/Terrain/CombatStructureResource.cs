// Translated from CombatStructure.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Terrain;

public partial class CombatStructureResource : CombatFixedImageLibraryTerrain.Resource
{
    public CombatStructureResource(ECustomNode owner) : base(owner)
    { }

    // CombatStructure.pas:256
    public new const int ClassId = 2229511;

    // CombatStructure.pas:262
    public override int ResourceUserClassId() => 2229510;

    // CombatStructure.pas:259
    public override string ResourceEditName() => "Combat Structure";

}
