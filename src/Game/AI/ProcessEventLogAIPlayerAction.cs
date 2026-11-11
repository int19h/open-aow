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
        FLogIndex = -1;
    }

    public int FLogIndex;

    // AI.pas:1626
    public override bool Process()
    {
        bool Done;
        EventLog Log;
        Done = false;
        while (!FControl.FPlayer.GetBusy() && !Done)
        {
            if (FLogIndex == -1)
            {
                FLogIndex = FControl.FPlayer.FEventLogbook.FindFirstLogEx(131811, FControl.FPlayer.FPlayerIndex, AoWHSMap.Current.FDay - 1) + 1;
            }
            else
            {
                FLogIndex += 1;
            }

            var count = FControl.FPlayer.FEventLogbook.GetCount();
            if (FLogIndex < count)
            {
                Log = FControl.FPlayer.FEventLogbook.GetLogs(FLogIndex);
                Log.ExecuteAI(FControl.FPlayer.FPlayerIndex);
            }
            else
            {
                Done = true;
            }
        }

        return !Done;
    }

    // intf_P5_06.inc:662

    // AI.pas:1621
    public static new int Priority => 999;

}
