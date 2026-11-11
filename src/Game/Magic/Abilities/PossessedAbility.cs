// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class PossessedAbility : PossessAbility
{
    // AoWAb.PAS:2562
    public PossessedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 107;
        FAbilityName = AbilityRules.TranslateRStr(PossessedRStr);
        FAbilityType = AbilityType.Enchantment;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // AoWAb.PAS:2597
    public override bool Expand(AbilityOwner Owner)
    {
        PossessedAbilityData Data;
        bool Result = false;
        Data = (PossessedAbilityData)Owner.GetAbilityData(FAbilityID);
        if (Data == null)
        {
            Data = new PossessedAbilityData(null);
            SetAb(Owner, FAbilityID, 1);
            Owner.AddAbilityData(Data);
            Result = true;
        }

        return Result;
    }

    // AoWAb.PAS:2612
    public override bool Remove(AbilityOwner owner)
    {
        PossessedAbilityData Data;
        bool Result = true;
        Data = (PossessedAbilityData)owner.GetAbilityData(FAbilityID);
        if (Data != null)
        {
            SetAb(owner, FAbilityID, 0);
            owner.RemoveAbilityData(Data);
        }
        else
        {
            SetAb(owner, FAbilityID, 0);
        }

        return Result;
    }

    // AoWAb.PAS:2571
    public override void CombatObjectDestroyed(CombatObject CombatObject)
    {
        Unit NewUnit;
        PossessedAbilityData Data;
        var combatUnit = (CombatUnit)CombatObject;
        combatUnit.SetStatus(combatUnit.FStatus - Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed));
        NewUnit = new Unit(null);
        NewUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(152));
        Data = (PossessedAbilityData)combatUnit.FUnit.GetAbilityData(107);
        if (Data != null)
        {
            NewUnit.SetExperience(Data.FSourceExperience);
        }

        if (combatUnit.FUnit.FUnitStatus.Contains(UnitStatusElement.Moving))
        {
            NewUnit.FUnitStatus = Set<UnitStatusElement>.Of(UnitStatusElement.Moving) + NewUnit.FUnitStatus;
        }

        combatUnit.FUnit.Killed();
        combatUnit.SetUnit(NewUnit);
        NewUnit.Release();
    }

    // AoWAb.PAS:2592
    public override EngineClass AbilityDataClass() => P4PossessedDataClass;

    private static EngineClass P4PossessedDataClass = EngineClass.Of<PossessedAbilityData>();
    private static readonly string PossessedRStr = "Possessed";

}
