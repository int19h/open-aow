using Aow.Rtl;

namespace Aow.Units.HSEngine;

public partial class THSMap
{
    // HSMAP.PAS:496. The view combines the two signed terrain bytes as one word.
    public bool CanChangeTerrain(TMapObject sender, int x, int y, int level, TTerrainList terrain)
    {
        bool result = true;
        var saved = TTerrainList.Create();
        saved.SetCount(terrain.FCount);
        var mapLevel = FMapContainer.GetMapLevel(level);
        THNWalkData walk = default;
        HSEngine.InitHNWalk(ref walk, x, y, 0);
        bool InRange() => terrain.GetUsed(walk.HN) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.FWidth && walk.Y < mapLevel.FHeight;
        while (walk.HN < terrain.FCount)
        {
            if (InRange())
            {
                var field = mapLevel.GetField(walk.X, walk.Y);
                saved.SetTandO(walk.HN, unchecked((ushort)((byte)field.FTerrainType | (byte)field.FOverlayType << 8)));
                sbyte value = terrain.GetTerrain(walk.HN);
                if (value >= 0) field.FTerrainType = value;
                value = terrain.GetOverlay(walk.HN);
                if (value >= 0) field.FOverlayType = value;
            }
            HSEngine.WalkToNextHN(ref walk);
        }
        HSEngine.InitHNWalk(ref walk, x, y, 0);
        while (walk.HN < terrain.FCount && result)
        {
            if (InRange()) result = mapLevel.GetField(walk.X, walk.Y).CanChangeTerrainEx(sender);
            HSEngine.WalkToNextHN(ref walk);
        }
        HSEngine.InitHNWalk(ref walk, x, y, 0);
        while (walk.HN < terrain.FCount)
        {
            if (InRange())
            {
                var field = mapLevel.GetField(walk.X, walk.Y);
                ushort pair = saved.GetTandO(walk.HN);
                field.FTerrainType = unchecked((sbyte)pair);
                field.FOverlayType = unchecked((sbyte)(pair >> 8));
            }
            HSEngine.WalkToNextHN(ref walk);
        }
        saved.Free();
        return result;
    }
}
