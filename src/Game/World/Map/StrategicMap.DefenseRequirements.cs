
using Aow.Game.Combat;

using Aow.Game.World.Movement;

namespace Aow.Game.World.Map;

public partial class StrategicMap
{
    public void RetrieveBattleFieldInfo(int x, int y, int level, int radius, ref BattleFieldInfo info)
    {
        var query = new BattleFieldQuery { Info = info };
        var message = new MapFieldMessage { MessageId = 0x1108 };
        var param = message.Param;
        param[0] = query;
        message.Param = param;
        var mapLevel = MapContainer.GetMapLevel(level);
        HexNumberWalkData walk = default;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, x, y, 0);
        try
        {
            while (walk.Radius <= radius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    var field = mapLevel.GetField(walk.X, walk.Y);
                    if ((field.FieldFlags & MapField.BattleFieldInfoFlag) != 0)
                    {
                        int friendly = query.Info.FriendlyStrength, enemy = query.Info.Strength;
                        field.SendMapFieldMessage(ref message);
                        var info2 = query.Info;
                        info2.FriendlyStrength = Math.Max(friendly, info2.FriendlyStrength);
                        query.Info = info2;
                        var info3 = query.Info;
                        info3.Strength = Math.Max(enemy, info3.Strength);
                        query.Info = info3;
                    }
                }
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
        finally { info = query.Info; }
    }
}
