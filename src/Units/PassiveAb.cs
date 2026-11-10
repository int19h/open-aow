// Translated from PassiveAb.pas.
using Aow.Game.Combat;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type. maximum level, level costs
public partial class VisionAbility : Aow.Game.Magic.Abilities.MultiLevelAbility
{
    // PassiveAb.pas:461
    public VisionAbility(ECustomNode owner) : base(owner)
    {
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of(Aow.Game.Magic.Abilities.AbilitySelectionType.Unit, Aow.Game.Magic.Abilities.AbilitySelectionType.HeadItem, Aow.Game.Magic.Abilities.AbilitySelectionType.TorsoItem, Aow.Game.Magic.Abilities.AbilitySelectionType.DefenseItem, Aow.Game.Magic.Abilities.AbilitySelectionType.RingItem, Aow.Game.Magic.Abilities.AbilitySelectionType.Editor);
        FMaxLevel = 4;
        FAbilityID = 64;
        FLevelCosts[1] = 5;
        FLevelCosts[2] = 5;
        FLevelCosts[3] = 5;
        FLevelCosts[4] = 5;
    }

    // PassiveAb.pas:473
    public override string GetLevelName(int Level)
    {
        string Result = "";
        switch (Level)
        {
            case 0:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.VisionAbility.VisionRStr);
                break;
            case 1:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.VisionAbility.VisionRStr) + " I";
                break;
            case 2:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.VisionAbility.VisionRStr) + " II";
                break;
            case 3:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.VisionAbility.VisionRStr) + " III";
                break;
            case 4:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.VisionAbility.VisionRStr) + " IV";
                break;
        }

        return Result;
    }

    private static readonly string VisionRStr = "Vision";
}

// inherited name, ability ID, selection types, ability type
public partial class HolyChampionAbility : Aow.Game.Magic.Abilities.EnhancementAbility
{
    // PassiveAb.pas:484
    public HolyChampionAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.HolyChampionRStr);
        FAbilityID = 146;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of(Aow.Game.Magic.Abilities.AbilitySelectionType.Unit, Aow.Game.Magic.Abilities.AbilitySelectionType.HeadItem, Aow.Game.Magic.Abilities.AbilitySelectionType.TorsoItem, Aow.Game.Magic.Abilities.AbilitySelectionType.DefenseItem, Aow.Game.Magic.Abilities.AbilitySelectionType.RingItem, Aow.Game.Magic.Abilities.AbilitySelectionType.Editor);
    }

    // PassiveAb.pas:492
    public override bool CanExpand(Aow.Game.Magic.Abilities.AbilityOwner owner)
    {
        bool Result = base.CanExpand(owner);
        if (Result && owner is Aow.Game.Magic.Abilities.AbstractUnit)
        {
            Result = Aow.Game.Magic.Abilities.AlignmentTables.alsPureGood.Contains(((Aow.Game.Magic.Abilities.AbstractUnit)owner).GetAlignment());
        }

        return Result;
    }
}

// inherited name, ability ID, selection types, ability type
public partial class UnholyChampionAbility : Aow.Game.Magic.Abilities.EnhancementAbility
{
    // PassiveAb.pas:499
    public UnholyChampionAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.UnholyChampionRStr);
        FAbilityID = 147;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of(Aow.Game.Magic.Abilities.AbilitySelectionType.Unit, Aow.Game.Magic.Abilities.AbilitySelectionType.HeadItem, Aow.Game.Magic.Abilities.AbilitySelectionType.TorsoItem, Aow.Game.Magic.Abilities.AbilitySelectionType.DefenseItem, Aow.Game.Magic.Abilities.AbilitySelectionType.RingItem, Aow.Game.Magic.Abilities.AbilitySelectionType.Editor);
    }

    // PassiveAb.pas:507
    public override bool CanExpand(Aow.Game.Magic.Abilities.AbilityOwner owner)
    {
        bool Result = base.CanExpand(owner);
        if (Result && owner is Aow.Game.Magic.Abilities.AbstractUnit)
        {
            Result = Aow.Game.Magic.Abilities.AlignmentTables.alsPureEvil.Contains(((Aow.Game.Magic.Abilities.AbstractUnit)owner).GetAlignment());
        }

        return Result;
    }
}

// inherited name, ability ID, selection types, ability type. duration mode
public partial class NaturesBlessingAbility : Aow.Game.Magic.Abilities.DurationAbility
{
    // PassiveAb.pas:514
    public NaturesBlessingAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.NaturesBlessingAbility.NaturesBlessingRStr);
        FAbilityID = 99;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Enchantment;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
        FDurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:524
    public override sbyte GetAttack(Aow.Game.Magic.Abilities.AbilityOwner Owner) => 1;

    // PassiveAb.pas:529
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.AbilityOwner Owner) => 1;

    // PassiveAb.pas:534
    public override sbyte GetResistance(Aow.Game.Magic.Abilities.AbilityOwner Owner) => 1;

    // PassiveAb.pas:539
    public override bool Expand(Aow.Game.Magic.Abilities.AbilityOwner Owner)
    {
        bool Result = base.Expand(Owner);
        if (Owner is Aow.Game.Magic.Abilities.AbstractUnit)
        {
            var unit = (Aow.Game.Magic.Abilities.AbstractUnit)Owner;
            unit.SetHitPoints(unit.GetHits());
        }

        return Result;
    }

    private static readonly string NaturesBlessingRStr = "Nature's Blessing";
}

// (name, ID, selection types, ability type, duration mode.)
public partial class HighPrayerBlessingAbility : Aow.Game.Magic.Abilities.DurationAbility
{
    // PassiveAb.pas:1267
    public HighPrayerBlessingAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.HighPrayerBlessingAbility.HighPrayerBlessingRStr);
        FAbilityID = 100;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Enhancement;
        FDurationMode = DurationMode.EntireCombat;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:1277
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.AbilityOwner Owner) => 1;

    // PassiveAb.pas:1282
    public override sbyte GetResistance(Aow.Game.Magic.Abilities.AbilityOwner Owner) => 1;

    private static readonly string HighPrayerBlessingRStr = "Blessed";
}

// inherited name, ability ID, selection types, ability type, duration mode. duration data
// countdown, player index
public partial class CrusaderAbility : Aow.Game.Magic.Abilities.DurationAbility
{
    // PassiveAb.pas:703
    public CrusaderAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.CrusaderAbility.CrusaderRStr);
        FAbilityID = 101;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Handicap;
        FDurationMode = DurationMode.Day;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:724
    public override void NewTurn(Aow.Game.Magic.Abilities.AbilityOwner Owner, sbyte Player)
    {
        Aow.Game.Magic.Abilities.DurationAbilityData Data;
        if (FDurationMode == DurationMode.Day)
        {
            Data = (Aow.Game.Magic.Abilities.DurationAbilityData)Owner.GetAbilityData(FAbilityID);
            if (Data != null && Data.FDuration != 0 && Data.FPlayerIndex == Player)
            {
                Data.FDuration = (byte)(Data.FDuration - 1);
            }
        }
    }

    // PassiveAb.pas:713
    public override bool Remove(Aow.Game.Magic.Abilities.AbilityOwner owner)
    {
        bool Result = base.Remove(owner);
        if (owner is Aow.Game.Magic.Abilities.AbstractUnit unit)
        {
            unit.Killed();
        }

        return Result;
    }

    // PassiveAb.pas:719
    public override int GetTotalDuration() => 3;

    private static readonly string CrusaderRStr = "Crusader";
}

// inherited name, ability ID, selection types, ability type. duration mode
public partial class BloodlustAbility : Aow.Game.Magic.Abilities.DurationAbility
{
    // PassiveAb.pas:546
    public BloodlustAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.BloodlustAbility.BloodlustRStr);
        FAbilityID = 168;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Enchantment;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
        FDurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:556
    public override sbyte GetAttack(Aow.Game.Magic.Abilities.AbilityOwner Owner) => 1;

    // PassiveAb.pas:561
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.AbilityOwner Owner) => -1;

    // PassiveAb.pas:566
    public override sbyte GetDamage(Aow.Game.Magic.Abilities.AbilityOwner Owner) => 2;

    private static readonly string BloodlustRStr = "Bloodlust";
}

// inherited name, ability ID, selection types, ability type. duration mode
public partial class PoisonedAbility : Aow.Game.Magic.Abilities.DurationAbility
{
    // PassiveAb.pas:571
    public PoisonedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.PoisonedAbility.PoisonedRStr);
        FAbilityID = 96;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Handicap;
        FDurationMode = DurationMode.Day;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:581
    public override sbyte GetAttack(Aow.Game.Magic.Abilities.AbilityOwner Owner) => -1;

    // PassiveAb.pas:586
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.AbilityOwner Owner) => -1;

    // PassiveAb.pas:591
    public override sbyte GetResistance(Aow.Game.Magic.Abilities.AbilityOwner Owner) => -1;

    // PassiveAb.pas:596
    public override sbyte GetDamage(Aow.Game.Magic.Abilities.AbilityOwner Owner) => -1;

    private static readonly string PoisonedRStr = "Poisoned";
}

// inherited name, ability ID, selection types, ability type, duration mode
public partial class DecayAbility : Aow.Game.Magic.Abilities.DurationAbility
{
    // PassiveAb.pas:844
    public DecayAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.DecayAbility.DecayRStr);
        FAbilityID = 131;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Handicap;
        FDurationMode = DurationMode.EntireCombat;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:855
    public override void NewCombatTurn(CombatObject CombatObject, sbyte Player)
    {
        Aow.Game.Magic.Abilities.DurationAbilityData Data;
        Data = (Aow.Game.Magic.Abilities.DurationAbilityData)CombatObject.GetAbilityOwner().GetAbilityData(FAbilityID);
        if (Data != null && Data.FPlayerIndex == Player)
        {
            if (CombatObject.GetAbilityOwner() is Aow.Game.Magic.Abilities.AbstractUnit)
            {
                CombatObject.ExecuteDamage(1);
            }
        }
    }

    // PassiveAb.pas:868
    public override void CombatDone(CombatObject CombatObject)
    {
        CombatObject.DestroyObject();
    }

    private static readonly string DecayRStr = "Decay";
}

// inherited name, ability ID, selection types, ability type, duration mode
public partial class WebbedAbility : Aow.Game.Magic.Abilities.DurationAbility
{
    // PassiveAb.pas:736
    public WebbedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.WebbedAbility.WebbedRStr);
        FAbilityID = 97;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Handicap;
        FDurationMode = DurationMode.CombatTurn;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:751
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.AbilityOwner Owner) => -2;

    // PassiveAb.pas:746
    public override int GetTotalDuration() => 3;

    private static readonly string WebbedRStr = "Webbed";
}

// inherited name, ability ID, selection types, ability type, duration mode
public partial class StunnedAbility : Aow.Game.Magic.Abilities.DurationAbility
{
    // PassiveAb.pas:793
    public StunnedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.StunnedAbility.StunnedRStr);
        FAbilityID = 92;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Handicap;
        FDurationMode = DurationMode.CombatTurn;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:810
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.AbilityOwner Owner) => -2;

    // PassiveAb.pas:804
    public override int GetTotalDuration() => 1;

    private static readonly string StunnedRStr = "Stunned";
}

// inherited name, ability ID, selection types, ability type, duration mode
public partial class BurningAbility : Aow.Game.Magic.Abilities.DurationAbility
{
    // PassiveAb.pas:756
    public BurningAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.BurningAbility.BurningRStr);
        FAbilityID = 127;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Handicap;
        FDurationMode = DurationMode.CombatTurn;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:773
    public override void NewCombatTurn(CombatObject CombatObject, sbyte Player)
    {
        Aow.Game.Magic.Abilities.DurationAbilityData Data;
        Data = (Aow.Game.Magic.Abilities.DurationAbilityData)CombatObject.GetAbilityOwner().GetAbilityData(FAbilityID);
        if (Data != null)
        {
            if (Data.FDuration != 0 && Data.FPlayerIndex == Player)
            {
                Data.FDuration = (byte)(Data.FDuration - 1);
                if (Data.FDuration == 0)
                {
                    CombatObject.GetAbilityOwner().RemoveAbility(FAbilityID);
                }

                if (CombatObject.GetAbilityOwner() is Aow.Game.Magic.Abilities.AbstractUnit unit)
                {
                    if (Aow.Game.Magic.Abilities.HitChance.HitRole(4 - unit.GetResistance()))
                    {
                        CombatObject.ExecuteDamage(1);
                    }
                }
            }
        }
        else
        {
            CombatObject.GetAbilityOwner().RemoveAbility(FAbilityID);
        }
    }

    // PassiveAb.pas:767
    public override int GetTotalDuration() => 3;

    private static readonly string BurningRStr = "Burning";
}

// inherited name, ability ID, selection types, ability type, duration mode
public partial class VertigoAbility : Aow.Game.Magic.Abilities.DurationAbility
{
    // PassiveAb.pas:952
    public VertigoAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.VertigoAbility.VertigoRStr);
        FAbilityID = 98;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Handicap;
        FDurationMode = DurationMode.Day;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:969
    public override sbyte GetAttack(Aow.Game.Magic.Abilities.AbilityOwner Owner) => -2;

    // PassiveAb.pas:975
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.AbilityOwner Owner) => -2;

    // PassiveAb.pas:963
    public override int GetTotalDuration() => 3;

    private static readonly string VertigoRStr = "Vertigo";
}

// inherited name, ability ID, selection types, ability type, duration mode
public partial class CursedAbility : Aow.Game.Magic.Abilities.DurationAbility
{
    // PassiveAb.pas:981
    public CursedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.CursedAbility.CursedRStr);
        FAbilityID = 93;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
        FDurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:1009
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.AbilityOwner Owner) => -2;

    // PassiveAb.pas:1015
    public override sbyte GetResistance(Aow.Game.Magic.Abilities.AbilityOwner Owner) => -2;

    // PassiveAb.pas:991
    public override string GetSourceName(Aow.Game.Magic.Abilities.AbilityOwner Owner)
    {
        string Result;
        Aow.Game.Magic.Abilities.DurationAbilityData Data;
        Data = (Aow.Game.Magic.Abilities.DurationAbilityData)Owner.GetAbilityData(FAbilityID);
        if (Data != null && Data.FSource == 1)
        {
            Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.DeathMasteryRStr);
        }
        else
        {
            Result = base.GetSourceName(Owner);
        }

        return Result;
    }

    // PassiveAb.pas:1003
    public override int GetTotalDuration() => 3;

    private static readonly string CursedRStr = "Cursed";
}

// inherited name, ability ID, selection types, ability type, duration mode
public partial class HolyFear : Aow.Game.Magic.Abilities.DurationAbility
{
    // PassiveAb.pas:816
    public HolyFear(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.HolyFear.HolyFearRStr);
        FAbilityID = 53;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Handicap;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
        FDurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:833
    public override string GetSourceName(Aow.Game.Magic.Abilities.AbilityOwner Owner)
    {
        string Result;
        // { Original assumes the ability data exists. }
        var durationAbilityData = (Aow.Game.Magic.Abilities.DurationAbilityData)Owner.GetAbilityData(FAbilityID);
        if (durationAbilityData.FSource == 1)
        {
            Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.LifeMasteryRStr);
        }
        else
        {
            Result = base.GetSourceName(Owner);
        }

        return Result;
    }

    // PassiveAb.pas:827
    public override int GetTotalDuration() => 3;

    private static readonly string HolyFearRStr = "Holy Fear";
}

// inherited name, ability ID, selection types, ability type, duration mode
public partial class PanickedAbility : Aow.Game.Magic.Abilities.DurationAbility
{
    // PassiveAb.pas:874
    public PanickedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.PanickedAbility.PanickedRStr);
        FAbilityID = 108;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Handicap;
        FDurationMode = DurationMode.EntireCombat;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:885
    public override bool Expand(Aow.Game.Magic.Abilities.AbilityOwner Owner)
    {
        bool Result = base.Expand(Owner);
        if (Owner is Aow.Game.Magic.Abilities.AbstractUnit)
        {
            Owner.Changed();
        }

        return Result;
    }

    private static readonly string PanickedRStr = "Panicked";
}

// inherited name, ability ID, selection types, ability type
public partial class WallClimbingAbility : Aow.Game.Magic.Abilities.AdjustableAbility
{
    // PassiveAb.pas:922
    public WallClimbingAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.WallClimbingAbility.WallClimbingRStr);
        FAbilityID = 36;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of(Aow.Game.Magic.Abilities.AbilitySelectionType.Unit, Aow.Game.Magic.Abilities.AbilitySelectionType.HeadItem, Aow.Game.Magic.Abilities.AbilitySelectionType.TorsoItem, Aow.Game.Magic.Abilities.AbilitySelectionType.DefenseItem, Aow.Game.Magic.Abilities.AbilitySelectionType.RingItem, Aow.Game.Magic.Abilities.AbilitySelectionType.Editor);
    }

    // PassiveAb.pas:931
    public override Set<WallCombatFeaturesElement> GetWallCombatFeatures(Aow.Game.Magic.Abilities.AbilityOwner Owner) => Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallPassing, WallCombatFeaturesElement.Participate);

    private static readonly string WallClimbingRStr = "Wall Climbing";
}

// inherited name, ability ID, selection types, ability type
public partial class PassWallAbility : Aow.Game.Magic.Abilities.AdjustableAbility
{
    // PassiveAb.pas:937
    public PassWallAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.PassWallAbility.PassWallRStr);
        FAbilityID = 66;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of(Aow.Game.Magic.Abilities.AbilitySelectionType.Unit, Aow.Game.Magic.Abilities.AbilitySelectionType.HeadItem, Aow.Game.Magic.Abilities.AbilitySelectionType.TorsoItem, Aow.Game.Magic.Abilities.AbilitySelectionType.DefenseItem, Aow.Game.Magic.Abilities.AbilitySelectionType.RingItem);
    }

    // PassiveAb.pas:946
    public override Set<WallCombatFeaturesElement> GetWallCombatFeatures(Aow.Game.Magic.Abilities.AbilityOwner Owner) => Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallPassing, WallCombatFeaturesElement.Participate);

    private static readonly string PassWallRStr = "Pass Wall";
}

// inherited name, ability ID, selection types, ability type
public partial class ResurrectedAbility : Aow.Game.Magic.Abilities.AdjustableAbility
{
    // PassiveAb.pas:892
    public ResurrectedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ResurrectedRStr);
        FAbilityID = 128;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Handicap;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }
}

// inherited name, ability ID, selection types, ability type
public partial class AnimatedAbility : Aow.Game.Magic.Abilities.AdjustableAbility
{
    // PassiveAb.pas:902
    public AnimatedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.AnimatedAbility.AnimatedRStr);
        FAbilityID = 129;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Handicap;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }

    private static readonly string AnimatedRStr = "Animated";
}

// inherited name, ability ID, selection types, ability type
public partial class CombatResurrectedAbility : Aow.Game.Magic.Abilities.AdjustableAbility
{
    // PassiveAb.pas:912
    public CombatResurrectedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ResurrectedRStr);
        FAbilityID = 130;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Handicap;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }
}

// (maximum level, level costs.)
public partial class MarksmanshipAbility : Aow.Game.Magic.Abilities.MultiLevelAbility
{
    // PassiveAb.pas:1384
    public MarksmanshipAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.MarksmanshipAbility.MarksmanshipRStr);
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Enhancement;
        FAbilityID = 32;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of(Aow.Game.Magic.Abilities.AbilitySelectionType.Unit, Aow.Game.Magic.Abilities.AbilitySelectionType.HeadItem, Aow.Game.Magic.Abilities.AbilitySelectionType.TorsoItem, Aow.Game.Magic.Abilities.AbilitySelectionType.DefenseItem, Aow.Game.Magic.Abilities.AbilitySelectionType.RingItem, Aow.Game.Magic.Abilities.AbilitySelectionType.Editor);
        FMaxLevel = 4;
        FLevelCosts[1] = 5;
        FLevelCosts[2] = 5;
        FLevelCosts[3] = 5;
        FLevelCosts[4] = 5;
    }

    // PassiveAb.pas:1398
    public override string GetLevelName(int Level)
    {
        string Result = "";
        switch (Level)
        {
            case 0:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.MarksmanshipAbility.MarksmanshipRStr);
                break;
            case 1:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.MarksmanshipAbility.MarksmanshipRStr) + " I";
                break;
            case 2:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.MarksmanshipAbility.MarksmanshipRStr) + " II";
                break;
            case 3:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.MarksmanshipAbility.MarksmanshipRStr) + " III";
                break;
            case 4:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.MarksmanshipAbility.MarksmanshipRStr) + " IV";
                break;
        }

        return Result;
    }

    private static readonly string MarksmanshipRStr = "Marksmanship";
}

// (FMoveTypes.)
public partial class MovementAbility : Aow.Game.Magic.Abilities.AdjustableAbility
{
    public Set<MoveType> FMoveTypes;

    // PassiveAb.pas:1409
    public MovementAbility(ECustomNode owner) : base(owner)
    {
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of(Aow.Game.Magic.Abilities.AbilitySelectionType.Unit, Aow.Game.Magic.Abilities.AbilitySelectionType.RingItem);
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Movement;
        FSkillPointCost = 0;
    }

    // PassiveAb.pas:1417
    public override Set<MoveType> GetMoveTypes(Aow.Game.Magic.Abilities.AbilityOwner Owner) => FMoveTypes;

    // PassiveAb.pas:1422
    public override Set<WallCombatFeaturesElement> GetWallCombatFeatures(Aow.Game.Magic.Abilities.AbilityOwner Owner)
    {
        Set<WallCombatFeaturesElement> Result;
        if (FAbilityID == 1)
        {
            Result = Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallPassing, WallCombatFeaturesElement.Participate);
        }
        else
        {
            Result = Set<WallCombatFeaturesElement>.Of();
        }

        return Result;
    }
}

// (FImmunityTypes.)
public partial class ImmunityAbility : Aow.Game.Magic.Abilities.EnhancementAbility
{
    public Set<DamageType> FImmunityTypes;

    // PassiveAb.pas:1428
    public ImmunityAbility(ECustomNode owner) : base(owner)
    {
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of(Aow.Game.Magic.Abilities.AbilitySelectionType.Unit, Aow.Game.Magic.Abilities.AbilitySelectionType.DefenseItem, Aow.Game.Magic.Abilities.AbilitySelectionType.RingItem);
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Enhancement;
    }

    // PassiveAb.pas:1440
    public override Set<MoveType> GetMoveTypes(Aow.Game.Magic.Abilities.AbilityOwner Owner)
    {
        Set<MoveType> Result;
        if (FImmunityTypes.Contains(DamageType.Fire))
        {
            Result = Set<MoveType>.Of(MoveType.LavaCrossing);
        }
        else
        {
            Result = Set<MoveType>.Of();
        }

        return Result;
    }

    // PassiveAb.pas:1435
    public override Set<DamageType> GetImmunityTypes(Aow.Game.Magic.Abilities.AbilityOwner Owner) => FImmunityTypes;
}

// (FProtectionTypes.)
public partial class ProtectionAbility : Aow.Game.Magic.Abilities.EnhancementAbility
{
    public Set<DamageType> FProtectionTypes;

    // PassiveAb.pas:1446
    public ProtectionAbility(ECustomNode owner) : base(owner)
    {
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of(Aow.Game.Magic.Abilities.AbilitySelectionType.Unit, Aow.Game.Magic.Abilities.AbilitySelectionType.DefenseItem, Aow.Game.Magic.Abilities.AbilitySelectionType.RingItem, Aow.Game.Magic.Abilities.AbilitySelectionType.Editor);
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Enhancement;
    }

    // PassiveAb.pas:1453
    public override Set<DamageType> GetProtectionTypes(Aow.Game.Magic.Abilities.AbilityOwner Owner) => FProtectionTypes;
}

// inherited name, ability ID, selection types, ability type
public partial class EntangledAbility : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:620
    public EntangledAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.EntangledAbility.EntangledRStr);
        FAbilityID = 94;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:638
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.AbilityOwner Owner) => -2;

    // PassiveAb.pas:628
    public override DurationMode GetDurationMode() => DurationMode.CombatTurn;

    // PassiveAb.pas:633
    public override sbyte GetTotalDuration() => 3;

    // PassiveAb.pas:643
    public override UnitEnchantment CreateEnchantment()
    {
        UnitEnchantment Result = base.CreateEnchantment();
        Result.FDispelResistance = 3;
        return Result;
    }

    private static readonly string EntangledRStr = "Entangled";
}

// inherited name, ability ID, selection types, ability type
public partial class FrozenAbility : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:649
    public FrozenAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.FrozenAbility.FrozenRStr);
        FAbilityID = 95;
        FAbilityType = Aow.Game.Magic.Abilities.AbilityType.Enchantment;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:674
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.AbilityOwner Owner) => -2;

    // PassiveAb.pas:658
    public override DurationMode GetDurationMode() => DurationMode.CombatTurn;

    // PassiveAb.pas:663
    public override sbyte GetTotalDuration() => 2;

    // PassiveAb.pas:668
    public override UnitEnchantment CreateEnchantment()
    {
        UnitEnchantment Result = base.CreateEnchantment();
        Result.FDispelResistance = 3;
        return Result;
    }

    private static readonly string FrozenRStr = "Frozen";
}

// inherited name, ability ID, selection types, ability type
public partial class TurnedUndeadAbility : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:679
    public TurnedUndeadAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TurnedUndeadAbility.TurnedUndeadRStr);
        FAbilityID = 34;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:693
    public override DurationMode GetDurationMode() => DurationMode.CombatTurn;

    // PassiveAb.pas:698
    public override sbyte GetTotalDuration() => 3;

    // PassiveAb.pas:687
    public override UnitEnchantment CreateEnchantment()
    {
        UnitEnchantment Result = base.CreateEnchantment();
        Result.FDispelResistance = 4;
        return Result;
    }

    private static readonly string TurnedUndeadRStr = "Turned Undead";
}

// inherited name, ability ID, selection types, ability type
public partial class HasteAbility : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1021
    public HasteAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.HasteRStr);
        FAbilityID = 152;
    }
}

// inherited name, ability ID, selection types, ability type
public partial class StoneSkinAbility : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1029
    public StoneSkinAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.StoneSkinRStr);
        FAbilityID = 153;
    }

    // PassiveAb.pas:1048
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.AbilityOwner Owner) => 2;

    // PassiveAb.pas:1037
    public override string GetSourceName(Aow.Game.Magic.Abilities.AbilityOwner Owner)
    {
        string Result;
        // { Original assumes the ability data and enchantment exist. }
        var unitEnchantmentAbilityData = (Aow.Game.Magic.Abilities.UnitEnchantmentAbilityData)Owner.GetAbilityData(FAbilityID);
        if (unitEnchantmentAbilityData.FEnchantment.FSource == -2)
        {
            Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.EarthMasteryRStr);
        }
        else
        {
            Result = base.GetSourceName(Owner);
        }

        return Result;
    }
}

// inherited name, ability ID, selection types, ability type
public partial class EnchantedWeaponAbility : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1073
    public EnchantedWeaponAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.EnchantedWeaponAbility.EnchantedWeaponRStr);
        FAbilityID = 154;
    }

    // PassiveAb.pas:1081
    public override sbyte GetAttack(Aow.Game.Magic.Abilities.AbilityOwner Owner) => 1;

    // PassiveAb.pas:1087
    public override sbyte GetDamage(Aow.Game.Magic.Abilities.AbilityOwner Owner) => 1;

    // PassiveAb.pas:1093
    public override int GetAIUpkeepPriority(Aow.Game.Magic.Abilities.AbilityOwner Owner)
    {
        if (Owner is not Aow.Game.Magic.Abilities.AbstractUnit unit)
        {
            return 0;
        }

        int Result = unit.GetUnitLevel() + 225;
        if (unit is Aow.Game.Magic.Abilities.Leader)
        {
            Result += 50;
        }

        return Result;
    }

    private static readonly string EnchantedWeaponRStr = "Enchanted Weapon";
}

// inherited name, ability ID, selection types, ability type
public partial class SummonedAbility : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1100
    public SummonedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.SummonedAbility.SummonedRStr);
        FAbilityID = 155;
    }

    // PassiveAb.pas:1122
    public override bool Remove(Aow.Game.Magic.Abilities.AbilityOwner owner)
    {
        bool Result = base.Remove(owner);
        if (Result && owner is Aow.Game.Magic.Abilities.AbstractUnit unit)
        {
            unit.Killed();
            unit.SetOwner(null);
        }

        return Result;
    }

    // PassiveAb.pas:1134
    public override int GetAIUpkeepPriority(Aow.Game.Magic.Abilities.AbilityOwner Owner)
    {
        if (Owner is not Aow.Game.Magic.Abilities.AbstractUnit unit)
        {
            return 0;
        }

        int Result;
        switch (unit.GetUnitLevel())
        {
            case 1:
                Result = 250;
                break;
            case 2:
                Result = 250;
                break;
            case 3:
                Result = 500;
                break;
            case 4:
                Result = 1000;
                break;
            default:
                Result = 1000;
                break;
        }

        return Result;
    }

    // PassiveAb.pas:1108
    // Static: the original calls this and GetDefaultDispelResistance on the nil result of GetSpell(155)
    // (PrdPlace.pas:336, SummonSpells.pas:258), which works there because they read only the unit argument.
    public static int GetDefaultManaUpkeep(Aow.Game.Magic.Abilities.AbstractUnit AUnit)
    {
        int Result;
        if (AUnit is Aow.Game.Magic.Abilities.Unit)
        {
            Result = (AUnit.GetUnitLevel() + 1) * 2;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // PassiveAb.pas:1115
    public static int GetDefaultDispelResistance(Aow.Game.Magic.Abilities.AbstractUnit AUnit)
    {
        int Result;
        if (AUnit is Aow.Game.Magic.Abilities.Unit)
        {
            Result = AUnit.GetUnitLevel() + 3;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // PassiveAb.pas:1147
    public int UnitIndexToAIUpkeepPriority(int UnitIndex)
    {
        int Result;
        switch (Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(UnitIndex).FLevel)
        {
            case 1:
                Result = 250;
                break;
            case 2:
                Result = 250;
                break;
            case 3:
                Result = 500;
                break;
            case 4:
                Result = 1000;
                break;
            default:
                Result = 1000;
                break;
        }

        return Result;
    }

    private static readonly string SummonedRStr = "Summoned";
}

// inherited name, ability ID, selection types, ability type
public partial class FuryAbility : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1160
    public FuryAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.FuryRStr);
        FAbilityID = 156;
    }

    // PassiveAb.pas:1168
    public override sbyte GetAttack(Aow.Game.Magic.Abilities.AbilityOwner Owner) => 2;

    // PassiveAb.pas:1174
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.AbilityOwner Owner) => -1;

    // PassiveAb.pas:1180
    public override int GetAIUpkeepPriority(Aow.Game.Magic.Abilities.AbilityOwner Owner)
    {
        if (Owner is not Aow.Game.Magic.Abilities.AbstractUnit unit)
        {
            return 0;
        }

        int Result = unit.GetUnitLevel() + 200;
        if (unit is Aow.Game.Magic.Abilities.Leader)
        {
            Result -= 50;
        }

        return Result;
    }
}

// inherited name, ability ID, selection types, ability type
public partial class FreeMovementAbility : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1187
    public FreeMovementAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.FreeMovementRStr);
        FAbilityID = 157;
    }
}

// inherited name, ability ID, selection types, ability type
public partial class HolyChampionEnchantment : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1195
    public HolyChampionEnchantment(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.HolyChampionRStr);
        FAbilityID = 160;
    }
}

// inherited name, ability ID, selection types, ability type
public partial class UnholyChampionEnchantment : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1203
    public UnholyChampionEnchantment(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.UnholyChampionRStr);
        FAbilityID = 161;
    }
}

// (name, ID, selection types, ability type.)
public partial class ConcealmentEnchantment : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1287
    public ConcealmentEnchantment(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ConcealmentRStr);
        FAbilityID = 162;
    }
}

// (name, ID, selection types, ability type.)
public partial class FireAuraEnchantment : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1294
    public FireAuraEnchantment(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.FireAuraRStr);
        FAbilityID = 163;
    }

    // PassiveAb.pas:1301
    public override bool Remove(Aow.Game.Magic.Abilities.AbilityOwner owner)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool Result = base.Remove(owner);
        if (Result && owner is Aow.Game.Magic.Abilities.AbstractUnit unit && owner.Owner is Army army)
        {
            unit.GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                var mapField = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(X, Y, L);
                army.ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
            }
        }

        return Result;
    }
}

// (name, ID, selection types, ability type.)
public partial class WaterWalkingEnchantment : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1315
    public WaterWalkingEnchantment(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.WaterWalkingRStr);
        FAbilityID = 164;
    }

    // PassiveAb.pas:1322
    public override bool Remove(Aow.Game.Magic.Abilities.AbilityOwner owner)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool Result = base.Remove(owner);
        if (Result && owner is Aow.Game.Magic.Abilities.AbstractUnit unit && owner.Owner is Army army)
        {
            unit.GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                var mapField = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(X, Y, L);
                army.ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
            }
        }

        return Result;
    }
}

// (name, ID, selection types, ability type.)
public partial class WindWalkingEnchantment : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1336
    public WindWalkingEnchantment(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.WindWalkingRStr);
        FAbilityID = 165;
    }

    // PassiveAb.pas:1343
    public override bool Remove(Aow.Game.Magic.Abilities.AbilityOwner owner)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool Result = base.Remove(owner);
        if (Result && owner is Aow.Game.Magic.Abilities.AbstractUnit unit && owner.Owner is Army army)
        {
            unit.GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                var mapField = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(X, Y, L);
                army.ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
            }
        }

        return Result;
    }
}

// (name, ID, selection types, ability type.)
public partial class LiquidFormEnchantment : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1357
    public LiquidFormEnchantment(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.LiquidFormRStr);
        FAbilityID = 166;
    }

    // PassiveAb.pas:1364
    public override bool Remove(Aow.Game.Magic.Abilities.AbilityOwner owner)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool Result = base.Remove(owner);
        if (Result && owner is Aow.Game.Magic.Abilities.AbstractUnit unit && owner.Owner is Army army)
        {
            unit.GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                var mapField = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(X, Y, L);
                army.ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
            }
        }

        return Result;
    }

    // PassiveAb.pas:1378
    public override int GetAIUpkeepPriority(Aow.Game.Magic.Abilities.AbilityOwner Owner)
    {
        if (Owner is not Aow.Game.Magic.Abilities.AbstractUnit unit)
        {
            return 0;
        }

        int Result = unit.GetUnitLevel() + 300;
        if (unit is Aow.Game.Magic.Abilities.Leader)
        {
            Result += 50;
        }

        return Result;
    }
}

// inherited name, ability ID, selection types, ability type
public partial class BlessedEnchantment : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1211
    public BlessedEnchantment(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.BlessedEnchantment.BlessedRStr);
        FAbilityID = 158;
    }

    // PassiveAb.pas:1219
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.AbilityOwner Owner) => 1;

    // PassiveAb.pas:1225
    public override sbyte GetResistance(Aow.Game.Magic.Abilities.AbilityOwner Owner) => 1;

    // PassiveAb.pas:1231
    public override string GetSourceName(Aow.Game.Magic.Abilities.AbilityOwner Owner)
    {
        string Result;
        // { Original assumes the ability data and enchantment exist. }
        var unitEnchantmentAbilityData = (Aow.Game.Magic.Abilities.UnitEnchantmentAbilityData)Owner.GetAbilityData(FAbilityID);
        if (unitEnchantmentAbilityData.FEnchantment.FSource == -2)
        {
            Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.LifeMasteryRStr);
        }
        else
        {
            Result = base.GetSourceName(Owner);
        }

        return Result;
    }

    // PassiveAb.pas:1242
    public override int GetAIUpkeepPriority(Aow.Game.Magic.Abilities.AbilityOwner Owner)
    {
        if (Owner is not Aow.Game.Magic.Abilities.AbstractUnit unit)
        {
            return 0;
        }

        int Result = unit.GetUnitLevel() + 200;
        if (unit is Aow.Game.Magic.Abilities.Leader)
        {
            Result += 50;
        }

        return Result;
    }

    private static readonly string BlessedRStr = "Blessed";
}

// (name, ID, selection types, ability type.)
public partial class DarkGiftEnchantment : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1249
    public DarkGiftEnchantment(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.DarkGiftRStr);
        FAbilityID = 169;
    }

    // PassiveAb.pas:1256
    public override sbyte GetDamage(Aow.Game.Magic.Abilities.AbilityOwner Owner) => 1;

    // PassiveAb.pas:1261
    public override int GetAIUpkeepPriority(Aow.Game.Magic.Abilities.AbilityOwner Owner)
    {
        if (Owner is not Aow.Game.Magic.Abilities.AbstractUnit unit)
        {
            return 0;
        }

        int Result = unit.GetUnitLevel() + 200;
        if (unit is Aow.Game.Magic.Abilities.Leader)
        {
            Result += 50;
        }

        return Result;
    }
}

// inherited name, ability ID, selection types, ability type
public partial class FireProtectionEnchantment : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:1054
    public FireProtectionEnchantment(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.FireProtectionEnchantment.FireProtectionRStr);
        FAbilityID = 167;
    }

    // PassiveAb.pas:1062
    public override string GetSourceName(Aow.Game.Magic.Abilities.AbilityOwner Owner)
    {
        string Result;
        // { Original assumes the ability data and enchantment exist. }
        var unitEnchantmentAbilityData = (Aow.Game.Magic.Abilities.UnitEnchantmentAbilityData)Owner.GetAbilityData(FAbilityID);
        if (unitEnchantmentAbilityData.FEnchantment.FSource == -2)
        {
            Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.FireMasteryRStr);
        }
        else
        {
            Result = base.GetSourceName(Owner);
        }

        return Result;
    }

    internal static readonly string FireProtectionRStr = "Fire Protection";
}

// inherited name, ability ID, selection types, ability type
public partial class SlowEnchantmentAbility : Aow.Game.Magic.Abilities.UnitEnchantmentAbility
{
    // PassiveAb.pas:601
    public SlowEnchantmentAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.SlowRStr);
        FAbilityID = 132;
        FSelectionTypes = Set<Aow.Game.Magic.Abilities.AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:615
    public override void CombatDone(CombatObject CombatObject)
    {
        CombatObject.GetAbilityOwner().RemoveAbility(FAbilityID);
    }

    // PassiveAb.pas:609
    public override UnitEnchantment CreateEnchantment()
    {
        UnitEnchantment Result = base.CreateEnchantment();
        Result.FDispelResistance = 3;
        return Result;
    }
}
