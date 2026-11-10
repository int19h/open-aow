using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
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
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells.GlobalTarget;

internal static class SpellTargets
{

    internal static Type SpellStructureClass = typeof(Structure);

    internal static readonly string SpellCancelRStr = "Cancel";

    internal static readonly string SpellXCastRStr = "%s cast";

    internal static void NotifySpellSphere(sbyte Player, MagicSphere Sphere)
    {
        EMsg Msg = default;
        EMsg.InitMsg(ref Msg, 537002242, null);
        Msg.Param[0] = Player;
        Msg.Param[1] = (int)Sphere;
        AoWHSMap.Current.SendMsg(ref Msg, null);
    }

}
