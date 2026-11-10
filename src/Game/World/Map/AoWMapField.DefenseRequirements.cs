using Aow.Game.Ai;
using Aow.Game.Ai.Groups;

using Aow.Game.World.Structures;

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
