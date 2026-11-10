using System.Runtime.InteropServices;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.World.Terrain;

internal static class CaveText
{
    internal static string[] RaceNames = ["Humans", "Azracs", "Lizards", "Frostlings", "Elves", "Halflings", "Dwarves", "Highmen", "Dark Elves", "Orcs", "Goblins", "Undead(plural)", "Independents"];

    internal static readonly string CaveNoWarRStr = "Not at war with the %s";

}
