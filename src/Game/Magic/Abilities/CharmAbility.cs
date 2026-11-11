// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities;

public partial class CharmAbility : CommandAbility
{
    // AoWAb.PAS:5339
    public CharmAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 148;
        FAbilityName = AbilityRules.TranslateRStr(CharmRStr);
        FCommandedAbilityID = 149;
    }

    // AoWAb.PAS:5346
    public override sbyte GetTouchAttack(int Level) => 5;

    // AoWAb.PAS:5355
    public override bool CanTouch(CombatObject Source, CombatObject Target)
    {
        bool Result = base.CanTouch(Source, Target);
        if (Result)
        {
            var combatUnit = (CombatUnit)Target;
            Result = combatUnit.FUnit.GetUnitType() == UnitType.Humanoid && (ushort)combatUnit.FUnit.GetRace() != 11 && !combatUnit.FUnit.GetAbilityEnabled(129);
        }

        return Result;
    }

    // AoWAb.PAS:5350
    public override bool CanTouchUnit(Troop Source, Troop Target) => base.CanTouchUnit(Source, Target) && (Target.GetUnitType() == UnitType.Humanoid && (ushort)Target.GetRace() != 11 && !Target.GetAbilityEnabled(129));

    private static readonly string CharmRStr = "Charm";
}
