#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Game.World.Movement;

namespace Aow.Game.World.Map;

public partial class GameMap
{
    // HSMAP.PAS:496. The view combines the two signed terrain bytes as one word.
    public bool CanChangeTerrain(MapObject sender, int x, int y, int level, TerrainList terrain)
    {
        bool result = true;
        TerrainList saved = new();
        saved.SetCount(terrain.Count);
        MapLevel mapLevel = MapContainer.GetMapLevel(level);
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, radius: 0);
        bool InRange() => terrain.GetUsed(walk.HexNumber) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height;
        while (walk.HexNumber < terrain.Count)
        {
            if (InRange())
            {
                // The field values before the footprint applies, as the original number pair: a field
                // without a terrain yet saves -1 and gets it back below.
                MapField field = mapLevel.GetField(walk.X, walk.Y);
                sbyte oldTerrain = field.TryGetTerrain(out TerrainType current) ? current.ToOriginal() : (sbyte)-1;
                saved.SetTandO(walk.HexNumber, value: (byte)oldTerrain | ((byte)field.OverlayType.ToOriginal() << 8));
                if (terrain.GetTerrain(walk.HexNumber).Value is { } newTerrain)
                {
                    field.SetTerrain(newTerrain);
                }

                if (terrain.GetOverlay(walk.HexNumber).Value is { } newOverlay)
                {
                    field.OverlayType = newOverlay;
                }
            }
            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
        HexNumberWalkData.InitializeHexNumberWalk(out walk, x, y, radius: 0);
        while (walk.HexNumber < terrain.Count && result)
        {
            if (InRange())
            {
                result = mapLevel.GetField(walk.X, walk.Y).CanChangeTerrain(sender);
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
        HexNumberWalkData.InitializeHexNumberWalk(out walk, x, y, radius: 0);
        while (walk.HexNumber < terrain.Count)
        {
            if (InRange())
            {
                MapField field = mapLevel.GetField(walk.X, walk.Y);
                int pair = saved.GetTandO(walk.HexNumber);
                field.SetTerrain(TerrainType.FromOriginal((sbyte)pair));
                field.OverlayType = OverlayType.FromOriginal((sbyte)(pair >> 8));
            }
            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
        return result;
    }
}
