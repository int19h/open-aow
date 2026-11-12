using Aow.Engine.Buffers;

namespace Aow.Game.UI.Dialogs;

internal static class ScenarioText
{
    internal static string ConfirmText { get; } = "Confirm";

    internal static string PartyJoinText { get; } = "Party Join";

    internal static string MadeLevelFormat { get; } = "%s made a level";

    internal static string MadeLevelsFormat { get; } = "%s made %d levels";

    internal static StringArray3 TacticalCombatAvailabilityTexts { get; } = StringArray3.From(["Never", "Between Humans", "Always"]);

    internal static string IndependentsText { get; } = "Independents";

    internal static StringArray4 VictoryConditionTexts { get; } = StringArray4.From(["None", "Conquer locations", "Accumulate gold", "Accumulate mana"]);

    internal static StringArray3 PlayerConnectionTypeTexts { get; } = StringArray3.From(["Single Computer", "Internet/Lan", "Play by email"]);

}
