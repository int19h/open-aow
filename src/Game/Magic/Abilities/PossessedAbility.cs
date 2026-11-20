#nullable disable
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
    public PossessedAbility(CustomNode owner, EngineObject holder) : base(owner, holder)
    {
        AbilityId = 107;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_possessedText);
        AbilityType = AbilityType.Enchantment;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // AoWAb.PAS:2597
    public override bool Expand(AbilityOwner owner)
    {
        PossessedAbilityData data = GetData(owner);
        if (data is null)
        {
            data = new PossessedAbilityData(null) { Holder = CreationHolder() };
            SetAbility(owner, AbilityId, 1);
            owner.AddAbilityData(data);
            return true;
        }

        return false;
    }

    // AoWAb.PAS:2612
    public override bool Remove(AbilityOwner owner)
    {
        PossessedAbilityData data = GetData(owner);
        if (data is not null)
        {
            SetAbility(owner, AbilityId, 0);
            owner.RemoveAbilityData(data);
        }
        else
        {
            SetAbility(owner, AbilityId, 0);
        }

        return true;
    }

    // AoWAb.PAS:2571
    public override void CombatObjectDestroyed(CombatObject combatObject)
    {
        if (combatObject is CombatUnit combatUnit)
        {
            combatUnit.SetStatus(combatUnit.Status - Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed));
            Unit newUnit = new(null) { Holder = CreationHolder() };
            newUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(152));
            PossessedAbilityData data = GetData(combatUnit.Troop);
            if (data is not null)
            {
                newUnit.SetExperience(data.SourceExperience);
            }

            if (combatUnit.Troop.UnitStatus.Contains(UnitStatusElement.Moving))
            {
                newUnit.UnitStatus = Set<UnitStatusElement>.Of(UnitStatusElement.Moving) + newUnit.UnitStatus;
            }

            combatUnit.Troop.Killed();
            combatUnit.SetUnit(newUnit);
            _ = newUnit.Release();
        }
    }

    // AoWAb.PAS:2592
    public override EngineClass AbilityDataClass() => s_possessedDataClass;

    public PossessedAbilityData GetData(AbilityOwner owner) => owner.GetAbilityData<PossessedAbilityData>(AbilityId);

    private static readonly EngineClass s_possessedDataClass = EngineClass.Of<PossessedAbilityData>();
    private static readonly string s_possessedText = "Possessed";

}
