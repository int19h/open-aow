using Aow.Game.Events;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Ai
{

    public sealed class AITargetQuery
    {
        public AITargetInfo Info;
        public AITargetData Data;
    }
}

namespace Aow.Game.Combat
{

    public sealed class BattleFieldQuery
    {
        public BattleFieldInfo Info;
    }
}

namespace Aow.Game.Engine
{

    public sealed class DefenseRequirementsQuery
    {
        public DefenseRequirements Requirements;
    }
}

namespace Aow.Game.Combat
{

    public sealed class StormDamageQuery
    {
        public StormDamageInfo Info;
    }

    public sealed class CombatResultQuery
    {
        public string Text = "";
    }
}

namespace Aow.Game.Engine
{

    public sealed class ShowMessageNotification
    {
        public ShowMessageInfo Info;
    }
}

namespace Aow.Game.World.Map
{

    public sealed class MapChangedNotification
    {
        public MapChangedInfo Info;
    }
}

namespace Aow.Game.Events
{

    public sealed class ExecuteEventLogNotification
    {
        public ExecuteEventLogInfo Info;
    }
}

namespace Aow.Game.Ai.Groups
{

    public partial class AIMoveExecuterControl
    {
        public AIMoveExecuterMove CreateMove(AIMoveExecuterArmy army, byte selection, sbyte x, sbyte y,
            sbyte level, Set<CreateMoveModeElement> mode)
        {
            byte result = 0;
            return CreateMove(army, selection, x, y, level, mode, ref result);
        }
    }
}
