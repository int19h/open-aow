// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Events;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.AI;

// log index
public partial class ProcessEventLogAIPlayerAction : AIPlayerAction, IAIPlayerActionClass
{
    // AI.pas:1610
    public ProcessEventLogAIPlayerAction(ECustomNode owner) : base(owner)
    {
        _logIndex = -1;
    }

    private int _logIndex;

    // AI.pas:1626
    public override bool Process()
    {
        bool done;
        EventLog log;
        done = false;
        while (!Control.Player.GetBusy() && !done)
        {
            if (_logIndex == -1)
            {
                _logIndex = Control.Player.EventLogbook.FindFirstLog(131811, Control.Player.PlayerIndex, StrategicMap.Current.Day - 1) + 1;
            }
            else
            {
                _logIndex += 1;
            }

            var count = Control.Player.EventLogbook.GetCount();
            if (_logIndex < count)
            {
                log = Control.Player.EventLogbook.GetLogs(_logIndex);
                log.ExecuteAI(Control.Player.PlayerIndex);
            }
            else
            {
                done = true;
            }
        }

        return !done;
    }

    // intf_P5_06.inc:662

    // AI.pas:1621
    public static new int Priority => 999;

}
