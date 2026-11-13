// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public partial class CharmedAbility : CommandedAbility
{
    // AoWAb.PAS:5333
    public CharmedAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 149;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_charmedText);
    }

    private static readonly string s_charmedText = "Charmed";
}
