
using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

public partial class MoveControlSettings
{
    public int GetMovePoints(int x, int y, int level) => _levelData[level] is null ? 0xffffff : _levelData[level][y * _width + x];
    public void SetMovePoints(int x, int y, int level, int value) => _levelData[level][y * _width + x] = value;

    public int GetMovePointsMapCoordinate(MapCoordinate location) => GetMovePoints(location.X, location.Y, location.MapLevel);

    public void SetMovePointsMapCoordinate(MapCoordinate location, int value) => SetMovePoints(location.X, location.Y, location.MapLevel, value);
}
