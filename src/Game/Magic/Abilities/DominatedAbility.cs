// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public class DominatedAbility : CommandedAbility
{
    // AoWAb.PAS:5315
    public DominatedAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 150;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_dominatedText);
    }

    private static readonly string s_dominatedText = "Dominated";
}
