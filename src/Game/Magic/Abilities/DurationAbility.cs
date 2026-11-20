#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.Magic.Abilities;

// (duration mode)
public partial class DurationAbility(CustomNode owner) : AdjustableAbility(owner)
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
                        int totalDuration = GetTotalDuration();
                        string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.DurationDaysFormat), [IntToStr(duration), IntToStr(totalDuration)]);
                        result = $"{result} {format}";
                    }

                    break;
                case DurationMode.CombatTurn:
                    byte duration2 = GetDuration(owner);
                    int totalDuration2 = GetTotalDuration();
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
                Duration = (byte)GetTotalDuration()
            };
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
        if (DurationMode == DurationMode.CombatTurn)
        {
            DurationAbilityData data = GetData(combatObject.GetAbilityOwner());
            if (data is not null)
            {
                if (data.Duration != 0 && data.PlayerIndex == player)
                {
                    data.Duration = (byte)(data.Duration - 1);
                    if (data.Duration == 0)
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

    // AoWAb.PAS:88
    public override void CombatDone(CombatObject combatObject)
    {
        switch (DurationMode)
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

    // AoWAb.PAS:120
    public virtual byte GetDuration(AbilityOwner abilityOwner)
    {
        DurationAbilityData data = GetData(abilityOwner);
        byte result = data is not null ? data.Duration : (byte)0;

        return result;
    }

    // AoWAb.PAS:35
    public virtual int GetTotalDuration() => 3;

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
