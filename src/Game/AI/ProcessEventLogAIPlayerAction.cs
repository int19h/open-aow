#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Events;

using Aow.Game.Session;

namespace Aow.Game.AI;

// log index
public class ProcessEventLogAIPlayerAction(CustomNode owner) : AIPlayerAction(owner), IAIPlayerActionClass
{

    private int _logIndex = -1;

    // AI.pas:1626
    public override bool Process()
    {
        bool done = false;
        while (!Control.Player.GetBusy() && !done)
        {
            if (_logIndex == -1)
            {
                _logIndex = Control.Player.EventLogbook.FindFirstLog(EndTurnEventLog.ClassId, player: Control.Player.PlayerIndex, day: GetEngine().StrategicMap.Day - 1) + 1;
            }
            else
            {
                _logIndex += 1;
            }

            int count = Control.Player.EventLogbook.GetCount();
            if (_logIndex < count)
            {
                EventLog log = Control.Player.EventLogbook[_logIndex];
                log.ExecuteAI(Control.Player.PlayerIndex);
            }
            else
            {
                done = true;
            }
        }

        return !done;
    }

    // AI.pas:1621
    public static new int Priority => 999;

}
