#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.Magic.Abilities;

// (duration mode)
public class DurationAbility(CustomNode owner) : AdjustableAbility(owner)
{
    public DurationMode DurationMode { get; protected set; }

    // AoWAb.PAS:96
    public override string GetName(AbilityOwner owner)
    {
        string result = "";
        if (owner is not null)
        {
            switch (DurationMode)
            {
                case DurationMode.Day:
                    result = base.GetName(owner);
                    if (GetDuration(owner) != 0)
                    {
                        byte duration = GetDuration(owner);
                        int totalDuration = TotalDuration;
                        string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.DurationDaysFormat), [IntToStr(duration), IntToStr(totalDuration)]);
                        return $"{result} {format}";
                    }

                    break;
                case DurationMode.CombatTurn:
                    byte duration2 = GetDuration(owner);
                    int totalDuration2 = TotalDuration;
                    string formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.DurationCombatTurnFormat), [IntToStr(duration2), IntToStr(totalDuration2)]);
                    return $"{base.GetName(owner)} {formattedText}";
                case DurationMode.EntireCombat:
                    string name = base.GetName(owner);
                    string durationEntireCombat = AbilityRules.TranslateResourceString(GetEngine(), GameText.DurationEntireCombatText);
                    return $"{name} {durationEntireCombat}";
                case DurationMode.None:
                default:
                    break;
            }
        }
        else
        {
            // { Original leaves the string untouched for dmNone and a nonnil owner. }
            return base.GetName(owner);
        }

        return result;
    }

    // AoWAb.PAS:40
    public override void NewDay(AbilityOwner owner)
    {
        if (StrategicMap.Current.Day == 1)
        {
            _ = owner.RemoveAbility(AbilityId);
        }
    }

    // AoWAb.PAS:46
    public override void NewTurn(AbilityOwner owner, sbyte player)
    {
        if (DurationMode == DurationMode.Day)
        {
            DurationAbilityData data = GetData(owner);
            if (data is not null)
            {
                if (data.Duration != 0 && data.PlayerIndex == player)
                {
                    data.Duration = (byte)(data.Duration - 1);
                    if (data.Duration == 0)
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

    // AoWAb.PAS:131
    public override bool Expand(AbilityOwner owner)
    {
        bool result = false;
        DurationAbilityData data = GetData(owner);
        if (data is null)
        {
            data = new DurationAbilityData(null)
            {
                Holder = CreationHolder(),
                AbilityId = (ushort)AbilityId,
                Duration = (byte)TotalDuration
            };
            SetAbility(owner, AbilityId, 1);
            owner.AddAbilityData(data);
            return true;
        }
        else
        {
            if (TotalDuration > data.Duration)
            {
                result = true;
                data.Duration = (byte)TotalDuration;
            }
        }

        return result;
    }

    // AoWAb.PAS:67
    public override void NewCombatTurn(CombatObject combatObject, sbyte player)
    {
        if (DurationMode == DurationMode.CombatTurn)
        {
            DurationAbilityData data = GetData(combatObject.AbilityOwner);
            if (data is not null)
            {
                if (data.Duration != 0 && data.PlayerIndex == player)
                {
                    data.Duration = (byte)(data.Duration - 1);
                    if (data.Duration == 0)
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

    // AoWAb.PAS:88
    public override void CombatDone(CombatObject combatObject)
    {
        switch (DurationMode)
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

    // AoWAb.PAS:120
    public virtual byte GetDuration(AbilityOwner abilityOwner)
    {
        DurationAbilityData data = GetData(abilityOwner);
        return data is not null ? data.Duration : (byte)0;
    }

    // AoWAb.PAS:35
    public virtual int TotalDuration => 3;

    // AoWAb.PAS:30
    public override EngineClass AbilityDataClass() => EngineClass.Of<DurationAbilityData>();

    public DurationAbilityData GetData(AbilityOwner owner) => owner.GetAbilityData<DurationAbilityData>(AbilityId);

    public override bool Remove(AbilityOwner owner)
    {
        AbilityData data = owner.GetAbilityData(AbilityId);
        SetAbility(owner, AbilityId, 0);
        if (data is null)
        {
            return false;
        }

        owner.RemoveAbilityData(data);
        return true;
    }
}
