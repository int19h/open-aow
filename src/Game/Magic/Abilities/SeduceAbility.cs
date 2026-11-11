// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class SeduceAbility : CommandAbility
{
    // AoWAb.PAS:5372
    public SeduceAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 29;
        FAbilityName = AbilityRules.TranslateRStr(SeduceRStr);
        FCommandedAbilityID = 48;
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
    }

    // AoWAb.PAS:5380
    public override sbyte GetTouchAttack(int Level) => 4;

    // AoWAb.PAS:5389
    public override bool CanTouch(CombatObject Source, CombatObject Target)
    {
        bool Result = base.CanTouch(Source, Target);
        if (Result)
        {
            var combatUnit = (CombatUnit)Target;
            Result = combatUnit.FUnit.GetGender() == UnitGender.Male && combatUnit.FUnit.GetUnitType() == UnitType.Humanoid;
        }

        return Result;
    }

    // AoWAb.PAS:5384
    public override bool CanTouchUnit(Troop Source, Troop Target) => base.CanTouchUnit(Source, Target) && (Target.GetGender() == UnitGender.Male && Target.GetUnitType() == UnitType.Humanoid);

    private static readonly string SeduceRStr = "Seduce";
}
