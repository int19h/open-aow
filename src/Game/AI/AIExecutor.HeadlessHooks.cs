#nullable enable
using Aow.Game.Host;

namespace Aow.Game.AI;

public partial class AIExecutor
{
    // AI.pas:612. The callback records the source work charge.
    public void UseProcessTime(int time)
    {
        HeadlessHooks.ProcessCost?.Invoke(time, ProcessTimeUsed);
        if (Status.Contains(AIExecutorStatusElement.Executing)) ProcessTimeUsed += time;
    }
}
