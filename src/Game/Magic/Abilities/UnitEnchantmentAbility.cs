#nullable disable warnings
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;

namespace Aow.Game.Magic.Abilities;

public class UnitEnchantmentAbility : AdjustableAbility
{

    // AoWAb.PAS:601
    public UnitEnchantmentAbility(CustomNode? owner) : base(owner)
    {
        AbilityType = AbilityType.Enchantment;
        SelectionTypes = AbilitySelectionTypes.None;
    }

    // AoWAb.PAS:613
    public override string GetName(AbilityOwner? owner)
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
                        int duration = GetDuration(owner);
                        int totalDuration = TotalDuration;
                        string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.DurationDaysFormat), arguments: [duration.ToString(CultureInfo.InvariantCulture), totalDuration.ToString(CultureInfo.InvariantCulture)]);
                        return $"{result} {format}";
                    }

                    break;
                case DurationMode.CombatTurn:
                    int duration2 = GetDuration(owner);
                    int totalDuration2 = TotalDuration;
                    string formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.DurationCombatTurnFormat), arguments: [duration2.ToString(CultureInfo.InvariantCulture), totalDuration2.ToString(CultureInfo.InvariantCulture)]);
                    return $"{base.GetName(owner)} {formattedText}";
                case DurationMode.EntireCombat:
                    string name = base.GetName(owner);
                    string durationEntireCombat = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.DurationEntireCombatText);
                    return $"{name} {durationEntireCombat}";
                case DurationMode.None:
                default:
                    return base.GetName(owner);
            }
        }
        else
        {
            return base.GetName(owner);
        }

        return result;
    }

    // AoWAb.PAS:637
    public override string GetSourceName(AbilityOwner? owner)
    {
        if (StrategicMap.Current is not null)
        {
            UnitEnchantmentAbilityData data = owner.GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
            Troop? source = StrategicMap.Current.UnitControl.FindUnit(data.Enchantment.UnitId);
            // { Original also reports player zero as unknown. }
            return data.Enchantment.Source <= 0
                ? AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.UnknownText)
                : source is not null && PlayerIdExtensions.ToOriginal(source.Player) == data.Enchantment.Source
                    ? AbilityRules.TranslateResourceString(GetEngine(), S: s_manaText)
                    : StrategicMap.Current.Players.GetPlayers(data.Enchantment.Source).Leader.HeroName;
        }
        else
        {
            return AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.UnknownText);
        }
    }

    // AoWAb.PAS:735
    public override void NewTurn(AbilityOwner owner, PlayerId player)
    {
        if (GetDurationMode() == DurationMode.Day)
        {
            UnitEnchantmentAbilityData data = owner.GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
            if (data is not null)
            {
                if (data.EnchantmentDuration > 0 && player == data.EnchantmentPlayerIndex)
                {
                    data.EnchantmentDuration = (data.EnchantmentDuration - 1).TruncateToSByte();
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
    public override bool Expand(AbilityOwner? owner)
    {
        if (owner is not Troop troop)
        {
            // { Preserve the original assertion diagnostic. }
            Assert(false, message: "Assertion failure", file: "D:\\AoWDev\\AoWE\\inc\\AoWAb.PAS", line: 46175);
            return false;
        }
        UnitEnchantmentAbilityData data = owner.GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
        if (data is null)
        {
            if (troop.UnitId <= 0)
            {
                // { Preserve the original assertion diagnostic. }
                Assert(false, message: "Assertion failure", file: "D:\\AoWDev\\AoWE\\inc\\AoWAb.PAS", line: 46181);
            }

            data = new UnitEnchantmentAbilityData(null)
            {
                Holder = CreationHolder(),
                EnchantmentAbilityId = (ushort)AbilityId,
                EnchantmentDuration = TotalDuration
            };
            UnitEnchantment enchantment = CreateEnchantment();
            data.Enchantment = enchantment;
            enchantment.EnchantmentAbilityId = AbilityId;
            enchantment.UnitId = troop.UnitId;
            SetAbility(owner, index: AbilityId, value: 1);
            owner.AddAbilityData(data);
            return true;
        }
        else
        {
            data.EnchantmentDuration = TotalDuration;
        }

        return false;
    }

    // AoWAb.PAS:694
    public override bool Remove(AbilityOwner? owner)
    {
        UnitEnchantmentAbilityData data = owner.GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
        if (data is not null)
        {
            if (StrategicMap.Current is not null && data.Enchantment.Source > -1)
            {
                _ = StrategicMap.Current.Players.GetPlayers(data.Enchantment.Source).MagicControl.UnregisterEnchantment(data.Enchantment);
            }

            SetAbility(owner, index: AbilityId, value: 0);
            owner.RemoveAbilityData(data);
        }
        else
        {
            SetAbility(owner, index: AbilityId, value: 0);
            return false;
        }

        return true;
    }

    // AoWAb.PAS:756
    public override void NewCombatTurn(CombatObject combatObject, PlayerId player)
    {
        if (GetDurationMode() == DurationMode.CombatTurn)
        {
            UnitEnchantmentAbilityData data = combatObject.AbilityOwner.GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
            if (data is not null)
            {
                if (data.EnchantmentDuration > 0 && player == data.EnchantmentPlayerIndex)
                {
                    data.EnchantmentDuration = (data.EnchantmentDuration - 1).TruncateToSByte();
                    if (data.EnchantmentDuration == 0)
                    {
                        _ = combatObject.AbilityOwner.RemoveAbility(AbilityId);
                    }
                }
            }
            else
            {
                _ = combatObject.AbilityOwner.RemoveAbility(AbilityId);
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
                _ = combatObject.AbilityOwner.RemoveAbility(AbilityId);
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
    public virtual int GetDuration(AbilityOwner owner)
    {
        UnitEnchantmentAbilityData data = owner.GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
        return data is not null ? data.EnchantmentDuration.TruncateToByte() : 0;
    }

    // AoWAb.PAS:608
    public virtual int GetAIUpkeepPriority(AbilityOwner owner) => 0;

    // AoWAb.PAS:730
    public virtual int TotalDuration => -1;

    // AoWAb.PAS:663
    public virtual UnitEnchantment CreateEnchantment() => new(null) { Holder = CreationHolder() };

    // AoWAb.PAS:658
    public override EngineClass AbilityDataClass() => EngineClass.Of<UnitEnchantmentAbilityData>();

    private static readonly string s_manaText = "Mana";

}
