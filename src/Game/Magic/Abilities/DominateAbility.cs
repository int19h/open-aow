// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class DominateAbility : CommandAbility
{
    // AoWAb.PAS:5321
    public DominateAbility(ECustomNode owner) : base(owner)
    {
        AbilityId = 28;
        AbilityName = AbilityRules.TranslateResourceString(s_dominateText);
        CommandedAbilityId = 150;
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
    }

    // AoWAb.PAS:5329
    public override sbyte GetTouchAttack(int level) => 6;

    private static readonly string s_dominateText = "Dominate";
}
