using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Combat;
using Aow.Game.Engine;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World.Map;

public partial class AoWMapField
{
    public bool GetAITarget(AIGroupControl control, in AITargetInfo info, ref AITargetData data)
    {
        var query = new AITargetQuery { Info = info };
        var message = new MapFieldMsg { MsgID = 0x1200, Result = 0 };
        message.Param[0] = query;
        message.Param[1] = query;
        message.Param[2] = control;
        try { SendMapFieldMsg(ref message); }
        finally { data = query.Data; }
        return data.Priority != 0;
    }

    public void GetDefenseRequirements(sbyte player, ref DefenseRequirements requirements)
    {
        var query = new DefenseRequirementsQuery();
        var message = new MapFieldMsg { MsgID = 0x1127, Result = 0 };
        message.Param[0] = player;
        message.Param[1] = query;
        try { SendMapFieldMsg(ref message); }
        finally { requirements = query.Requirements; }
    }
}

public partial class AoWHSMap
{
    public void RetrieveBattleFieldInfo(int x, int y, int level, int radius, ref BattleFieldInfo info)
    {
        var query = new BattleFieldQuery { Info = info };
        var message = new MapFieldMsg { MsgID = 0x1108 };
        message.Param[0] = query;
        var mapLevel = MapContainer.GetMapLevel(level);
        HNWalkData walk = default;
        Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref walk, x, y, 0);
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
                Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref walk);
            }
        }
        finally { info = query.Info; }
    }
}
