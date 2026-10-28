using Aow.Units.HSEngine;

namespace Aow.Units.AoWE;

public partial class TAoWHSMap
{
    public bool ValidateObservation(sbyte player, sbyte x, sbyte y, sbyte level, int radius)
    {
        if (!FShowIndependentMoves || player == AoWE.AoWHSMap.FSeatedPlayer) return false;
        THNWalkData walk = default;
        HSEngine.HSEngine.InitHNWalk(ref walk, x, y, 0);
        TMapLevel mapLevel = null;
        bool result = false;
        while (walk.Radius <= radius && !result)
        {
            // The original rejects negative coordinates before dereferencing its level pointer.
            // A caster without a map location supplies (-1, -1, -1) with radius zero.
            if (walk.X >= 0 && walk.Y >= 0)
            {
                mapLevel ??= MapContainer.GetMapLevel(level);
                if (walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    var field = (TAoWMapField)mapLevel.GetField(walk.X, walk.Y);
                    result = (((field.FFieldFlags >> 16) & 0xff) != 0 || !AoWE.AoWHSMap.FFogEnabled) &&
                        (field.GetExplored(AoWE.AoWHSMap.FSeatedPlayer) || !AoWE.AoWHSMap.FExplorationEnabled);
                }
            }
            if (!result) HSEngine.HSEngine.WalkToNextHN(ref walk);
        }
        if (result) MakeVisible(walk.X, walk.Y, level, 3);
        return result;
    }
}
