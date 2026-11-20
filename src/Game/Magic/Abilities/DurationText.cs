
using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.Magic.Abilities;

internal static class DurationText
{
    internal static string FormatTurnsLeftString(GameEngine engine, int turns)
    {
        return turns == 1
            ? AbilityRules.Format(engine.TranslateResourceString(GameText.TurnLeftFormat), [(byte)49])
            : AbilityRules.Format(engine.TranslateResourceString(GameText.TurnsLeftFormat), [IntToStr(turns)]);
    }

    internal static string FormatDaysString(GameEngine engine, int days)
    {
        return days == 1
            ? AbilityRules.Format(engine.TranslateResourceString(GameText.DayFormat), [(byte)49])
            : AbilityRules.Format(engine.TranslateResourceString(GameText.DaysFormat), [IntToStr(days)]);
    }

    internal static string FormatDaysLeftString(GameEngine engine, int days)
    {
        return days == 1
            ? AbilityRules.Format(engine.TranslateResourceString(GameText.DayLeftFormat), [(byte)49])
            : AbilityRules.Format(engine.TranslateResourceString(GameText.DaysLeftFormat), [IntToStr(days)]);
    }

}
