// Translated from Townhall.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Terrain;

namespace Aow.Game.World.Terrain;

public partial class TTownhall : TCombatStructure
{
    public TTownhall(TECustomNode owner) : base(owner)
    { }

    // Townhall.pas:36
    public new const int ClassID = 2229520;

}

public partial class TTownhallResource : TCombatStructureResource
{
    public TTownhallResource(TECustomNode owner) : base(owner)
    { }

    // Townhall.pas:39
    public new const int ClassID = 2229521;

    // Townhall.pas:45
    public override int ResourceUserClassID() => 2229520;

    // Townhall.pas:42
    public override string ResourceEditName() => "Combat Townhall";

}
