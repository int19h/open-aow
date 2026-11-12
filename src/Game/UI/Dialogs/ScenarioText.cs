using Aow.Engine.Buffers;

namespace Aow.Game.UI.Dialogs;

internal static class ScenarioText
{
    internal static readonly string s_confirmText = "Confirm";

    internal static readonly string s_partyJoinText = "Party Join";

    internal static readonly string s_madeLevelFormat = "%s made a level";

    internal static readonly string s_madeLevelsFormat = "%s made %d levels";

    internal static StringArray3 s_tacticalCombatAvailabilityTexts = StringArray3.From(["Never", "Between Humans", "Always"]);

    internal static readonly string s_independentsText = "Independents";

    internal static StringArray4 s_victoryConditionTexts = StringArray4.From(["None", "Conquer locations", "Accumulate gold", "Accumulate mana"]);

    internal static StringArray3 s_playerConnectionTypeTexts = StringArray3.From(["Single Computer", "Internet/Lan", "Play by email"]);

}
