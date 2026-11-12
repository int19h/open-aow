
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Map;

public partial class MapField
{
    public bool GetAITarget(AIGroupControl control, in AITargetInfo info, out AITargetData data)
    {
        var query = new AITargetQuery { Info = info };
        var message = new MapFieldMessage { MessageId = 0x1200, Result = 0 };
        message.Param[0] = query;
        message.Param[1] = query;
        message.Param[2] = control;
        try { SendMapFieldMessage(ref message); }
        finally { data = query.Data; }
        return data.Priority != 0;
    }

    public void GetDefenseRequirements(sbyte player, out DefenseRequirements requirements)
    {
        var query = new DefenseRequirementsQuery();
        var message = new MapFieldMessage { MessageId = 0x1127, Result = 0 };
        message.Param[0] = player;
        message.Param[1] = query;
        try { SendMapFieldMessage(ref message); }
        finally { requirements = query.Requirements; }
    }
}
