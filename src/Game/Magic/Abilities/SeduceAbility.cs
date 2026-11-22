#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities;

public class SeduceAbility : CommandAbility
{
    // AoWAb.PAS:5372
    public SeduceAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 29;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_seduceText);
        CommandedAbilityId = 48;
        SelectionTypes = AbilitySelectionTypes.Unit;
    }

    // AoWAb.PAS:5380
    public override int GetTouchAttack(int level) => 4;

    // AoWAb.PAS:5389
    public override bool CanTouch(CombatObject source, CombatObject target) => base.CanTouch(source, target) && target is CombatUnit combatUnit && combatUnit.Troop.Gender == UnitGender.Male && combatUnit.Troop.GetUnitType() == UnitType.Humanoid;

    // AoWAb.PAS:5384
    public override bool CanTouchUnit(Troop source, Troop target) => base.CanTouchUnit(source, target) && target.Gender == UnitGender.Male && target.GetUnitType() == UnitType.Humanoid;

    private static readonly string s_seduceText = "Seduce";
}
