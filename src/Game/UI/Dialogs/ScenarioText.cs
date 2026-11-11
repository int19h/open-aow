using Aow.Engine.Buffers;

namespace Aow.Game.UI.Dialogs;

internal static class ScenarioText
{
    internal static readonly string ConfirmRStr = "Confirm";

    internal static readonly string PartyJoinRStr = "Party Join";

    internal static readonly string XMadeALevelRStr = "%s made a level";

    internal static readonly string XMadeYLevelsRStr = "%s made %d levels";

    internal static StringArray3 TTacticalCombatAvailabilityRStr = StringArray3.From(["Never", "Between Humans", "Always"]);

    internal static readonly string IndependentsRStr = "Independents";

    internal static StringArray4 TVictoryConditionRStr = StringArray4.From(["None", "Conquer locations", "Accumulate gold", "Accumulate mana"]);

    internal static StringArray3 TPlayerConnectionTypeRStr = StringArray3.From(["Single Computer", "Internet/Lan", "Play by email"]);

}
