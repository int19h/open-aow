// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public partial class DominatedAbility : CommandedAbility
{
    // AoWAb.PAS:5315
    public DominatedAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 150;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_dominatedText);
    }

    private static readonly string s_dominatedText = "Dominated";
}
