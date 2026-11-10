using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

internal static class TeleportText
{
    internal static string[] RaceNames = ["Humans", "Azracs", "Lizards", "Frostlings", "Elves", "Halflings", "Dwarves", "Highmen", "Dark Elves", "Orcs", "Goblins", "Undead(plural)", "Independents"];

    internal static readonly string TeleportNoWarRStr = "Not at war with the %s";

    internal static readonly string TeleportWarningRStr = "Warning";

}
