
using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.Magic.Abilities;

internal static class DurationText
{
    internal static string FormatTurnsLeftString(GameEngine engine, int turns)
    {
        return turns == 1
            ? AbilityRules.Format(engine.TranslateResourceString(GameText.TurnLeftFormat), arguments: [(byte)49])
            : AbilityRules.Format(engine.TranslateResourceString(GameText.TurnsLeftFormat), arguments: [IntToStr(turns)]);
    }

    internal static string FormatDaysString(GameEngine engine, int days)
    {
        return days == 1
            ? AbilityRules.Format(engine.TranslateResourceString(GameText.DayFormat), arguments: [(byte)49])
            : AbilityRules.Format(engine.TranslateResourceString(GameText.DaysFormat), arguments: [IntToStr(days)]);
    }

    internal static string FormatDaysLeftString(GameEngine engine, int days)
    {
        return days == 1
            ? AbilityRules.Format(engine.TranslateResourceString(GameText.DayLeftFormat), arguments: [(byte)49])
            : AbilityRules.Format(engine.TranslateResourceString(GameText.DaysLeftFormat), arguments: [IntToStr(days)]);
    }
}
