// Translated from Ruin.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class TRuin : TItemExplorationSite
{
    public TRuin(TECustomNode owner) : base(owner)
    { }

    // Ruin.pas:34
    public new const int ClassID = 131994;

}

public partial class TRuinResource : TItemExplorationSiteResource
{
    public TRuinResource(TECustomNode owner) : base(owner)
    { }

    // Ruin.pas:40
    public new const int ClassID = 131995;

    // Ruin.pas:43
    public override int ResourceUserClassID() => 131994;

    // Ruin.pas:37
    public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TRuinResource.CastleRuinRStr);

    private static readonly string CastleRuinRStr = "Castle Ruin";

}
