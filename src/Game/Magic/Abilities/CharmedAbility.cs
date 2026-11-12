// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public partial class CharmedAbility : CommandedAbility
{
    // AoWAb.PAS:5333
    public CharmedAbility(ECustomNode owner) : base(owner)
    {
        AbilityId = 149;
        AbilityName = AbilityRules.TranslateResourceString(s_charmedText);
    }

    private static readonly string s_charmedText = "Charmed";
}
