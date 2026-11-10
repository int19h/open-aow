// Translated from WorkShop.pas.
using Aow.Engine.Nodes;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Terrain;

namespace Aow.Game.World.Terrain;

public partial class Workshop : CombatStructure
{
    public Workshop(ECustomNode owner) : base(owner)
    { }

    // WorkShop.pas:36
    public new const int ClassID = 2229512;

    public new partial class Resource : CombatStructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // WorkShop.pas:39
        public new const int ClassID = 2229513;

        // WorkShop.pas:45
        public override int ResourceUserClassID() => 2229512;

        // WorkShop.pas:42
        public override string ResourceEditName() => "Combat Workshop";

    }

}
