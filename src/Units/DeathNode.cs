// Translated from DeathNode.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class TDeathNode : TProductionPlace
{
    public TDeathNode(TECustomNode owner) : base(owner)
    { }

    // DeathNode.pas:37
    public new const int ClassID = 132270;

    // DeathNode.pas:40
    public override TMagicSphere GetMagicSphere() => TMagicSphere.msDeath;

}

public partial class TDeathNodeResource : TProductionPlaceResource
{
    public TDeathNodeResource(TECustomNode owner) : base(owner)
    {
        FRazeDefenderBudget = 200;
        FRebuildGold = 25;
        FRebuildDays = 2;
        FTerrainBound = false;
    }

    // DeathNode.pas:55
    public new const int ClassID = 132271;

    // DeathNode.pas:43

    // DeathNode.pas:58
    public override int ResourceUserClassID() => 132270;

    // DeathNode.pas:27

    // DeathNode.pas:52
    public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TDeathNodeResource.DeathNodeRStr);

    private static readonly string DeathNodeRStr = "Death Node";

}
