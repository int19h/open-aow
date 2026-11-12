
using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

public partial class MoveControlSettings
{
    public int GetMovePoints(int x, int y, int level) => LevelData[level] is null ? 0xffffff : LevelData[level][y * Width + x];
    public void SetMovePoints(int x, int y, int level, int value) => LevelData[level][y * Width + x] = value;

    public int GetMovePointsMapCoordinate(int packed)
    {
        var location = new MapCoordinate { Packed = packed };
        return GetMovePoints(location.X, location.Y, location.MapLevel);
    }

    public void SetMovePointsMapCoordinate(int packed, int value)
    {
        var location = new MapCoordinate { Packed = packed };
        SetMovePoints(location.X, location.Y, location.MapLevel, value);
    }
}
