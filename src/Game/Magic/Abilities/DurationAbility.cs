// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

// (duration mode)
public partial class DurationAbility : AdjustableAbility
{

    // AoWAb.PAS:25
    public DurationAbility(ECustomNode owner) : base(owner)
    {

    }

    public DurationMode FDurationMode;

    // AoWAb.PAS:96
    public override string GetName(AbilityOwner Owner)
    {
        string Result = "";
        if (Owner != null)
        {
            switch (FDurationMode)
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
            }
        }
        else
        {
            // { Original leaves the string untouched for dmNone and a nonnil owner. }
            Result = base.GetName(Owner);
        }

        return Result;
    }

    // AoWAb.PAS:40
    public override void NewDay(AbilityOwner Owner)
    {
        if (AoWHSMap.Current.FDay == 1)
        {
            Owner.RemoveAbility(FAbilityID);
        }
    }

    // AoWAb.PAS:46
    public override void NewTurn(AbilityOwner Owner, sbyte Player)
    {
        DurationAbilityData Data;
        if (FDurationMode == DurationMode.Day)
        {
            Data = (DurationAbilityData)Owner.GetAbilityData(FAbilityID);
            if (Data != null)
            {
                if (Data.FDuration != 0 && Data.FPlayerIndex == Player)
                {
                    Data.FDuration = (byte)(Data.FDuration - 1);
                    if (Data.FDuration == 0)
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

    // AoWAb.PAS:131
    public override bool Expand(AbilityOwner Owner)
    {
        DurationAbilityData Data;
        bool Result = false;
        Data = (DurationAbilityData)Owner.GetAbilityData(FAbilityID);
        if (Data == null)
        {
            Data = new DurationAbilityData(null);
            Data.FAbilityID = (ushort)FAbilityID;
            Data.FDuration = (byte)GetTotalDuration();
            SetAb(Owner, FAbilityID, 1);
            Owner.AddAbilityData(Data);
            Result = true;
        }
        else
        {
            if (GetTotalDuration() > Data.FDuration)
            {
                Result = true;
                Data.FDuration = (byte)GetTotalDuration();
            }
        }

        return Result;
    }

    // AoWAb.PAS:67
    public override void NewCombatTurn(CombatObject CombatObject, sbyte Player)
    {
        DurationAbilityData Data;
        if (FDurationMode == DurationMode.CombatTurn)
        {
            Data = (DurationAbilityData)CombatObject.GetAbilityOwner().GetAbilityData(FAbilityID);
            if (Data != null)
            {
                if (Data.FDuration != 0 && Data.FPlayerIndex == Player)
                {
                    Data.FDuration = (byte)(Data.FDuration - 1);
                    if (Data.FDuration == 0)
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

    // AoWAb.PAS:88
    public override void CombatDone(CombatObject CombatObject)
    {
        switch (FDurationMode)
        {
            case DurationMode.CombatTurn:
            case DurationMode.EntireCombat:
                CombatObject.GetAbilityOwner().RemoveAbility(FAbilityID);
                break;
        }
    }

    // AoWAb.PAS:120
    public virtual byte GetDuration(AbilityOwner AbilityOwner)
    {
        byte Result;
        DurationAbilityData Data;
        Data = (DurationAbilityData)AbilityOwner.GetAbilityData(FAbilityID);
        if (Data != null)
        {
            Result = Data.FDuration;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:35
    public virtual int GetTotalDuration() => 3;

    // AoWAb.PAS:30
    public override EClass AbilityDataClass() => EClass.Of<DurationAbilityData>();

    public override bool Remove(AbilityOwner owner)
    {
        var data = owner.GetAbilityData(FAbilityID);
        SetAb(owner, FAbilityID, 0);
        if (data == null) return false;
        owner.RemoveAbilityData(data);
        return true;
    }
}
