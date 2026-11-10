// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class DominateAbility : CommandAbility
{
    // AoWAb.PAS:5321
    public DominateAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 28;
        FAbilityName = AbilityRules.TranslateRStr(DominateRStr);
        FCommandedAbilityID = 150;
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
    }

    // AoWAb.PAS:5329
    public override sbyte GetTouchAttack(int Level) => 6;

    private static readonly string DominateRStr = "Dominate";
}
