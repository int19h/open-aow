// Translated from WorkShop.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Terrain;

namespace Aow.Game.World.Terrain;

public partial class TWorkshop : TCombatStructure
{
    public TWorkshop(TECustomNode owner) : base(owner)
    { }

    // WorkShop.pas:36
    public new const int ClassID = 2229512;

}

public partial class TWorkshopResource : TCombatStructureResource
{
    public TWorkshopResource(TECustomNode owner) : base(owner)
    { }

    // WorkShop.pas:39
    public new const int ClassID = 2229513;

    // WorkShop.pas:45
    public override int ResourceUserClassID() => 2229512;

    // WorkShop.pas:42
    public override string ResourceEditName() => "Combat Workshop";

}
