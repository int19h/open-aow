// Translated from EarthNode.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class TEarthNode : TProductionPlace
{
    public TEarthNode(TECustomNode owner) : base(owner)
    { }

    // EarthNode.pas:36
    public new const int ClassID = 132302;

    // EarthNode.pas:39
    public override TMagicSphere GetMagicSphere() => TMagicSphere.msEarth;

}

public partial class TEarthNodeResource : TProductionPlaceResource
{
    public TEarthNodeResource(TECustomNode owner) : base(owner)
    {
        FRazeDefenderBudget = 200;
        FRebuildGold = 35;
        FRebuildDays = 2;
    }

    // EarthNode.pas:53
    public new const int ClassID = 132303;

    // EarthNode.pas:42

    // EarthNode.pas:56
    public override int ResourceUserClassID() => 132302;

    // EarthNode.pas:26

    // EarthNode.pas:50
    public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TEarthNodeResource.EarthNodeRStr);

    private static readonly string EarthNodeRStr = "Earth Node";

}
