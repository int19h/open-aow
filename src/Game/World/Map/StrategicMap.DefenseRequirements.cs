
using Aow.Game.Combat;

using Aow.Game.World.Movement;

namespace Aow.Game.World.Map;

public partial class StrategicMap
{
    public void RetrieveBattleFieldInfo(int x, int y, int level, int radius, ref BattleFieldInfo info)
    {
        BattleFieldQuery query = new() { Info = info };
        MapFieldMessage message = new() { MessageId = 0x1108 };
        MapFieldMessageParameters param = message.Param;
        param[0] = query;
        message.Param = param;
        MapLevel mapLevel = MapContainer.GetMapLevel(level);
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, 0);
        try
        {
            while (walk.Radius <= radius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    MapField field = mapLevel.GetField(walk.X, walk.Y);
                    if ((field.FieldFlags & MapField.BattleFieldInfoFlag) != 0)
                    {
                        int friendly = query.Info.FriendlyStrength, enemy = query.Info.Strength;
                        field.SendMapFieldMessage(ref message);
                        BattleFieldInfo info2 = query.Info;
                        info2.FriendlyStrength = Math.Max(friendly, info2.FriendlyStrength);
                        query.Info = info2;
                        BattleFieldInfo info3 = query.Info;
                        info3.Strength = Math.Max(enemy, info3.Strength);
                        query.Info = info3;
                    }
                }
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
        finally
        {
            info = query.Info;
        }
    }
}
