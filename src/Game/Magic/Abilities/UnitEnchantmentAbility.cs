// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

public partial class UnitEnchantmentAbility : AdjustableAbility
{

    // AoWAb.PAS:601
    public UnitEnchantmentAbility(ECustomNode owner) : base(owner)
    {
        FAbilityType = AbilityType.Enchantment;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // AoWAb.PAS:613
    public override string GetName(AbilityOwner Owner)
    {
        string Result;
        if (Owner != null)
        {
            switch (GetDurationMode())
            {
                case DurationMode.Day:
                    Result = base.GetName(Owner);
                    if (GetDuration(Owner) != 0)
                    {
                        var duration = GetDuration(Owner);
                        var totalDuration = GetTotalDuration();
                        var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.DurationDaysRStr), [IntToStr(duration), IntToStr(totalDuration)]);
                        Result = Result + " " + format;
                    }

                    break;
                case DurationMode.CombatTurn:
                    var duration2 = GetDuration(Owner);
                    var totalDuration2 = GetTotalDuration();
                    var aoWEFormat = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.DurationCombatTurnRStr), [IntToStr(duration2), IntToStr(totalDuration2)]);
                    Result = base.GetName(Owner) + " " + aoWEFormat;
                    break;
                case DurationMode.EntireCombat:
                    var name = base.GetName(Owner);
                    var durationEntireCombat = AbilityRules.TranslateRStr(GameText.DurationEntireCombatRStr);
                    Result = name + " " + durationEntireCombat;
                    break;
                default:
                    Result = base.GetName(Owner);
                    break;
            }
        }
        else
        {
            Result = base.GetName(Owner);
        }

        return Result;
    }

    // AoWAb.PAS:637
    public override string GetSourceName(AbilityOwner Owner)
    {
        string Result;
        UnitEnchantmentAbilityData Data;
        AbstractUnit Source;
        if (AoWHSMap.Current != null)
        {
            Data = (UnitEnchantmentAbilityData)Owner.GetAbilityData(FAbilityID);
            Source = AoWHSMap.Current.FUnitControl.FindUnit(Data.FEnchantment.FUnitID);
            // { Original also reports player zero as unknown. }
            if (Data.FEnchantment.FSource <= 0)
            {
                Result = AbilityRules.TranslateRStr(GameTables.UnknownRStr);
            }
            else
            {
                if (Source != null && Source.FPlayer == Data.FEnchantment.FSource)
                {
                    Result = AbilityRules.TranslateRStr(P4ManaRStr);
                }
                else
                {
                    Result = AoWHSMap.Current.FPlayers.GetPlayers(Data.FEnchantment.FSource).FLeader.FHeroName;
                }
            }
        }
        else
        {
            Result = AbilityRules.TranslateRStr(GameTables.UnknownRStr);
        }

        return Result;
    }

    // AoWAb.PAS:735
    public override void NewTurn(AbilityOwner Owner, sbyte Player)
    {
        UnitEnchantmentAbilityData Data;
        if (GetDurationMode() == DurationMode.Day)
        {
            Data = (UnitEnchantmentAbilityData)Owner.GetAbilityData(FAbilityID);
            if (Data != null)
            {
                if (Data.FEnchantmentDuration > 0 && Player == Data.FEnchantmentPlayerIndex)
                {
                    Data.FEnchantmentDuration = (sbyte)(Data.FEnchantmentDuration - 1);
                    if (Data.FEnchantmentDuration == 0)
                    {
                        Owner.RemoveAbility(FAbilityID);
                    }
                }
            }
            else
            {
                Owner.RemoveAbility(FAbilityID);
            }
        }
    }

    // AoWAb.PAS:668
    public override bool Expand(AbilityOwner Owner)
    {
        UnitEnchantmentAbilityData Data;
        UnitEnchantment Enchantment;
        if (Owner is not AbstractUnit unit)
        {
            // { Preserve the original assertion diagnostic. }
            Assert(false, "Assertion failure", "D:\\AoWDev\\AoWE\\inc\\AoWAb.PAS", 46175);
            return false;
        }

        bool Result = false;
        Data = (UnitEnchantmentAbilityData)Owner.GetAbilityData(FAbilityID);
        if (Data == null)
        {
            if (unit.FUnitID <= 0)
            {
                // { Preserve the original assertion diagnostic. }
                Assert(false, "Assertion failure", "D:\\AoWDev\\AoWE\\inc\\AoWAb.PAS", 46181);
            }

            Data = new UnitEnchantmentAbilityData(null);
            Data.FEnchantmentAbilityID = (ushort)FAbilityID;
            Data.FEnchantmentDuration = GetTotalDuration();
            Enchantment = CreateEnchantment();
            Data.FEnchantment = Enchantment;
            Enchantment.FEnchantmentAbilityID = FAbilityID;
            Enchantment.FUnitID = unit.FUnitID;
            SetAb(Owner, FAbilityID, 1);
            Owner.AddAbilityData(Data);
            Result = true;
        }
        else
        {
            Data.FEnchantmentDuration = GetTotalDuration();
        }

        return Result;
    }

    // AoWAb.PAS:694
    public override bool Remove(AbilityOwner owner)
    {
        UnitEnchantmentAbilityData Data;
        bool Result = true;
        Data = (UnitEnchantmentAbilityData)owner.GetAbilityData(FAbilityID);
        if (Data != null)
        {
            if (AoWHSMap.Current != null && Data.FEnchantment.FSource > -1)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(Data.FEnchantment.FSource).FMagicControl.UnregisterEnchantment(Data.FEnchantment);
            }

            SetAb(owner, FAbilityID, 0);
            owner.RemoveAbilityData(Data);
        }
        else
        {
            SetAb(owner, FAbilityID, 0);
            Result = false;
        }

        return Result;
    }

    // AoWAb.PAS:756
    public override void NewCombatTurn(CombatObject CombatObject, sbyte Player)
    {
        UnitEnchantmentAbilityData Data;
        if (GetDurationMode() == DurationMode.CombatTurn)
        {
            Data = (UnitEnchantmentAbilityData)CombatObject.GetAbilityOwner().GetAbilityData(FAbilityID);
            if (Data != null)
            {
                if (Data.FEnchantmentDuration > 0 && Player == Data.FEnchantmentPlayerIndex)
                {
                    Data.FEnchantmentDuration = (sbyte)(Data.FEnchantmentDuration - 1);
                    if (Data.FEnchantmentDuration == 0)
                    {
                        CombatObject.GetAbilityOwner().RemoveAbility(FAbilityID);
                    }
                }
            }
            else
            {
                CombatObject.GetAbilityOwner().RemoveAbility(FAbilityID);
            }
        }
    }

    // AoWAb.PAS:777
    public override void CombatDone(CombatObject CombatObject)
    {
        switch (GetDurationMode())
        {
            case DurationMode.CombatTurn:
            case DurationMode.EntireCombat:
                CombatObject.GetAbilityOwner().RemoveAbility(FAbilityID);
                break;
        }
    }

    // AoWAb.PAS:714
    public virtual DurationMode GetDurationMode() => DurationMode.None;

    // AoWAb.PAS:719
    public virtual byte GetDuration(AbilityOwner Owner)
    {
        byte Result;
        UnitEnchantmentAbilityData Data;
        Data = (UnitEnchantmentAbilityData)Owner.GetAbilityData(FAbilityID);
        if (Data != null)
        {
            Result = (byte)Data.FEnchantmentDuration;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:608
    public virtual int GetAIUpkeepPriority(AbilityOwner Owner) => 0;

    // AoWAb.PAS:730
    public virtual sbyte GetTotalDuration() => -1;

    // AoWAb.PAS:663
    public virtual UnitEnchantment CreateEnchantment() => new UnitEnchantment(null);

    // AoWAb.PAS:658
    public override EClass AbilityDataClass() => EClass.Of<UnitEnchantmentAbilityData>();

    private static readonly string P4ManaRStr = "Mana";

}
