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
        AbilityId = 107;
        AbilityName = AbilityRules.TranslateResourceString(s_possessedText);
        AbilityType = AbilityType.Enchantment;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // AoWAb.PAS:2597
    public override bool Expand(AbilityOwner owner)
    {
        PossessedAbilityData data;
        bool result = false;
        data = (PossessedAbilityData)owner.GetAbilityData(AbilityId);
        if (data == null)
        {
            data = new PossessedAbilityData(null);
            SetAbility(owner, AbilityId, 1);
            owner.AddAbilityData(data);
            result = true;
        }

        return result;
    }

    // AoWAb.PAS:2612
    public override bool Remove(AbilityOwner owner)
    {
        PossessedAbilityData data;
        bool result = true;
        data = (PossessedAbilityData)owner.GetAbilityData(AbilityId);
        if (data != null)
        {
            SetAbility(owner, AbilityId, 0);
            owner.RemoveAbilityData(data);
        }
        else
        {
            SetAbility(owner, AbilityId, 0);
        }

        return result;
    }

    // AoWAb.PAS:2571
    public override void CombatObjectDestroyed(CombatObject combatObject)
    {
        Unit newUnit;
        PossessedAbilityData data;
        var combatUnit = (CombatUnit)combatObject;
        combatUnit.SetStatus(combatUnit.Status - Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed));
        newUnit = new Unit(null);
        newUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(152));
        data = (PossessedAbilityData)combatUnit.Troop.GetAbilityData(107);
        if (data != null)
        {
            newUnit.SetExperience(data.SourceExperience);
        }

        if (combatUnit.Troop.UnitStatus.Contains(UnitStatusElement.Moving))
        {
            newUnit.UnitStatus = Set<UnitStatusElement>.Of(UnitStatusElement.Moving) + newUnit.UnitStatus;
        }

        combatUnit.Troop.Killed();
        combatUnit.SetUnit(newUnit);
        newUnit.Release();
    }

    // AoWAb.PAS:2592
    public override EngineClass AbilityDataClass() => s_possessedDataClass;

    private static EngineClass s_possessedDataClass = EngineClass.Of<PossessedAbilityData>();
    private static readonly string s_possessedText = "Possessed";

}
