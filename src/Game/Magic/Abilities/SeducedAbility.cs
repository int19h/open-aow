// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public partial class SeducedAbility : CommandedAbility
{
    // AoWAb.PAS:5366
    public SeducedAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 48;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_seducedText);
    }

    private static readonly string s_seducedText = "Seduced";
}
