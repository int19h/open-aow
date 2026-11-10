// Translated from FireNode.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class TFireNode : TProductionPlace
{
    public TFireNode(TECustomNode owner) : base(owner)
    { }

    // FireNode.pas:36
    public new const int ClassID = 132062;

    // FireNode.pas:39
    public override TMagicSphere GetMagicSphere() => TMagicSphere.msFire;

}

public partial class TFireNodeResource : TProductionPlaceResource
{
    public TFireNodeResource(TECustomNode owner) : base(owner)
    {
        FRazeDefenderBudget = 200;
        FRebuildGold = 50;
        FRebuildDays = 3;
    }

    // FireNode.pas:53
    public new const int ClassID = 132063;

    // FireNode.pas:42

    // FireNode.pas:56
    public override int ResourceUserClassID() => 132062;

    // FireNode.pas:26

    // FireNode.pas:50
    public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TFireNodeResource.FireNodeRStr);

    private static readonly string FireNodeRStr = "Fire Node";

}
