
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

internal static class DurationText
{
    internal static string FormatTurnsLeftString(GameEngine engine, int turns)
    {
        string result;
        if (turns == 1)
        {
            result = AbilityRules.Format(engine.TranslateResourceString(GameText.TurnLeftFormat), [(byte)49]);
        }
        else
        {
            result = AbilityRules.Format(engine.TranslateResourceString(GameText.TurnsLeftFormat), [IntToStr(turns)]);
        }

        return result;
    }

    internal static string FormatDaysString(GameEngine engine, int days)
    {
        string result;
        if (days == 1)
        {
            result = AbilityRules.Format(engine.TranslateResourceString(GameText.DayFormat), [(byte)49]);
        }
        else
        {
            result = AbilityRules.Format(engine.TranslateResourceString(GameText.DaysFormat), [IntToStr(days)]);
        }

        return result;
    }

    internal static string FormatDaysLeftString(GameEngine engine, int days)
    {
        string result;
        if (days == 1)
        {
            result = AbilityRules.Format(engine.TranslateResourceString(GameText.DayLeftFormat), [(byte)49]);
        }
        else
        {
            result = AbilityRules.Format(engine.TranslateResourceString(GameText.DaysLeftFormat), [IntToStr(days)]);
        }

        return result;
    }

}
