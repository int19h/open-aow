#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.Magic.Abilities;

public partial class UnitEnchantmentAbility : AdjustableAbility
{

    // AoWAb.PAS:601
    public UnitEnchantmentAbility(CustomNode owner) : base(owner)
    {
        AbilityType = AbilityType.Enchantment;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // AoWAb.PAS:613
    public override string GetName(AbilityOwner owner)
    {
        string result;
        if (owner is not null)
        {
            switch (GetDurationMode())
            {
                case DurationMode.Day:
                    result = base.GetName(owner);
                    if (GetDuration(owner) != 0)
                    {
                        byte duration = GetDuration(owner);
                        sbyte totalDuration = GetTotalDuration();
                        string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.DurationDaysFormat), [IntToStr(duration), IntToStr(totalDuration)]);
                        result = $"{result} {format}";
                    }

                    break;
                case DurationMode.CombatTurn:
                    byte duration2 = GetDuration(owner);
                    sbyte totalDuration2 = GetTotalDuration();
                    string formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.DurationCombatTurnFormat), [IntToStr(duration2), IntToStr(totalDuration2)]);
                    result = $"{base.GetName(owner)} {formattedText}";
                    break;
                case DurationMode.EntireCombat:
                    string name = base.GetName(owner);
                    string durationEntireCombat = AbilityRules.TranslateResourceString(GetEngine(), GameText.DurationEntireCombatText);
                    result = $"{name} {durationEntireCombat}";
                    break;
                case DurationMode.None:
                default:
                    result = base.GetName(owner);
                    break;
            }
        }
        else
        {
            result = base.GetName(owner);
        }

        return result;
    }

    // AoWAb.PAS:637
    public override string GetSourceName(AbilityOwner owner)
    {
        string result;
        if (StrategicMap.Current is not null)
        {
            UnitEnchantmentAbilityData data = owner.GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
            Troop source = StrategicMap.Current.UnitControl.FindUnit(data.Enchantment.UnitId);
            // { Original also reports player zero as unknown. }
            result = data.Enchantment.Source <= 0
                ? AbilityRules.TranslateResourceString(GetEngine(), GameTables.UnknownText)
                : source is not null && source.Player == data.Enchantment.Source
                    ? AbilityRules.TranslateResourceString(GetEngine(), s_manaText)
                    : StrategicMap.Current.Players.GetPlayers(data.Enchantment.Source).Leader.HeroName;
        }
        else
        {
            result = AbilityRules.TranslateResourceString(GetEngine(), GameTables.UnknownText);
        }

        return result;
    }

    // AoWAb.PAS:735
    public override void NewTurn(AbilityOwner owner, sbyte player)
    {
        if (GetDurationMode() == DurationMode.Day)
        {
            UnitEnchantmentAbilityData data = owner.GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
            if (data is not null)
            {
                if (data.EnchantmentDuration > 0 && player == data.EnchantmentPlayerIndex)
                {
                    data.EnchantmentDuration = (sbyte)(data.EnchantmentDuration - 1);
                    if (data.EnchantmentDuration == 0)
                    {
                        _ = owner.RemoveAbility(AbilityId);
                    }
                }
            }
            else
            {
                _ = owner.RemoveAbility(AbilityId);
            }
        }
    }

    // AoWAb.PAS:668
    public override bool Expand(AbilityOwner owner)
    {
        if (owner is not Troop troop)
        {
            // { Preserve the original assertion diagnostic. }
            Assert(false, "Assertion failure", "D:\\AoWDev\\AoWE\\inc\\AoWAb.PAS", 46175);
            return false;
        }

        bool result = false;
        UnitEnchantmentAbilityData data = owner.GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
        if (data is null)
        {
            if (troop.UnitId <= 0)
            {
                // { Preserve the original assertion diagnostic. }
                Assert(false, "Assertion failure", "D:\\AoWDev\\AoWE\\inc\\AoWAb.PAS", 46181);
            }

            data = new UnitEnchantmentAbilityData(null)
            {
                Holder = CreationHolder(),
                EnchantmentAbilityId = (ushort)AbilityId,
                EnchantmentDuration = GetTotalDuration()
            };
            UnitEnchantment enchantment = CreateEnchantment();
            data.Enchantment = enchantment;
            enchantment.EnchantmentAbilityId = AbilityId;
            enchantment.UnitId = troop.UnitId;
            SetAbility(owner, AbilityId, 1);
            owner.AddAbilityData(data);
            result = true;
        }
        else
        {
            data.EnchantmentDuration = GetTotalDuration();
        }

        return result;
    }

    // AoWAb.PAS:694
    public override bool Remove(AbilityOwner owner)
    {
        bool result = true;
        UnitEnchantmentAbilityData data = owner.GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
        if (data is not null)
        {
            if (StrategicMap.Current is not null && data.Enchantment.Source > -1)
            {
                _ = StrategicMap.Current.Players.GetPlayers(data.Enchantment.Source).MagicControl.UnregisterEnchantment(data.Enchantment);
            }

            SetAbility(owner, AbilityId, 0);
            owner.RemoveAbilityData(data);
        }
        else
        {
            SetAbility(owner, AbilityId, 0);
            result = false;
        }

        return result;
    }

    // AoWAb.PAS:756
    public override void NewCombatTurn(CombatObject combatObject, sbyte player)
    {
        if (GetDurationMode() == DurationMode.CombatTurn)
        {
            UnitEnchantmentAbilityData data = combatObject.GetAbilityOwner().GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
            if (data is not null)
            {
                if (data.EnchantmentDuration > 0 && player == data.EnchantmentPlayerIndex)
                {
                    data.EnchantmentDuration = (sbyte)(data.EnchantmentDuration - 1);
                    if (data.EnchantmentDuration == 0)
                    {
                        _ = combatObject.GetAbilityOwner().RemoveAbility(AbilityId);
                    }
                }
            }
            else
            {
                _ = combatObject.GetAbilityOwner().RemoveAbility(AbilityId);
            }
        }
    }

    // AoWAb.PAS:777
    public override void CombatDone(CombatObject combatObject)
    {
        switch (GetDurationMode())
        {
            case DurationMode.CombatTurn:
            case DurationMode.EntireCombat:
                _ = combatObject.GetAbilityOwner().RemoveAbility(AbilityId);
                break;
            case DurationMode.None:
            case DurationMode.Day:
            default:
                break;
        }
    }

    // AoWAb.PAS:714
    public virtual DurationMode GetDurationMode() => DurationMode.None;

    // AoWAb.PAS:719
    public virtual byte GetDuration(AbilityOwner owner)
    {
        UnitEnchantmentAbilityData data = owner.GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
        byte result = data is not null ? (byte)data.EnchantmentDuration : (byte)0;

        return result;
    }

    // AoWAb.PAS:608
    public virtual int GetAIUpkeepPriority(AbilityOwner owner) => 0;

    // AoWAb.PAS:730
    public virtual sbyte GetTotalDuration() => -1;

    // AoWAb.PAS:663
    public virtual UnitEnchantment CreateEnchantment() => new(null) { Holder = CreationHolder() };

    // AoWAb.PAS:658
    public override EngineClass AbilityDataClass() => EngineClass.Of<UnitEnchantmentAbilityData>();

    private static readonly string s_manaText = "Mana";

}
