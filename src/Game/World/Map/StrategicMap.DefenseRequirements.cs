
using Aow.Game.Combat;

using Aow.Game.World.Movement;

namespace Aow.Game.World.Map;

public partial class StrategicMap
{
    public void RetrieveBattleFieldInfo(int x, int y, int level, int radius, ref BattleFieldInfo info)
    {
        var query = new BattleFieldQuery { Info = info };
        var message = new MapFieldMessage { MessageId = 0x1108 };
        message.Param[0] = query;
        var mapLevel = MapContainer.GetMapLevel(level);
        HexNumberWalkData walk = default;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, x, y, 0);
        try
        {
            while (walk.Radius <= radius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    var field = mapLevel.GetField(walk.X, walk.Y);
                    if ((field.FieldFlags & 0x04000000) != 0)
                    {
                        int friendly = query.Info.FriendlyStrength, enemy = query.Info.Strength;
                        field.SendMapFieldMessage(ref message);
                        query.Info.FriendlyStrength = Math.Max(friendly, query.Info.FriendlyStrength);
                        query.Info.Strength = Math.Max(enemy, query.Info.Strength);
                    }
                }
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
        finally { info = query.Info; }
    }
}
