
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

internal static class DurationText
{
    internal static string FormatTurnsLeftString(int turns)
    {
        string result;
        if (turns == 1)
        {
            result = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.TurnLeftFormat), [(byte)49]);
        }
        else
        {
            result = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.TurnsLeftFormat), [IntToStr(turns)]);
        }

        return result;
    }

    internal static string FormatDaysString(int days)
    {
        string result;
        if (days == 1)
        {
            result = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.DayFormat), [(byte)49]);
        }
        else
        {
            result = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.DaysFormat), [IntToStr(days)]);
        }

        return result;
    }

    internal static string FormatDaysLeftString(int days)
    {
        string result;
        if (days == 1)
        {
            result = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.DayLeftFormat), [(byte)49]);
        }
        else
        {
            result = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.DaysLeftFormat), [IntToStr(days)]);
        }

        return result;
    }

}
