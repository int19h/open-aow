// Translated from Ruin.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.ItemExpS;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Ruin;

public partial class TRuin : TItemExplorationSite
{
    public TRuin(TECustomNode owner) : base(owner)
    {}

    // Ruin.pas:34
    public new const int ClassID = 131994;

}

public partial class TRuinResource : TItemExplorationSiteResource
{
    public TRuinResource(TECustomNode owner) : base(owner)
    {}

    // Ruin.pas:40
    public new const int ClassID = 131995;

    // Ruin.pas:43
    public override int ResourceUserClassID() => 131994;

    // Ruin.pas:37
    public override string ResourceEditName() => TranslateRStr(CastleRuinRStr);

}

public static partial class Ruin
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
