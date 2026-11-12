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

    public DurationMode DurationMode { get; protected set; }

    // AoWAb.PAS:96
    public override string GetName(AbilityOwner owner)
    {
        string result = "";
        if (owner != null)
        {
            switch (DurationMode)
            {
                case DurationMode.Day:
                    result = base.GetName(owner);
                    if (GetDuration(owner) != 0)
                    {
                        var duration = GetDuration(owner);
                        var totalDuration = GetTotalDuration();
                        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.DurationDaysFormat), [IntToStr(duration), IntToStr(totalDuration)]);
                        result = result + " " + format;
                    }

                    break;
                case DurationMode.CombatTurn:
                    var duration2 = GetDuration(owner);
                    var totalDuration2 = GetTotalDuration();
                    var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.DurationCombatTurnFormat), [IntToStr(duration2), IntToStr(totalDuration2)]);
                    result = base.GetName(owner) + " " + formattedText;
                    break;
                case DurationMode.EntireCombat:
                    var name = base.GetName(owner);
                    var durationEntireCombat = AbilityRules.TranslateResourceString(GetEngine(), GameText.DurationEntireCombatText);
                    result = name + " " + durationEntireCombat;
                    break;
            }
        }
        else
        {
            // { Original leaves the string untouched for dmNone and a nonnil owner. }
            result = base.GetName(owner);
        }

        return result;
    }

    // AoWAb.PAS:40
    public override void NewDay(AbilityOwner owner)
    {
        if (StrategicMap.Current.Day == 1)
        {
            owner.RemoveAbility(AbilityId);
        }
    }

    // AoWAb.PAS:46
    public override void NewTurn(AbilityOwner owner, sbyte player)
    {
        DurationAbilityData data;
        if (DurationMode == DurationMode.Day)
        {
            data = (DurationAbilityData)owner.GetAbilityData(AbilityId);
            if (data != null)
            {
                if (data.Duration != 0 && data.PlayerIndex == player)
                {
                    data.Duration = (byte)(data.Duration - 1);
                    if (data.Duration == 0)
                    {
                        owner.RemoveAbility(AbilityId);
                    }
                }
            }
            else
            {
                owner.RemoveAbility(AbilityId);
            }
        }
    }

    // AoWAb.PAS:131
    public override bool Expand(AbilityOwner owner)
    {
        DurationAbilityData data;
        bool result = false;
        data = (DurationAbilityData)owner.GetAbilityData(AbilityId);
        if (data == null)
        {
            data = new DurationAbilityData(null) { Holder = CreationHolder() };
            data.AbilityId = (ushort)AbilityId;
            data.Duration = (byte)GetTotalDuration();
            SetAbility(owner, AbilityId, 1);
            owner.AddAbilityData(data);
            result = true;
        }
        else
        {
            if (GetTotalDuration() > data.Duration)
            {
                result = true;
                data.Duration = (byte)GetTotalDuration();
            }
        }

        return result;
    }

    // AoWAb.PAS:67
    public override void NewCombatTurn(CombatObject combatObject, sbyte player)
    {
        DurationAbilityData data;
        if (DurationMode == DurationMode.CombatTurn)
        {
            data = (DurationAbilityData)combatObject.GetAbilityOwner().GetAbilityData(AbilityId);
            if (data != null)
            {
                if (data.Duration != 0 && data.PlayerIndex == player)
                {
                    data.Duration = (byte)(data.Duration - 1);
                    if (data.Duration == 0)
                    {
                        combatObject.GetAbilityOwner().RemoveAbility(AbilityId);
                    }
                }
            }
            else
            {
                combatObject.GetAbilityOwner().RemoveAbility(AbilityId);
            }
        }
    }

    // AoWAb.PAS:88
    public override void CombatDone(CombatObject combatObject)
    {
        switch (DurationMode)
        {
            case DurationMode.CombatTurn:
            case DurationMode.EntireCombat:
                combatObject.GetAbilityOwner().RemoveAbility(AbilityId);
                break;
        }
    }

    // AoWAb.PAS:120
    public virtual byte GetDuration(AbilityOwner abilityOwner)
    {
        byte result;
        DurationAbilityData data;
        data = (DurationAbilityData)abilityOwner.GetAbilityData(AbilityId);
        if (data != null)
        {
            result = data.Duration;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWAb.PAS:35
    public virtual int GetTotalDuration() => 3;

    // AoWAb.PAS:30
    public override EngineClass AbilityDataClass() => EngineClass.Of<DurationAbilityData>();

    public override bool Remove(AbilityOwner owner)
    {
        var data = owner.GetAbilityData(AbilityId);
        SetAbility(owner, AbilityId, 0);
        if (data == null) return false;
        owner.RemoveAbilityData(data);
        return true;
    }
}
