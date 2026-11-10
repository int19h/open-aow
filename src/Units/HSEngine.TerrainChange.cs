using Aow.Game.World;
using Aow.Game.World.Movement;

namespace Aow.Game.World.Map;

public partial class HSMap
{
    // HSMAP.PAS:496. The view combines the two signed terrain bytes as one word.
    public bool CanChangeTerrain(MapObject sender, int x, int y, int level, TerrainList terrain)
    {
        bool result = true;
        var saved = new TerrainList();
        saved.SetCount(terrain.FCount);
        var mapLevel = FMapContainer.GetMapLevel(level);
        HNWalkData walk = default;
        Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref walk, x, y, 0);
        bool InRange() => terrain.GetUsed(walk.HN) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.FWidth && walk.Y < mapLevel.FHeight;
        while (walk.HN < terrain.FCount)
        {
            if (InRange())
            {
                var field = mapLevel.GetField(walk.X, walk.Y);
                saved.SetTandO(walk.HN, (ushort)((byte)field.FTerrainType | (byte)field.FOverlayType << 8));
                sbyte value = terrain.GetTerrain(walk.HN);
                if (value >= 0) field.FTerrainType = value;
                value = terrain.GetOverlay(walk.HN);
                if (value >= 0) field.FOverlayType = value;
            }
            Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref walk);
        }
        Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref walk, x, y, 0);
        while (walk.HN < terrain.FCount && result)
        {
            if (InRange()) result = mapLevel.GetField(walk.X, walk.Y).CanChangeTerrainEx(sender);
            Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref walk);
        }
        Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref walk, x, y, 0);
        while (walk.HN < terrain.FCount)
        {
            if (InRange())
            {
                var field = mapLevel.GetField(walk.X, walk.Y);
                ushort pair = saved.GetTandO(walk.HN);
                field.FTerrainType = (sbyte)pair;
                field.FOverlayType = (sbyte)(pair >> 8);
            }
            Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref walk);
        }
        saved?.Dispose();
        return result;
    }
}
