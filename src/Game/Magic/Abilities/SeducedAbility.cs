// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public partial class SeducedAbility : CommandedAbility
{
    // AoWAb.PAS:5366
    public SeducedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 48;
        FAbilityName = AbilityRules.TranslateRStr(SeducedRStr);
    }

    private static readonly string SeducedRStr = "Seduced";
}
