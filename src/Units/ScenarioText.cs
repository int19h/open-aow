using Aow.Engine.Buffers;
using Aow.Game.Host;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.World.Structures;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

internal static class ScenarioText
{
    internal static readonly string ConfirmRStr = "Confirm";

    internal static readonly string PartyJoinRStr = "Party Join";

    internal static readonly string XMadeALevelRStr = "%s made a level";

    internal static readonly string XMadeYLevelsRStr = "%s made %d levels";

    internal static FixedArray_string_3 TTacticalCombatAvailabilityRStr = FixedArray_string_3.From(["Never", "Between Humans", "Always"]);

    internal static readonly string IndependentsRStr = "Independents";

    internal static FixedArray_string_4 TVictoryConditionRStr = FixedArray_string_4.From(["None", "Conquer locations", "Accumulate gold", "Accumulate mana"]);

    internal static FixedArray_string_3 TPlayerConnectionTypeRStr = FixedArray_string_3.From(["Single Computer", "Internet/Lan", "Play by email"]);

}
