
using Aow.Game.Combat;

using Aow.Game.World.Movement;

namespace Aow.Game.World.Map;

public partial class AoWHSMap
{
    public void RetrieveBattleFieldInfo(int x, int y, int level, int radius, ref BattleFieldInfo info)
    {
        var query = new BattleFieldQuery { Info = info };
        var message = new MapFieldMessage { MsgID = 0x1108 };
        message.Param[0] = query;
        var mapLevel = MapContainer.GetMapLevel(level);
        HexNumberWalkData walk = default;
        HexNumberWalkData.InitHNWalk(ref walk, x, y, 0);
        try
        {
            while (walk.Radius <= radius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    var field = (AoWMapField)mapLevel.GetField(walk.X, walk.Y);
                    if ((field.FFieldFlags & 0x04000000) != 0)
                    {
                        int friendly = query.Info.FriendlyStrength, enemy = query.Info.Strength;
                        field.SendMapFieldMsg(ref message);
                        query.Info.FriendlyStrength = Math.Max(friendly, query.Info.FriendlyStrength);
                        query.Info.Strength = Math.Max(enemy, query.Info.Strength);
                    }
                }
                HexNumberWalkData.WalkToNextHN(ref walk);
            }
        }
        finally { info = query.Info; }
    }
}
