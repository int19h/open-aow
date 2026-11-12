
using Aow.Game.World.Movement;

namespace Aow.Game.World.Map;

public partial class HSMap
{
    // HSMAP.PAS:496. The view combines the two signed terrain bytes as one word.
    public bool CanChangeTerrain(MapObject sender, int x, int y, int level, TerrainList terrain)
    {
        bool result = true;
        var saved = new TerrainList();
        saved.SetCount(terrain._count);
        var mapLevel = _mapContainer.GetMapLevel(level);
        HexNumberWalkData walk = default;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, x, y, 0);
        bool InRange() => terrain.GetUsed(walk.HexNumber) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height;
        while (walk.HexNumber < terrain._count)
        {
            if (InRange())
            {
                var field = mapLevel.GetField(walk.X, walk.Y);
                saved.SetTandO(walk.HexNumber, (ushort)((byte)field.TerrainType | (byte)field.OverlayType << 8));
                sbyte value = terrain.GetTerrain(walk.HexNumber);
                if (value >= 0) field.TerrainType = value;
                value = terrain.GetOverlay(walk.HexNumber);
                if (value >= 0) field.OverlayType = value;
            }
            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, x, y, 0);
        while (walk.HexNumber < terrain._count && result)
        {
            if (InRange()) result = mapLevel.GetField(walk.X, walk.Y).CanChangeTerrain(sender);
            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, x, y, 0);
        while (walk.HexNumber < terrain._count)
        {
            if (InRange())
            {
                var field = mapLevel.GetField(walk.X, walk.Y);
                ushort pair = saved.GetTandO(walk.HexNumber);
                field.TerrainType = (sbyte)pair;
                field.OverlayType = (sbyte)(pair >> 8);
            }
            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
        saved?.Dispose();
        return result;
    }
}
