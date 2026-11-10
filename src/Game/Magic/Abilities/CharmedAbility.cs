// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public partial class CharmedAbility : CommandedAbility
{
    // AoWAb.PAS:5333
    public CharmedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 149;
        FAbilityName = AbilityRules.TranslateRStr(CharmedRStr);
    }

    private static readonly string CharmedRStr = "Charmed";
}
