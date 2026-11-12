
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

internal static class DurationText
{
    internal static string FormatTurnsLeftString(int turns)
    {
        string result;
        if (turns == 1)
        {
            result = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_turnLeftFormat), [(byte)49]);
        }
        else
        {
            result = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_turnsLeftFormat), [IntToStr(turns)]);
        }

        return result;
    }

    internal static string FormatDaysString(int days)
    {
        string result;
        if (days == 1)
        {
            result = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_dayFormat), [(byte)49]);
        }
        else
        {
            result = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_daysFormat), [IntToStr(days)]);
        }

        return result;
    }

    internal static string FormatDaysLeftString(int days)
    {
        string result;
        if (days == 1)
        {
            result = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_dayLeftFormat), [(byte)49]);
        }
        else
        {
            result = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_daysLeftFormat), [IntToStr(days)]);
        }

        return result;
    }

}
