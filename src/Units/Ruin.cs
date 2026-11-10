// Translated from Ruin.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;
using static Aow.Game.Magic.Abilities.AoWEUnit;

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
    public override string ResourceEditName() => TranslateRStr(CastleRuinRStr);

}

public static partial class RuinUnit
{
    // Ruin.pas:46
    public static void RegisterRuin(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TRuin>(), EClass.Of<TRuinResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
