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
        FAbilityType = AbilityType.Handicap;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // AoWAb.PAS:5023
    public override bool Remove(AbilityOwner owner)
    {
        bool Result = base.Remove(owner);
        if (Result)
        {
            owner.RemoveAbilityDataID(FAbilityID);
        }

        return Result;
    }

    // AoWAb.PAS:5032
    public override void CombatObjectDestroyed(CombatObject CombatObject)
    {
        CommandedAbilityData Data;
        Data = (CommandedAbilityData)CombatObject.GetAbilityOwner().GetAbilityData(FAbilityID);
        if (Data != null)
        {
            ((CommandAbility)AoWHSSet.Current.FAbilityControl.GetAbility(Data.FCommandAbilityID)).Uncommand((CombatUnit)CombatObject);
        }
    }

    // AoWAb.PAS:5028
    public override void CombatDone(CombatObject CombatObject)
    {
        Remove(CombatObject.GetAbilityOwner());
    }

    // AoWAb.PAS:5019
    public override EngineClass AbilityDataClass() => P4CommandedDataClass;

    private static EngineClass P4CommandedDataClass = EngineClass.Of<CommandedAbilityData>();

}
