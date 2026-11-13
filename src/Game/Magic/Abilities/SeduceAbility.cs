// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class SeduceAbility : CommandAbility
{
    // AoWAb.PAS:5372
    public SeduceAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 29;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_seduceText);
        CommandedAbilityId = 48;
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
    }

    // AoWAb.PAS:5380
    public override sbyte GetTouchAttack(int level) => 4;

    // AoWAb.PAS:5389
    public override bool CanTouch(CombatObject source, CombatObject target)
    {
        bool result = base.CanTouch(source, target);
        if (result)
        {
            var combatUnit = (CombatUnit)target;
            result = combatUnit.Troop.GetGender() == UnitGender.Male && combatUnit.Troop.GetUnitType() == UnitType.Humanoid;
        }

        return result;
    }

    // AoWAb.PAS:5384
    public override bool CanTouchUnit(Troop source, Troop target) => base.CanTouchUnit(source, target) && (target.GetGender() == UnitGender.Male && target.GetUnitType() == UnitType.Humanoid);

    private static readonly string s_seduceText = "Seduce";
}
