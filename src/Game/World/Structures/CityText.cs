
using Aow.Engine.Buffers;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

internal static class CityText
{
    internal static int PlayerCities(sbyte Player)
    {
        int Result;
        Player P;
        PlayerStructure Structure;
        if (AoWHSMap.Current != null)
        {
            P = AoWHSMap.Current.FPlayers.GetPlayers(Player);
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
