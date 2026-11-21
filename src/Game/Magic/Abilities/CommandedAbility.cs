#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities;

public class CommandedAbility : AdjustableAbility
{
    // AoWAb.PAS:5013
    public CommandedAbility(CustomNode owner) : base(owner)
    {
        AbilityType = AbilityType.Handicap;
        SelectionTypes = AbilitySelectionTypes.None;
    }

    // AoWAb.PAS:5023
    public override bool Remove(AbilityOwner owner)
    {
        bool result = base.Remove(owner);
        if (result)
        {
            owner.RemoveAbilityDataId(AbilityId);
        }

        return result;
    }

    // AoWAb.PAS:5032
    public override void CombatObjectDestroyed(CombatObject combatObject)
    {
        CommandedAbilityData? data = GetData(combatObject.AbilityOwner);
        if (data is not null && combatObject is CombatUnit combatUnit)
        {
            GamePack.Current.AbilityControl.GetAbility<CommandAbility>(data.CommandAbilityId).Uncommand(combatUnit);
        }
    }

    // AoWAb.PAS:5028
    public override void CombatDone(CombatObject combatObject) => Remove(combatObject.AbilityOwner);

    // AoWAb.PAS:5019
    public override EngineClass AbilityDataClass() => s_commandedDataClass;

    public CommandedAbilityData? GetData(AbilityOwner owner) => owner.GetAbilityData<CommandedAbilityData>(AbilityId);

    private static readonly EngineClass s_commandedDataClass = EngineClass.Of<CommandedAbilityData>();

}
