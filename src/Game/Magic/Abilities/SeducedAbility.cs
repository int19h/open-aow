// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public class SeducedAbility : CommandedAbility
{
    // AoWAb.PAS:5366
    public SeducedAbility(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        AbilityId = 48;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_seducedText);
    }

    private static readonly string s_seducedText = "Seduced";
}
