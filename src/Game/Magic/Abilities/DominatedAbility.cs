// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public partial class DominatedAbility : CommandedAbility
{
    // AoWAb.PAS:5315
    public DominatedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 150;
        FAbilityName = AbilityRules.TranslateRStr(DominatedRStr);
    }

    private static readonly string DominatedRStr = "Dominated";
}
