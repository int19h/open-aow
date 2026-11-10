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
public partial class TVisionAbility : Aow.Game.Magic.Abilities.TMultiLevelAbility
{
    // PassiveAb.pas:461
    public TVisionAbility(TECustomNode owner) : base(owner)
    {
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of(Aow.Game.Magic.Abilities.TAbilitySelectionType.astUnit, Aow.Game.Magic.Abilities.TAbilitySelectionType.astHeadItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astTorsoItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astDefenseItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astRingItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astEditor);
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
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TVisionAbility.VisionRStr);
                break;
            case 1:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TVisionAbility.VisionRStr) + " I";
                break;
            case 2:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TVisionAbility.VisionRStr) + " II";
                break;
            case 3:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TVisionAbility.VisionRStr) + " III";
                break;
            case 4:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TVisionAbility.VisionRStr) + " IV";
                break;
        }

        return Result;
    }

    private static readonly string VisionRStr = "Vision";
}

// inherited name, ability ID, selection types, ability type
public partial class THolyChampionAbility : Aow.Game.Magic.Abilities.TEnhancementAbility
{
    // PassiveAb.pas:484
    public THolyChampionAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.HolyChampionRStr);
        FAbilityID = 146;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of(Aow.Game.Magic.Abilities.TAbilitySelectionType.astUnit, Aow.Game.Magic.Abilities.TAbilitySelectionType.astHeadItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astTorsoItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astDefenseItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astRingItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astEditor);
    }

    // PassiveAb.pas:492
    public override bool CanExpand(Aow.Game.Magic.Abilities.TAbilityOwner owner)
    {
        bool Result = base.CanExpand(owner);
        if (Result && owner is Aow.Game.Magic.Abilities.TAbstractUnit)
        {
            Result = Aow.Game.Magic.Abilities.AlignmentTables.alsPureGood.Contains(((Aow.Game.Magic.Abilities.TAbstractUnit)owner).GetAlignment());
        }

        return Result;
    }
}

// inherited name, ability ID, selection types, ability type
public partial class TUnholyChampionAbility : Aow.Game.Magic.Abilities.TEnhancementAbility
{
    // PassiveAb.pas:499
    public TUnholyChampionAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.UnholyChampionRStr);
        FAbilityID = 147;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of(Aow.Game.Magic.Abilities.TAbilitySelectionType.astUnit, Aow.Game.Magic.Abilities.TAbilitySelectionType.astHeadItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astTorsoItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astDefenseItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astRingItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astEditor);
    }

    // PassiveAb.pas:507
    public override bool CanExpand(Aow.Game.Magic.Abilities.TAbilityOwner owner)
    {
        bool Result = base.CanExpand(owner);
        if (Result && owner is Aow.Game.Magic.Abilities.TAbstractUnit)
        {
            Result = Aow.Game.Magic.Abilities.AlignmentTables.alsPureEvil.Contains(((Aow.Game.Magic.Abilities.TAbstractUnit)owner).GetAlignment());
        }

        return Result;
    }
}

// inherited name, ability ID, selection types, ability type. duration mode
public partial class TNaturesBlessingAbility : Aow.Game.Magic.Abilities.TDurationAbility
{
    // PassiveAb.pas:514
    public TNaturesBlessingAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TNaturesBlessingAbility.NaturesBlessingRStr);
        FAbilityID = 99;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atEnchantment;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
        FDurationMode = TDurationMode.dmDay;
    }

    // PassiveAb.pas:524
    public override sbyte GetAttack(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => 1;

    // PassiveAb.pas:529
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => 1;

    // PassiveAb.pas:534
    public override sbyte GetResistance(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => 1;

    // PassiveAb.pas:539
    public override bool Expand(Aow.Game.Magic.Abilities.TAbilityOwner Owner)
    {
        bool Result = base.Expand(Owner);
        if (Owner is Aow.Game.Magic.Abilities.TAbstractUnit)
        {
            var unit = (Aow.Game.Magic.Abilities.TAbstractUnit)Owner;
            unit.SetHitPoints(unit.GetHits());
        }

        return Result;
    }

    private static readonly string NaturesBlessingRStr = "Nature's Blessing";
}

// (name, ID, selection types, ability type, duration mode.)
public partial class THighPrayerBlessingAbility : Aow.Game.Magic.Abilities.TDurationAbility
{
    // PassiveAb.pas:1267
    public THighPrayerBlessingAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.THighPrayerBlessingAbility.HighPrayerBlessingRStr);
        FAbilityID = 100;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atEnhancement;
        FDurationMode = TDurationMode.dmEntireCombat;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }

    // PassiveAb.pas:1277
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => 1;

    // PassiveAb.pas:1282
    public override sbyte GetResistance(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => 1;

    private static readonly string HighPrayerBlessingRStr = "Blessed";
}

// inherited name, ability ID, selection types, ability type, duration mode. duration data
// countdown, player index
public partial class TCrusaderAbility : Aow.Game.Magic.Abilities.TDurationAbility
{
    // PassiveAb.pas:703
    public TCrusaderAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TCrusaderAbility.CrusaderRStr);
        FAbilityID = 101;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atHandicap;
        FDurationMode = TDurationMode.dmDay;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }

    // PassiveAb.pas:724
    public override void NewTurn(Aow.Game.Magic.Abilities.TAbilityOwner Owner, sbyte Player)
    {
        Aow.Game.Magic.Abilities.TDurationAbilityData Data;
        if (FDurationMode == TDurationMode.dmDay)
        {
            Data = (Aow.Game.Magic.Abilities.TDurationAbilityData)Owner.GetAbilityData(FAbilityID);
            if (Data != null && Data.FDuration != 0 && Data.FPlayerIndex == Player)
            {
                Data.FDuration = (byte)(Data.FDuration - 1);
            }
        }
    }

    // PassiveAb.pas:713
    public override bool Remove(Aow.Game.Magic.Abilities.TAbilityOwner owner)
    {
        bool Result = base.Remove(owner);
        if (owner is Aow.Game.Magic.Abilities.TAbstractUnit unit)
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
public partial class TBloodlustAbility : Aow.Game.Magic.Abilities.TDurationAbility
{
    // PassiveAb.pas:546
    public TBloodlustAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TBloodlustAbility.BloodlustRStr);
        FAbilityID = 168;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atEnchantment;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
        FDurationMode = TDurationMode.dmDay;
    }

    // PassiveAb.pas:556
    public override sbyte GetAttack(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => 1;

    // PassiveAb.pas:561
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => -1;

    // PassiveAb.pas:566
    public override sbyte GetDamage(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => 2;

    private static readonly string BloodlustRStr = "Bloodlust";
}

// inherited name, ability ID, selection types, ability type. duration mode
public partial class TPoisonedAbility : Aow.Game.Magic.Abilities.TDurationAbility
{
    // PassiveAb.pas:571
    public TPoisonedAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TPoisonedAbility.PoisonedRStr);
        FAbilityID = 96;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atHandicap;
        FDurationMode = TDurationMode.dmDay;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }

    // PassiveAb.pas:581
    public override sbyte GetAttack(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => -1;

    // PassiveAb.pas:586
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => -1;

    // PassiveAb.pas:591
    public override sbyte GetResistance(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => -1;

    // PassiveAb.pas:596
    public override sbyte GetDamage(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => -1;

    private static readonly string PoisonedRStr = "Poisoned";
}

// inherited name, ability ID, selection types, ability type, duration mode
public partial class TDecayAbility : Aow.Game.Magic.Abilities.TDurationAbility
{
    // PassiveAb.pas:844
    public TDecayAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TDecayAbility.DecayRStr);
        FAbilityID = 131;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atHandicap;
        FDurationMode = TDurationMode.dmEntireCombat;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }

    // PassiveAb.pas:855
    public override void NewCombatTurn(TCombatObject CombatObject, sbyte Player)
    {
        Aow.Game.Magic.Abilities.TDurationAbilityData Data;
        Data = (Aow.Game.Magic.Abilities.TDurationAbilityData)CombatObject.GetAbilityOwner().GetAbilityData(FAbilityID);
        if (Data != null && Data.FPlayerIndex == Player)
        {
            if (CombatObject.GetAbilityOwner() is Aow.Game.Magic.Abilities.TAbstractUnit)
            {
                CombatObject.ExecuteDamage(1);
            }
        }
    }

    // PassiveAb.pas:868
    public override void CombatDone(TCombatObject CombatObject)
    {
        CombatObject.DestroyObject();
    }

    private static readonly string DecayRStr = "Decay";
}

// inherited name, ability ID, selection types, ability type, duration mode
public partial class TWebbedAbility : Aow.Game.Magic.Abilities.TDurationAbility
{
    // PassiveAb.pas:736
    public TWebbedAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TWebbedAbility.WebbedRStr);
        FAbilityID = 97;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atHandicap;
        FDurationMode = TDurationMode.dmCombatTurn;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }

    // PassiveAb.pas:751
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => -2;

    // PassiveAb.pas:746
    public override int GetTotalDuration() => 3;

    private static readonly string WebbedRStr = "Webbed";
}

// inherited name, ability ID, selection types, ability type, duration mode
public partial class TStunnedAbility : Aow.Game.Magic.Abilities.TDurationAbility
{
    // PassiveAb.pas:793
    public TStunnedAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TStunnedAbility.StunnedRStr);
        FAbilityID = 92;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atHandicap;
        FDurationMode = TDurationMode.dmCombatTurn;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }

    // PassiveAb.pas:810
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => -2;

    // PassiveAb.pas:804
    public override int GetTotalDuration() => 1;

    private static readonly string StunnedRStr = "Stunned";
}

// inherited name, ability ID, selection types, ability type, duration mode
public partial class TBurningAbility : Aow.Game.Magic.Abilities.TDurationAbility
{
    // PassiveAb.pas:756
    public TBurningAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TBurningAbility.BurningRStr);
        FAbilityID = 127;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atHandicap;
        FDurationMode = TDurationMode.dmCombatTurn;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }

    // PassiveAb.pas:773
    public override void NewCombatTurn(TCombatObject CombatObject, sbyte Player)
    {
        Aow.Game.Magic.Abilities.TDurationAbilityData Data;
        Data = (Aow.Game.Magic.Abilities.TDurationAbilityData)CombatObject.GetAbilityOwner().GetAbilityData(FAbilityID);
        if (Data != null)
        {
            if (Data.FDuration != 0 && Data.FPlayerIndex == Player)
            {
                Data.FDuration = (byte)(Data.FDuration - 1);
                if (Data.FDuration == 0)
                {
                    CombatObject.GetAbilityOwner().RemoveAbility(FAbilityID);
                }

                if (CombatObject.GetAbilityOwner() is Aow.Game.Magic.Abilities.TAbstractUnit unit)
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
public partial class TVertigoAbility : Aow.Game.Magic.Abilities.TDurationAbility
{
    // PassiveAb.pas:952
    public TVertigoAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TVertigoAbility.VertigoRStr);
        FAbilityID = 98;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atHandicap;
        FDurationMode = TDurationMode.dmDay;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }

    // PassiveAb.pas:969
    public override sbyte GetAttack(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => -2;

    // PassiveAb.pas:975
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => -2;

    // PassiveAb.pas:963
    public override int GetTotalDuration() => 3;

    private static readonly string VertigoRStr = "Vertigo";
}

// inherited name, ability ID, selection types, ability type, duration mode
public partial class TCursedAbility : Aow.Game.Magic.Abilities.TDurationAbility
{
    // PassiveAb.pas:981
    public TCursedAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TCursedAbility.CursedRStr);
        FAbilityID = 93;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
        FDurationMode = TDurationMode.dmDay;
    }

    // PassiveAb.pas:1009
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => -2;

    // PassiveAb.pas:1015
    public override sbyte GetResistance(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => -2;

    // PassiveAb.pas:991
    public override string GetSourceName(Aow.Game.Magic.Abilities.TAbilityOwner Owner)
    {
        string Result;
        Aow.Game.Magic.Abilities.TDurationAbilityData Data;
        Data = (Aow.Game.Magic.Abilities.TDurationAbilityData)Owner.GetAbilityData(FAbilityID);
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
public partial class THolyFear : Aow.Game.Magic.Abilities.TDurationAbility
{
    // PassiveAb.pas:816
    public THolyFear(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.THolyFear.HolyFearRStr);
        FAbilityID = 53;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atHandicap;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
        FDurationMode = TDurationMode.dmDay;
    }

    // PassiveAb.pas:833
    public override string GetSourceName(Aow.Game.Magic.Abilities.TAbilityOwner Owner)
    {
        string Result;
        // { Original assumes the ability data exists. }
        var durationAbilityData = (Aow.Game.Magic.Abilities.TDurationAbilityData)Owner.GetAbilityData(FAbilityID);
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
public partial class TPanickedAbility : Aow.Game.Magic.Abilities.TDurationAbility
{
    // PassiveAb.pas:874
    public TPanickedAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TPanickedAbility.PanickedRStr);
        FAbilityID = 108;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atHandicap;
        FDurationMode = TDurationMode.dmEntireCombat;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }

    // PassiveAb.pas:885
    public override bool Expand(Aow.Game.Magic.Abilities.TAbilityOwner Owner)
    {
        bool Result = base.Expand(Owner);
        if (Owner is Aow.Game.Magic.Abilities.TAbstractUnit)
        {
            Owner.Changed();
        }

        return Result;
    }

    private static readonly string PanickedRStr = "Panicked";
}

// inherited name, ability ID, selection types, ability type
public partial class TWallClimbingAbility : Aow.Game.Magic.Abilities.TAdjustableAbility
{
    // PassiveAb.pas:922
    public TWallClimbingAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TWallClimbingAbility.WallClimbingRStr);
        FAbilityID = 36;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of(Aow.Game.Magic.Abilities.TAbilitySelectionType.astUnit, Aow.Game.Magic.Abilities.TAbilitySelectionType.astHeadItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astTorsoItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astDefenseItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astRingItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astEditor);
    }

    // PassiveAb.pas:931
    public override TSet<TWallCombatFeaturesElement> GetWallCombatFeatures(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => TSet<TWallCombatFeaturesElement>.Of(TWallCombatFeaturesElement.wcWallPassing, TWallCombatFeaturesElement.wcParticipate);

    private static readonly string WallClimbingRStr = "Wall Climbing";
}

// inherited name, ability ID, selection types, ability type
public partial class TPassWallAbility : Aow.Game.Magic.Abilities.TAdjustableAbility
{
    // PassiveAb.pas:937
    public TPassWallAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TPassWallAbility.PassWallRStr);
        FAbilityID = 66;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of(Aow.Game.Magic.Abilities.TAbilitySelectionType.astUnit, Aow.Game.Magic.Abilities.TAbilitySelectionType.astHeadItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astTorsoItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astDefenseItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astRingItem);
    }

    // PassiveAb.pas:946
    public override TSet<TWallCombatFeaturesElement> GetWallCombatFeatures(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => TSet<TWallCombatFeaturesElement>.Of(TWallCombatFeaturesElement.wcWallPassing, TWallCombatFeaturesElement.wcParticipate);

    private static readonly string PassWallRStr = "Pass Wall";
}

// inherited name, ability ID, selection types, ability type
public partial class TResurrectedAbility : Aow.Game.Magic.Abilities.TAdjustableAbility
{
    // PassiveAb.pas:892
    public TResurrectedAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ResurrectedRStr);
        FAbilityID = 128;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atHandicap;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }
}

// inherited name, ability ID, selection types, ability type
public partial class TAnimatedAbility : Aow.Game.Magic.Abilities.TAdjustableAbility
{
    // PassiveAb.pas:902
    public TAnimatedAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TAnimatedAbility.AnimatedRStr);
        FAbilityID = 129;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atHandicap;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }

    private static readonly string AnimatedRStr = "Animated";
}

// inherited name, ability ID, selection types, ability type
public partial class TCombatResurrectedAbility : Aow.Game.Magic.Abilities.TAdjustableAbility
{
    // PassiveAb.pas:912
    public TCombatResurrectedAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ResurrectedRStr);
        FAbilityID = 130;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atHandicap;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }
}

// (maximum level, level costs.)
public partial class TMarksmanshipAbility : Aow.Game.Magic.Abilities.TMultiLevelAbility
{
    // PassiveAb.pas:1384
    public TMarksmanshipAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TMarksmanshipAbility.MarksmanshipRStr);
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atEnhancement;
        FAbilityID = 32;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of(Aow.Game.Magic.Abilities.TAbilitySelectionType.astUnit, Aow.Game.Magic.Abilities.TAbilitySelectionType.astHeadItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astTorsoItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astDefenseItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astRingItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astEditor);
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
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TMarksmanshipAbility.MarksmanshipRStr);
                break;
            case 1:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TMarksmanshipAbility.MarksmanshipRStr) + " I";
                break;
            case 2:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TMarksmanshipAbility.MarksmanshipRStr) + " II";
                break;
            case 3:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TMarksmanshipAbility.MarksmanshipRStr) + " III";
                break;
            case 4:
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TMarksmanshipAbility.MarksmanshipRStr) + " IV";
                break;
        }

        return Result;
    }

    private static readonly string MarksmanshipRStr = "Marksmanship";
}

// (FMoveTypes.)
public partial class TMovementAbility : Aow.Game.Magic.Abilities.TAdjustableAbility
{
    public TSet<TMoveType> FMoveTypes;

    // PassiveAb.pas:1409
    public TMovementAbility(TECustomNode owner) : base(owner)
    {
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of(Aow.Game.Magic.Abilities.TAbilitySelectionType.astUnit, Aow.Game.Magic.Abilities.TAbilitySelectionType.astRingItem);
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atMovement;
        FSkillPointCost = 0;
    }

    // PassiveAb.pas:1417
    public override TSet<TMoveType> GetMoveTypes(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => FMoveTypes;

    // PassiveAb.pas:1422
    public override TSet<TWallCombatFeaturesElement> GetWallCombatFeatures(Aow.Game.Magic.Abilities.TAbilityOwner Owner)
    {
        TSet<TWallCombatFeaturesElement> Result;
        if (FAbilityID == 1)
        {
            Result = TSet<TWallCombatFeaturesElement>.Of(TWallCombatFeaturesElement.wcWallPassing, TWallCombatFeaturesElement.wcParticipate);
        }
        else
        {
            Result = TSet<TWallCombatFeaturesElement>.Of();
        }

        return Result;
    }
}

// (FImmunityTypes.)
public partial class TImmunityAbility : Aow.Game.Magic.Abilities.TEnhancementAbility
{
    public TSet<TDamageType> FImmunityTypes;

    // PassiveAb.pas:1428
    public TImmunityAbility(TECustomNode owner) : base(owner)
    {
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of(Aow.Game.Magic.Abilities.TAbilitySelectionType.astUnit, Aow.Game.Magic.Abilities.TAbilitySelectionType.astDefenseItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astRingItem);
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atEnhancement;
    }

    // PassiveAb.pas:1440
    public override TSet<TMoveType> GetMoveTypes(Aow.Game.Magic.Abilities.TAbilityOwner Owner)
    {
        TSet<TMoveType> Result;
        if (FImmunityTypes.Contains(TDamageType.dtFire))
        {
            Result = TSet<TMoveType>.Of(TMoveType.mtLavaCrossing);
        }
        else
        {
            Result = TSet<TMoveType>.Of();
        }

        return Result;
    }

    // PassiveAb.pas:1435
    public override TSet<TDamageType> GetImmunityTypes(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => FImmunityTypes;
}

// (FProtectionTypes.)
public partial class TProtectionAbility : Aow.Game.Magic.Abilities.TEnhancementAbility
{
    public TSet<TDamageType> FProtectionTypes;

    // PassiveAb.pas:1446
    public TProtectionAbility(TECustomNode owner) : base(owner)
    {
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of(Aow.Game.Magic.Abilities.TAbilitySelectionType.astUnit, Aow.Game.Magic.Abilities.TAbilitySelectionType.astDefenseItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astRingItem, Aow.Game.Magic.Abilities.TAbilitySelectionType.astEditor);
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atEnhancement;
    }

    // PassiveAb.pas:1453
    public override TSet<TDamageType> GetProtectionTypes(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => FProtectionTypes;
}

// inherited name, ability ID, selection types, ability type
public partial class TEntangledAbility : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:620
    public TEntangledAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TEntangledAbility.EntangledRStr);
        FAbilityID = 94;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }

    // PassiveAb.pas:638
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => -2;

    // PassiveAb.pas:628
    public override TDurationMode GetDurationMode() => TDurationMode.dmCombatTurn;

    // PassiveAb.pas:633
    public override sbyte GetTotalDuration() => 3;

    // PassiveAb.pas:643
    public override TUnitEnchantment CreateEnchantment()
    {
        TUnitEnchantment Result = base.CreateEnchantment();
        Result.FDispelResistance = 3;
        return Result;
    }

    private static readonly string EntangledRStr = "Entangled";
}

// inherited name, ability ID, selection types, ability type
public partial class TFrozenAbility : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:649
    public TFrozenAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TFrozenAbility.FrozenRStr);
        FAbilityID = 95;
        FAbilityType = Aow.Game.Magic.Abilities.TAbilityType.atEnchantment;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }

    // PassiveAb.pas:674
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => -2;

    // PassiveAb.pas:658
    public override TDurationMode GetDurationMode() => TDurationMode.dmCombatTurn;

    // PassiveAb.pas:663
    public override sbyte GetTotalDuration() => 2;

    // PassiveAb.pas:668
    public override TUnitEnchantment CreateEnchantment()
    {
        TUnitEnchantment Result = base.CreateEnchantment();
        Result.FDispelResistance = 3;
        return Result;
    }

    private static readonly string FrozenRStr = "Frozen";
}

// inherited name, ability ID, selection types, ability type
public partial class TTurnedUndeadAbility : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:679
    public TTurnedUndeadAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TTurnedUndeadAbility.TurnedUndeadRStr);
        FAbilityID = 34;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }

    // PassiveAb.pas:693
    public override TDurationMode GetDurationMode() => TDurationMode.dmCombatTurn;

    // PassiveAb.pas:698
    public override sbyte GetTotalDuration() => 3;

    // PassiveAb.pas:687
    public override TUnitEnchantment CreateEnchantment()
    {
        TUnitEnchantment Result = base.CreateEnchantment();
        Result.FDispelResistance = 4;
        return Result;
    }

    private static readonly string TurnedUndeadRStr = "Turned Undead";
}

// inherited name, ability ID, selection types, ability type
public partial class THasteAbility : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1021
    public THasteAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.HasteRStr);
        FAbilityID = 152;
    }
}

// inherited name, ability ID, selection types, ability type
public partial class TStoneSkinAbility : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1029
    public TStoneSkinAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.StoneSkinRStr);
        FAbilityID = 153;
    }

    // PassiveAb.pas:1048
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => 2;

    // PassiveAb.pas:1037
    public override string GetSourceName(Aow.Game.Magic.Abilities.TAbilityOwner Owner)
    {
        string Result;
        // { Original assumes the ability data and enchantment exist. }
        var unitEnchantmentAbilityData = (Aow.Game.Magic.Abilities.TUnitEnchantmentAbilityData)Owner.GetAbilityData(FAbilityID);
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
public partial class TEnchantedWeaponAbility : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1073
    public TEnchantedWeaponAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TEnchantedWeaponAbility.EnchantedWeaponRStr);
        FAbilityID = 154;
    }

    // PassiveAb.pas:1081
    public override sbyte GetAttack(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => 1;

    // PassiveAb.pas:1087
    public override sbyte GetDamage(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => 1;

    // PassiveAb.pas:1093
    public override int GetAIUpkeepPriority(Aow.Game.Magic.Abilities.TAbilityOwner Owner)
    {
        if (Owner is not Aow.Game.Magic.Abilities.TAbstractUnit unit)
        {
            return 0;
        }

        int Result = unit.GetUnitLevel() + 225;
        if (unit is Aow.Game.Magic.Abilities.TLeader)
        {
            Result += 50;
        }

        return Result;
    }

    private static readonly string EnchantedWeaponRStr = "Enchanted Weapon";
}

// inherited name, ability ID, selection types, ability type
public partial class TSummonedAbility : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1100
    public TSummonedAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TSummonedAbility.SummonedRStr);
        FAbilityID = 155;
    }

    // PassiveAb.pas:1122
    public override bool Remove(Aow.Game.Magic.Abilities.TAbilityOwner owner)
    {
        bool Result = base.Remove(owner);
        if (Result && owner is Aow.Game.Magic.Abilities.TAbstractUnit unit)
        {
            unit.Killed();
            unit.SetOwner(null);
        }

        return Result;
    }

    // PassiveAb.pas:1134
    public override int GetAIUpkeepPriority(Aow.Game.Magic.Abilities.TAbilityOwner Owner)
    {
        if (Owner is not Aow.Game.Magic.Abilities.TAbstractUnit unit)
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
    public static int GetDefaultManaUpkeep(Aow.Game.Magic.Abilities.TAbstractUnit AUnit)
    {
        int Result;
        if (AUnit is Aow.Game.Magic.Abilities.TUnit)
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
    public static int GetDefaultDispelResistance(Aow.Game.Magic.Abilities.TAbstractUnit AUnit)
    {
        int Result;
        if (AUnit is Aow.Game.Magic.Abilities.TUnit)
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
        switch (Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(UnitIndex).FLevel)
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
public partial class TFuryAbility : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1160
    public TFuryAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.FuryRStr);
        FAbilityID = 156;
    }

    // PassiveAb.pas:1168
    public override sbyte GetAttack(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => 2;

    // PassiveAb.pas:1174
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => -1;

    // PassiveAb.pas:1180
    public override int GetAIUpkeepPriority(Aow.Game.Magic.Abilities.TAbilityOwner Owner)
    {
        if (Owner is not Aow.Game.Magic.Abilities.TAbstractUnit unit)
        {
            return 0;
        }

        int Result = unit.GetUnitLevel() + 200;
        if (unit is Aow.Game.Magic.Abilities.TLeader)
        {
            Result -= 50;
        }

        return Result;
    }
}

// inherited name, ability ID, selection types, ability type
public partial class TFreeMovementAbility : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1187
    public TFreeMovementAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.FreeMovementRStr);
        FAbilityID = 157;
    }
}

// inherited name, ability ID, selection types, ability type
public partial class THolyChampionEnchantment : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1195
    public THolyChampionEnchantment(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.HolyChampionRStr);
        FAbilityID = 160;
    }
}

// inherited name, ability ID, selection types, ability type
public partial class TUnholyChampionEnchantment : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1203
    public TUnholyChampionEnchantment(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.UnholyChampionRStr);
        FAbilityID = 161;
    }
}

// (name, ID, selection types, ability type.)
public partial class TConcealmentEnchantment : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1287
    public TConcealmentEnchantment(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ConcealmentRStr);
        FAbilityID = 162;
    }
}

// (name, ID, selection types, ability type.)
public partial class TFireAuraEnchantment : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1294
    public TFireAuraEnchantment(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.FireAuraRStr);
        FAbilityID = 163;
    }

    // PassiveAb.pas:1301
    public override bool Remove(Aow.Game.Magic.Abilities.TAbilityOwner owner)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool Result = base.Remove(owner);
        if (Result && owner is Aow.Game.Magic.Abilities.TAbstractUnit unit && owner.Owner is TArmy army)
        {
            unit.GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                var mapField = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(X, Y, L);
                army.ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
            }
        }

        return Result;
    }
}

// (name, ID, selection types, ability type.)
public partial class TWaterWalkingEnchantment : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1315
    public TWaterWalkingEnchantment(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.WaterWalkingRStr);
        FAbilityID = 164;
    }

    // PassiveAb.pas:1322
    public override bool Remove(Aow.Game.Magic.Abilities.TAbilityOwner owner)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool Result = base.Remove(owner);
        if (Result && owner is Aow.Game.Magic.Abilities.TAbstractUnit unit && owner.Owner is TArmy army)
        {
            unit.GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                var mapField = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(X, Y, L);
                army.ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
            }
        }

        return Result;
    }
}

// (name, ID, selection types, ability type.)
public partial class TWindWalkingEnchantment : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1336
    public TWindWalkingEnchantment(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.WindWalkingRStr);
        FAbilityID = 165;
    }

    // PassiveAb.pas:1343
    public override bool Remove(Aow.Game.Magic.Abilities.TAbilityOwner owner)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool Result = base.Remove(owner);
        if (Result && owner is Aow.Game.Magic.Abilities.TAbstractUnit unit && owner.Owner is TArmy army)
        {
            unit.GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                var mapField = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(X, Y, L);
                army.ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
            }
        }

        return Result;
    }
}

// (name, ID, selection types, ability type.)
public partial class TLiquidFormEnchantment : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1357
    public TLiquidFormEnchantment(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.LiquidFormRStr);
        FAbilityID = 166;
    }

    // PassiveAb.pas:1364
    public override bool Remove(Aow.Game.Magic.Abilities.TAbilityOwner owner)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool Result = base.Remove(owner);
        if (Result && owner is Aow.Game.Magic.Abilities.TAbstractUnit unit && owner.Owner is TArmy army)
        {
            unit.GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                var mapField = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(X, Y, L);
                army.ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
            }
        }

        return Result;
    }

    // PassiveAb.pas:1378
    public override int GetAIUpkeepPriority(Aow.Game.Magic.Abilities.TAbilityOwner Owner)
    {
        if (Owner is not Aow.Game.Magic.Abilities.TAbstractUnit unit)
        {
            return 0;
        }

        int Result = unit.GetUnitLevel() + 300;
        if (unit is Aow.Game.Magic.Abilities.TLeader)
        {
            Result += 50;
        }

        return Result;
    }
}

// inherited name, ability ID, selection types, ability type
public partial class TBlessedEnchantment : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1211
    public TBlessedEnchantment(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TBlessedEnchantment.BlessedRStr);
        FAbilityID = 158;
    }

    // PassiveAb.pas:1219
    public override sbyte GetDefense(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => 1;

    // PassiveAb.pas:1225
    public override sbyte GetResistance(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => 1;

    // PassiveAb.pas:1231
    public override string GetSourceName(Aow.Game.Magic.Abilities.TAbilityOwner Owner)
    {
        string Result;
        // { Original assumes the ability data and enchantment exist. }
        var unitEnchantmentAbilityData = (Aow.Game.Magic.Abilities.TUnitEnchantmentAbilityData)Owner.GetAbilityData(FAbilityID);
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
    public override int GetAIUpkeepPriority(Aow.Game.Magic.Abilities.TAbilityOwner Owner)
    {
        if (Owner is not Aow.Game.Magic.Abilities.TAbstractUnit unit)
        {
            return 0;
        }

        int Result = unit.GetUnitLevel() + 200;
        if (unit is Aow.Game.Magic.Abilities.TLeader)
        {
            Result += 50;
        }

        return Result;
    }

    private static readonly string BlessedRStr = "Blessed";
}

// (name, ID, selection types, ability type.)
public partial class TDarkGiftEnchantment : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1249
    public TDarkGiftEnchantment(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.DarkGiftRStr);
        FAbilityID = 169;
    }

    // PassiveAb.pas:1256
    public override sbyte GetDamage(Aow.Game.Magic.Abilities.TAbilityOwner Owner) => 1;

    // PassiveAb.pas:1261
    public override int GetAIUpkeepPriority(Aow.Game.Magic.Abilities.TAbilityOwner Owner)
    {
        if (Owner is not Aow.Game.Magic.Abilities.TAbstractUnit unit)
        {
            return 0;
        }

        int Result = unit.GetUnitLevel() + 200;
        if (unit is Aow.Game.Magic.Abilities.TLeader)
        {
            Result += 50;
        }

        return Result;
    }
}

// inherited name, ability ID, selection types, ability type
public partial class TFireProtectionEnchantment : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:1054
    public TFireProtectionEnchantment(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.Passive.TFireProtectionEnchantment.FireProtectionRStr);
        FAbilityID = 167;
    }

    // PassiveAb.pas:1062
    public override string GetSourceName(Aow.Game.Magic.Abilities.TAbilityOwner Owner)
    {
        string Result;
        // { Original assumes the ability data and enchantment exist. }
        var unitEnchantmentAbilityData = (Aow.Game.Magic.Abilities.TUnitEnchantmentAbilityData)Owner.GetAbilityData(FAbilityID);
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
public partial class TSlowEnchantmentAbility : Aow.Game.Magic.Abilities.TUnitEnchantmentAbility
{
    // PassiveAb.pas:601
    public TSlowEnchantmentAbility(TECustomNode owner) : base(owner)
    {
        FAbilityName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.SlowRStr);
        FAbilityID = 132;
        FSelectionTypes = TSet<Aow.Game.Magic.Abilities.TAbilitySelectionType>.Of();
    }

    // PassiveAb.pas:615
    public override void CombatDone(TCombatObject CombatObject)
    {
        CombatObject.GetAbilityOwner().RemoveAbility(FAbilityID);
    }

    // PassiveAb.pas:609
    public override TUnitEnchantment CreateEnchantment()
    {
        TUnitEnchantment Result = base.CreateEnchantment();
        Result.FDispelResistance = 3;
        return Result;
    }
}
