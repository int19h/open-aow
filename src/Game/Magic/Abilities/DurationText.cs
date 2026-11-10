
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

internal static class DurationText
{
    internal static string FmtTurnsLeftStr(int Turns)
    {
        string Result;
        if (Turns == 1)
        {
            Result = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.XTurnLeftRStr), [(byte)49]);
        }
        else
        {
            Result = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.XTurnsLeftRStr), [IntToStr(Turns)]);
        }

        return Result;
    }

    internal static string FmtDaysStr(int Days)
    {
        string Result;
        if (Days == 1)
        {
            Result = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.XDayRStr), [(byte)49]);
        }
        else
        {
            Result = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.XDaysRStr), [IntToStr(Days)]);
        }

        return Result;
    }

    internal static string FmtDaysLeftStr(int Days)
    {
        string Result;
        if (Days == 1)
        {
            Result = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.XDayLeftRStr), [(byte)49]);
        }
        else
        {
            Result = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.XDaysLeftRStr), [IntToStr(Days)]);
        }

        return Result;
    }

}
