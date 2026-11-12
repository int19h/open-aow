// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class CommandedAbility : AdjustableAbility
{
    // AoWAb.PAS:5013
    public CommandedAbility(ECustomNode owner) : base(owner)
    {
        AbilityType = AbilityType.Handicap;
        SelectionTypes = Set<AbilitySelectionType>.Of();
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
        CommandedAbilityData data;
        data = (CommandedAbilityData)combatObject.GetAbilityOwner().GetAbilityData(AbilityId);
        if (data != null)
        {
            ((CommandAbility)AoWHSSet.Current.AbilityControl.GetAbility(data.CommandAbilityId)).Uncommand((CombatUnit)combatObject);
        }
    }

    // AoWAb.PAS:5028
    public override void CombatDone(CombatObject combatObject)
    {
        Remove(combatObject.GetAbilityOwner());
    }

    // AoWAb.PAS:5019
    public override EngineClass AbilityDataClass() => s_commandedDataClass;

    private static EngineClass s_commandedDataClass = EngineClass.Of<CommandedAbilityData>();

}
