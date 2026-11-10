using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

internal static class RankTables
{
    internal static FixedArray_sbyte_3 P2RankDefense = FixedArray_sbyte_3.From([0, 1, 1]);

    internal static FixedArray_sbyte_3 P2RankAttack = FixedArray_sbyte_3.From([0, 1, 1]);

    internal static FixedArray_sbyte_3 P2RankDamage = FixedArray_sbyte_3.From([0, 0, 1]);

    internal static FixedArray_sbyte_3 P2RankResistance = FixedArray_sbyte_3.From([0, 0, 1]);

    internal static FixedArray_sbyte_3 P2RankHits = FixedArray_sbyte_3.From([0, 1, 2]);

}
