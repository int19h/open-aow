// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public class CharmedAbility : CommandedAbility
{
    // AoWAb.PAS:5333
    public CharmedAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 149;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_charmedText);
    }

    private static readonly string s_charmedText = "Charmed";
}
