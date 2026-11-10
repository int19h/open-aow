using System.Runtime.CompilerServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
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
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

internal static class CityText
{
    internal static int PlayerCities(sbyte Player)
    {
        int Result;
        TPlayer P;
        TPlayerStructure Structure;
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
        {
            P = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player);
            Result = 0;
            for (int I = 0, last = P.FStructures.Count - 1; I <= last; I++)
            {
                Structure = P.FStructures.GetStructure(I);
                if (Structure.EClass.ClassID == 131336 && Structure.FPlayer == Player)
                {
                    Result += 1;
                }
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    internal static FixedArray_string_9 TCityActionRStr = FixedArray_string_9.From(["", "", "", "", "", "", "", "", ""]);

    internal static readonly string CityNotEnoughGoldRStr = "Not enough gold.";

}
